using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SaveGameManager : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    
    private string _mySettingsKey = "mysettings";
    private string _playerSettingsKey = "playersettings";
    private string _audioSettingsKey = "audioSettings";
    
    public Board board;
    public AudioSettingsStorage audioSettingsStorage;
    public TilesDataStorage tilesDataStorage;
    public PlayerSettingsStorage playerSettingsStorage;
    public SoundManager soundManager;
    public MusicManager musicManager;
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
        
        tilesDataStorage.list = board.listOfAllTilesToSave;
        tilesDataStorage.activePieceRotation = board.ActivePieceInitialRotation; //activePiece.rotationIndex
        tilesDataStorage.nextPieceRotation = board.nextPieceStartRotation;
        
        string json = JsonUtility.ToJson(tilesDataStorage);
        PlayerPrefs.SetString(_mySettingsKey, json);
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
        PlayerPrefs.SetString(_playerSettingsKey, json);
        PlayerPrefs.Save();
    }

    public void SaveAudioSettings()
    {
        audioSettingsStorage.soundsOn = soundManager.soundsOn;
        audioSettingsStorage.soundsVolume = soundManager.soundsVolume;
        audioSettingsStorage.musicOn = musicManager.musicOn;
        audioSettingsStorage.musicVolume = musicManager.musicVolume;
        
        string json = JsonUtility.ToJson(audioSettingsStorage);
        PlayerPrefs.SetString(_audioSettingsKey, json);
        PlayerPrefs.Save();
        
    }
    public void LoadData()
    {
        SettingLoader();
        board.listOfAllTilesToSave = tilesDataStorage.list;
        board.ActivePieceInitialRotation = tilesDataStorage.activePieceRotation;
        board.nextPieceStartRotation = tilesDataStorage.nextPieceRotation;
    }
    private bool SettingLoader()
    {
        if (PlayerPrefs.HasKey(_mySettingsKey))
        {
            string json = PlayerPrefs.GetString(_mySettingsKey);
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
        if (PlayerPrefs.HasKey(_playerSettingsKey))
        {
            string json = PlayerPrefs.GetString(_playerSettingsKey);
            playerSettingsStorage = JsonUtility.FromJson<PlayerSettingsStorage>(json);
            return true;
        }
        playerSettingsStorage = new PlayerSettingsStorage();
        return false;
        
    }
    
    public void LoadAudioData()
    {
        AudioSettingLoader();
        soundManager.soundsOn = audioSettingsStorage.soundsOn;
        soundManager.soundsVolume = audioSettingsStorage.soundsVolume;
        musicManager.musicOn = audioSettingsStorage.musicOn;
        musicManager.musicVolume = audioSettingsStorage.musicVolume;
    }
    
    private bool AudioSettingLoader()
    {
        if (PlayerPrefs.HasKey(_audioSettingsKey))
        {
            string json = PlayerPrefs.GetString(_audioSettingsKey);
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