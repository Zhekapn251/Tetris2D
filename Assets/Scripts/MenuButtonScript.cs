using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuButtonScript : MonoBehaviour
{
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] Board board;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button pauseBtn;

    private void Start()
    {
        EventsManager.OneMenuBtnClicked += OpenMainMenu;
        menuButton.onClick.AddListener(EventsManager.SendMenuBtnClicked);
        EventsManager.OnePauseBtnClicked += PauseGame;
        pauseBtn.onClick.AddListener(EventsManager.SendPauseBtnClicked);
    }

    public void OpenMainMenu()
    {
        if (menuCanvas.activeInHierarchy == false)
        {
            board.allowStepping = false;
            menuCanvas.SetActive(true);
        }
    }

    private void PauseGame()
    {
        board.allowStepping = !board.allowStepping;
    }
    public void Reload()
    {
        SceneManager.LoadScene(0);
    }

    private void OnDestroy()
    {
        EventsManager.OneMenuBtnClicked -= OpenMainMenu;
        EventsManager.OnePauseBtnClicked -= PauseGame;
    }
}
