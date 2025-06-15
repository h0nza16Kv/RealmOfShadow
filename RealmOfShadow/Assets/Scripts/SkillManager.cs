using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private void OnEnable()
    {
        SkillSlot.OnAbilityPointsSpent += HandleAbilityPointSpent;
    }

    private void OnDisable()
    {
        SkillSlot.OnAbilityPointsSpent -= HandleAbilityPointSpent;
    }

    private void HandleAbilityPointSpent(SkillSlot slot)
    {
        string skillName = slot.skillSO.skillName;
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            PlayerShooting playerShooting = player.GetComponent<PlayerShooting>();
            PlayerAttack attackScript = player.GetComponent<PlayerAttack>();
            Player playerScript = player.GetComponent<Player>();
            Health playerHealth = player.GetComponent<Health>();

            if (playerScript != null && attackScript != null && playerHealth != null && playerShooting != null)
            {
                switch (skillName)
                {
                    case "HealthBoost":
                        playerHealth.IncreaseMaxHealth(1);
                        GameManager.Instance.playerMaxHealth = playerHealth.startingHealth; 
                        break;

                    case "DoubleJump":
                        playerScript.canDoubleJump = true;
                        GameManager.Instance.hasDoubleJump = true;
                        break;

                    case "HeavyAttack":
                        attackScript.canHeavyAttack = true;
                        GameManager.Instance.hasHeavyAttack = true;
                        break;

                    case "FastAttack":
                        attackScript.canFastAttack = true;
                        GameManager.Instance.hasFastAttack = true;
                        break;

                    case "Shield":
                        attackScript.canBlock = true;
                        GameManager.Instance.hasShield = true;
                        break;

                    case "Wave":
                        attackScript.canWaveAttack = true;
                        GameManager.Instance.hasWaveAttack = true;
                        break;

                    case "Thunder":
                        attackScript.canThunderAttack = true;
                        GameManager.Instance.hasThunderAttack = true;
                        break;

                    case "RangeAttack":
                        playerShooting.canShoot = true;
                        GameManager.Instance.hasRangeAttack = true;
                        break;
                }
            }
        }
    }
}