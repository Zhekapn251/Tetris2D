using UnityEngine.Tilemaps;
using UnityEngine;
public enum Tetromino    
    {
        I,
        O,
        T,
        J,
        L,
        S,
        Z,
        M,
    }

[System.Serializable]
public struct TetrominoData
{
    public Tetromino tetromino;
    public Tile tile;
    public Vector2Int[] cells { get; set; }
    public Vector2Int[,] wallCollisions { get; set; }

    public void Initialize()
    {
        this.cells = Data.Cells[this.tetromino];
        this.wallCollisions = Data.WallKicks[this.tetromino];
    }
}

