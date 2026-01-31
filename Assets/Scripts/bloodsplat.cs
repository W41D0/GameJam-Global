using UnityEngine;

public class BloodSpawner : MonoBehaviour
{
    // Assign your blood splat prefab here in the Unity Inspector
    public GameObject bloodSplatPrefab; 

    // Function to call when you want to spawn blood
    public void SpawnBlood(Vector3 spawnPosition)
    {
        // Instantiate the prefab at the specified position with no rotation (Quaternion.identity)
        GameObject newSplat = Instantiate(bloodSplatPrefab, spawnPosition, Quaternion.identity);

        // Optional: Make the blood disappear after a few seconds to clean up the scene
        Destroy(newSplat, 5f); 
    }
}

