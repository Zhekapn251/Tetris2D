using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoseGame : MonoBehaviour
{
    [SerializeField] private Board board;
    [SerializeField] private TMP_Text score;
    [SerializeField]private SaveGameManager _saveGameManager;
    [SerializeField] private GameOverFade _gameOverFade;
    public void ShowLoseGame()
    {
        //board.allowStepping = false;
        gameObject.SetActive(true);
        score.text = board.score.ToString();
        _saveGameManager.ResetData();
    }
    
    public void HideLoseGame()
    {
        gameObject.SetActive(false);
        board.StartGameRoutinesWithoutSaving();
        board.allowStepping = true;
        _gameOverFade.LooseUnFade();
        
    }

}
