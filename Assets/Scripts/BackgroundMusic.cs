using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic instance;

    void Awake()
    {
        // 1. FORCE ROOT: Detach from any parents so DontDestroyOnLoad works
        transform.SetParent(null); 

        // 2. THE CHECK:
        if (instance != null)
        {
            // If we find an old music player, we destroy OURSELVES (the new one)
            // so the old one can keep singing.
            Destroy(gameObject); 
            return;
        }

        // 3. CLAIM THE THRONE:
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}