using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveButton : MonoBehaviour
{
   // Start is called before the first frame update
   public Board board;
   public SaveGameManager SaveGameManager;
   public void SaveButtonClicked()
       {
           SaveGame();
       }

       void SaveGame()
       {
           board.list.Clear();
           board.SaveCurrentPiece();
           board.SaveNextPiece();
           board.SaveBoardPixels();
           SaveGameManager.SettingsSaver();
           Debug.Log("SaveButton Clicked");
           //SaveGameManager.
       }
}

