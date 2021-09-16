using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
  [SerializeField] private List<AudioClip> footsteps = new List<AudioClip>();
  [SerializeField] private List<AudioClip> meows = new List<AudioClip>();
  private AudioSource audioSource;
  private System.Random random = new System.Random();

  void Start()
  {
    audioSource = GetComponent<AudioSource>();
  }

  public void PlayFootstep()
  {
    PlayRandom(footsteps);
  }

  public void PlayMeow()
  {
    PlayRandom(meows);
  }

  private void SafePlayOneShot(AudioClip clip)
  {
    if (clip != null) audioSource.PlayOneShot(clip);
  }

  private void PlayRandom(List<AudioClip> clips)
  {
    int randomIndex = random.Next(clips.Count);
    AudioClip clip = clips[randomIndex];

    SafePlayOneShot(clip);
  }
}
