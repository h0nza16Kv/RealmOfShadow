using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float startingHealth = 100f;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool isDead = false;
    public bool IsDead => isDead;

    public int xp = 3;
    public delegate void EnemyDefeated(int xp);
    public static event EnemyDefeated OnEnemyDefeated;

    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;

    [SerializeField] private EnemyPatrol patrolScript;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("Hurt");
            SoundManager.instance.PlaySound(hurtSound);
        }
        else
        {
            anim.SetTrigger("Death");
            SoundManager.instance.PlaySound(deathSound);
            isDead = true;

            Collider2D col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null) rb.simulated = false;

            if (patrolScript != null)
            {
                patrolScript.enabled = false;
            }

            if (TryGetComponent<Enemy>(out var meleeEnemy))
            {
                meleeEnemy.enabled = false;
            }
            if (TryGetComponent<RangedEnemy>(out var rangedEnemy))
            {
                rangedEnemy.enabled = false;
            }

            OnEnemyDefeated?.Invoke(xp);
        }
    }

    
}
