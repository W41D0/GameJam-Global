using UnityEngine;
using UnityEngine.InputSystem; // 1. Add this namespace

public class AimScript : MonoBehaviour
{
    int initialAttendees;
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        initialAttendees = AttendeeBehaviour.numOfAttendeesAlive;
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameObject.SetActive(true);      // Re-enable gameObject at start of each game
        spriteRenderer.enabled = true;   // Ensure sprite is visible at start of each game
    }
    // In ScreenToWorldPoint, the Z value is "distance from camera", not movement speed.
    // I renamed 'speed' to 'depth' to make it clearer.
    public float depth = 10f; 
    [Header("Recoil")]
    public float recoilAmount = 0.2f; // world units to move up
    public float recoilUpDuration = 0.03f;
    public float recoilReturnDuration = 0.12f;

    private Vector3 recoilOffset = Vector3.zero;
    private Coroutine recoilCoroutine;

    void Update()
    {
        // Check if all assassins are killed or an attendee died
        if (AttendeeBehaviour.numOfAssasinsAlive == 0 || AttendeeBehaviour.numOfAttendeesAlive < initialAttendees)
        {
            spriteRenderer.enabled = false;
            return;
        }

        // 2. Check if a mouse is connected
        if (Mouse.current == null) return;

        // 3. Get the position from the New Input System
        Vector2 mousePos2D = Mouse.current.position.ReadValue();

        // 4. Create the vector (X, Y, Depth)
        Vector3 screenPos = new Vector3(mousePos2D.x, mousePos2D.y, depth);

        // 5. Convert to world and apply recoil offset
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        transform.position = worldPos + recoilOffset;

        // 6. Trigger recoil on left mouse click
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (recoilCoroutine != null) StopCoroutine(recoilCoroutine);
            recoilCoroutine = StartCoroutine(RecoilRoutine());
        }
    }

    private System.Collections.IEnumerator RecoilRoutine()
    {
        float elapsed = 0f;

        // move up
        while (elapsed < recoilUpDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / recoilUpDuration);
            recoilOffset.y = Mathf.Lerp(0f, recoilAmount, t);
            yield return null;
        }

        // return
        elapsed = 0f;
        float start = recoilOffset.y;
        while (elapsed < recoilReturnDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / recoilReturnDuration);
            recoilOffset.y = Mathf.Lerp(start, 0f, t);
            yield return null;
        }

        recoilOffset.y = 0f;
        recoilCoroutine = null;
    }
}