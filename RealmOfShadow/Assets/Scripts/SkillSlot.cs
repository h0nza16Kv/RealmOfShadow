using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    public SkillSO skillSO;
    public Image skillIcon;
    public int currentLevel;
    public TMP_Text skillLevelText;
    public TMP_Text price;
    public Button skillButton;
    private SkillTree skillTree;

    public static event Action<SkillSlot> OnAbilityPointsSpent;

    public void Initialize(SkillTree skillTree)
    {
        this.skillTree = skillTree;
    }

    private void Start()
    {
        if (GameManager.Instance != null && skillSO != null)
        {
            currentLevel = GameManager.Instance.GetSkillLevel(skillSO.skillName);
            UpdateUI();
        }
    }

    private void OnValidate()
    {
        if (skillSO != null)
        {
            UpdateUI();
        }
    }

    public void TryUpgradeSkill()
    {
        if (currentLevel < skillSO.maxLevel && skillTree.availablePoints >= skillSO.price)
        {
            currentLevel++;
            GameManager.Instance.SetSkillLevel(skillSO.skillName, currentLevel);

            OnAbilityPointsSpent?.Invoke(this);
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        skillIcon.sprite = skillSO.skillIcon;
        skillButton.interactable = true;
        skillLevelText.text = currentLevel + "/" + skillSO.maxLevel;
        skillIcon.color = Color.white;
        price.text = skillSO.price.ToString();
    }

    public void ResetSkill()
    {
        currentLevel = 0;
        UpdateUI();
    }


}
