using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SaveGameManager : MonoBehaviour
{
    public Board board;
    public AudioSettings audioSettings;
    public SaveDataStorage saveDataStorage;
    public SavePlayerSettings savePlayerSettings;
    public SoundManager soundManager;
    public MusicManager musicManager;
    [SerializeField] private LevelManager levelManager;
    private string mySettingsKey = "mysettings";
    private string playerSettingsKey = "playersettings";
    private string audioSettingsKey = "audioSettings";
    public  bool isSaved { get; set; }
    private void Start()
    {
        SetOrthoSize((float)Screen.height / Screen.width);
    }

    private void SetOrthoSize(float aspectRatio)
    {
        if(aspectRatio < 1.8f)
            Camera.main.orthographicSize = 16.4f;
        else if(aspectRatio < 2.0f)
            Camera.main.orthographicSize = 17.9f;
        else if(aspectRatio < 2.1f)
            Camera.main.orthographicSize = 19f;
        else if(aspectRatio < 2.3f)
            Camera.main.orthographicSize = 20f;
        else
        {
            Camera.main.orthographicSize = 21f;
        }
    }

    public void SaveGame()
    {
        board.listOfAllTilesToSave.Clear();
        board.SaveCurrentPiece();
        board.SaveNextPiece();
        board.SaveBoardPixels();
        SaveSettings();
        SavePlayersSettings(true);
    }
    
    public void SaveSettings()
    {
        
        saveDataStorage.list = board.listOfAllTilesToSave;
        saveDataStorage.activePieceRotation = board.activePiece.rotationIndex;
        saveDataStorage.nextPieceRotation = board.nextPieceStartRotation;
        
        string json = JsonUtility.ToJson(saveDataStorage);
        PlayerPrefs.SetString(mySettingsKey, json);
        PlayerPrefs.Save();
        Debug.Log(json);
    }
    public void SavePlayersSettings(bool saveGame)
    {
        savePlayerSettings.isSaved = saveGame; 
        savePlayerSettings.level = board.level;
        savePlayerSettings.score = board.score;
        savePlayerSettings.speed = board.stepSpeed;
        savePlayerSettings.lines = levelManager.lines;
        savePlayerSettings.levelGoal = levelManager.levelGoal;
        string json = JsonUtility.ToJson(savePlayerSettings);
        PlayerPrefs.SetString(playerSettingsKey, json);
        PlayerPrefs.Save();
    }

    public void SaveAudioSettings()
    {
        audioSettings.soundsOn = soundManager.SoundsOn;
        audioSettings.soundsVolume = soundManager.SoundsVolume;
        audioSettings.musicOn = musicManager.MusicOn;
        audioSettings.musicVolume = musicManager.MusicVolume;
        
        string json = JsonUtility.ToJson(audioSettings);
        PlayerPrefs.SetString(audioSettingsKey, json);
        PlayerPrefs.Save();
    }
    public void LoadData()
    {
        SettingLoader();
        board.listOfAllTilesToSave = saveDataStorage.list;
        board.activePiece.rotationIndex = saveDataStorage.activePieceRotation;
        board.nextPieceStartRotation = saveDataStorage.nextPieceRotation;
    }
    private bool SettingLoader()
    {
        if (PlayerPrefs.HasKey(mySettingsKey))
        {
            string json = PlayerPrefs.GetString(mySettingsKey);
            saveDataStorage = JsonUtility.FromJson<SaveDataStorage>(json);
            return true;
        }
        saveDataStorage = new SaveDataStorage();
        return false;
    }

    public void LoadPlayerData()
    {
        PlayerSettingLoader();
        isSaved = savePlayerSettings.isSaved;
        board.level = savePlayerSettings.level;
        board.score = savePlayerSettings.score;
        board.stepSpeed = savePlayerSettings.speed;
        levelManager.lines = savePlayerSettings.lines;
        levelManager.levelGoal = savePlayerSettings.levelGoal;
    }
    private bool PlayerSettingLoader()
    {
        if (PlayerPrefs.HasKey(playerSettingsKey))
        {
            string json = PlayerPrefs.GetString(playerSettingsKey);
            savePlayerSettings = JsonUtility.FromJson<SavePlayerSettings>(json);
            return true;
        }
        savePlayerSettings = new SavePlayerSettings();
        return false;
        
    }
    
    public void LoadAudioData()
    {
        AudioSettingLoader();
        soundManager.SoundsOn = audioSettings.soundsOn;
        soundManager.SoundsVolume = audioSettings.soundsVolume;
        musicManager.MusicOn = audioSettings.musicOn;
        musicManager.MusicVolume = audioSettings.musicVolume;
    }
    
    private bool AudioSettingLoader()
    {
        if (PlayerPrefs.HasKey(audioSettingsKey))
        {
            string json = PlayerPrefs.GetString(audioSettingsKey);
            audioSettings = JsonUtility.FromJson<AudioSettings>(json);
            return true;
        }
        audioSettings = new AudioSettings();
        return false;
    }
    public void ResetData()
    {
        board.level = 1;
        board.score = 0;
        SavePlayersSettings(false);
    }
}