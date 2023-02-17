using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;

public class Piece : MonoBehaviour
{
    public Board board;
    public TetrominoData data{get; private set;}
    public Vector3Int position{get; private set;}
    public Vector3Int[] cells{get; private set;}
    public int rotationIndex{get; set;}

    public float moveDelay = 0.1f;
    public float lockDelay = 0.5f;
    [SerializeField] private CoroutinesManager _coroutinesManager;
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private PoolOfBullets PoolOfBullets;
    private float stepTime;

    
    //private float moveTime;   
    private float lockTime;
    private bool isFireing = false;
    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        this.board = board;
        this.position = position;
        this.data = data;
        rotationIndex = 0;
        stepTime = Time.time + board.stepSpeed;
        //moveTime = Time.time + moveDelay;
        lockTime = 0f;

        cells = new Vector3Int[data.cells.Length];
        for(int i=0; i<data.cells.Length; i++)
        {
            cells[i] = (Vector3Int)data.cells[i];
        }
        for (int i = 0; i < board.activePieceRotation; i++)
        {
            Rotate(1);
        }
    }
    
    private void Update()
    {
        if (!board.allowStepping) return;
        board.Clear(this);
 
        lockTime += Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.Q))   ///rotation ccw
        {
            _coroutinesManager.StartAppearingCoroutine(Arrow.rotate);
            _soundManager.PlaySound(Sounds.Rotate);
            Rotate(-1);
        }
        else if (Input.GetKeyDown(KeyCode.E)||Input.GetKeyDown(KeyCode.UpArrow))   //rotation cw
        {
            _coroutinesManager.StartAppearingCoroutine(Arrow.rotate);
            _soundManager.PlaySound(Sounds.Rotate);
            Rotate(+1);
            
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow))   ///move left
        {
            _coroutinesManager.StartAppearingCoroutine(Arrow.left);
            Move( Vector2Int.left);
            _soundManager.PlaySound(Sounds.MoveAside);
            
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))   //move right
        {
            _coroutinesManager.StartAppearingCoroutine(Arrow.right);
            Move( Vector2Int.right);
            _soundManager.PlaySound(Sounds.MoveAside);
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))   ///move left
        {
            _coroutinesManager.StartAppearingCoroutine(Arrow.down);
            Move( Vector2Int.down);
            _soundManager.PlaySound(Sounds.MoveDown);
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (data.tetromino == Tetromino.M)
            {
                PiuPiu();
            }
            else
            {
                HardDrop();    
            }
        }
        if (Time.time > stepTime)
        {
            Step();
        }
        board.Set(this);
    }
    

    private void Step()
    {
        stepTime = Time.time + board.stepSpeed;
            Move(Vector2Int.down);
            _soundManager.PlaySound(Sounds.MoveDown);
            if (lockTime >= lockDelay)
            {
                Lock();
            }
    }

    private void Lock()
    {
        _soundManager.PlaySound(Sounds.Lock);
        if (this.data.tetromino == Tetromino.M)
        {
            board.Clear(this);
        }
        else
        {
            board.Set(this);
        }
        board.NextClear(board.nextactivePiece);
        board.ClearLines();
    }

    public void Rotate(int direction)
    {
        int originalRotation = rotationIndex;
        rotationIndex = Wrap(rotationIndex + direction, 0, 4);
        ApplyRotationMatrix(direction);

        if (TestWallKicks(rotationIndex, direction)) return;
        rotationIndex = originalRotation;
        ApplyRotationMatrix(-direction);
    }

    private void ApplyRotationMatrix(int direction)
    {
        float[] matrix = Data.RotationMatrix;
        // Rotate all of the cells using the rotation matrix
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3 cell = cells[i];

            int x, y;

            switch (data.tetromino)
            {
                case Tetromino.M:
                    x = Mathf.RoundToInt(cell.x);
                    y = Mathf.RoundToInt(cell.y);
                    break;
                case Tetromino.I:
                case Tetromino.O:
                    // "I" and "O" are rotated from an offset center point
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * matrix[0] * direction) + (cell.y * matrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * matrix[2] * direction) + (cell.y * matrix[3] * direction));
                    break;

                default:
                    x = Mathf.RoundToInt((cell.x * matrix[0] * direction) + (cell.y * matrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * matrix[2] * direction) + (cell.y * matrix[3] * direction));
                    break;
            }
            cells[i] = new Vector3Int(x, y, 0);
        }
    }

    public void HardDrop()
    {
        stepTime = Time.time + board.stepSpeed;
          
        while (Move(Vector2Int.down))
            {
                continue;
            }
        Lock();
    }

    IEnumerator PifPuff()
    {
        if (!isFireing)
        {
            RectInt bounds = board.Bounds;
            int col= this.position.x;//x - col         y - row
            int row = position.y-2;
            while (row >= bounds.yMin)
            {
                Vector3Int position = new Vector3Int(col, row, 0);
                if (board.tilemap.HasTile(position))
                {
                    isFireing = true;
                    _soundManager.PlaySound(Sounds.Fire);
                    Bullet bullet = PoolOfBullets.GetBullet();
                    bullet.FireBullet(this.position, position);
                    yield return new WaitForSeconds(0.25f);
                    board.tilemap.SetTile(position,null);
                    isFireing = false;
                    break;
                }
                row--;
            }
        }
    }

    public void PiuPiu()
    {
        StartCoroutine(PifPuff());
    }
    private bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);
        
        for (int i = 0; i < data.wallKicks.GetLength(1); i++)  //i<5
        {
            Vector2Int translation = data.wallKicks[wallKickIndex, i];

            if (Move( translation)) {
                return true;
            }
        }
    
        return false;
    }

    private int GetWallKickIndex(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = rotationIndex * 2;

        if (rotationDirection < 0) {
            wallKickIndex--;
        }

        return Wrap(wallKickIndex, 0, data.wallKicks.GetLength(0));
    }
    
    private int Wrap(int input, int min, int max) =>
        input < min 
            ? max - (min - input) % (max - min)
            : min + (input - min) % (max - min);

    public bool Move(Vector2Int translation)        // translation -- sdvig
    {
        Vector3Int newPosition = position;
        newPosition.x +=translation.x;
        newPosition.y+=translation.y;
        bool valid = this.board.IsValidPosition(this, newPosition);
        if(valid)
        {
            position = newPosition;
            //moveTime = Time.time + moveDelay;
            lockTime = 0f; // reset
        }
        return valid;
    }
}
