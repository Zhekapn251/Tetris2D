using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class PlayerSettingsStorage
{
    public int score = 0;
    public int level = 1;
    public bool isSaved = false;
    public float speed = 1f;
    public int lines = 0;
    public int levelGoal = 5;

}