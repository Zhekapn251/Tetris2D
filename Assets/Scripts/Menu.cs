using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] Board board;
    [SerializeField] SaveGameManager SaveGameManager;
    [SerializeField] private Button soundsBtn;
    [SerializeField] private Button speedBtn;
    [SerializeField] private Button saveBtn;
    [SerializeField] private Button quitBtn;
    [SerializeField] private Button exitBtn;
    [SerializeField] private SoundSettings _soundSettings;
    [SerializeField] private SpeedSettings _speedSettings;
    [SerializeField] private ExitDialog _exitDialog;
    
    private void Start()
    {
        soundsBtn.onClick.AddListener(_soundSettings.SoundsMenuOn);
        speedBtn.onClick.AddListener(_speedSettings.SpeedMenuOn);
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
        SaveGameManager.SaveGame();
    }

    private void ConfirmExit()
    {
        _exitDialog.gameObject.SetActive(true);
    }

    private void ExitMenu()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
            _exitDialog.CancelExit();
            board.AllowStepping(true);
            //Time.timeScale = 1;
        }
    }
}
