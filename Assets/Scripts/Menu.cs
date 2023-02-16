using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] Board board;
    [SerializeField] SaveGameManager SaveGameManager;
    [SerializeField] GameObject soundMenu;
    [SerializeField] private GameObject speedMenu;
    [SerializeField] private GameObject exitDialog;
    public void SoundsMenuOn()
    {
        soundMenu.SetActive(true);
    }
    public void SoundsMenuOff()
    {
        soundMenu.SetActive(false);
    }
    
    public void SpeedMenuOn()
    {
        speedMenu.SetActive(true);
    }
    public void SpeedMenuOff()
    {
        speedMenu.SetActive(false);
    }
    
    public void SaveButtonClicked()
    {
        SaveGame();
    }

    public void ConfirmExit()
    {
        exitDialog.SetActive(true);
    }

    public void CancelingExit()
    {
        exitDialog.SetActive(false);
    }

    public void ConfirmingExit()
    {
        ExitGame();
    }
    void SaveGame()
    {
        board.list.Clear();
        board.SaveCurrentPiece();
        board.SaveNextPiece();
        board.SaveBoardPixels();
        SaveGameManager.SettingsSaver();
        Debug.Log("SaveButton Clicked");
        //SaveGameManager.
    }

    private void ExitGame()
    {
        Debug.Log("App_Quit");
        Application.Quit();
    }
}
