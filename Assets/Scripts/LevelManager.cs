using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private AdsInitializer adsInitializer;
    [SerializeField] SoundManager _soundManager;
    [SerializeField] private SaveGameManager _saveGameManager;
    [SerializeField] private Image progressBar;
    [SerializeField] private Fireworks _fireworks;
    
    private int minGoalCount = 3;
    private int maxGoalCount = 10;
    private int amountOfGeneratedObstacles;
    public int levelGoal;
    public int lines;
    public Board board;
    
    
    public void GoalsGenerator()
    {
        lines = 0;
        GenerateGoalsForLevel();
        GoalsBarClear();
        GenerateObstacles(amountOfGeneratedObstacles);
    }

    private void GenerateGoalsForLevel()
    {
        levelGoal = Random.Range(minGoalCount, maxGoalCount);
        //goalsIncrement = (float)1/(levelGoal);
    }
    
    private void GenerateObstacles(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            board.GenerateObstacle();
        }
    }
    private void GoalsBarClear()
    {
        progressBar.fillAmount = 0f;
    }

    public void UpdateGoalsLines()
    {
        lines++;
        var value = (float)lines/levelGoal;
        AnimateProgressBar(value);
        if (GoalsIsReached())
        {
            StartWinSequence();
        }
    }

    public float ProgressBarInit()
    {
        float progressBarAmount;
        return progressBarAmount = (float)lines / levelGoal;
    }
    private void AnimateProgressBar(float goals)
    {
        DOTween.To(() => progressBar.fillAmount, value => progressBar.fillAmount = value, goals, 1);
    }

    private void StartWinSequence()
    {

        board.level++;
        board.AllowStepping(false);
        FireBaseInit.instance.FirebaseStartLevel(board.level);
        board.UpdateTextLevelOnUI();
        _soundManager.PlaySound(Sounds.Win);
        _saveGameManager.SavePlayersSettings(false);
        _fireworks.FireworksEnable();
        adsInitializer.ShowAds = true;
    }

    private bool GoalsIsReached()
    {
        bool goalIsReached = lines == levelGoal;
        return goalIsReached;
    }

}
