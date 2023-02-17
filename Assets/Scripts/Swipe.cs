

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Swipe : MonoBehaviour, IPointerDownHandler, IPointerUpHandler

{
    Vector3 startPosition;
    Vector3 endPosition;
    private int blindMoveZone = 10;
    private int doubleMove = 100;
    private int tripleMove = 300;
    public Board board;
    [FormerlySerializedAs("_soundManager")] public SoundManager soundManager;
    private  float swipeOrTap= 20f;
    private float delta = 0f;
    private bool isPresed;
    private float distanceX=0f;
    private float prevPosition=0f;

    //[SerializeField] Bullet bullet;
    //private float prevDeltaMove = 0f;
    //float distanceX = 0f;
    float distanceY = 0f;

    [SerializeField] CoroutinesManager coroutinesManager;
    //private PointerEventData pointerEvent = new PointerEventData(EventSystem.current);

    private void Start()
    {
      //  _coroutinesManager = GetComponent<CoroutinesManager>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPresed = true;
        startPosition = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!board.allowStepping) return;
        endPosition = eventData.position;
        CalculateDistance();
        if (delta <= swipeOrTap) //tap
        {
            Tapping();
        }
        else if (Mathf.Abs(distanceX)>Mathf.Abs(distanceY))
        {
            SwipingLeftRight();
        }
        else if (Mathf.Abs(distanceX)<=Mathf.Abs(distanceY))
        {
            SwipingUpDown();
        }
        isPresed = false;
    }
    
    private void CalculateDistance()
    {
        delta = (startPosition-endPosition).magnitude;
        distanceX = startPosition.x - endPosition.x;
        distanceY = startPosition.y - endPosition.y;
    }
    private void Tapping()
    {
        if (board.activePiece.data.tetromino == Tetromino.M)
        {
            //Piece.PiuPiu();
            board.activePiece.PiuPiu();
            //board.activePiece.PiuPiu();
            
        }
        else
        {
            board.Clear(board.activePiece);
            board.activePiece.HardDrop();
            //soundManager.PlaySound(Sounds.Drop);
            board.Set(board.activePiece);
        }
    }


    private void SwipingLeftRight()
    {
        if ((distanceX > tripleMove)/*&&(frameCount%2==0)*/)
        {
            coroutinesManager.StartAppearingCoroutine(Arrow.left);
            board.Clear(board.activePiece);
            board.activePiece.Move(Vector2Int.left);
            board.Set(board.activePiece);
            board.Clear(board.activePiece);
            board.activePiece.Move(Vector2Int.left);
            board.Set(board.activePiece);
            board.Clear(board.activePiece);
            board.activePiece.Move(Vector2Int.left);
            board.Set(board.activePiece);
        }
        
        else if (distanceX < -tripleMove)
        { 
            coroutinesManager.StartAppearingCoroutine(Arrow.right);
            board.Clear(board.activePiece);
            board.activePiece.Move(Vector2Int.right);
            board.Set(board.activePiece);
            board.Clear(board.activePiece);
            board.activePiece.Move(Vector2Int.right);
            board.Set(board.activePiece);
            board.Clear(board.activePiece);
            board.activePiece.Move(Vector2Int.right);
            board.Set(board.activePiece);
        }
        
        else if (distanceX > doubleMove)
        {
            coroutinesManager.StartAppearingCoroutine(Arrow.left);
            board.Clear(board.activePiece);
            board.activePiece.Move(Vector2Int.left);
            board.Set(board.activePiece);
            board.Clear(board.activePiece);
            board.activePiece.Move(Vector2Int.left);
            board.Set(board.activePiece);
        }
        
        else if (distanceX < -doubleMove)
        { 
            coroutinesManager.StartAppearingCoroutine(Arrow.right);
            board.Clear(board.activePiece);
            board.activePiece.Move(Vector2Int.right);
            board.Set(board.activePiece);
            board.Clear(board.activePiece);
            board.activePiece.Move(Vector2Int.right);
            board.Set(board.activePiece);
        }
        
        else if ((distanceX > blindMoveZone)&&(distanceX < doubleMove)/*&&(frameCount==1)*/)
        {
            coroutinesManager.StartAppearingCoroutine(Arrow.left);
            board.Clear(board.activePiece);
            board.activePiece.Move(Vector2Int.left);
            board.Set(board.activePiece);
        }
        
        else if ((distanceX < -blindMoveZone)&&(distanceX > -doubleMove)/*&&(frameCount==1)*/)
        {
            coroutinesManager.StartAppearingCoroutine(Arrow.right);
            board.Clear(board.activePiece);
            board.activePiece.Move(Vector2Int.right);
            board.Set(board.activePiece);
        }
        soundManager.PlaySound(Sounds.MoveAside);
        distanceX = 0f;


    }
    private void SwipingUpDown()
    {
        if (distanceY > 150)
        {
            coroutinesManager.StartAppearingCoroutine(Arrow.down);
            board.Clear(board.activePiece);
            board.activePiece.Move(Vector2Int.down);
            board.Set(board.activePiece);
            soundManager.PlaySound(Sounds.MoveDown);
        }
        else if (distanceY < -150)
        {
            coroutinesManager.StartAppearingCoroutine(Arrow.rotate);
            board.Clear(board.activePiece);
            board.activePiece.Rotate(1);
            board.Set(board.activePiece);
            soundManager.PlaySound(Sounds.Rotate);

        }

        distanceY = 0f;
    }
    


    
}
