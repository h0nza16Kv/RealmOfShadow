using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DiedScene : MonoBehaviour
{
    public void ReturnToMenu()
    {
        GameManager.Instance.ResetData(); 
        SceneManager.LoadScene(0);
    }

    public void QuitApp()
    {
        Application.Quit();
    }
}

