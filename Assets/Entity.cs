using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator anim;

    protected int facingDir = 1;
    protected bool facingRight = true;

    [Header("Collision info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;

    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    protected bool isGrounded;
    protected bool isWallDected;


    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        if (wallCheck == null)
            wallCheck = transform;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CollisionChecks();
    }
    protected virtual void CollisionChecks()
    {
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);//发射探测线，起点，方向，终点，LayerMask指定探测的层
        isWallDected = Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance * facingDir, whatIsGround);
    }


    protected virtual void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }



    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));//可视化向下延伸的线条，调整跳跃触碰层
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
    }
}
