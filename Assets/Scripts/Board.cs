using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }
    public NextPiece nextactivePiece { get; private set; }
    public TetrominoData[] tetrominoes;
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public Vector3Int spawnPosition = new Vector3Int(-1, 8, 0);
    public Vector3Int spawnPositionOfNextPiece = new Vector3Int(7, 7, 0);
    public int randomNextPiece=-1;
    int random;
    public Tile testtile;
    public TMP_Text text;
    int score = 0;

    public TMP_Text levelText;
    int levelValue = 0;

    public int startRotation;
    public int nextPieceStartRotation;
    
    
    

    public RectInt Bounds
    {
        get
        {
           Vector2Int position = new Vector2Int(-this.boardSize.x/2,-this.boardSize.y/2);
           return new RectInt(position, this.boardSize);
        }
    }


    private void Awake()
    {
        
        startRotation = UnityEngine.Random.Range(0, 4);
        nextPieceStartRotation = UnityEngine.Random.Range(0, 4);
        tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();
        this.nextactivePiece = GetComponentInChildren<NextPiece>();
        
        
        for (int i=0; i<this.tetrominoes.Length; i++)
        {
            this.tetrominoes[i].Initialize();
        }
        Debug.Log("Awake() ---- OK");
    }
    
    
    private void Start()
    {
        PrintLevel(levelValue);
        UpdateScore(score);
        SpawnPiece();
        Debug.Log("Start() ---- OK");
        
        
    }

    

    public void SpawnPiece()
    {
        
        if(randomNextPiece == -1)
        {
            random = Random.Range(0, this.tetrominoes.Length);
        }
        randomNextPiece = Random.Range(0, this.tetrominoes.Length);
        TetrominoData data = this.tetrominoes[random];
        TetrominoData nextPiecedata= this.tetrominoes[randomNextPiece];
        
        random=randomNextPiece;
        
        activePiece.Initialize(this, spawnPosition, data);
        nextactivePiece.Initialize(this, this.spawnPositionOfNextPiece,  nextPiecedata);
        startRotation = nextPieceStartRotation;
        nextPieceStartRotation = UnityEngine.Random.Range(0, 4);
        if (IsValidPOsition(this.activePiece, activePiece.position))
        {
            Set(this.activePiece);
            SetNext(nextactivePiece);
        }
        else{
            
            GameOver();
            EraseScore();
        }
    }

     private void GameOver()
     {
        this.tilemap.ClearAllTiles();
     }

    public void Set(Piece piece)
    {
        for (int i=0; i<piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i]+piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void Clear(Piece piece)
    {
        for (int i=0; i<piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i]+piece.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    public bool  IsValidPOsition(Piece piece, Vector3Int position)
    {
        
        RectInt bounds = this.Bounds;
        for( int i=0; i<piece.cells.Length;i++)
        {
            Vector3Int tilePosition = piece.cells[i]+position;
            if(this.tilemap.HasTile(tilePosition))
            {
                Debug.Log("has tile");
                return false;
            }
            if(!bounds.Contains((Vector2Int)tilePosition))
            {
                Debug.Log("bounds not conain" + tilePosition);
                return false;
            }
        }
        return true;
    }

    public void ClearLines()
    {
        RectInt bounds = this.Bounds;
        int row= bounds.yMin;
        while (row<bounds.yMax)
        {
            if(IsLineFull(row))
            {
                LineClear(row);
                score++;
                UpdateScore(score);
            }
            else {
                row++;
            }
        }

    }

    public void LineClear(int row)
    {
        RectInt bounds = Bounds;

        // Clear all tiles in the row
        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            tilemap.SetTile(position, null);
        }

        // Shift every row above down one
        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                tilemap.SetTile(position, above);
            }

            row++;
        }
    }

    bool IsLineFull(int row)
    {
        RectInt bounds = this.Bounds;

        for(int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            if(!this.tilemap.HasTile(position))
            {
                return false;
            }
        }
        return true;
    }
/****************************************************************************************/
public void SetNext(NextPiece piece)
    {
        NextClear(piece);
        for (int i=0; i<piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i]+piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }
public void NextClear(NextPiece piece)
    {
        for (int i=0; i<piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i]+piece.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    void UpdateScore(int score)
    {
       text.text = "Score: " + score.ToString();
    }
    void EraseScore()
    {
        score = 0;
        UpdateScore(score);
    }
    void PrintLevel(int levelValue)
    {
        levelText.text = "Level : "+ levelValue.ToString();
    }
}
