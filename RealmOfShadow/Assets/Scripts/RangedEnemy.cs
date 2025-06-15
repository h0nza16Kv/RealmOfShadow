using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("Ranged Attack")]
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject[] fireballs;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    private EnemyPatrol enemyPatrol;
    private Health playerHealth;
    private Animator anim;  // Added Animator
    [SerializeField] private AudioClip shoot;

    private void Awake()
    {
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
        anim = GetComponent<Animator>();  // Initialize Animator

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<Health>();
        }
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (playerHealth != null && !playerHealth.IsDead)
        {
            if (PlayerInSight())
            {
                if (cooldownTimer >= attackCooldown)
                {
                    cooldownTimer = 0;
                    RangedAttack();
                }
                anim.SetBool("moving", false);  // Stop movement when attacking
            }
            else
            {
                anim.SetBool("moving", true);  // Enable movement when not attacking
            }

            if (enemyPatrol != null)
                enemyPatrol.enabled = !PlayerInSight();
        }
        else
        {
            if (enemyPatrol != null)
                enemyPatrol.enabled = false;
            anim.SetBool("moving", false);
        }
    }

    private void RangedAttack()
    {
        GameObject fireball = fireballs[FindFireball()];
        fireball.transform.position = firepoint.position;
        fireball.GetComponent<EnemyProjectile>().ActivateProjectile();
        SoundManager.instance.PlaySound(shoot);
    }

    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        return hit.collider != null && playerHealth != null && !playerHealth.IsDead;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
}
