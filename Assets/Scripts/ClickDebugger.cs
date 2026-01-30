using UnityEngine;

public class ClickDebugger : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Convert mouse to world position
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            // Fire a laser at that spot
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log($"✅ HIT: {hit.collider.gameObject.name} (Layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)})");
            }
            else
            {
                Debug.Log("❌ MISSED: The mouse clicked empty space (or the collider is ignored).");
            }
        }
    }
}