using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class XpManager : MonoBehaviour
{
    public int level;
    public int currentXp;
    public int xpToLevel = 10;
    public float xpGrow = 1.2f;
    public Slider xpSlider;
    public TMP_Text currentLevelText;
    public SkillTree skillTree;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            level = GameManager.Instance.playerLevel;
            currentXp = GameManager.Instance.currentXp;
            xpToLevel = GameManager.Instance.xpToLevel;
        }

        UpdateUI();
    }

    private void OnEnable()
    {
        EnemyHealth.OnEnemyDefeated += GainXp;
        BossHealth.OnEnemyDefeated += GainXp;
    }

    private void OnDisable()
    {
        EnemyHealth.OnEnemyDefeated -= GainXp;
        BossHealth.OnEnemyDefeated -= GainXp;
    }

    public void GainXp(int amount)
    {
        currentXp += amount;
        if (currentXp >= xpToLevel)
        {
            LevelUp();
        }
        UpdateGameManager();
        UpdateUI();
    }

    private void LevelUp()
    {
        if (level < 10) 
        {
            level++;
            currentXp -= xpToLevel;

            if (level < 10)
            {
                xpToLevel = Mathf.RoundToInt(xpToLevel * xpGrow);
            }
            else
            {
                currentXp = xpToLevel; 
            }

            if (skillTree != null)
            {
                skillTree.UpdateAbilityPoints(2);
            }

            UpdateGameManager();
        }
    }


    private void UpdateGameManager()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.playerLevel = level;
            GameManager.Instance.currentXp = currentXp;
            GameManager.Instance.xpToLevel = xpToLevel;
        }
    }

    public void GainXpFromItem(int amount)
    {
        GainXp(amount);
    }

    private void UpdateUI()
    {
        xpSlider.maxValue = xpToLevel;
        xpSlider.value = currentXp;
        currentLevelText.text = level.ToString();
    }
}
