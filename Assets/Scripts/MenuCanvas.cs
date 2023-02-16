using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuCanvas : MonoBehaviour
{
    [SerializeField] private Board board;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button pauseBtn;
    [SerializeField] private Menu menu;
    [SerializeField] private Image progressBarFill;
    [SerializeField] private LevelManager levelManager;

    private void Start()
    {
        pauseBtn.onClick.AddListener(PauseGame);
        menuButton.onClick.AddListener(OpenMainMenu);
        progressBarFill.fillAmount = levelManager.ProgressBarInit();
    }

    private void OpenMainMenu()
    {
        menu.OpenMainMenu();
    }

    private void PauseGame()
    {
        board.AllowStepping(!board.allowStepping);
    }
}
