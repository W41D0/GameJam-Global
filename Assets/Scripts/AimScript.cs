using UnityEngine;
using UnityEngine.InputSystem; // 1. Add this namespace

public class AimScript : MonoBehaviour
{
    // In ScreenToWorldPoint, the Z value is "distance from camera", not movement speed.
    // I renamed 'speed' to 'depth' to make it clearer.
    public float depth = 10f; 

    void Update()
    {
        // 2. Check if a mouse is connected
        if (Mouse.current == null) return;

        // 3. Get the position from the New Input System
        Vector2 mousePos2D = Mouse.current.position.ReadValue();

        // 4. Create the vector (X, Y, Depth)
        Vector3 screenPos = new Vector3(mousePos2D.x, mousePos2D.y, depth);

        // 5. Convert
        transform.position = Camera.main.ScreenToWorldPoint(screenPos);
    }
}
