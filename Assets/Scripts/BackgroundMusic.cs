using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    // Static reference so the GameManager can find it easily
    public static BackgroundMusic instance;

    void Awake()
    {
        // THE CHECK:
        // If a Music Player already exists from the previous level...
        if (instance != null)
        {
            // ...destroy this NEW one so the OLD one keeps playing seamless audio.
            Destroy(gameObject); 
            return;
        }

        // If this is the first one, set it as the "Instance" and keep it alive.
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}