using UnityEngine;

public class WaterAudioControllerSingleton : MonoBehaviour
{
    public static WaterAudioControllerSingleton Instance { get; private set; }

    private AudioSource audioSource;

    void Awake()
    {
        // Check if an instance already exists
        if (Instance != null && Instance != this)
        {
            // If another instance exists, destroy this one
            Destroy(this.gameObject);
            return;
        }

        // Assign this object as the instance and make sure it persists across scenes
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();

        // Optionally, check and warn if there's no AudioSource component found
        if (!audioSource)
        {
            Debug.LogWarning("WaterAudioControllerSingleton: No AudioSource component found on the GameObject.");
        }
    }

    // Public method to start playing the audio
    public void PlayWaterAudio()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    // Public method to stop playing the audio
    public void StopWaterAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
