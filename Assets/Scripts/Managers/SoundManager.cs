using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioClip winSound;
    [SerializeField] AudioClip loseSound;
    [Header("Pieces")]
    [SerializeField] AudioClip moveAsideSound;
    [SerializeField] AudioClip rotateSound;
    [SerializeField] AudioClip moveDownSound;
    [SerializeField] AudioClip deleteLineSound;
    [SerializeField] AudioClip fireSound;
    //[SerializeField] AudioClip dropSound;
    [SerializeField] private AudioClip lockSound;
    [SerializeField] private SaveGameManager _saveGameManager;
    public AudioSource soundsAudioSourse;
    public bool SoundsOn;
    public float SoundsVolume;
    void Awake()
    {
        soundsAudioSourse = GetComponent<AudioSource>();
        if (soundsAudioSourse == null)
        {
            Debug.LogError("SoundManager:: AudioSource is null");
        }
        
    }

    private void Start()
    {
        //_saveGameManager.LoadAudioData();
    }

    public void ChangeButtonState(bool buttonState)
    {
        SoundsOn = buttonState;
        _saveGameManager.SaveAudioSettings();
    }
    
    public void SetVolume(float volume)
    {
        soundsAudioSourse.volume = volume;
        SoundsVolume = volume;
        _saveGameManager.SaveAudioSettings();

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
            //Sounds.MoveDown => moveDownSound,
            Sounds.Fire => fireSound,
            //Sounds.Drop => dropSound,
            Sounds.DeleteLine => deleteLineSound,
            Sounds.Lock => lockSound,
            _ => null
        };
    }
}
