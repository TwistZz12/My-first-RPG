using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    [SerializeField]private float xSpeed;
    [SerializeField]private float ySpeed;
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

        AnimatorControllers();

        FlipController();
    }

    private void CollisionChecks()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);//发射探测线，起点，方向，终点，LayerMask指定探测的层
    }

    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void Movement()
    {
        rb.velocity = new Vector2(xInput * xSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        if(isGrounded)
        rb.velocity = new Vector2(rb.velocity.x, ySpeed);
    }

    private void AnimatorControllers()
    {
        bool isMoving = rb.velocity.x != 0;

        anim.SetFloat("yVelocity", rb.velocity.y);

        anim.SetBool("isMoving",isMoving);
        anim.SetBool("isGrounded", isGrounded);

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
        Gizmos.DrawLine(transform.position , new Vector3(transform.position.x,transform.position.y - groundCheckDistance));//可视化向下延伸的线条，调整跳跃触碰层
    }
}
