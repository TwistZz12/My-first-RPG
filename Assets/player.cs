using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    [SerializeField] private float xSpeed;
    [SerializeField] private float ySpeed;
    [SerializeField] private float dashSpeed;

    [Header("Dash info")]
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;
    private float dashTime;
    private float dashCooldownTimer;

    [Header("Attack info")]
   [SerializeField] private float comboTime = .3f;
    private float comboTimeWindow;
    private bool isAttacking;
    private int comboCounter;

    private float xInput;


    private int facingDir = 1;
    private bool facingRight = true;

    [Header("Collision info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        CheckInput();

        CollisionChecks();


        dashTime -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;//从0自减
        comboTimeWindow -= Time.deltaTime;


        AnimatorControllers();

        FlipController();
    }

    public void AttackOver()
    {
        isAttacking = false;
        comboCounter++;

        if (comboCounter > 2)
            comboCounter = 0;

  
    }

    private void CollisionChecks()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);//发射探测线，起点，方向，终点，LayerMask指定探测的层
    }

    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartAttackEvent();

        }




        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))//触发条件赋值自减
        {
            DashAbility();
        }

    }

    private void StartAttackEvent()
    {
        if (!isGrounded)
            return;

        if (comboTimeWindow < 0)
            comboCounter = 0;



        isAttacking = true;
        comboTimeWindow = comboTime;
    }

    private void DashAbility()
    {
        if (dashCooldownTimer < 0 && !isAttacking)
        {
            dashTime = dashDuration;
            dashCooldown = dashCooldownTimer;
        }
    }

    private void Movement()
    {
        if (isAttacking)
        {
            rb.velocity = new Vector2 (0, 0);
        }
        else if (dashTime > 0)
        {
            rb.velocity = new Vector2(facingDir * dashSpeed, 0);
        }
        else
        {
            rb.velocity = new Vector2(xInput * xSpeed, rb.velocity.y);
        }
    }

    private void Jump()
    {
        if (isGrounded)
            rb.velocity = new Vector2(rb.velocity.x, ySpeed);
    }

    private void AnimatorControllers()
    {
        bool isMoving = rb.velocity.x != 0;

        anim.SetFloat("yVelocity", rb.velocity.y);//设置动画状态机的默认值
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isDashing", dashTime > 0);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetInteger("comboCounter", comboCounter);

    }
    private void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private void FlipController()
    {
        if (rb.velocity.x > 0 && !facingRight)
            Flip();
        else if (rb.velocity.x < 0 && facingRight)
            Flip();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));//可视化向下延伸的线条，调整跳跃触碰层
    }
}
