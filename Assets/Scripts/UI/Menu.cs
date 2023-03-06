using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] Board board;
    [SerializeField] SaveGameManager saveGameManager;
    [SerializeField] private Button soundsBtn;
    [SerializeField] private Button speedBtn;
    [SerializeField] private Button saveBtn;
    [SerializeField] private Button quitBtn;
    [SerializeField] private Button exitBtn;
    [SerializeField] private SoundSettings soundSettings;
    [SerializeField] private SpeedSettings speedSettings;
    [SerializeField] private ExitDialog exitDialog;
    
    private void Start()
    {
        soundsBtn.onClick.AddListener(soundSettings.SoundsMenuOn);
        speedBtn.onClick.AddListener(speedSettings.SpeedMenuOn);
        saveBtn.onClick.AddListener(SaveButtonClicked);
        quitBtn.onClick.AddListener(ConfirmExit);
        exitBtn.onClick.AddListener(ExitMenu);
    }
    
    public void OpenMainMenu()
    {
        if (gameObject.activeInHierarchy == false)
        {
            board.AllowStepping(false);
            gameObject.SetActive(true);
        }
    }
    private void SaveButtonClicked()
    {
        saveGameManager.SaveGame();
    }

    private void ConfirmExit()
    {
        exitDialog.gameObject.SetActive(true);
    }

    private void ExitMenu()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
            exitDialog.CancelExit();
            board.AllowStepping(true);
        }
    }
}
