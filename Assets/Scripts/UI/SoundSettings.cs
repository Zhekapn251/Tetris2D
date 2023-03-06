using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] private Image grayImageMusicButton;
    [SerializeField] private Image orangeImageMusicButton;
    [SerializeField] private Image grayImageSoundsButton;
    [SerializeField] private Image orangeImageSoundsButton;
    [SerializeField] public Slider soundsSlider;
    [SerializeField] public Slider musicSlider;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private Button soundsBtn;
    [SerializeField] private Button musicBtn;
    [SerializeField] private Button exitBtn;
    [SerializeField] private Transform musicBtnPositionOn;
    [SerializeField] private Transform musicBtnPositionOff;
    [SerializeField] private Transform soundBtnPositionOn;
    [SerializeField] private Transform soundBtnPositionOff;
    
    private bool _musicIsEnabled;
    private float _musicVolume;
    private bool _soundsIsEnabled;
    private float _soundsVolume;
    private void Start()
    {
        soundsBtn.onClick.AddListener(SoundsButtonCliked);
        musicBtn.onClick.AddListener(MusicButtonCliked);
        exitBtn.onClick.AddListener(SoundsMenuOff);
        soundsSlider.onValueChanged.AddListener(SoundsSlider);
        musicSlider.onValueChanged.AddListener(MusicSlider);
        AudioInit();
    }

    public void SoundsMenuOn()
    {
        gameObject.SetActive(true);
    }

    private void SoundsMenuOff()
    {
        gameObject.SetActive(false);
    }
    private void AudioInit()
    {
        _soundsIsEnabled = soundManager.soundsOn;
        _musicIsEnabled = musicManager.musicOn;
        musicSlider.value = musicManager.musicVolume;
        soundsSlider.value = soundManager.soundsVolume;
        AudioUIInit();
    }
    private void AudioUIInit()
    {
        if (!_musicIsEnabled) OffAnimation(grayImageMusicButton, orangeImageMusicButton, musicBtnPositionOff, 0f);
        if (!_soundsIsEnabled) OffAnimation(grayImageSoundsButton, orangeImageSoundsButton, soundBtnPositionOff,0f);
    }
    private void OnAnimation(Image bottom, Image top, Transform targetPosition)
    {
        bottom.transform.DOMoveX(targetPosition.position.x, 0.3f);
        top.DOFade(1, 0.3f);
    }
    
    private void OffAnimation(Image bottom, Image top, Transform targetPosition, float fadeSpeed = 0.3f)
    {
        bottom.transform.DOMoveX(targetPosition.position.x, fadeSpeed);
        top.DOFade(0, 0.3f);
    }

    private void MusicSlider(float sliderValue)
    {
        _musicVolume = sliderValue;
        musicManager.SetVolume(_musicVolume);
    }

    private void SoundsSlider(float sliderValue)
    {
        _soundsVolume = sliderValue;
        soundManager.SetVolume(_soundsVolume);
    }
    
    public void MusicButtonCliked()
    {
        if (_musicIsEnabled)
        {
            OffAnimation(grayImageMusicButton,orangeImageMusicButton, musicBtnPositionOff);
            _musicIsEnabled = false;
            musicManager.musicAudioSource.mute = true;
        }
        else
        {
            OnAnimation(grayImageMusicButton,orangeImageMusicButton, musicBtnPositionOn);
            _musicIsEnabled = true;
            musicManager.musicAudioSource.mute = false;
        }
        musicManager.ChangeButtonState(_musicIsEnabled);
    }
    
    public void SoundsButtonCliked()  
    {
        if (_soundsIsEnabled)
        {
            OffAnimation(grayImageSoundsButton,orangeImageSoundsButton, soundBtnPositionOff);
            _soundsIsEnabled = false;
            soundManager.soundsAudioSourse.mute = true;
        }
        else
        {
            OnAnimation(grayImageSoundsButton,orangeImageSoundsButton, soundBtnPositionOn);
            _soundsIsEnabled = true;
            soundManager.soundsAudioSourse.mute = false;
        }
        soundManager.ChangeButtonState(_soundsIsEnabled);
    }
}
