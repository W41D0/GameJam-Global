using UnityEngine;

public class Animation : MonoBehaviour
{
    [Header("Bobbing Animation")]
    [SerializeField] private float bobbingSpeed = 5f;
    [SerializeField] private float bobbingHeight = 0.2f;
    
    private Vector3 originalPosition;
    private float bobbingTimer = 0f;
    private wiggascript playerMovement;
    private Rigidbody2D rb;

    void Start()
    {
        originalPosition = transform.position;
        playerMovement = GetComponent<wiggascript>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Check if the game object is moving
        bool isMoving = false;
        
        if (playerMovement != null)
        {
            // Check if there's any input or velocity
            isMoving = playerMovement.enabled && (Mathf.Abs(rb.linearVelocity.x) > 0.1f || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D));
        }
        else if (rb != null)
        {
            isMoving = Mathf.Abs(rb.linearVelocity.x) > 0.1f;
        }

        // Apply bobbing animation if moving
        if (isMoving)
        {
            bobbingTimer += Time.deltaTime * bobbingSpeed;
            float bobbingOffset = Mathf.Sin(bobbingTimer) * bobbingHeight;
            
            Vector3 newPosition = transform.position;
            newPosition.y = originalPosition.y + bobbingOffset;
            transform.position = newPosition;
        }
        else
        {
            // Reset to original position when not moving
            bobbingTimer = 0f;
            Vector3 newPosition = transform.position;
            newPosition.y = originalPosition.y;
            transform.position = newPosition;
        }
    }
}
