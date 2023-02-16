using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SaveGameManager : MonoBehaviour
{
    public Board board;
    public AudioSettings AudioSettings;
    public SaveDataStorage SaveDataStorage;
    public SavePlayerSettings SavePlayerSettings;
    public SoundManager _SoundManager;
    public MusicManager _MusicManager;
    private string mySettingsKey = "mysettings";
    private string playerSettingsKey = "playersettings";
    private string audioSettingsKey = "audioSettings";
    public  bool isSaved { get; set; }
    private void Start()
    {
        
    }
    public void SaveGame()
    {
        board.list.Clear();
        board.SaveCurrentPiece();
        board.SaveNextPiece();
        board.SaveBoardPixels();
        SaveSettings();
        SavePlayersSettings(true);
        Debug.Log("SaveButton Clicked");
    }
    
    public void SaveSettings()
    {
        
        SaveDataStorage.list = board.list;
        SaveDataStorage.activePieceRotation = board.activePiece.rotationIndex;
        SaveDataStorage.nextPieceRotation = board.nextPieceStartRotation;
        
        string json = JsonUtility.ToJson(SaveDataStorage);
        PlayerPrefs.SetString(mySettingsKey, json);
        PlayerPrefs.Save();
        Debug.Log(json);
    }
    public void SavePlayersSettings(bool saveGame)
    {
        SavePlayerSettings.isSaved = saveGame; 
        SavePlayerSettings.level = board.level;
        SavePlayerSettings.score = board.score;
        SavePlayerSettings.speed = board.stepSpeed;

        string json = JsonUtility.ToJson(SavePlayerSettings);
        PlayerPrefs.SetString(playerSettingsKey, json);
        PlayerPrefs.Save();
        Debug.Log(json);
    }

    public void SaveAudioSettings()
    {
        AudioSettings.soundsOn = _SoundManager.SoundsOn;
        AudioSettings.soundsVolume = _SoundManager.SoundsVolume;
        AudioSettings.musicOn = _MusicManager.MusicOn;
        AudioSettings.musicVolume = _MusicManager.MusicVolume;
        
        string json = JsonUtility.ToJson(AudioSettings);
        PlayerPrefs.SetString(audioSettingsKey, json);
        PlayerPrefs.Save();
    }
    public void LoadData()
    {
        SettingLoader();
        board.list = SaveDataStorage.list;
        board.activePiece.rotationIndex = SaveDataStorage.activePieceRotation;
        board.nextPieceStartRotation = SaveDataStorage.nextPieceRotation;
    }
    private bool SettingLoader()
    {
        if (PlayerPrefs.HasKey(mySettingsKey))
        {
            string json = PlayerPrefs.GetString(mySettingsKey);
            SaveDataStorage = JsonUtility.FromJson<SaveDataStorage>(json);
            Debug.Log(json);
            
            return true;
        }
        SaveDataStorage = new SaveDataStorage();
        return false;
    }

    public void LoadPlayerData()
    {
        PlayerSettingLoader();
        isSaved = SavePlayerSettings.isSaved;
        board.level = SavePlayerSettings.level;
        board.score = SavePlayerSettings.score;
        board.stepSpeed = SavePlayerSettings.speed;
        Debug.Log("loaded speed = "+board.stepSpeed);
    }
    private bool PlayerSettingLoader()
    {
        if (PlayerPrefs.HasKey(playerSettingsKey))
        {
            string json = PlayerPrefs.GetString(playerSettingsKey);
            SavePlayerSettings = JsonUtility.FromJson<SavePlayerSettings>(json);
            Debug.Log(json);
            
            return true;
        }
        SavePlayerSettings = new SavePlayerSettings();
        return false;
        
    }
    
    public void LoadAudioData()
    {
        AudioSettingLoader();
        _SoundManager.SoundsOn = AudioSettings.soundsOn;
        _SoundManager.SoundsVolume = AudioSettings.soundsVolume;
        _MusicManager.MusicOn = AudioSettings.musicOn;
        _MusicManager.MusicVolume = AudioSettings.musicVolume;
    }
    
    private bool AudioSettingLoader()
    {
        if (PlayerPrefs.HasKey(audioSettingsKey))
        {
            string json = PlayerPrefs.GetString(audioSettingsKey);
            AudioSettings = JsonUtility.FromJson<AudioSettings>(json);
            return true;
        }
        AudioSettings = new AudioSettings();
        return false;
        
    }
    
    public void ResetData()
    {
        board.level = 1;
        board.score = 0;
        SavePlayersSettings(false);
    }
}