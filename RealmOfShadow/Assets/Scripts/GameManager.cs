using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int playerLevel = 1;
    public int currentXp = 0;
    public int xpToLevel = 10;
    public int abilityPoints = 0;
    public float playerMaxHealth = 5;

    public bool hasDoubleJump = false;
    public bool hasHeavyAttack = false;
    public bool hasFastAttack = false;
    public bool hasShield = false;
    public bool hasWaveAttack = false;
    public bool hasThunderAttack = false;
    public bool hasRangeAttack = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private Dictionary<string, int> skillLevels = new Dictionary<string, int>();

    public int GetSkillLevel(string skillName)
    {
        if (skillLevels.TryGetValue(skillName, out int level))
            return level;
        return 0;
    }

    public void SetSkillLevel(string skillName, int level)
    {
        skillLevels[skillName] = level;
    }

    public void ResetData()
    {
        playerLevel = 1;
        currentXp = 0;
        xpToLevel = 10;
        abilityPoints = 0;
        playerMaxHealth = 5;

        hasDoubleJump = false;
        hasHeavyAttack = false;
        hasFastAttack = false;
        hasShield = false;
        hasWaveAttack = false;
        hasThunderAttack = false;
        hasRangeAttack = false;
        skillLevels.Clear();
    }


}
