using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudio : MonoBehaviour
{
  [SerializeField] private List<AudioClip> footsteps;
  [SerializeField] private List<AudioClip> meows;
  [SerializeField] private List<AudioClip> clothScratches;
  [SerializeField] private string footstepDirectory = "Audio/cat-footsteps";
  [SerializeField] private string meowDirectory = "Audio/meows";
  [SerializeField] private string clothScratchesDirectory = "Audio/cloth-scratches";
  private AudioSource audioSource;
  private System.Random random = new System.Random();

  void Start()
  {
    LoadAudioClips();
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

  public void PlayClothScratches()
  {
    PlayRandom(clothScratches);
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

  private void LoadAudioClips()
  {
    footsteps = Resources.LoadAll<AudioClip>(footstepDirectory).ToList<AudioClip>();
    meows = Resources.LoadAll<AudioClip>(meowDirectory).ToList<AudioClip>();
    clothScratches = Resources.LoadAll<AudioClip>(clothScratchesDirectory).ToList<AudioClip>();
  }
}
