using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using DG.Tweening;
using UnityEngine.UI;

public class Board : MonoBehaviour
{

    public SaveGameManager SaveGameManager;
    public FireEffectPool FireEffectPool;
    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }
    public NextPiece nextactivePiece { get; private set; }
    public TetrominoData[] tetrominoes;
    public List<int> list = new List<int>();
    [SerializeField] private SpeedSettings _speedSettings;
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private LoseGame _loseGame;
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public Vector3Int spawnPosition = new Vector3Int(-3, 8, 0);
    public Vector3Int spawnPositionOfNextPiece = new Vector3Int(6, 7, 0);
    public int randomNextPiece=-1;
    int random;
    public TMP_Text textScore;
    
    public int score = 0;
    //SaveMyGame saveMyGame = new SaveMyGame();
    public TMP_Text textLevel;
    public  int level = 1;
    public int activePieceRotation;
    public int nextPieceStartRotation;
    public bool notAnimated=true;
    private List<string> tempLine;
    private int frequencyOfTetrominoMAppearance = 2;
    private int countTetrominoM = 0;
    public LevelManager levelManager;
    private TetrominoData data;
    private TetrominoData nextPiecedata;
    public float stepSpeed = 1f;
    public bool allowStepping = true;
    [SerializeField] GameOverFade _fadeGameOver;
    

    private void Awake()
    {
        //swipe = GetComponent<Swipe>();
        DOTween.Init();
        
        Debug.Log("Awake() ---- OK");
    }
    
    private void Start()
    {
        _speedSettings.SpeedSettingsInit();
        tilemap = GetComponentInChildren<Tilemap>();
        activePiece = GetComponent<Piece>();
        nextactivePiece = GetComponent<NextPiece>();
        SaveGameManager.LoadPlayerData();
        


        for (int i=0; i<tetrominoes.Length; i++)
        {
            tetrominoes[i].Initialize();
        }
        
        if (SaveGameManager.isSaved)
        {
            StartGameRoutinesWithSaving();
        }
        else
        {
            StartGameRoutinesWithoutSaving();
        }
        
        Debug.Log("Start() ---- OK");
    }

    public float CalculateNumberOfStars()
    {
        float rate;
        int highestTileInBoard = 0;
        RectInt bounds = Bounds;

        int  row = bounds.yMax;
        // Shift every row above down one
        while (row > bounds.yMin && highestTileInBoard==0)
        {
            for (int col = bounds.xMin; col < bounds.xMax && highestTileInBoard==0; col++)
            {
                Vector3Int position = new Vector3Int(col, row, 0);
                if (tilemap.HasTile(position))
                {
                    highestTileInBoard = position.y;
                    Debug.Log("highest = "+highestTileInBoard);
                }
            }    
            row--;
        }
        if (highestTileInBoard<-4)
        {
            rate = 1f;
        }
        else if (highestTileInBoard < 1)
        {
            rate = 0.67f;
        }
        else
        {
            rate = 0.34f;
        }
        Debug.Log("rate = "+ rate);

        return rate;
    }
