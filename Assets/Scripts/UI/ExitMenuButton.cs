using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitMenuButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void whenButtonClicked()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
