using System.Collections;
using UnityEngine;
public class BackgroundAnimation : MonoBehaviour
{
   [SerializeField] private SpriteRenderer rocks2;
   [SerializeField] private SpriteRenderer sky;
   [SerializeField] private SpriteRenderer planet1;
   [SerializeField] private SpriteRenderer planet2;
   [SerializeField] private SpriteRenderer sun;
   private bool isAnimating = true;
   public float rocks2Speed = 0.2f;
   public float planet2Speed = 0.5f;
   public float planet1Speed = 0.2f;
   public float skySpeed = 0.2f;
   private void Start()
   {
      StartCoroutine(Animation());
   }

   private IEnumerator Animation()
   {
      while (isAnimating)
      {
         yield return null;
         Animate();
         WrapAnimation();
      }
   }
   private void Animate()
   {
      rocks2.size += Time.deltaTime * rocks2Speed * new Vector2(0.05f, 0.00f);
      sky.size += Time.deltaTime * skySpeed * new Vector2(0.05f, 0.00f);
      sun.transform.localPosition += skySpeed * 0.2f * Time.deltaTime * Vector3.right;
      planet1.transform.localPosition += planet1Speed * Time.deltaTime * Vector3.right;
      planet2.transform.localPosition += planet2Speed * Time.deltaTime * Vector3.right;
   }
   
   private void WrapAnimation()
   {
      if (sun.transform.localPosition.x < -5) sun.transform.localPosition = new Vector3(5f, -2.98f, -3f);
      if (planet2.transform.localPosition.x > 11.5f) planet2.transform.localPosition = new Vector3(-11.5f, 6f, -2f);
      if (planet1.transform.localPosition.x > 13f) planet1.transform.localPosition = new Vector3(-13f, 12f, -2.5f);
   }
}
