using UnityEngine;

public class FinalHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 50f;
    [SerializeField] private float healThreshold = 25f;
    [SerializeField] private float healAmount = 25f;
    [SerializeField] private Teleport teleport; 

    private float currentHealth;
    private bool isDead = false;
    private bool hasHealed = false;
    private Animator anim;

    public bool IsDead => isDead;

    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;

    private void Awake()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("Hurt");
            SoundManager.instance.PlaySound(hurtSound);

            if (currentHealth <= healThreshold && !hasHealed)
            {
                HealBoss();
            }
        }
        else
        {
            Die();
        }
    }

    private void HealBoss()
    {
        currentHealth = Mathf.Clamp(currentHealth + healAmount, 0, maxHealth);
        hasHealed = true;
    }

    private void Die()
    {
        isDead = true;
        anim.SetTrigger("Death");
        SoundManager.instance.PlaySound(deathSound);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
        teleport.ActivateTeleport();
    }
}
