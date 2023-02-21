using UnityEngine;

public class NextPiece : MonoBehaviour
{
    
    public Board board{get; private set;}
    public TetrominoData data{get; private set;}
    public Vector3Int position{get; private set;}
    public Vector3Int[] cells{get; private set;}
    public int rotationIndex{get; private set;}

    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        this.board = board;
        this.position = position;
        this.data = data;
        
        //{
            this.cells = new Vector3Int[data.cells.Length];
        //}
        for(int i=0; i<data.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int)data.cells[i]; 
        }
        for (int i = 0; i < board.nextPieceStartRotation; i++)
        {
            Rotate(1);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        //this.board.SetNext(this);
    }
    


private void Rotate(int direction)
    {
        // Store the current rotation in case the rotation fails
        // and we need to revert
        int originalRotation = rotationIndex;

        // Rotate all of the cells using a rotation matrix
        rotationIndex = Wrap(rotationIndex + direction, 0, 4);
        ApplyRotationMatrix(direction);
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
    
    private int Wrap(int input, int min, int max)
    {
        if (input < min) {
            return max - (min - input) % (max - min);
        } else {
            return min + (input - min) % (max - min);
        }
    }
    
}
