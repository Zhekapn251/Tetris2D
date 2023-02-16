using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
 
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private MusicManager _musicManager;
    [SerializeField] private SaveGameManager _saveGameManager;
    private SoundSettings _soundSettings;
    private bool run;


    private void Start()
    {
        _saveGameManager.LoadAudioData();
        _soundManager.SetVolume(_soundManager.SoundsVolume);
        _musicManager.SetVolume(_musicManager.MusicVolume);
        _soundManager.soundsAudioSourse.mute = !_soundManager.SoundsOn;
        _musicManager.musicAudioSourse.mute = !_musicManager.MusicOn;
    }

    
}
