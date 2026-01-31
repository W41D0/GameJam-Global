using UnityEngine;

public class ShootSound : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip gunshotClip;

    [Header("Volume Settings")]
    [Range(0f, 1f)] 
    public float volume = 0.5f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlaySound();
        }
    }

    void PlaySound()
    {
        if (audioSource != null && gunshotClip != null)
        {
            audioSource.PlayOneShot(gunshotClip, volume);
        }
        else
        {
            Debug.LogWarning("Missing Audio Source or Clip on ShootSound script!");
        }
    }
}