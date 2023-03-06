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
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private SaveGameManager saveGameManager;
    [SerializeField] private Image progressBar;
    [SerializeField] private Fireworks fireworks;
    
    private int _minGoalCount = 3;
    private int _maxGoalCount = 10;
    private int _amountOfGeneratedObstacles;
    
    public int levelGoal;
    public int lines;
    public Board board;
    
    public void GoalsGenerator()
    {
        lines = 0;
        GoalsBarClear();
        GenerateGoalsDataForLevel();
        GenerateObstacles(_amountOfGeneratedObstacles);
    }

    private void GenerateGoalsDataForLevel()
    {
        if (board.level < 5)
        {
            _minGoalCount = 2;
            _maxGoalCount = 5;
            _amountOfGeneratedObstacles = 0;
        }
        else if ( board.level < 10)
        {
            _minGoalCount = 6;
            _maxGoalCount = 12;
            _amountOfGeneratedObstacles = 2;
        }
        else if (board.level < 15)
        {
            _minGoalCount = 13;
            _maxGoalCount = 20;
            _amountOfGeneratedObstacles = 5;
        }
        else
        {
            _minGoalCount = 21;
            _maxGoalCount = 35;
            _amountOfGeneratedObstacles = 10;
        }
        levelGoal = Random.Range(_minGoalCount, _maxGoalCount);
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
        return (float)lines / levelGoal;
        
    }
    private void AnimateProgressBar(float goals)
    {
        DOTween.To(() => progressBar.fillAmount, value => progressBar.fillAmount = value, goals, 1);
    }

    private void StartWinSequence()
    {
        board.level++;
        board.AllowStepping(false);
        FireBaseInit.Instance.FirebaseStartLevel(board.level);
        board.UpdateTextLevelOnUI();
        soundManager.PlaySound(Sounds.Win);
        saveGameManager.SavePlayersSettings(false);
        fireworks.FireworksEnable();
        adsInitializer.showAds = true;
    }

    private bool GoalsIsReached()
    {
        bool goalIsReached = lines >= levelGoal;
        return goalIsReached;
    }

}
