using UnityEngine;


public class AudioManager : MonoBehaviour
{
 
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private SaveGameManager saveGameManager;

    private void Start()
    {
        saveGameManager.LoadAudioData();
        soundManager.SetVolume(soundManager.soundsVolume);
        musicManager.SetVolume(musicManager.musicVolume);
        soundManager.soundsAudioSourse.mute = !soundManager.soundsOn;
        musicManager.musicAudioSource.mute = !musicManager.musicOn;
    }

    
}
