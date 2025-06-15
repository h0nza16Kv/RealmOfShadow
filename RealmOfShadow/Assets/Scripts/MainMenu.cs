using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public CanvasGroup helpCanvasGroup;

    private void Start()
    {
        helpCanvasGroup.alpha = 0;
        helpCanvasGroup.blocksRaycasts = false;
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitApp()
    {
        Application.Quit();
    }

    public void OpenHelp()
    {
        helpCanvasGroup.alpha = 1;
        helpCanvasGroup.blocksRaycasts = true;
    }

    public void CloseHelp()
    {
        helpCanvasGroup.alpha = 0;
        helpCanvasGroup.blocksRaycasts = false;
    }
}
