using UnityEngine;
using UnityEngine.InputSystem;

public class wiggascript : MonoBehaviour
{
    public float speed = 5f;
    public float gravityScale = 0.5f;
    public float jumpForce = 6f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isJumping = false;
    private bool isGrounded = false;
    private bool wasFalling = false;
    private Collider2D col;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        float hitboxHeight = 1f;
        if (col != null)
        {
            hitboxHeight = col.bounds.size.y;
        }
        jumpForce = 2f * hitboxHeight;
        if (rb != null)
        {
            rb.gravityScale = gravityScale;
        }
    }

    void Update()
    {
        float x = 0f;
        float y = 0f;

        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.wKey.isPressed && isGrounded)
            {
                Jump();
            }
            if (keyboard.sKey.isPressed)
            {
                y -= 1f;
                // Cancel jump if "s" is pressed while jumping
                if (isJumping && rb != null && rb.linearVelocity.y > 0)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
                    isJumping = false;
                }
            }
            if (keyboard.dKey.isPressed) x += 1f;
            if (keyboard.aKey.isPressed) x -= 1f;
        }

        moveInput = new Vector2(x, y);
        if (moveInput.sqrMagnitude > 1f) moveInput.Normalize();

        // Track falling state
        if (rb != null)
        {
            wasFalling = rb.linearVelocity.y < -0.01f;
        }

        if (rb == null)
        {
            if (moveInput != Vector2.zero)
            {
                transform.Translate(moveInput * speed * Time.deltaTime, Space.World);
            }
            transform.rotation = Quaternion.identity;
        }
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            float vx = moveInput.x * speed;
            rb.linearVelocity = new Vector2(vx, rb.linearVelocity.y);
            rb.rotation = 0f;
        }
    }

    void Jump()
    {
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            Vector2 jumpDirection = Vector2.up;
            
            // Allow diagonal jumping based on horizontal input
            if (moveInput.x != 0f)
            {
                jumpDirection = new Vector2(moveInput.x, 1f).normalized;
            }
            
            rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
            isJumping = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (var contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                if (wasFalling && rb != null)
                {
                    rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
                }
                isJumping = false;
                break;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
