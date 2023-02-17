using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    // Start is called before the first frame update
    
    
    private bool musicIsEnabled;
    private float musicVolume;
    private bool soundsIsEnabled;
    private float soundsVolume;
    [SerializeField] private Image grayImageMusicButton;
    [SerializeField] private Image orangeImageMusicButton;
    [SerializeField] private Image grayImageSoundsButton;
    [SerializeField] private Image orangeImageSoundsButton;
    [SerializeField] public Slider soundsSlider;
    [SerializeField] public Slider musicSlider;
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private MusicManager _musicManager;
    [SerializeField] private Button soundsBtn;
    [SerializeField] private Button musicBtn;
    [SerializeField] private Button exitBtn;
    [SerializeField] private Transform musicBtnPositionOn;
    [SerializeField] private Transform musicBtnPositionOff;
    [SerializeField] private Transform soundBtnPositionOn;
    [SerializeField] private Transform soundBtnPositionOff;
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
        soundsIsEnabled = _soundManager.SoundsOn;
        musicIsEnabled = _musicManager.MusicOn;
        musicSlider.value = _musicManager.MusicVolume;
        soundsSlider.value = _soundManager.SoundsVolume;
        AudioUIInit();
    }
    private void AudioUIInit()
    {
        if (!musicIsEnabled) OffAnimation(grayImageMusicButton, orangeImageMusicButton, musicBtnPositionOff, 0f);
        if (!soundsIsEnabled) OffAnimation(grayImageSoundsButton, orangeImageSoundsButton, soundBtnPositionOff,0f);
    }
    private void OnAnimation(Image bottom, Image top, Transform targetPosition)
    {
        bottom.transform.DOMoveX(targetPosition.position.x, 0.3f);
        Debug.Log(bottom.transform.localPosition.x);
        top.DOFade(1, 0.3f);
    }
    
    private void OffAnimation(Image bottom, Image top, Transform targetPosition, float fadeSpeed = 0.3f)
    {
        bottom.transform.DOMoveX(targetPosition.position.x, fadeSpeed);
        Debug.Log(bottom.transform.localPosition.x);
        top.DOFade(0, 0.3f);
    }

    private void MusicSlider(float sliderValue)
    {
        musicVolume = sliderValue;
        _musicManager.SetVolume(musicVolume);
    }

    private void SoundsSlider(float sliderValue)
    {
        soundsVolume = sliderValue;
        _soundManager.SetVolume(soundsVolume);
    }
    
    public void MusicButtonCliked()
    {
        if (musicIsEnabled)
        {
            OffAnimation(grayImageMusicButton,orangeImageMusicButton, musicBtnPositionOff);
            musicIsEnabled = false;
            _musicManager.musicAudioSourse.mute = true;
        }
        else
        {
            OnAnimation(grayImageMusicButton,orangeImageMusicButton, musicBtnPositionOn);
            musicIsEnabled = true;
            _musicManager.musicAudioSourse.mute = false;
        }
        _musicManager.ChangeButtonState(musicIsEnabled);
    }
    
    public void SoundsButtonCliked()  
    {
        if (soundsIsEnabled)
        {
            OffAnimation(grayImageSoundsButton,orangeImageSoundsButton, soundBtnPositionOff);
            soundsIsEnabled = false;
            _soundManager.soundsAudioSourse.mute = true;
        }
        else
        {
            OnAnimation(grayImageSoundsButton,orangeImageSoundsButton, soundBtnPositionOn);
            soundsIsEnabled = true;
            _soundManager.soundsAudioSourse.mute = false;
        }
        _soundManager.ChangeButtonState(soundsIsEnabled);
    }
}
