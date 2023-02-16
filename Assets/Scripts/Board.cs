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
    //public Piece nextactivePiece;
    public Tile testtile;
    public TMP_Text text;
    int score = 0;

    public TMP_Text levelText;
    int levelValue = 0;

    public int startRotation;
    

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
        //Tilemap [] tileMaps = GetComponentsInChildren<Tilemap>();
        //this.tilemap = tileMaps[0];
        //nexttilemap = tileMaps[1];
        //Debug.Log("number of tilemaps:"+ tileMaps.Length+"  first: "+tileMaps[0].name+"  second: "+tileMaps[1].name);
        tilemap = GetComponentInChildren<Tilemap>();
        Debug.Log("tilemap = GetComponentInChildren<Tilemap> ---- OK");
        this.activePiece = GetComponentInChildren<Piece>();
        this.nextactivePiece = GetComponentInChildren<NextPiece>();
        Debug.Log("activePiece = GetComponentInChildren<Piece> ---- OK");
        //this.nextactivePiece = GetComponentInChildren<Piece>();
        
        
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
        //testtile.color = Color.green;
        //tilemap.SetTile(new Vector3Int(0,0,0), testtile);
         Debug.Log("Start() ---- OK");
        
        
    }

    

    public void SpawnPiece()
    {
        startRotation = 1;///Random.Range(0, 4);
        if(randomNextPiece == -1)
        {
            random = Random.Range(0, this.tetrominoes.Length);
        }
        randomNextPiece = Random.Range(0, this.tetrominoes.Length);
        TetrominoData data = this.tetrominoes[random];
        TetrominoData nextPiecedata= this.tetrominoes[randomNextPiece];
        //TetrominoData nextPieceData = this.tetrominoes[randomNextPiece];
        
        //Debug.Log("randomNextPiece: "+tetrominoes[randomNextPiece].tetromino);
        
        //Debug.Log("randomPiece: "+tetrominoes[random].tetromino);
        random=randomNextPiece;
        
          activePiece.Initialize(this, spawnPosition, data);
        nextactivePiece.Initialize(this, this.spawnPositionOfNextPiece,  nextPiecedata);
        //this.activePiece.Initialize(this, this.spawnPosition,  data);
        //this.
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
            //Debug.Log("Set Next Tile --- " + tilePosition);
        }
    }
public void NextClear(NextPiece piece)
    {
        for (int i=0; i<piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i]+piece.position;
            this.tilemap.SetTile(tilePosition, null);
            //Debug.Log("Clear Next Tile --- "+ tilePosition);
        }
    }

    void UpdateScore(int score)
    {
        
        //public 
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
