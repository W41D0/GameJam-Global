using UnityEngine;
using UnityEngine.InputSystem;

public class TempMovingScript : MonoBehaviour
{
    [Tooltip("Movement speed in units per second")]
    public float moveSpeed = 5f;
    Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveX = 0f;
        float moveY = 0f;

        var kb = Keyboard.current;
        if (kb != null)
        {
            if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) moveX -= 1f;
            if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) moveX += 1f;
            if (kb.wKey.isPressed || kb.upArrowKey.isPressed) moveY += 1f;
            if (kb.sKey.isPressed || kb.downArrowKey.isPressed) moveY -= 1f;
        }

        Vector2 movement = new Vector2(moveX, moveY);
        if (movement.sqrMagnitude > 1f) movement.Normalize();

        rb2d.linearVelocity = movement * moveSpeed;
    }
}
