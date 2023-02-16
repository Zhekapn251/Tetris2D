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
    public Board board;
    //public Dictionary<GoalsType,GoalsStructure> goalsStructures = new Dictionary<GoalsType, GoalsStructure>();
    private int minGoalCount = 3;
    private int maxGoalCount = 10;
    private int amountOfGeneratedObstacles;
    private int levalGoal;
    private float goalsIncrement;
    private int lines;
    [SerializeField] private Image progressBar;
    [SerializeField] private LoseGame _loseGame;
    [SerializeField] private WinGame _winGame;
    [SerializeField] private Fireworks _fireworks;
    
    public void GoalsGenerator()
    {
        lines = 0;
        GenerateGoalsForLevel();
        GoalsBarClear();
        GenerateObstacles(amountOfGeneratedObstacles);
    }

    private void GenerateGoalsForLevel()
    {
        levalGoal = Random.Range(minGoalCount, maxGoalCount);
        goalsIncrement = (float)1/(levalGoal);
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
        var value = (float)lines/levalGoal;
        AnimateProgressBar(value);
        if (GoalsIsReached())
        {
            StartWinSequence();
        }
    }

    private void AnimateProgressBar(float goals)
    {
        DOTween.To(() => progressBar.fillAmount, value => progressBar.fillAmount = value, goals, 1);
    }

    private void StartWinSequence()
    {

        board.level++;
        board.allowStepping = false;
        FireBaseInit.instance.FirebaseStartLevel(board.level);
        board.textLevel.text = board.level.ToString();
        _soundManager.PlaySound(Sounds.Win);
        _saveGameManager.SavePlayersSettings(false);
        _fireworks.FireworksEnable();
        adsInitializer.ShowAds = true;
    }

    private bool GoalsIsReached()
    {
        bool goalIsReached = lines == levalGoal;
        return goalIsReached;
    }

}
