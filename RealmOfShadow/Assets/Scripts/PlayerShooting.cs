using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletTransform;
    public float timeBetweenShots = 0.5f;
    [SerializeField] private AudioClip shoot;

    private bool canFire = true;
    private float fireTimer;
    private Player player;
    private PlayerAttack attack;
    private Health health;
    public bool canShoot = false;
    private ToggleSkillTree skillTree;
    private PauseMenu pauseMenu;

    private void Start()
    {
        player = GetComponentInParent<Player>();
        health = GetComponent<Health>();
        skillTree = FindObjectOfType<ToggleSkillTree>();
        attack = GetComponentInParent<PlayerAttack>();
        canShoot = GameManager.Instance.hasRangeAttack;
        pauseMenu = GetComponent<PauseMenu>();
    }

    private void Update()
    {
        if (skillTree != null && skillTree.IsSkillTreeOpen())
            return;

        if (pauseMenu != null && pauseMenu.IsPaused()) 
            return;

        if (System.Type.GetType("MapCameraController") != null && MapCameraController.isMapCameraActive)
            return;


        fireTimer += Time.deltaTime;

        if (fireTimer >= timeBetweenShots && !health.IsDead)
        {
            canFire = true;
        }

        if (Input.GetMouseButtonDown(1) && canFire && canShoot && !health.IsDead && !attack.isBlocking && !player.GetComponent<Animator>().GetBool("Crouch"))
        {
            FireBullet();
        }
    }

    private void FireBullet()
    {
        canFire = false;
        fireTimer = 0;

        float direction = Mathf.Sign(player.transform.localScale.x);
        float offsetX = 0.5f;

        Vector3 bulletSpawnPosition = bulletTransform.position;
        bulletSpawnPosition.x = player.transform.position.x + (direction * offsetX);

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPosition, Quaternion.identity);

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(direction);
        }

        SoundManager.instance.PlaySound(shoot);
    }
}
