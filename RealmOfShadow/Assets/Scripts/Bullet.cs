using Unity.Burst.CompilerServices;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 1f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(float direction)
    {
        rb.velocity = new Vector2(direction * speed, 0f);

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }

    private void Update()
    {
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
        if (enemyHealth != null && !enemyHealth.IsDead)
        {
            enemyHealth.TakeDamage(damage);
        }

        BossHealth bossHealth = collision.GetComponent<BossHealth>();
        if (bossHealth != null && !bossHealth.IsDead)
        {
            bossHealth.TakeDamage(damage);
        }
        FinalHealth final = collision.GetComponent<FinalHealth>();
        if (final != null && !final.IsDead)
        {
            final.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
