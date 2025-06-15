using UnityEngine;

public class ToggleSkillTree : MonoBehaviour
{
    public CanvasGroup canvas;
    private bool skillTreeOpen = false;
    private Health health;
    private PauseMenu pauseMenu;

    private void Start()
    {
        canvas.alpha = 0;
        health = FindObjectOfType<Health>();
        pauseMenu = FindObjectOfType<PauseMenu>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && health != null && !health.IsDead)
        {
            if (!pauseMenu.IsPaused())
            {
                if (skillTreeOpen)
                {
                    Time.timeScale = 1;
                    canvas.alpha = 0;
                    canvas.blocksRaycasts = false;
                    skillTreeOpen = false;
                }
                else
                {
                    Time.timeScale = 0;
                    canvas.alpha = 1;
                    canvas.blocksRaycasts = true;
                    skillTreeOpen = true;
                }
            }
        }
    }

    public bool IsSkillTreeOpen()
    {
        return skillTreeOpen;
    }
}
