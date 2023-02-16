using System;
using UnityEngine;

[Serializable]
public class SaveMyGame
{
    // Start is called before the first frame update


    public string stringToSave = "";
    public int score = 0;
    public int level = 0;


public void SaveGame()
{
    PlayerPrefs.SetInt("SavedScore", score);
    PlayerPrefs.SetInt("SavedLevel", level);
    PlayerPrefs.SetString("SavedString", stringToSave);
    PlayerPrefs.Save();
    Debug.Log("Game data saved!");
}

public void LoadGame()
{
    if (PlayerPrefs.HasKey("SavedInteger"))
    {
        score = PlayerPrefs.GetInt("SavedScore");
        level = PlayerPrefs.GetInt("SavedLevel");
        stringToSave = PlayerPrefs.GetString("SavedString");
        Debug.Log("Game data loaded!");
    }
    else
    {
        Debug.Log("There is no save data!");
        score = 3;
        level = 4;
        stringToSave = "12345";
        PlayerPrefs.SetInt("SavedScore", score);
        PlayerPrefs.SetInt("SavedLevel", level);
        PlayerPrefs.SetString("SavedString", stringToSave);
        PlayerPrefs.Save();
        Debug.Log("Game data saved!");
    }

}
public void ResetData()
{
    PlayerPrefs.DeleteAll();
    score = 0;
    level = 0;
    stringToSave = "";
    Debug.Log("Data reset complete");
}

}
