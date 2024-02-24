using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioClip scriptAClip; // Assign this in the inspector
    public AudioClip scriptBClip; // Assign this in the inspector
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlaySelectedAudio();
    }

    void PlaySelectedAudio()
    {
        switch (DataManager.Instance.selectedScript)
        {
            case "ScriptA":
                audioSource.clip = scriptAClip;
                break;
            case "ScriptB":
                audioSource.clip = scriptBClip;
                break;
        }
        audioSource.Play();
    }
}
