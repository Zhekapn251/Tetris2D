using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FeedbackArrowsDriver : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Image[] images = new Image[4];
    
    
    IEnumerator Disappear(Image image)
    {
        float alpha = 0.8f;
        while (alpha >= 0)
        {
            alpha -= Time.deltaTime * speed;
            image.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
        image.color = Color.clear;
    }
    
    IEnumerator Appear(Image image)
    {
        float alpha = 0f;
        while (alpha <= 0.8)
        {
            alpha += Time.deltaTime * speed;
            image.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
        
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(Disappear(image));
    }

    
    public void StartAppearingCoroutine(Arrow arrow)
    {
        Image objArrow = null;       
        switch (arrow)
        {
            case Arrow.down:
                objArrow = images[(int)Arrow.down];
                break;
            case Arrow.left:
                objArrow = images[(int)Arrow.left];
                break;
            case Arrow.right:
                objArrow = images[(int)Arrow.right];
                break;
            case Arrow.rotate:
                objArrow = images[(int)Arrow.rotate];
                break;
        }
        StartCoroutine(Appear(objArrow));
    }
}
