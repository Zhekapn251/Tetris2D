using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Image grayImageMusicButton;
    [SerializeField] private Image orangeImageMusicButton;
    [SerializeField] private Slider musicSlider;
    private bool musicIsEnabled;
    private float musicVolume;
    [SerializeField] Image grayImageSoundsButton;
    [SerializeField] private Image orangeImageSoundsButton;
    [SerializeField] private Slider soundsSlider;
    private bool soundsIsEnabled;
    private float soundsVolume;

    private void Start()
    {
        if (orangeImageMusicButton.color.a < 0.5f)
        {
            musicIsEnabled = false;
        }
        else
        {
            musicIsEnabled = true;
        }
        
        if (orangeImageSoundsButton.color.a < 0.5f)
        {
            soundsIsEnabled = false;
        }
        else
        {
            soundsIsEnabled = true;
        }
    }

    private void OnAnimation(Image bottom, Image top)
    {
        Debug.Log("start animation on");
        bottom.transform.DOMoveX(bottom.transform.position.x + 70, 0.3f);
        top.DOFade(1, 0.3f);
    }

    private void OffAnimation(Image bottom, Image top)
    {
        Debug.Log("start animation off");
        bottom.transform.DOMoveX(bottom.transform.position.x - 70, 0.3f);
        top.DOFade(0, 0.3f);
    }
    
    public void MusicSlider()
    {
        musicVolume = musicSlider.value;
    }
    public void SoundsSlider()
    {
        soundsVolume = soundsSlider.value;
    }
    
    public void MusicButton()
    {
        if (musicIsEnabled)
        {
            OffAnimation(grayImageMusicButton,orangeImageMusicButton);
            musicIsEnabled = false;
        }
        else
        {
            OnAnimation(grayImageMusicButton,orangeImageMusicButton);
            musicIsEnabled = true;
        }
    }
    
    public void SoundsButton()  
    {
        if (soundsIsEnabled)
        {
            OffAnimation(grayImageSoundsButton,orangeImageSoundsButton);
            soundsIsEnabled = false;
        }
        else
        {
            OnAnimation(grayImageSoundsButton,orangeImageSoundsButton);
            soundsIsEnabled = true;
        }
    }
}
