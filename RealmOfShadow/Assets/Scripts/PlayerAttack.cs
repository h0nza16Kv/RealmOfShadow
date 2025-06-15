using System.Collections;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private float heavyAttackCooldown = 5f;
    [SerializeField] private float fastAttackCooldown = 3f;
    [SerializeField] private float thunderAttackCooldown = 10f;
    [SerializeField] private float waveAttackCooldown = 7.5f;
    [SerializeField] private float attackRange = 1.0f;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private int heavyAttackDamage = 3;
    [SerializeField] private int fastAttackDamage = 2;
    [SerializeField] private int thunderAttackDamage = 10;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject wavePrefab;


    private Animator anim;
    private float cooldownTimer = Mathf.Infinity;
    private float heavyCooldownTimer = Mathf.Infinity;
    private float fastCooldownTimer = Mathf.Infinity;
    private float waveAttackCooldownTimer = Mathf.Infinity;
    private float thunderAttackCooldownTImer = Mathf.Infinity;
    private ToggleSkillTree skillTree;
    private PauseMenu pauseMenu;
    private Health playerHealth;
    private Player player;
    //private EnemyHealth enemyHealth;

    public bool canHeavyAttack = false;
    public bool canFastAttack = false;
    public bool canBlock = false;
    public bool canWaveAttack = false;
    public bool canThunderAttack = false;
    public bool isBlocking = false;

    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip thunderSound;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        skillTree = FindObjectOfType<ToggleSkillTree>();
        playerHealth = GetComponent<Health>();
        player = GetComponent<Player>();
        pauseMenu = GetComponent<PauseMenu>();
        //enemyHealth = GetComponent<EnemyHealth>();
    }

    private void Start()
    {
        canHeavyAttack = GameManager.Instance.hasHeavyAttack;
        canFastAttack = GameManager.Instance.hasFastAttack;
        canBlock = GameManager.Instance.hasShield;
        canWaveAttack = GameManager.Instance.hasWaveAttack;
        canThunderAttack = GameManager.Instance.hasThunderAttack;
    }

    private void Update()
    {
        if (skillTree != null && skillTree.IsSkillTreeOpen())
            return;

        if (pauseMenu != null && pauseMenu.IsPaused())
            return;

        if (System.Type.GetType("MapCameraController") != null && MapCameraController.isMapCameraActive)
            return;


        cooldownTimer += Time.deltaTime;
        heavyCooldownTimer += Time.deltaTime;
        fastCooldownTimer += Time.deltaTime;
        waveAttackCooldownTimer += Time.deltaTime;
        thunderAttackCooldownTImer += Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && cooldownTimer >= attackCooldown)
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.E) && canHeavyAttack && heavyCooldownTimer >= heavyAttackCooldown)
        {
            HeavyAttack();
        }

        if (Input.GetKeyDown(KeyCode.Q) && canFastAttack && fastCooldownTimer >= fastAttackCooldown)
        {
            FastAttack();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canBlock)
        {
            StartBlocking();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            StopBlocking();
        }
        if (Input.GetKeyDown(KeyCode.R) && canThunderAttack && thunderAttackCooldownTImer >= thunderAttackCooldown && !player.grounded)
        {
            ThunderAttack();
        }
        if (Input.GetKeyDown(KeyCode.F) && canWaveAttack && waveAttackCooldownTimer >= waveAttackCooldown)
        {
            WaveAttack();
        }
    }

    private void Attack()
    {
        cooldownTimer = 0;
        anim.SetTrigger("Attack1");
        PerformHit(attackDamage);
        SoundManager.instance.PlaySound(attackSound);
            
    }

    private void HeavyAttack()
    {
        heavyCooldownTimer = 0;
        anim.SetTrigger("HeavyAttack");
        PerformHit(heavyAttackDamage);
        SoundManager.instance.PlaySound(attackSound);
    }

    private void FastAttack()
    {
        fastCooldownTimer = 0;
        anim.SetTrigger("FastAttack");
    }


    private void WaveAttack()
    {
        waveAttackCooldownTimer = 0;
        anim.SetTrigger("WaveAttack");
        SoundManager.instance.PlaySound(attackSound);
    }

    public void SpawnWave()
    {
        Vector3 spawnPosition = transform.position + new Vector3(transform.localScale.x * 1.5f, 0.7f, 0);
        GameObject wave = Instantiate(wavePrefab, spawnPosition, Quaternion.identity);
        wave.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
    }


    public void FastAttackHit()
    {
        PerformHit(fastAttackDamage);
        SoundManager.instance.PlaySound(attackSound);
    }

    private void ThunderAttack()
    {
        thunderAttackCooldownTImer = 0;

        player.rb2d.velocity = new Vector2(player.rb2d.velocity.x, -20f);
        anim.SetTrigger("ThunderAttack");
        SoundManager.instance.PlaySound(thunderSound);
        StartCoroutine(WaitForLanding());
    }

    private void PerformHit(int damage)
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(
            transform.position + transform.right * attackRange * transform.localScale.x / 2,
            new Vector2(attackRange, 1.0f), 0, Vector2.zero, 0, enemyLayer);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                EnemyHealth enemyHealth = hit.transform.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);
                }

                BossHealth bossHealth = hit.transform.GetComponent<BossHealth>();
                if (bossHealth != null)
                {
                    bossHealth.TakeDamage(damage);
                }

                TrainingDummy dummy = hit.transform.GetComponent<TrainingDummy>();
                if (dummy != null)
                {
                    dummy.TriggerHurtAnimation();
                }
                FinalHealth final = hit.transform.GetComponent<FinalHealth>();
                if (final != null)
                {
                    final.TakeDamage(damage);
                }
            }
        }
    }

    private IEnumerator WaitForLanding()
    {
        yield return new WaitUntil(() => player.grounded);

        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(transform.position, 2.5f, enemyLayer);

        foreach (Collider2D enemy in enemiesHit)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(thunderAttackDamage);
            }

            BossHealth bossHealth = enemy.GetComponent<BossHealth>();
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(thunderAttackDamage);
            }

            FinalHealth final = enemy.transform.GetComponent<FinalHealth>();
            if (final != null)
            {
                final.TakeDamage(thunderAttackDamage);
            }
        }
    }



    private void StartBlocking()
    {
        isBlocking = true;
        anim.SetTrigger("Block");
        anim.SetBool("BlockIdle", true);
        playerHealth.ActivateBlock();
    }

    private void StopBlocking()
    {
        isBlocking = false;
        anim.ResetTrigger("Block");
        anim.Play("Idle", 0, 0f);
        anim.SetBool("BlockIdle", false);
        playerHealth.DeactivateBlock();
    }
}