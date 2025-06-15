using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Health : MonoBehaviour
{
    public float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool isDead = false;
    public bool IsDead => isDead;
    public bool isDamageBlocked = false;

    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;

    private void Awake()
    {
        if (GameManager.Instance != null)
        {
            startingHealth = GameManager.Instance.playerMaxHealth;
        }

        currentHealth = startingHealth;

        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        if (isDead || isDamageBlocked)
            return;

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("Hurt");
            SoundManager.instance.PlaySound(hurtSound);
        }
        else
        {
            anim.SetTrigger("Death");

            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Player>().enabled = false;
            GetComponent<PlayerAttack>().enabled = false;
            SoundManager.instance.PlaySound(deathSound);
            isDead = true;
            StartCoroutine(DelayedDeath());
        }
    }

    private IEnumerator DelayedDeath()
    {
        yield return new WaitForSeconds(3f);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetData(); 
        }

        SceneManager.LoadScene("Death");
    }

    public void ActivateBlock() => isDamageBlocked = true;
    public void DeactivateBlock() => isDamageBlocked = false;

    public void AddHealth(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth + value, 0, startingHealth);
    }

    public void IncreaseMaxHealth(float amount)
    {
        startingHealth += amount;
        currentHealth = Mathf.Clamp(startingHealth + amount, 0, startingHealth);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.playerMaxHealth = startingHealth;
        }

        HealthBar healthBar = FindObjectOfType<HealthBar>();
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeathZone"))
        {
            TakeDamage(startingHealth);
        }
    }

}
