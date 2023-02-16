using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SaveGameManager : MonoBehaviour
{
    public Board board;
    public SaveDataStorage SaveDataStorage;
    private string key = "mysettings";

    public void SettingsSaver()
    {
        SaveDataStorage.list = board.list;
        SaveDataStorage.activePieceRotation = board.activePiece.rotationIndex;
        SaveDataStorage.nextPieceRotation = board.nextPieceStartRotation;
        SaveDataStorage.level = board.level;
        SaveDataStorage.score = board.score;
        string json = JsonUtility.ToJson(SaveDataStorage);
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();
        Debug.Log(json);
    }

    public bool SettingLoader()
    {
        if (PlayerPrefs.HasKey(key))
        {
            string json = PlayerPrefs.GetString(key);
            SaveDataStorage = JsonUtility.FromJson<SaveDataStorage>(json);
            Debug.Log(json);
            
            return true;
        }
        return false;
        
    }

    public bool isSaved()
    {
        if (PlayerPrefs.HasKey(key))
        {
            return true;
        }
        return false;
        
    }
    
    public void ResetData()
    {
        SaveDataStorage.level = 0;
        SaveDataStorage.score = 0;
        string json = JsonUtility.ToJson(SaveDataStorage);
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();
        SettingLoader();
    }
}