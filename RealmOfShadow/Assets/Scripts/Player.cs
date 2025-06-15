using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 4.0f;
    [SerializeField] private float crouchSpeed = 3.0f;
    [SerializeField] private float jumpForce = 7.5f;

    private Animator anim;
    public Rigidbody2D rb2d;
    private BoxCollider2D col;
    public bool grounded = false;
    private bool hasDoubleJumped = false;
    public int direction = 1;

    public bool canDoubleJump = false;

    private bool isKnockbacked = false;
    private float knockbackDuration = 0.3f;
    private float knockbackTimer = 0f;


    void Start()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        canDoubleJump = GameManager.Instance.hasDoubleJump;
    }

    void Update()
    {
        if (isKnockbacked)
        {
            knockbackTimer += Time.deltaTime;
            if (knockbackTimer >= knockbackDuration)
            {
                isKnockbacked = false;
                knockbackTimer = 0f;
            }
            else
            {
                return;
            }
        }

        HandleInput();
        UpdateAnimatorParameters();
        UpdateColliderSize();
        UpdateGroundedState();
    }

    void UpdateGroundedState()
    {
        grounded = IsTouchingGround();
        anim.SetBool("Grounded", grounded);
    }

    void HandleInput()
    {
        if (System.Type.GetType("MapCameraController") != null && MapCameraController.isMapCameraActive)
            return;

        float inputX = Input.GetAxis("Horizontal");

        bool isCrouching = Input.GetKey(KeyCode.C);
        float movementSpeed = isCrouching ? crouchSpeed : speed;
        rb2d.velocity = new Vector2(inputX * movementSpeed, rb2d.velocity.y);

        if (inputX > 0)
            direction = 1;
        else if (inputX < 0)
            direction = -1;

        transform.localScale = new Vector3(direction, transform.localScale.y, transform.localScale.z);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded)
            {
                Jump();
            }
            else if (canDoubleJump && !hasDoubleJumped)
            {
                Jump();
                hasDoubleJumped = true;
            }
        }
    }

    void Jump()
    {
        grounded = false;
        anim.SetTrigger("Jump");
        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            hasDoubleJumped = false;
            grounded = true;
        }
    }

    private bool IsTouchingGround()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 0.2f, LayerMask.GetMask("Ground"));
    }

    void UpdateAnimatorParameters()
    {
        anim.SetFloat("AirSpeedY", rb2d.velocity.y);

        bool isCrouching = Input.GetKey(KeyCode.C);
        anim.SetBool("CrouchIdle", isCrouching);

        if (isCrouching)
        {
            anim.SetInteger("AnimState", 2);
        }
        else if (Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon)
        {
            anim.SetInteger("AnimState", 1);
        }
        else
        {
            anim.SetInteger("AnimState", 0);
        }
    }

    void UpdateColliderSize()
    {
        if (col != null)
        {
            bool isCrouching = anim.GetBool("CrouchIdle");

            float newHeight = isCrouching ? 0.83f : 1.2f;
            float heightDifference = col.size.y - newHeight;

            col.size = new Vector2(col.size.x, newHeight);
            col.offset = new Vector2(col.offset.x, col.offset.y - (heightDifference / 2f));
        }
    }

    public void ApplyKnockback(Vector2 force)
    {
        isKnockbacked = true;
        knockbackTimer = 0f;
        rb2d.velocity = Vector2.zero;  
        rb2d.AddForce(force, ForceMode2D.Impulse);
    }

}
