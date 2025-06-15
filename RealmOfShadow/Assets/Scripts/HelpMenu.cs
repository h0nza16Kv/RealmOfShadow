using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public CanvasGroup helpCanvasGroup;  
    public CanvasGroup menuCanvasGroup;  

    private void Start()
    {
        helpCanvasGroup.alpha = 0;
        helpCanvasGroup.blocksRaycasts = false;

        if (SceneManager.GetActiveScene().name == "Menu")
        {
            menuCanvasGroup.alpha = 1;
            menuCanvasGroup.blocksRaycasts = true;
        }
        else
        {
            menuCanvasGroup.alpha = 0;
            menuCanvasGroup.blocksRaycasts = false;
        }
    }

    public void ShowHelp()
    {
        helpCanvasGroup.alpha = 1; 
        helpCanvasGroup.blocksRaycasts = true;  
        menuCanvasGroup.alpha = 0; 
        menuCanvasGroup.blocksRaycasts = false; 
        Time.timeScale = 0;  
    }

    public void CloseHelp()
    {
        helpCanvasGroup.alpha = 0;
        helpCanvasGroup.blocksRaycasts = false;
        menuCanvasGroup.alpha = 1;
        menuCanvasGroup.blocksRaycasts = true;

        
        if (!FindObjectOfType<PauseMenu>().IsPaused())
        {
            Time.timeScale = 1;
        }
    }

    public void CloseHelp2()
    {
        helpCanvasGroup.alpha = 0;
        helpCanvasGroup.blocksRaycasts = false;
        menuCanvasGroup.alpha = 1;
        menuCanvasGroup.blocksRaycasts = true;
    }

}
