using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class SavePlayerSettings
{
    public int score = 0;
    public int level = 1;
    public bool isSaved = false;
    public float speed = 0.5f;
    
}