using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public CanvasGroup pauseCanvas;
    public CanvasGroup helpCanvas;
    private ToggleSkillTree skillTree;
    private Health health;

    private bool isPaused = false;
    private bool isOpen = false;

    public bool IsPaused()
    {
        return isPaused;
    }

    public bool IsHelpOpen()
    {
        return helpCanvas.alpha > 0;
    }

    private void Start()
    {
        pauseCanvas.alpha = 0;
        pauseCanvas.blocksRaycasts = false;
        helpCanvas.alpha = 0;
        helpCanvas.blocksRaycasts = false;

        skillTree = FindObjectOfType<ToggleSkillTree>();
        health = FindObjectOfType<Health>();
    }

    private void Update()
    {
        if (health != null && !health.IsDead)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (IsHelpOpen())
                {
                    CloseHelp();
                }
                else if (!skillTree.IsSkillTreeOpen())
                {
                    if (isPaused || isOpen)
                    {
                        Resume();
                    }
                    else
                    {
                        Pause();
                    }
                }
            }
        }

        if (!isPaused && pauseCanvas.alpha > 0)
        {
            pauseCanvas.alpha = 0;
            pauseCanvas.blocksRaycasts = false;
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseCanvas.alpha = 0;
        pauseCanvas.blocksRaycasts = false;
        isOpen = false;
        isPaused = false;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pauseCanvas.alpha = 1;
        pauseCanvas.blocksRaycasts = true;
        isOpen = true;
        isPaused = true;
    }

    public void OpenHelp()
    {
        helpCanvas.alpha = 1;
        helpCanvas.blocksRaycasts = true;
    }

    public void CloseHelp()
    {
        helpCanvas.alpha = 0;
        helpCanvas.blocksRaycasts = false;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        GameManager.Instance.ResetData();
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
