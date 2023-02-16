using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SquareGoalItem : MonoBehaviour
{
    public Image ImaGoalType;
    public TMP_Text text;
    public Image GreenMark;
    public Goal[] NameAndSpriteOfGoal;
    public GoalsType goalsType;
    public void SquareInit(GoalsType goalType)
    {
        goalsType = goalType;
        GreenMark.enabled=false;
        text.text = "0";
        Goal goal = NameAndSpriteOfGoal.First(i=>i.typeOfGoal==goalType);
        ImaGoalType.sprite = goal.goalsSprite;
    }
}
