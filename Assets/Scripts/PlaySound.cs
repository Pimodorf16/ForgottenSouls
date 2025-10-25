using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioClip[] clips; 
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Play one by index
    public void PlayByIndex(int index)
    {
        if (clips.Length > 0 && index >= 0 && index < clips.Length)
        {
            audioSource.PlayOneShot(clips[index]);
        }
    }
}
