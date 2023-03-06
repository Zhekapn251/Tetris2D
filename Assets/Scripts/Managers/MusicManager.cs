using UnityEngine;

public class MusicManager : MonoBehaviour
{ 
    [SerializeField] private SaveGameManager saveGameManager;
    
    public AudioSource musicAudioSource;
    public bool musicOn;
    public float musicVolume;
    
    void Awake()
    {
        musicAudioSource = GetComponent<AudioSource>();
        if (musicAudioSource == null)
        {
            Debug.LogError("MusicManager:: AudioSource is null");
        }
    }
    
    public void SetVolume(float volume)
    {
        musicAudioSource.volume = volume;
        musicVolume = volume;
        saveGameManager.SaveAudioSettings();
    }

    public void ChangeButtonState(bool buttonState)
    {
        musicOn = buttonState;
        saveGameManager.SaveAudioSettings();
    }
}
