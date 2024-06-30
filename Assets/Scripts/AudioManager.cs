using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private List<AudioClip> audios;
    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        GameControl.OnRevealed += TryToPlayAudio;
    }

    private void OnDisable()
    {
        GameControl.OnRevealed -= TryToPlayAudio;
    }

    private void TryToPlayAudio(string _audioName)
    {
        AudioClip _audio = audios.Find(_audio => _audio.name.Trim().ToLower() == _audioName.Trim().ToLower());
        if (_audio==null)
        {
            Debug.Log("Cant find audio for "+_audioName);
            return;
        }
        
        audioSource.PlayOneShot(_audio);
    }

    public void PlayCardReveal()
    {
        AudioClip _audio = audios.Find(_audio => _audio.name.Trim().ToLower() == "CardReveal".Trim().ToLower());
        if (_audio==null)
        {
            Debug.Log("Cant find audio for CardReveal");
            return;
        }
        
        audioSource.PlayOneShot(_audio);
    }
}
