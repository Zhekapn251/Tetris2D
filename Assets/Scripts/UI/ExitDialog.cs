using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitDialog : MonoBehaviour
{
    [SerializeField] private Button yesBtn;
    [SerializeField] private Button noBtn;
    
    private SaveGameManager _saveGameManager;

    private void Start()
    {
        _saveGameManager = FindObjectOfType<SaveGameManager>();
        if (_saveGameManager == null) Debug.Log("ExitDialog:: SaveGameManager is null");
        yesBtn.onClick.AddListener(ConfirmingExit);
        noBtn.onClick.AddListener(CancelExit);
    }

    public void CancelExit()   
    {
        gameObject.SetActive(false);
    }

    private void ConfirmingExit()
    {
        ExitGame();
    }
    
    private void ExitGame()
    {
        //Debug.Log("App_Quit");
        Application.Quit();
    }
}
