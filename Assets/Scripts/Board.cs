using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using Random = UnityEngine.Random;
using DG.Tweening;

public class Board : MonoBehaviour
{

    [SerializeField] private SaveGameManager saveGameManager;
    [SerializeField] private FireEffectPool fireEffectPool;
    [SerializeField] private SpeedSettings speedSettings;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private GameOverFade fadeGameOver;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private TMP_Text textScore;
    [SerializeField] private TMP_Text textLevel;
    private Vector2Int _boardSize = new Vector2Int(10, 20);
    private Vector3Int _spawnPosition = new Vector3Int(-3, 8, 0);
    private Vector3Int _spawnPositionOfNextPiece = new Vector3Int(6, 7, 0);
    private int randomNextPiece=-1;
    private int _random;  
    private List<string> _tempLine;
    private int frequencyOfTetrominoMAppearance = 2;
    private int _countTetrominoM = 0;
    private int areaWithoutObstacles = 10;
    private TetrominoData _data;
    private TetrominoData _nextPieceData;
    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }
    public NextPiece nextactivePiece { get; private set; }
    public TetrominoData[] tetrominoes;
    public List<int> listOfAllTilesToSave = new List<int>();
    public int score = 0;
    public  int level = 1;
    private int activePieceInitialRotation;
    public int ActivePieceInitialRotation
    {
        get => activePieceInitialRotation;
        set
        {
            int min = 0; 
            int max = 4;
            activePieceInitialRotation = Utils.Wrap(value, min, max);
            if (activePieceInitialRotation == max) activePieceInitialRotation = min;
        }

    }
    public int nextPieceStartRotation;
    public float stepSpeed { set; get; }
    public bool allowStepping { private set; get; } = true;
    public RectInt Bounds
    { get
        {
            Vector2Int position = new Vector2Int(-_boardSize.x/2-2,-_boardSize.y/2);
            return new RectInt(position, _boardSize);
        }
    }
    private void Awake()
    {
        DOTween.Init();
    }
    
    private void Start()
    {
        InitGameComponents();
        saveGameManager.LoadPlayerData();
        speedSettings.SpeedSettingsInit(stepSpeed);
        for (int i=0; i<tetrominoes.Length; i++)
        {
            tetrominoes[i].Initialize();
        }
        if (saveGameManager.isSaved)
        {
            StartGameRoutinesWithSaving();
        }
        else
        {
            StartGameRoutinesWithoutSaving();
        }
    }

    private void InitGameComponents()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        activePiece = GetComponent<Piece>();
        nextactivePiece = GetComponent<NextPiece>();
    }

    public void AllowStepping(bool allowSteps)
    {
        allowStepping = allowSteps;
    }
    public float CalculateNumberOfStars()
    {
        float rate;
        var highestTileInBoard = 0;

        var  row = Bounds.yMax;
        
        while (row > Bounds.yMin && highestTileInBoard==0)
        {
            for (var col = Bounds.xMin; col < Bounds.xMax && highestTileInBoard==0; col++)
            {
                var position = new Vector3Int(col, row, 0);
                if (!tilemap.HasTile(position)) continue;
                highestTileInBoard = position.y;
                
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
        return rate;
    }
    private void LoadGameSettings()
    {
        saveGameManager.LoadData();
    }

    private void StartGameRoutinesWithSaving()
    {
        LoadGameSettings();
        activePiece.Initialize(this, new Vector3Int(saveGameManager.tilesDataStorage.list[1],
            saveGameManager.tilesDataStorage.list[2],0), tetrominoes[saveGameManager.tilesDataStorage.list[0]]);
            
        nextactivePiece.Initialize(this, new Vector3Int(saveGameManager.tilesDataStorage.list[4],
            saveGameManager.tilesDataStorage.list[5],0), tetrominoes[saveGameManager.tilesDataStorage.list[3]]);
        randomNextPiece = saveGameManager.tilesDataStorage.list[3];
        _random = randomNextPiece;
        Set(activePiece);
        SetNext(nextactivePiece);
        LoadTilesFromSaveSpot();
        allowStepping = true;
        PrintLevel(level);
        UpdateScore(score);
    }

    public void UpdateTextLevelOnUI()
    {
        textLevel.text = level.ToString();
    }
    public void StartGameRoutinesWithoutSaving()
    {
        tilemap.ClearAllTiles();
        StartLevelGeneration();
        activePieceInitialRotation = Random.Range(0, 4);
        nextPieceStartRotation = Random.Range(0, 4);
        allowStepping = true;
        PrintLevel(level);
        UpdateScore(score);
        SpawnPiece();
    }

    private void StartLevelGeneration()
    {
        levelManager.GoalsGenerator();    
    }
    
    private void LoadTilesFromSaveSpot()
    {
        int itemInList = 0;
        int  row = Bounds.yMin;
        while (row < Bounds.yMax)
        {
            for (var col = Bounds.xMin; col < Bounds.xMax; col++)
            {
                if (saveGameManager.tilesDataStorage.list[itemInList + 6] < 100)
                {
                    var generatedSavedPosition = new Vector3Int(saveGameManager.tilesDataStorage.list[itemInList + 7],
                        saveGameManager.tilesDataStorage.list[itemInList + 8], 0);
                    var savedTile = tetrominoes[saveGameManager.tilesDataStorage.list[itemInList + 6]].tile;
                    tilemap.SetTile(generatedSavedPosition, savedTile);
                }
                itemInList += 3;
            }
            row++;
        }
    }

    private void SpawnPiece()
    {
        if (!allowStepping) return;
        if(randomNextPiece == -1)
        {
            _random = Random.Range(0, tetrominoes.Length);
        }
        GenerateActivePieceAndNextPiece();
       
        if (_nextPieceData.tetromino == Tetromino.M)
        {
            _countTetrominoM++;
            if (_countTetrominoM < frequencyOfTetrominoMAppearance)
            {
                GenerateActivePieceAndNextPiece(true);
            }
            else
            {
                _countTetrominoM = 0;
            }
        }
        _data = tetrominoes[_random];
        _random=randomNextPiece;
        activePieceInitialRotation = nextPieceStartRotation;
        nextPieceStartRotation = _nextPieceData.tetromino == Tetromino.M ? 0 : Random.Range(0, 4);
        
        activePiece.Initialize(this, _spawnPosition, _data);
        Vector3Int correctedSpawnPositionOfNextPiece = _nextPieceData.tetromino switch
        {
            Tetromino.I when nextPieceStartRotation % 2 == 0 => _spawnPositionOfNextPiece + Vector3Int.left,
            Tetromino.M => _spawnPositionOfNextPiece + Vector3Int.up,
            _ => _spawnPositionOfNextPiece
        };
        
        nextactivePiece.Initialize(this, correctedSpawnPositionOfNextPiece,  _nextPieceData);
        
        if (IsValidPosition(activePiece, activePiece.position))
        {
            Set(activePiece); 
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
        randomNextPiece = Random.Range(0, tetrominoes.Length-tetrominoM);
        _nextPieceData = tetrominoes[randomNextPiece];
    }

    private void GameOver()
     {
         soundManager.PlaySound(Sounds.Lose);
         allowStepping = false;
         fadeGameOver.LooseFade();
     }
    
    public void Set(Piece piece)
    {
        foreach (var cell in piece.cells)
        {
            Vector3Int tilePosition = cell+piece.position;
            tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }
    
    public void Clear(Piece piece)
    {
        foreach (var cell in piece.cells)
        {
            Vector3Int tilePosition = cell+piece.position;
            tilemap.SetTile(tilePosition, null);
        }
    }

    public bool  IsValidPosition(Piece piece, Vector3Int position)
    {
        foreach (var cell in piece.cells)
        {
            Vector3Int tilePosition = cell+position;
            if(tilemap.HasTile(tilePosition))
            {
                return false;
            }
            if(!Bounds.Contains((Vector2Int)tilePosition))
            {
                return false;
            }
        }
        return true;
    }

    private int ScanTileMap(Vector3Int position)
    {
        if (tilemap.GetTile(position) == null) return 100;
        string name = tilemap.GetTile(position).name;
        return name switch
        {
            "Red" => (int)Tetromino.Z,
            "Yellow" => (int)Tetromino.O,
            "Green" => (int)Tetromino.S,
            "Cyan" => (int)Tetromino.I,
            "Blue" => (int)Tetromino.J,
            "Orange" => (int)Tetromino.L,
            "Purple" => (int)Tetromino.T,
            "Magenta" => (int)Tetromino.M,
            _ => 100
        };
    }

     public void ClearLines()
    {
        var fullRowsDelete = new List<int>();
        int row= Bounds.yMin;
        while (row<Bounds.yMax)
        {
            if(IsLineFull(row))
            {
                fullRowsDelete.Add(row);
                TilesColorDetect(row);
                levelManager.UpdateGoalsLines();
            }
            row++;
        }
        
        foreach (var fulRowDelete in fullRowsDelete)
        {
            FireEffect fireEffect = fireEffectPool.GetEffect();
            fireEffect.transform.position = new Vector3(-2, fulRowDelete + 0.5f, 0);
            fireEffect.EnableEffects();
        }

        if (fullRowsDelete.Count > 0)
        {
            StartCoroutine(DeleteRows(fullRowsDelete));
            return;
        }
        SpawnPiece();
    }

     private  void TilesColorDetect(int row)
     {
         _tempLine = new List<string>();
         for(int col = Bounds.xMin; col < Bounds.xMax; col++)
         {
             Vector3Int position = new Vector3Int(col, row, 0);
             var tempTile = tilemap.GetTile(position);
             _tempLine.Add(tempTile.name);
         }
     }

     private IEnumerator DeleteRows(List<int> rows)
    {
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < rows.Count; i++)
        {
            soundManager.PlaySound(Sounds.DeleteLine);
            LineClear(rows[i]);
            score++;
            UpdateScore(score);
            for(int item=0; item<rows.Count; item++)
            {
                rows[item]--;
            }
        }
        SpawnPiece();
    }

    public void LineClear(int row)
    {
        for (int col = Bounds.xMin; col < Bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            tilemap.SetTile(position, null);
        }
        while (row < Bounds.yMax)
        {
            for (int col = Bounds.xMin; col < Bounds.xMax; col++)
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
        listOfAllTilesToSave.Add((int)activePiece.data.tetromino);
        listOfAllTilesToSave.Add(activePiece.position.x);
        listOfAllTilesToSave.Add(activePiece.position.y);
    }
    
    public void SaveNextPiece() 
    {
        listOfAllTilesToSave.Add((int)nextactivePiece.data.tetromino);
        listOfAllTilesToSave.Add(nextactivePiece.position.x);
        listOfAllTilesToSave.Add(nextactivePiece.position.y);
    }
    
    public void SaveBoardPixels()
    {
        int  row = Bounds.yMin;
        while (row < Bounds.yMax)
        {
            for (int col = Bounds.xMin; col < Bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row, 0);
                listOfAllTilesToSave.Add(ScanTileMap(position));
                listOfAllTilesToSave.Add(position.x);
                listOfAllTilesToSave.Add(position.y);
            }
            row++;
        }
    }


    private bool IsLineFull(int row)
    {
        for(int col = Bounds.xMin; col < Bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            if(!tilemap.HasTile(position))
            {
                return false;
            }
        }
        return true;
    }

    private void SetNext(NextPiece piece)
    {
        NextClear(piece);
        for (int i=0; i<piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i]+piece.position;
            tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }
    
    public void NextClear(NextPiece piece)
    {
        foreach (var cell in piece.cells)
        {
            Vector3Int tilePosition = cell+piece.position;
            tilemap.SetTile(tilePosition, null);
        }
    }

    private void UpdateScore(int score)
    {
       textScore.text = "Score: " + score.ToString();
    }

    private void PrintLevel(int levelValue)
    {
        textLevel.text = levelValue.ToString();
    }

    private void EraseScore()
    {
        saveGameManager.ResetData();
        UpdateScore(score);
    }
    
    public void GenerateObstacle()
    {
        Vector3Int position = new Vector3Int();
       do
        {
            position =
                new Vector3Int(Random.Range(Bounds.xMin, Bounds.xMax ), Random.Range(Bounds.yMin, Bounds.yMax - areaWithoutObstacles),
                    0);
        } 
       while (tilemap.HasTile(position));
       var randomTile = tetrominoes[Random.Range(0,tetrominoes.Length)].tile;
       tilemap.SetTile(position,randomTile);
    }
}
