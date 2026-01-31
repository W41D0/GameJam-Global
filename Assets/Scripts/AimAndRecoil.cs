using UnityEngine;
using UnityEngine.InputSystem; 

public class AimScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool canCheckConditions = false; 

    public float depth = 10f; 
    
    public float recoilAmount = 0.2f; 
    public float recoilUpDuration = 0.03f;
    public float recoilReturnDuration = 0.12f;

    private Vector3 recoilOffset = Vector3.zero;
    private Coroutine recoilCoroutine;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        gameObject.SetActive(true);      
        spriteRenderer.enabled = true;   

        StartCoroutine(EnableLogicAfterDelay());
    }

    System.Collections.IEnumerator EnableLogicAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        canCheckConditions = true;
    }

    void Update()
    {
        if (canCheckConditions && GameManager.isGameOver)
        {
            spriteRenderer.enabled = false;
        }

        if (Mouse.current == null) return;

        Vector2 mousePos2D = Mouse.current.position.ReadValue();
        Vector3 screenPos = new Vector3(mousePos2D.x, mousePos2D.y, depth);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        transform.position = worldPos + recoilOffset;

        if (spriteRenderer.enabled && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (recoilCoroutine != null) StopCoroutine(recoilCoroutine);
            recoilCoroutine = StartCoroutine(RecoilRoutine());
        }
    }

    private System.Collections.IEnumerator RecoilRoutine()
    {
        float elapsed = 0f;

        while (elapsed < recoilUpDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / recoilUpDuration);
            recoilOffset.y = Mathf.Lerp(0f, recoilAmount, t);
            yield return null;
        }

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