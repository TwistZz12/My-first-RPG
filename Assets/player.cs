using UnityEngine;

public class Player : Entity
{

    [Header("Move info")]
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

    

    protected override void Start()
    {
        base.Start();
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        Movement();
        CheckInput();



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


    private void FlipController()
    {
        if (rb.velocity.x > 0 && !facingRight)
            Flip();
        else if (rb.velocity.x < 0 && facingRight)
            Flip();
    }
    
}
