using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedSettings : MonoBehaviour
{
    [SerializeField] private Image speedBar;
    [SerializeField] private Board board;
    [SerializeField] private Button increaseBtn;
    [SerializeField] private Button decreaseBtn;
    [SerializeField] private Button exitMenu;
    [SerializeField] private SaveGameManager saveGameManager;
    
    private List<float> _steplist = new List<float>() { 0f, 0.32f, 0.64f, 1f };
    private List<float> _stepSpeed = new List<float>() { 2f, 1f, 0.5f, 0.25f };
    private int _step;
    private float _speed;

    private void Start()
    {
     increaseBtn.onClick.AddListener(IncreaseFillBar);
     decreaseBtn.onClick.AddListener(DecreaseFillBar);
     exitMenu.onClick.AddListener(SpeedMenuOff);
     _speed = saveGameManager.playerSettingsStorage.speed;
     SetSpeedBar();
    }

    public void SpeedMenuOn()
    {
     gameObject.SetActive(true);
    }

    private void SpeedMenuOff()
    {
     gameObject.SetActive(false);
    }

    private void SetSpeedBar()
    {
     if (Math.Abs(_speed - 0.25f) < 0.1f)
     {
      speedBar.fillAmount = 1;
      _step = 3;
     }  
     else if (Math.Abs(_speed - 0.5f) < 0.1f )
     {
      speedBar.fillAmount = 0.641f;
      _step = 2;
     }
     else if (Math.Abs(_speed - 1f) < 0.1f)
     {
      speedBar.fillAmount = 0.321f;
      _step = 1;
     }
     else if (Math.Abs(_speed - 2f) < 0.1f)
     {
      speedBar.fillAmount = 0;
      _step = 0;
     }
     else
     {
      _step = 0;
     }
    }
    public void SpeedSettingsInit(float speed)
    {
     
     if (Math.Abs(speed - 2f) < 0.1f)
     {
      _step = 0;
     }  
     else if (Math.Abs(speed - 1f) < 0.1f)
     {
      _step = 1;
     }
     else if (Math.Abs(speed - 0.5f) < 0.1f)
     {
      _step = 2;
     }
     else if (Math.Abs(speed - 0.25f) < 0.1f)
     {
      _step = 3;
     }
     else
     {
      _step = 0;
     }
     ChangeSpeedBar(_step);
     ChangeSpeed(_step);
    }
    
    private void ChangeSpeed(int step)
    {
     board.stepSpeed = _stepSpeed[step];
    }
    private void IncreaseFillBar()
    {
     if(_step<_steplist.Count-1) _step++;
     ChangeSpeedBar(_step);
     ChangeSpeed(_step);
     SaveUpdatedSpeed();
    }
    
    private void DecreaseFillBar()
    {
     if(_step>0) _step--;
     ChangeSpeedBar(_step);
     ChangeSpeed(_step);
     SaveUpdatedSpeed();
    }

    private void SaveUpdatedSpeed()
    {
     saveGameManager.SavePlayersSettings(false);
    }
 
    private void ChangeSpeedBar(int step)
    { 
     speedBar.fillAmount = _steplist[step];
    }

}
