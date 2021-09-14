using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public List<AudioClip> footsteps = new List<AudioClip>();
    private System.Random random = new System.Random();


    public void PlayFootstep()
    {
        int randomIndex = random.Next(footsteps.Count);
        AudioClip footstep = footsteps[randomIndex];

        audioSource.PlayOneShot(footstep);
    }
}
