using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource musicAudioSourse;
    public bool MusicOn;
    public float MusicVolume;
    [SerializeField] private SaveGameManager _saveGameManager;
    void Awake()
    {
        musicAudioSourse = GetComponent<AudioSource>();
        if (musicAudioSourse == null)
        {
            Debug.LogError("MusicManager:: AudioSource is null");
        }
        
    }
    
    public void SetVolume(float volume)
    {
        musicAudioSourse.volume = volume;
        MusicVolume = volume;
        _saveGameManager.SaveAudioSettings();
    }

    public void ChangeButtonState(bool buttonState)
    {
        MusicOn = buttonState;
        _saveGameManager.SaveAudioSettings();
    }
}
