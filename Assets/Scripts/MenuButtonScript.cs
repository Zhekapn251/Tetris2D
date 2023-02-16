using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MenuButtonScript : MonoBehaviour
{
    [SerializeField] private GameObject menuCanvas;
    // Start is called before the first frame update
    public void whenButtonClicked()
    {
        if (menuCanvas.activeInHierarchy == false)
        {
            Time.timeScale = 0;
            menuCanvas.SetActive(true);
            
        }
    }
}
