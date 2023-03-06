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
    
    private Color _pausedBtnColor = new Color(0.82f, 0.47f, 0.47f, 1f);
    private Color _unPausedBtnColor = Color.white;
    private ColorBlock _pauseBtnColors;

    private void Start()
    {
        _pauseBtnColors = pauseBtn.colors;
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
        ChangePauseBtnColor(!board.allowStepping);
    }

    private void ChangePauseBtnColor(bool isPressed)
    {
        if (isPressed)
        {
            _pauseBtnColors.selectedColor = _pausedBtnColor;
            pauseBtn.colors = _pauseBtnColors;
        }
        else
        {
            _pauseBtnColors.selectedColor = _unPausedBtnColor;
            pauseBtn.colors = _pauseBtnColors;
        }
    }
}