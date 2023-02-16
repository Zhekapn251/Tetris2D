using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class SaveDataStorage
{
    // Start is called before the first frame update


    public List<int> list;
    public int activePieceRotation;
    public int nextPieceRotation;
    public int score;
    public int level;
    public string userName;

}

