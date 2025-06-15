using TMPro;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    public SkillSlot[] slots;
    public TMP_Text pointsText;
    public int availablePoints;

    private void OnEnable()
    {
        SkillSlot.OnAbilityPointsSpent += HandleAbilityPointsSpent;
    }

    private void OnDisable()
    {
        SkillSlot.OnAbilityPointsSpent -= HandleAbilityPointsSpent;
    }

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            availablePoints = GameManager.Instance.abilityPoints;
        }

        foreach (SkillSlot slot in slots)
        {
            slot.Initialize(this);
            slot.skillButton.onClick.AddListener(() => CheckAvailablePoints(slot));
        }

        UpdateAbilityPoints(0);
    }

    private void CheckAvailablePoints(SkillSlot slot)
    {
        if (availablePoints >= slot.skillSO.price)
        {
            slot.TryUpgradeSkill();
        }
    }

    private void HandleAbilityPointsSpent(SkillSlot skillSlot)
    {
        UpdateAbilityPoints(-skillSlot.skillSO.price);
    }

    public void UpdateAbilityPoints(int amount)
    {
        availablePoints += amount;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.abilityPoints = availablePoints;
        }

        pointsText.text = "Points: " + availablePoints;
    }

    public void ResetAllSkills()
    {
        foreach (SkillSlot slot in slots)
        {
            slot.ResetSkill();
        }

        availablePoints = 0;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.abilityPoints = 0;
            GameManager.Instance.ResetData();
        }

        UpdateAbilityPoints(0);
    }
}
