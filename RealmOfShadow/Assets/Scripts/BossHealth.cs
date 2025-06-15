using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    [SerializeField] private Teleport teleport;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool isDead = false;
    public bool IsDead => isDead;

    public int xp = 10; 
    public delegate void EnemyDefeated(int xp);
    public static event EnemyDefeated OnEnemyDefeated;

    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;

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

            GetComponent<Boss>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().simulated = false;
            teleport.ActivateTeleport();
            OnEnemyDefeated?.Invoke(xp);
        }
    }
}
