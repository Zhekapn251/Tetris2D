using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip loseSound;
    [Header("Pieces")]
    [SerializeField] private AudioClip moveAsideSound;
    [SerializeField] private AudioClip rotateSound;
    [SerializeField] private AudioClip deleteLineSound;
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private AudioClip lockSound;
    [SerializeField] private SaveGameManager saveGameManager;
    
    public AudioSource soundsAudioSourse;
    public bool soundsOn;
    public float soundsVolume;
    void Awake()
    {
        soundsAudioSourse = GetComponent<AudioSource>();
        if (soundsAudioSourse == null)
        {
            Debug.LogError("SoundManager:: AudioSource is null");
        }
    }
    

    public void ChangeButtonState(bool buttonState)
    {
        soundsOn = buttonState;
        saveGameManager.SaveAudioSettings();
    }
    
    public void SetVolume(float volume)
    {
        soundsAudioSourse.volume = volume;
        soundsVolume = volume;
        saveGameManager.SaveAudioSettings();
    }
    public void PlaySound(Sounds sound)
    {
        var clip = GetAudioClip(sound);
        if(clip == null) return;
        soundsAudioSourse.PlayOneShot(clip);
    }

    private AudioClip GetAudioClip(Sounds sound)
    {
        return sound switch
        {
            Sounds.Win => winSound,
            Sounds.Lose => loseSound,
            Sounds.MoveAside => moveAsideSound,
            Sounds.Rotate => rotateSound,
            Sounds.Fire => fireSound,
            Sounds.DeleteLine => deleteLineSound,
            Sounds.Lock => lockSound,
            _ => null
        };
    }
}