private void LoadGameSettings()
    {
        SaveGameManager.LoadData();
    }

    public void StartGameRoutinesWithSaving()
    {
        LoadGameSettings();
        activePiece.Initialize(this, new Vector3Int(SaveGameManager.SaveDataStorage.list[1],
            SaveGameManager.SaveDataStorage.list[2],0), tetrominoes[SaveGameManager.SaveDataStorage.list[0]]);
            
        nextactivePiece.Initialize(this, new Vector3Int(SaveGameManager.SaveDataStorage.list[4],
            SaveGameManager.SaveDataStorage.list[5],0), tetrominoes[SaveGameManager.SaveDataStorage.list[3]]);
        randomNextPiece = SaveGameManager.SaveDataStorage.list[3];
        random = randomNextPiece;
            
        Set(activePiece);
        SetNext(nextactivePiece);
        LoadTilesFromSaveSpot();
        
        allowStepping = true;
        PrintLevel(level);
        UpdateScore(score);
    }
    
    public void StartGameRoutinesWithoutSaving()
    {
        //Clear(activePiece);
        tilemap.ClearAllTiles();
        StartLevelGeneration();
        activePieceRotation = UnityEngine.Random.Range(0, 4);
        nextPieceStartRotation = UnityEngine.Random.Range(0, 4);
        allowStepping = true;
        PrintLevel(level);
        UpdateScore(score);
        SpawnPiece();
    }

    private void StartLevelGeneration()
    {
        levelManager.GoalsGenerator();    
    }

    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-this.boardSize.x/2-2,-this.boardSize.y/2);
            return new RectInt(position, this.boardSize);
        }
    }
    
    public void LoadTilesFromSaveSpot()
    {
        RectInt bounds = Bounds;
        int itemInList = 0;
        Tile savedTile;
        int  row = bounds.yMin;
        // Shift every row above down one
        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row, 0);
                if (SaveGameManager.SaveDataStorage.list[itemInList + 6] < 100)
                {
                    Vector3Int generatedSavedPosition = new Vector3Int(SaveGameManager.SaveDataStorage.list[itemInList + 7],
                        SaveGameManager.SaveDataStorage.list[itemInList + 8], 0);
                    savedTile = tetrominoes[SaveGameManager.SaveDataStorage.list[itemInList + 6]].tile;
                    tilemap.SetTile(generatedSavedPosition, savedTile);
                }
                itemInList = itemInList+3;
            }
            row++;
        }
    }
    
    public void SpawnPiece()
    {
        if(randomNextPiece == -1)
        {
            random = Random.Range(0,this.tetrominoes.Length-1);
        }
        GenerateActivePieceAndNextPiece();
       
        if (nextPiecedata.tetromino == Tetromino.M)
        {
            countTetrominoM++;
            if (countTetrominoM > frequencyOfTetrominoMAppearance)
            {
                countTetrominoM = 0;
            }

            if (countTetrominoM < frequencyOfTetrominoMAppearance)
            {
                GenerateActivePieceAndNextPiece(true);
            }
        }
        data = tetrominoes[random];
        random=randomNextPiece;
        activePieceRotation = nextPieceStartRotation;
        if (nextPiecedata.tetromino == Tetromino.M)
        {
            nextPieceStartRotation = 0;
        }
        else
        {
            nextPieceStartRotation = UnityEngine.Random.Range(0, 4);
        }
        
        activePiece.Initialize(this, spawnPosition, data);
        Vector3Int correctedspawnPositionOfNextPiece = spawnPositionOfNextPiece;
        if (nextPiecedata.tetromino == Tetromino.I && nextPieceStartRotation % 2 == 0)
        {
            correctedspawnPositionOfNextPiece = spawnPositionOfNextPiece+Vector3Int.left;
        }
        if (nextPiecedata.tetromino == Tetromino.M)
        {
            correctedspawnPositionOfNextPiece = spawnPositionOfNextPiece+Vector3Int.up;
        }
        
        

        nextactivePiece.Initialize(this, correctedspawnPositionOfNextPiece,  nextPiecedata);
        
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
    
    private void GenerateActivePieceAndNextPiece(bool generateWithoutTetrominoM=false)
    {

        int tetrominoM = generateWithoutTetrominoM ? 1 : 0;
        
        randomNextPiece = Random.Range(0, tetrominoes.Length-1-tetrominoM);//
        nextPiecedata = tetrominoes[randomNextPiece];
        
    }

    private void GameOver()
     {
         
         _soundManager.PlaySound(Sounds.Lose);
         //this.tilemap.ClearAllTiles();
         //_loseGame.ShowLoseGame();
         allowStepping = false;
         GameOverSequence();
         
     }

    public void GameOverSequence()
    {
        _fadeGameOver.LooseFade();
    }

    
    public void Set(Piece piece)
    {
        for (int i=0; i<piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i]+piece.position;
            tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void Clear(Piece piece)
    {
        for (int i=0; i<piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i]+piece.position;
            tilemap.SetTile(tilePosition, null);
        }
    }

    public bool  IsValidPOsition(Piece piece, Vector3Int position)
    {
        RectInt bounds = this.Bounds;
        for( int i=0; i<piece.cells.Length;i++)
        {
            Vector3Int tilePosition = piece.cells[i]+position;
            if(tilemap.HasTile(tilePosition))
            {
                return false;
            }
            if(!bounds.Contains((Vector2Int)tilePosition))
            {
                return false;
            }
        }
        return true;
    }

    public int ScanTileMap(Vector3Int position)
    {
        if (tilemap.GetTile(position) == null) return 100;
        string name = tilemap.GetTile(position).name;
        switch (name)
        {
            case "Red":
                return  (int)Tetromino.Z;
            case "Yellow":
                return (int)Tetromino.O;
            case "Green":
                return (int)Tetromino.S;
            case "Cyan":
                return (int)Tetromino.I;
            case "Blue":
                return (int)Tetromino.J;
            case "Orange":
                return (int)Tetromino.L;
            case "Purple":
                return (int)Tetromino.T;
            case "Magenta":
                return (int)Tetromino.M;
        }

        return 100;
    }

     public bool ClearLines()
    {
        List<int> fullRowsDelete = new List<int>();
        RectInt bounds = this.Bounds;
        int row= bounds.yMin;
        while (row<bounds.yMax)
        {
            if(IsLineFull(row))
            {
                fullRowsDelete.Add(row);
                TilesColorDetect(row);
                Debug.Log("+line");
                levelManager.UpdateGoalsLines();
            }
            row++;
        }
        
        for (int i = 0; i < fullRowsDelete.Count; i++)
        {
            FireEffect fireEffect = FireEffectPool.GetEffect();
            fireEffect.transform.position = new Vector3(-2, fullRowsDelete[i] + 0.5f, 0);
            fireEffect.EnableEffects();
        }
        
        if (fullRowsDelete.Count > 0)
        {
            StartCoroutine(DeleteRows(fullRowsDelete));
            return true;
        }
        if (fullRowsDelete.Count==0)
        {
            SpawnPiece();
        }
        return false;
    }

     private  void TilesColorDetect(int row)
     {
         tempLine = new List<string>();
         RectInt bounds = this.Bounds;
         TileBase tempTile= ScriptableObject.CreateInstance<Tile>();
         for(int col = bounds.xMin; col < bounds.xMax; col++)
         {
             Vector3Int position = new Vector3Int(col, row, 0);
             tempTile=tilemap.GetTile(position);
             tempLine.Add(tempTile.name);
         }
     }
     
     
    IEnumerator DeleteRows(List<int> rows)
    {
        notAnimated = false;
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < rows.Count; i++)
        {
            LineClear(rows[i]);
            score++;//score++;
            //////////////LevelGenerator.CheckGoal(GoalsType.Lines);
            UpdateScore(score);
            for(int item=0; item<rows.Count; item++)
            {
                rows[item]--;
            }
        }
        SpawnPiece();
        notAnimated = true;
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

    public void SaveCurrentPiece() 
    {
        list.Add((int)activePiece.data.tetromino);
        list.Add(activePiece.position.x);
        list.Add(activePiece.position.y);
    }
    
    public void SaveNextPiece() 
    {
        list.Add((int)nextactivePiece.data.tetromino);
        list.Add(nextactivePiece.position.x);
        list.Add(nextactivePiece.position.y);
    }
    
    public void SaveBoardPixels()
    {
        RectInt bounds = Bounds;

       int  row = bounds.yMin;
        // Shift every row above down one
        while (row < bounds.yMax)
        {
        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            list.Add(ScanTileMap(position));
            list.Add(position.x);
            list.Add(position.y);
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
       textScore.text = "Score: " + score.ToString();
    }
    
    public void PrintLevel(int levelValue)
    {
        textLevel.text = levelValue.ToString();
    }
    
    void EraseScore()
    {
        //saveMyGame.score = 0;//score = 0;
        SaveGameManager.ResetData();
        UpdateScore(score);//UpdateScore(score);
    }
    
    

    public void GenerateObstacle()
    {
        Vector3Int position = new Vector3Int();
       do
        {
            Debug.Log(position);
            position =
                new Vector3Int(Random.Range(Bounds.xMin, Bounds.xMax ), Random.Range(Bounds.yMin, Bounds.yMax - 3),
                    0);
       } while (tilemap.HasTile(position));  
        
        
        Tile randomTile = ScriptableObject.CreateInstance<Tile>();
        randomTile = tetrominoes[Random.Range(0,tetrominoes.Length)].tile;
        tilemap.SetTile(position,randomTile);
    }
}
