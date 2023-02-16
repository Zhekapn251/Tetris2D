using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameOverFade : MonoBehaviour
{
    private Image _image;
    [SerializeField] private LoseGame _loseGame;

    private void Start()
    {
        _image = GetComponentInChildren<Image>();
        if (_image == null)
        {
            Debug.LogError("GameOverFade:: Image is null");
        }
    }

    public void LooseFade()
    {
        DOTween.Sequence()
            .Append(_image.DOFade(0.8f, 1f))
            .AppendCallback(ShowLooseMenu);
    }
    private void ShowLooseMenu()
    {
        _loseGame.ShowLoseGame();
    }

    public void LooseUnFade()
    {
        _image.DOFade(0f, 0f);
    }
}
