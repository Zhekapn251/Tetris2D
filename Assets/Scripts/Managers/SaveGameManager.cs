using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SaveGameManager : MonoBehaviour
{
    public Board board;
    public AudioSettingsStorage audioSettingsStorage;
    public TilesDataStorage tilesDataStorage;
    public PlayerSettingsStorage playerSettingsStorage;
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
        Debug.Log("Save game");
    }
    
    public void SaveSettings()
    {
        
        tilesDataStorage.list = board.listOfAllTilesToSave;
        tilesDataStorage.activePieceRotation = board.ActivePieceInitialRotation; //activePiece.rotationIndex
        tilesDataStorage.nextPieceRotation = board.nextPieceStartRotation;
        
        string json = JsonUtility.ToJson(tilesDataStorage);
        PlayerPrefs.SetString(mySettingsKey, json);
        PlayerPrefs.Save();
    }
    public void SavePlayersSettings(bool saveGame)
    {
        playerSettingsStorage.isSaved = saveGame; 
        playerSettingsStorage.level = board.level;
        playerSettingsStorage.score = board.score;
        playerSettingsStorage.speed = board.stepSpeed;
        playerSettingsStorage.lines = levelManager.lines;
        playerSettingsStorage.levelGoal = levelManager.levelGoal;
        string json = JsonUtility.ToJson(playerSettingsStorage);
        PlayerPrefs.SetString(playerSettingsKey, json);
        PlayerPrefs.Save();
    }

    public void SaveAudioSettings()
    {
        audioSettingsStorage.soundsOn = soundManager.SoundsOn;
        audioSettingsStorage.soundsVolume = soundManager.SoundsVolume;
        audioSettingsStorage.musicOn = musicManager.MusicOn;
        audioSettingsStorage.musicVolume = musicManager.MusicVolume;
        
        string json = JsonUtility.ToJson(audioSettingsStorage);
        PlayerPrefs.SetString(audioSettingsKey, json);
        PlayerPrefs.Save();
        
    }
    public void LoadData()
    {
        SettingLoader();
        board.listOfAllTilesToSave = tilesDataStorage.list;
        board.ActivePieceInitialRotation = tilesDataStorage.activePieceRotation; //activePiece.rotationIndex
        board.nextPieceStartRotation = tilesDataStorage.nextPieceRotation;
    }
    private bool SettingLoader()
    {
        if (PlayerPrefs.HasKey(mySettingsKey))
        {
            string json = PlayerPrefs.GetString(mySettingsKey);
            tilesDataStorage = JsonUtility.FromJson<TilesDataStorage>(json);
            return true;
        }
        tilesDataStorage = new TilesDataStorage();
        return false;
    }

    public void LoadPlayerData()
    {
        PlayerSettingLoader();
        isSaved = playerSettingsStorage.isSaved;
        board.level = playerSettingsStorage.level;
        board.score = playerSettingsStorage.score;
        board.stepSpeed = playerSettingsStorage.speed;
        levelManager.lines = playerSettingsStorage.lines;
        levelManager.levelGoal = playerSettingsStorage.levelGoal;
    }
    private bool PlayerSettingLoader()
    {
        if (PlayerPrefs.HasKey(playerSettingsKey))
        {
            string json = PlayerPrefs.GetString(playerSettingsKey);
            playerSettingsStorage = JsonUtility.FromJson<PlayerSettingsStorage>(json);
            return true;
        }
        playerSettingsStorage = new PlayerSettingsStorage();
        return false;
        
    }
    
    public void LoadAudioData()
    {
        AudioSettingLoader();
        soundManager.SoundsOn = audioSettingsStorage.soundsOn;
        soundManager.SoundsVolume = audioSettingsStorage.soundsVolume;
        musicManager.MusicOn = audioSettingsStorage.musicOn;
        musicManager.MusicVolume = audioSettingsStorage.musicVolume;
    }
    
    private bool AudioSettingLoader()
    {
        if (PlayerPrefs.HasKey(audioSettingsKey))
        {
            string json = PlayerPrefs.GetString(audioSettingsKey);
            audioSettingsStorage = JsonUtility.FromJson<AudioSettingsStorage>(json);
            return true;
        }
        audioSettingsStorage = new AudioSettingsStorage();
        return false;
    }
    public void ResetData()
    {
        board.level = 1;
        board.score = 0;
        SavePlayersSettings(false);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus) return;
        SaveGame();
    }
}