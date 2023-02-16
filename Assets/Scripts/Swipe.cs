

using UnityEngine;
using UnityEngine.EventSystems;

public class Swipe : MonoBehaviour, IPointerDownHandler, IPointerUpHandler

{
    Vector3 startPosition;
    Vector3 endPosition;
    private int slowMove = 35;
    private int fastMove = 200;
    private int frameCount = 0;
    public Board board;
    public SoundManager _soundManager;
    private  float swipeOrTap= 20f;
    private float delta = 0f;
    private bool isPresed;
    private float deltaMoveX=0f;
    private float prevPosition=0f;
    private float prevDeltaMove = 0f;
    float distanceX = 0f;
    float distanceY = 0f;
    //private PointerEventData pointerEvent = new PointerEventData(EventSystem.current);
    
    
    public void OnPointerDown(PointerEventData eventData)
    {
        isPresed = true;
        startPosition = eventData.position;
        // Debug.Log("pointer is NOT moving1");
        // Debug.Log(pointerEvent.IsPointerMoving());
        // while (pointerEvent.IsPointerMoving())
        // {
        //     
        // }
        Debug.Log("pointer is NOT moving2");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        prevPosition = 0;
        endPosition = eventData.position;
        CalculateDistance();
        if (delta <= swipeOrTap) //tap
        {
            Tapping();
        }
        else
        {
            SwipingUpDown();
        }
        isPresed = false;
    }
    
    private void Update()
    {
        if (isPresed)
        {

            if (prevPosition != 0)
            {
                frameCount ++;
                if (frameCount==11)
                {
                    frameCount = 0;
                }
                deltaMoveX = Time.deltaTime*(Input.mousePosition.x - prevPosition)*1000;
                board.PrintLevel(deltaMoveX.ToString());
                if (Mathf.Abs(deltaMoveX) > 35)
                {
                    SwipingLeftRight();
                }
                
            }
            else
            {
                prevPosition = Input.mousePosition.x;
            }
        }
    }

    
    private void CalculateDistance()
    {
        delta = (startPosition-endPosition).magnitude;
        distanceX = startPosition.x - endPosition.x;
        distanceY = startPosition.y - endPosition.y;
    }
    private void Tapping()
    {
        board.Clear(board.activePiece);
        board.activePiece.HardDrop();
        board.Set(board.activePiece);
    }

    private void SwipingLeftRight()
    {
        if ((deltaMoveX > fastMove)&&(frameCount%2==0))
        {
            board.Clear(board.activePiece);
            board.activePiece.Move(Vector2Int.right);
            board.Set(board.activePiece);
        }
        if ((deltaMoveX < -fastMove)&&(frameCount%2==0))
        {
            board.Clear(board.activePiece);
            board.activePiece.Move(Vector2Int.left);
            board.Set(board.activePiece);
        }
        if ((deltaMoveX > slowMove)&&(deltaMoveX < fastMove)&&(frameCount==1))
        {
            board.Clear(board.activePiece);
            board.activePiece.Move(Vector2Int.right);
            board.Set(board.activePiece);
        }
        if ((deltaMoveX < -slowMove)&&(deltaMoveX > -fastMove)&&(frameCount==1))
        {
            board.Clear(board.activePiece);
            board.activePiece.Move(Vector2Int.left);
            board.Set(board.activePiece);
        }
        _soundManager.PlaySound("move");
        deltaMoveX = 0f;
        //board.PrintLevel(deltaMoveX.ToString());
        prevPosition = 0f;
        
            
    }
    private void SwipingUpDown()
    {
        if (distanceY > 150)
        {
            board.Clear(board.activePiece);
            board.activePiece.Move(Vector2Int.down);
            board.Set(board.activePiece);
        }
        else if (distanceY < -150)
        {
            board.Clear(board.activePiece);
            board.activePiece.Rotate(1);
            board.Set(board.activePiece);
            _soundManager.PlaySound("rotate");

        }
    }
    


    
}
