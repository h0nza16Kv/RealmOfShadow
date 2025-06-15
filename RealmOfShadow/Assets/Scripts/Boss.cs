using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float strongAttackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;
    [SerializeField] private int strongDamage;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;

    private float cooldownTimer = Mathf.Infinity;
    private float strongCooldownTimer = Mathf.Infinity;

    private Animator anim;
    private Transform player;
    private Health playerHealth;
    private Rigidbody2D rb;

    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip attackSound2;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (player == null) return;

        cooldownTimer += Time.deltaTime;
        strongCooldownTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            rb.velocity = Vector2.zero;

            if (strongCooldownTimer >= strongAttackCooldown)
            {
                strongCooldownTimer = 0;
                anim.SetTrigger("Attack2");
                SoundManager.instance.PlaySound(attackSound);
            }
            else if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("Attack1");
                SoundManager.instance.PlaySound(attackSound2);
            }

            anim.SetBool("Moving", false);
        }
        else
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

            if (direction.x > 0)
                transform.localScale = new Vector3(1, 1, 1);
            else if (direction.x < 0)
                transform.localScale = new Vector3(-1, 1, 1);

            anim.SetBool("Moving", true);
        }
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null && playerHealth != null && !playerHealth.IsDead;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamagePlayer()
    {
        if (PlayerInSight())
        {
            playerHealth.TakeDamage(damage);
        }
    }

    private void StrongDamagePlayer()
    {
        if (PlayerInSight())
        {
            playerHealth.TakeDamage(strongDamage);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && gameObject.layer != LayerMask.NameToLayer("Enemy"))
        {
            Player playerScript = collision.gameObject.GetComponent<Player>();
            if (playerScript != null)
            {
                float direction = collision.transform.position.x - transform.position.x;
                if (Mathf.Abs(direction) < 0.1f)
                    direction = 1f;

                direction = Mathf.Sign(direction);
                Vector2 knockback = new Vector2(direction * 5f, 5f);

                playerScript.ApplyKnockback(knockback);
            }
        }
    }
}

