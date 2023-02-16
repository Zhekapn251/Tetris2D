using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BackgroundAnimation : MonoBehaviour
{
   [SerializeField] SpriteRenderer rocks1;
   [SerializeField] SpriteRenderer rocks2;
   [SerializeField] SpriteRenderer rocks3;
   [SerializeField] SpriteRenderer sky;
   [SerializeField] SpriteRenderer planet1;
   [SerializeField] private SpriteRenderer planet2;
      [SerializeField] SpriteRenderer sun;
   private bool isAnimating = true;
   public float rocks2Speed = 1f;
   public float planet2Speed = 0.5f;
   public float planet1Speed = 0.2f;
   public float skySpeed = 4f;
   private void Start()
   {
      StartCoroutine(Animation());
   }

   IEnumerator Animation()
   {

      while (isAnimating)
      {
         yield return null;
         rocks2.size +=  Time.deltaTime * rocks2Speed * new Vector2(0.05f, 0.00f);
         sky.size +=  Time.deltaTime * skySpeed * new Vector2(0.05f, 0.00f);
         planet2.transform.localPosition += planet2Speed * Time.deltaTime * Vector3.right;
         if(sun.transform.localPosition.x <- 5) sun.transform.localPosition = new Vector3(5f, -2.98f, -3f);
         sun.transform.localPosition += skySpeed * 0.2f * Time.deltaTime * Vector3.right;
         if(planet2.transform.localPosition.x > 11.5f) planet2.transform.localPosition = new Vector3(-11.5f, 6f, -2f);
         planet1.transform.localPosition += planet1Speed * Time.deltaTime * Vector3.right;
         if(planet1.transform.localPosition.x > 13f) planet1.transform.localPosition = new Vector3(-13f, 12f, -2.5f);
      }
   }
}
