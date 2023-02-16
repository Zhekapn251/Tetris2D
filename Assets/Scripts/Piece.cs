using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;

public class Piece : MonoBehaviour
{
    public Board board{get; private set;}
    public TetrominoData data{get; private set;}
    public Vector3Int position{get; private set;}
    public Vector3Int[] cells{get; private set;}
    public int rotationIndex{get; private set;}

    public float stepDelay = 1f;
    public float moveDelay = 0.1f;
    public float lockDelay = 0.5f;

    private float stepTime;
    private float moveTime;
    private float lockTime;

    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        this.board = board;
        this.position = position;
        this.data = data;
        this.rotationIndex = 0;//Random.Range(0, 4);//UnityEngine.Random.Range(0, 4);//0;
        stepTime = Time.time + stepDelay;
        moveTime = Time.time + moveDelay;
        lockTime = 0f;
        string str1="rotation #";
        string str2="";
        string str3="init position = ";
        
        if (this.cells ==null)
        {
            this.cells = new Vector3Int[data.cells.Length];
        }
        for(int i=0; i<data.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int)data.cells[i];
            str3=str3+(cells[i]+position).ToString()+"; ";
        }
        
        for (int i=0; i<board.startRotation; i++)
        {            
            Rotate(board.startRotation);
            for(int j=0; j<cells.Length; j++)
            {    
            str1=str1+i.ToString()+(cells[j]+position).ToString()+"; ";
            }
            
            //if(!board.IsValidPOsition(this,  position))
            
            for(int j=0; j<cells.Length; j++)
            {
          
            
            //str2=str2+"i=" + i.ToString() + ": Valid = " +  board.IsValidPOsition(this,  position).ToString()+"; ";
              
            }
        
        }
        
        Debug.Log(board.startRotation);
        Debug.Log(str3);
        Debug.Log(str1); 
        str1="rotation #";
        //Debug.Log(str2); 
         

    }
    
    
    private void Update()
    {
       
        this.board.Clear(this);
        
        

         // We use a timer to allow the player to make adjustments to the piece
        // before it locks in place
        lockTime += Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.Q))   ///rotation ccw
        {
            Rotate(-1);
        }
        else if (Input.GetKeyDown(KeyCode.E)||Input.GetKeyDown(KeyCode.UpArrow))   //rotation cw
        {
            Rotate(+1);
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow))   ///move left
        {
            Move(Vector2Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))   //move right
        {
            Move(Vector2Int.right);
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))   ///move left
        {
            Move(Vector2Int.down);
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            HardDrop();
        }

        // Advance the piece to the next row every x seconds
        if (Time.time > stepTime) {
            Step();
        }

        this.board.Set(this);        
        
    }
    

    private void Step()
    {
        stepTime = Time.time + stepDelay;

        // Step down to the next row
        Move(Vector2Int.down);

        // Once the piece has been inactive for too long it becomes locked
        if (lockTime >= lockDelay) {
            Lock();
        }
       
    }

    private void Lock()
    {
        board.Set(this);
        board.NextClear(board.nextactivePiece);
        board.ClearLines();
        board.SpawnPiece();
    }

    private void Rotate(int direction)
    {
        // Store the current rotation in case the rotation fails
        // and we need to revert
        int originalRotation = rotationIndex;

        // Rotate all of the cells using a rotation matrix
        rotationIndex = Wrap(rotationIndex + direction, 0, 4);
        ApplyRotationMatrix(direction);

        if (!TestWallKicks(rotationIndex, direction))
        {
            rotationIndex = originalRotation;
            ApplyRotationMatrix(-direction);
        }
    }

    private void ApplyRotationMatrix(int direction)
    {
        float[] matrix = Data.RotationMatrix;
        string temp="just after rotation: ";
        // Rotate all of the cells using the rotation matrix
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3 cell = cells[i];

            int x, y;

            switch (data.tetromino)
            {
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
            temp=temp+cells[i].ToString();

        }
        Debug.Log(temp);
        temp = "just after rotation: ";
    }

    private void HardDrop()
    {
        while (Move(Vector2Int.down))
        {
            continue;
        }
    Lock();
    
    }

    
    private bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);
        Debug.Log("wallKickIndex= "+wallKickIndex);
        
        for (int i = 0; i < data.wallKicks.GetLength(1); i++)  //i<5
        {
            Vector2Int translation = data.wallKicks[wallKickIndex, i];

            if (Move(translation)) {
                Debug.Log("wallKickIndex= "+i.ToString()+" is valid");
                return true;
                
            }
            Debug.Log("wallKickIndex= "+i.ToString()+" is NOT valid");
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
    
    private int Wrap(int input, int min, int max)
    {
        if (input < min) {
            return max - (min - input) % (max - min);
        } else {
            return min + (input - min) % (max - min);
        }
    }
    private bool Move(Vector2Int translation)        // translation -- sdvig
    {
        Vector3Int newPosition = position;
        Debug.Log("Func Move "+position);
        newPosition.x +=translation.x;
        newPosition.y+=translation.y;
        Debug.Log("Func Move newPosition "+newPosition);
        bool valid = this.board.IsValidPOsition(this, newPosition);
        Debug.Log("valid "+valid);
        if(valid)
        {
            position = newPosition;
            moveTime = Time.time + moveDelay;
            lockTime = 0f; // reset
        }
        return valid;
    }
}
