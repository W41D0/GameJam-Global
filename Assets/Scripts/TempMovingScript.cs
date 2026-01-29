using UnityEngine;

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

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) moveX -= 1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) moveX += 1f;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) moveY += 1f;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) moveY -= 1f;

        Vector3 movement = new Vector3(moveX, moveY, 0f);
        if (movement.sqrMagnitude > 1f) movement.Normalize();

        transform.position += movement * moveSpeed * Time.deltaTime;
    }
}
