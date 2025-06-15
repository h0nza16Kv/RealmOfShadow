using UnityEngine;

public class WaveAttack : MonoBehaviour
{
    public float speed = 7.5f;
    //private Rigidbody2D rb;

    public float damage = 4f;
    public float knockbackForce = 4f;

    private void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
        //rb.velocity = transform.right * speed * transform.localScale.x;
    }

    private void Update()
    {
        transform.Translate(Vector2.right * speed * transform.localScale.x * Time.deltaTime);
        CheckCollision();
        Destroy(gameObject, 3f);
    }

    private void CheckCollision()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 0.5f, LayerMask.GetMask("Enemy", "RangeEnemy", "Boss"));

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                enemy.transform.position += new Vector3(transform.localScale.x * 1.5f, 0, 0);
                Destroy(gameObject);
            }

            BossHealth bossHealth = enemy.GetComponent<BossHealth>();
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(damage);
                enemy.transform.position += new Vector3(transform.localScale.x * 1.5f, 0, 0);
                Destroy(gameObject);
            }

            FinalHealth final = enemy.GetComponent<FinalHealth>();
            if (final != null)
            {
                final.TakeDamage(damage);
                final.transform.position += new Vector3(transform.localScale.x * 1.5f, 0, 0);
                Destroy(gameObject);
            }
        }
    }



    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Health enemyHealth = collision.gameObject.GetComponent<Health>();

    //    if (enemyHealth != null && !enemyHealth.IsDead)
    //    {
    //        enemyHealth.TakeDamage(damage);

    //        Rigidbody2D enemyRb = collision.gameObject.GetComponent<Rigidbody2D>();
    //        if (enemyRb != null)
    //        {
    //            enemyRb.AddForce(Vector2.right * knockbackForce * transform.localScale.x, ForceMode2D.Impulse);
    //        }
    //        else
    //        {
    //            collision.gameObject.transform.position += new Vector3(transform.localScale.x * 1.5f, 0, 0);
    //        }
    //    }

    //    Destroy(gameObject);
    //}

}