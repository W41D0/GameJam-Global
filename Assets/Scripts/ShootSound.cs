using UnityEngine;

public class ShootSound : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;  // Drag the Audio Source component here
    public AudioClip gunshotClip;    // Drag your .mp3 or .wav file here

    [Header("Volume Settings")]
    [Range(0f, 1f)] 
    public float volume = 0.5f;      // Adjust volume slider in Inspector

    void Update()
    {
        // 0 = Left Click
        if (Input.GetMouseButtonDown(0))
        {
            PlaySound();
        }
    }

    void PlaySound()
    {
        // Safety Check: Prevent crash if you forgot to drag the sound
        if (audioSource != null && gunshotClip != null)
        {
            // PlayOneShot allows multiple shots to overlap without cutting off
            audioSource.PlayOneShot(gunshotClip, volume);
        }
        else
        {
            Debug.LogWarning("Missing Audio Source or Clip on ShootSound script!");
        }
    }
}