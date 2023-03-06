using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseGame : MonoBehaviour
{
    [SerializeField] private Board board;
    [SerializeField] private TMP_Text score;
    [SerializeField] private Button menuBtn;
    [SerializeField] private Button resetBtn;
    [SerializeField]private SaveGameManager saveGameManager;
    [SerializeField] private GameOverFade gameOverFade;
    [SerializeField] private Menu menu;
    
    private void Start()
    {
        menuBtn.onClick.AddListener(OpenMainMenu);
        resetBtn.onClick.AddListener(ResetGame);
    }

    private void OpenMainMenu()
    {
        menu.OpenMainMenu();
        HideLoseGame();
    }

    private void ResetGame()
    {    
        SceneManager.LoadScene(0);
        HideLoseGame();
    }
    public void ShowLoseGame()
    {
        gameObject.SetActive(true);
        score.text = board.score.ToString();
        saveGameManager.ResetData();
    }

    private void HideLoseGame()
    {
        gameObject.SetActive(false);
        board.StartGameRoutinesWithoutSaving();
        board.AllowStepping(true);
        gameOverFade.LooseUnFade();
    }

}
