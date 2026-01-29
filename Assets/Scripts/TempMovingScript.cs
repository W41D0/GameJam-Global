using UnityEngine;
using UnityEngine.InputSystem;

public class TempMovingScript : MonoBehaviour
{
    [Tooltip("Movement speed in units per second")]
    public float moveSpeed = 5f;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
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

        Vector3 movement = new Vector3(moveX, moveY, 0f);
        if (movement.sqrMagnitude > 1f) movement.Normalize();

        transform.position += movement * moveSpeed * Time.deltaTime;
    }
}
