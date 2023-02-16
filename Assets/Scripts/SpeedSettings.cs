using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedSettings : MonoBehaviour
{
    [SerializeField] private Image speedBar;
    private List<float> steplist = new List<float>() { 0f, 0.32f, 0.64f, 1f };
    private int _step;

    private void Start()
    {
     float speedcase = speedBar.fillAmount;
     if (speedcase == 0f)
     {
      _step = 0;
     }  
     else if (speedcase < 0.321f)
     {
      _step = 1;
     }
     else if (0.321f <= speedcase && speedcase < 0.641f)
     {
      _step = 2;
     }
     else if (0.641f <= speedcase && speedcase <= 1f)
     {
      _step = 3;
     }
     else
     {
       _step = 0;
     }
    }
    
    public void EncreaseFillBar()
 {
  if(_step<steplist.Count-1) _step++;
  ChangeSpeedBar(_step);
 }
 
 public void DencreaseFillBar()
 {
  if(_step>0) _step--;
  ChangeSpeedBar(_step);
 }
 
 private void ChangeSpeedBar(int step)
 {
  speedBar.fillAmount = steplist[step];
 }

}
