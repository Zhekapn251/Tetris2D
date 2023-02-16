using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    [SerializeField] SoundManager _soundManager;
    public Board board;
    public Dictionary<GoalsType,GoalsStructure> goalsStructures = new Dictionary<GoalsType, GoalsStructure>();
    private int minGoalCount = 1;
    private int maxGoalCount = 1;
    private int countOfGeneratedGoals = 3;
    private int amountOfGeneratedObstacles;
    public void GoalsGenerator()
    {
        ChooseHardnessOfLevel(board.level);

        GenerateGoalsForLevel();
        GoalsInit();
        GenerateObstacles(amountOfGeneratedObstacles);
    }

    private void GenerateGoalsForLevel()
    {
        for (int i = 0; i < countOfGeneratedGoals;)
        {
            GoalsStructure goalsStructure = new GoalsStructure();
            
            int randomGoalsStructure = Random.Range(0, Enum.GetNames(typeof(GoalsType)).Length);
            goalsStructure.GoalsType = (GoalsType)randomGoalsStructure;
            goalsStructure.goalCount = Random.Range(minGoalCount, maxGoalCount);
            bool containKey = goalsStructures.ContainsKey(goalsStructure.GoalsType);
            if (!containKey)
            {
                goalsStructures.Add((GoalsType)randomGoalsStructure, goalsStructure);
                i++;
            }
        }
    }

    private void ChooseHardnessOfLevel(int boardLevel)
    {
        if (boardLevel < 5)
        {
            minGoalCount = 2;
            maxGoalCount = 5;
            amountOfGeneratedObstacles = 0;
        }
        else if ( boardLevel < 10)
        {
            minGoalCount = 4;
            maxGoalCount = 8;
            amountOfGeneratedObstacles = 5;
        }
        else if (boardLevel < 15)
        {
            minGoalCount = 4;
            maxGoalCount = 8;
            amountOfGeneratedObstacles = 10;
        }
        else
        {
            minGoalCount = 10;
            maxGoalCount = 15;
            amountOfGeneratedObstacles = 15;
        }
    }

    private void GenerateObstacles(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            board.GenerateObstacle();
        }
    }
    private void GoalsInit()
    {
        int i = 0;
        foreach (KeyValuePair<GoalsType, GoalsStructure> entry in goalsStructures)
        {
            board.SquareGoalItems[i].SquareInit(entry.Key);

            i++;
        }
        UpdateGoalsValuesOnScreen();
    }

    private Array GetArrayOfKeys(Dictionary<GoalsType, GoalsStructure> dictionaryOfGoalsStructures)
    {
        GoalsType[] arrayOfAllKeys = dictionaryOfGoalsStructures.Keys.ToArray();

        return arrayOfAllKeys;
    }

    public void UpdateGoalsLines()
    {
        if (HasGoal(GoalsType.Lines)) ;
    }
    public void UpdateGoalsValues(List<string> tempLine)
    {
        //foreach (KeyValuePair<GoalsType, GoalsStructure> entry in goalsStructures)
        foreach (string item in tempLine)
        {
            GoalsType key;
            switch (item)
            {
                case "Red":
                    key = GoalsType.Red;
                    if (HasGoal(key)) ;
                    break;
                case "Yellow":
                    key = GoalsType.Yellow;
                    if (HasGoal(key)) ;
                    break;
                case "Green":
                    key = GoalsType.Green;
                    if (HasGoal(key)) ;
                    break;
                case "Cyan":
                    key = GoalsType.Cyan;
                    if (HasGoal(key)) ;
                    break;
                case "Blue":
                    key = GoalsType.Blue;
                    if (HasGoal(key)) ;
                    break;
                case "Orange":
                    key = GoalsType.Orange;
                    if (HasGoal(key)) ;
                    break;
                case "Purple":
                    key = GoalsType.Purple;
                    if (HasGoal(key)) ;
                    break;
            }
            
        }
        UpdateGoalsValuesOnScreen();
        if (GoalsIsReached())
        {
            StartWinSequence();
        }
        
    }

    private void StartWinSequence()
    {

        board.level++;
        board.textLevel.text = "Level: "+board.level;
        _soundManager.PlaySound(Sounds.Win);
        goalsStructures.Clear();
        board.tilemap.ClearAllTiles();
        GoalsGenerator();
    }

    private bool HasGoal(GoalsType key)
    {
        if (goalsStructures.ContainsKey(key))
        {
            //goalsStructures[key].currentCount++;
            if (!goalsStructures[key].goalIsReached)
            {
                goalsStructures[key].currentCount++;
                if (goalsStructures[key].currentCount >= goalsStructures[key].goalCount)
                {
                    goalsStructures[key].goalIsReached = true;
                    goalsStructures[key].currentCount = goalsStructures[key].goalCount;
                    SquareGoalItem item = board.SquareGoalItems.First(i => i.goalsType == key);
                    item.GreenMark.enabled = true;
                }
            }
        }
        return true;
    }

    private void UpdateGoalsValuesOnScreen()
    {
        
        foreach (var item in board.SquareGoalItems)
        {
            item.text.text = GetGoalsString(item.goalsType);
        }
        
    }

    private string GetGoalsString(GoalsType entryKey)
    {
        return goalsStructures[entryKey].currentCount+"/"+
            goalsStructures[entryKey].goalCount;
    }

    private bool GoalsIsReached()
    {
        bool goalIsReached = false;
        foreach (KeyValuePair<GoalsType, GoalsStructure> entry in goalsStructures)
        {
            if (entry.Value.currentCount >= entry.Value.goalCount)
            {
                entry.Value.goalIsReached = true;
                goalIsReached = true;
            }
            else
            {
                goalIsReached = false;
                break;
            }
        }
        return goalIsReached;
    }
       
}
