using UnityEngine;
using UnityEngine.InputSystem; 

public class AimScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    
    // Safety flag to prevent the script from thinking we won/lost 
    // before the enemies have even spawned.
    private bool canCheckConditions = false; 

    public float depth = 10f; 
    
    [Header("Recoil")]
    public float recoilAmount = 0.2f; 
    public float recoilUpDuration = 0.03f;
    public float recoilReturnDuration = 0.12f;

    private Vector3 recoilOffset = Vector3.zero;
    private Coroutine recoilCoroutine;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // 1. Force Visible immediately
        gameObject.SetActive(true);      
        spriteRenderer.enabled = true;   

        // 2. Start the Grace Period (Wait 0.5s before checking Win/Loss)
        StartCoroutine(EnableLogicAfterDelay());
    }

    System.Collections.IEnumerator EnableLogicAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        canCheckConditions = true;
    }

    void Update()
    {
        // 3. Only check for Win/Loss if the level is fully loaded
        if (canCheckConditions)
        {
            // We use GameManager.currentAttendees because it's the reliable number
            if (AttendeeBehaviour.numOfAssasinsAlive == 0 || 
                (GameManager.currentAttendees > 0 && AttendeeBehaviour.numOfAttendeesAlive < GameManager.currentAttendees))
            {
                spriteRenderer.enabled = false;
                // We do NOT return here, so the cursor position still updates 
                // (otherwise the recoil gets stuck in mid-air if you lose)
            }
        }

        if (Mouse.current == null) return;

        // Position Logic
        Vector2 mousePos2D = Mouse.current.position.ReadValue();
        Vector3 screenPos = new Vector3(mousePos2D.x, mousePos2D.y, depth);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        transform.position = worldPos + recoilOffset;

        // Recoil Logic
        // We add 'spriteRenderer.enabled' check so you can't shoot if the cursor is hidden
        if (spriteRenderer.enabled && Mouse.current.leftButton.wasPressedThisFrame)
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