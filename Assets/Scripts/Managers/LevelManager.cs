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

    private void Start()
    {
        Debug.Log("On Start LevelManager: lines = "+lines);
        Debug.Log("On Start LevelManager:levelGoal = "+levelGoal);
    }

    public void GoalsGenerator()
    {
        lines = 0;
        GoalsBarClear();
        GenerateGoalsDataForLevel();
        GenerateObstacles(amountOfGeneratedObstacles);
    }

    private void GenerateGoalsDataForLevel()
    {
        if (board.level < 5)
        {
            minGoalCount = 2;
            maxGoalCount = 5;
            amountOfGeneratedObstacles = 0;
        }
        else if ( board.level < 10)
        {
            minGoalCount = 6;
            maxGoalCount = 12;
            amountOfGeneratedObstacles = 2;
        }
        else if (board.level < 15)
        {
            minGoalCount = 13;
            maxGoalCount = 20;
            amountOfGeneratedObstacles = 5;
        }
        else
        {
            minGoalCount = 21;
            maxGoalCount = 35;
            amountOfGeneratedObstacles = 10;
        }
        levelGoal = Random.Range(minGoalCount, maxGoalCount);
        
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
        FireBaseInit.instance.FirebaseStartLevel(board.level);
        board.UpdateTextLevelOnUI();
        _soundManager.PlaySound(Sounds.Win);
        _saveGameManager.SavePlayersSettings(false);
        _fireworks.FireworksEnable();
        adsInitializer.ShowAds = true;
    }

    private bool GoalsIsReached()
    {
        bool goalIsReached = lines >= levelGoal;
        return goalIsReached;
    }

}
