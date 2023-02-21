using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinGame : MonoBehaviour
{
    [SerializeField] private Board board;
    [SerializeField] private TMP_Text score;
    [SerializeField] private Fireworks _fireworks;
    private float starsamount;

    [SerializeField] private Image filledStarsAmount;
    private void Start()
    {
        EventsManager.OneWinMenuBtnClicked += HideWinGame;
    }
    public void ShowWinGame()
    {
        board.AllowStepping(false);
        gameObject.SetActive(true);
        score.text = board.score.ToString();
        GetWinnerRate();
        ShowStars();
        ClearWinnerRate();
    }

    private void ClearWinnerRate()
    {
        starsamount = 0f;
    }

    private void ShowStars()
    {
        filledStarsAmount.fillAmount = starsamount;
    }

    private void GetWinnerRate()
    {
        starsamount = board.CalculateNumberOfStars();
    }
    public void HideWinGame()
    {
        gameObject.SetActive(false);
        _fireworks.FireWorksDisable();
        board.StartGameRoutinesWithoutSaving();
        board.AllowStepping(true);
    }
    public void MenuOnWinGame()
    {
        gameObject.SetActive(false);
        
    }
    private void OnDestroy()
    {
        EventsManager.OneWinMenuBtnClicked -= HideWinGame;
    }

}
