using UnityEngine;

public class BloodOnClick : MonoBehaviour
{
    // This creates a slot in the Inspector
    public BloodSpawner bloodSpawner; 

    private void OnMouseDown()
    {
        if (bloodSpawner != null)
        {
            // Call the method on the specific instance
            bloodSpawner.SpawnBlood(transform.position);
        }
        else
        {
            Debug.LogWarning("Assign the BloodSpawner to the slot on " + gameObject.name);
        }
    }
}