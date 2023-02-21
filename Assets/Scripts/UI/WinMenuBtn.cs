using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinMenuBtn : MonoBehaviour
{
    private Button okBtn;
    private void Awake()
    {
        okBtn = GetComponent<Button>();
    }

    private void Start()
    {
        okBtn.onClick.AddListener(EventsManager.SendWinMenuBtnClicked);
    }
}
