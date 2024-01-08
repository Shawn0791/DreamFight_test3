using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenMode : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    bool isHurt;
    float veloccityX;
    bool isOnGround;
    bool isJumping;
    bool jumpAttempted;

    [Header("移动")]
    public float accelerateTime;
    public float decelerateTime;
    public float moveSpeed;
    [Header("跳跃1")]
    public float jumpF;
    public float fallMultiplier;
    public float jumpMultiplier;
    [Header("跳跃2")]
    public float totalJumpTime;
    public float maxJumpTime;
    [Header("地面检测")]
    public Vector2 pointOffset;
    public Vector2 size;
    public LayerMask groundLayerMask;
    
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        //地面检测
        isOnGround = OnGround();
        movement();
        Jump1();
    }

    void movement()
    {
        //移动
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            rb.velocity = new Vector2(Mathf.SmoothDamp(rb.velocity.x,
                moveSpeed * Time.fixedDeltaTime * 60,ref veloccityX, accelerateTime), rb.velocity.y);
            anim.SetBool("isMoving", true);
        }
        else if(Input.GetAxisRaw("Horizontal") < 0)
        {
            rb.velocity = new Vector2(Mathf.SmoothDamp(rb.velocity.x, 
                moveSpeed * Time.fixedDeltaTime * -60,ref veloccityX, accelerateTime), rb.velocity.y);
            anim.SetBool("isMoving", true);
        }
        else
        {
            rb.velocity = new Vector2(Mathf.SmoothDamp(rb.velocity.x, 0, ref veloccityX, decelerateTime),
                rb.velocity.y);
            anim.SetBool("isMoving", false);
        }

        //转向
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        if(horizontalMove != 0)
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);
        }
    }
    //跳跃
    void Jump1()
    {
        if (Input.GetAxisRaw("Jump") == 1 && !isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpF);
            isJumping = true;
        }
        if (isOnGround && Input.GetAxisRaw("Jump") == 0)
        {
            isJumping = false;
        }
        //玩家下坠
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        //玩家上升且没按space
        else if (rb.velocity.y > 0 && Input.GetAxisRaw("Jump") != 1)
        {
            rb.velocity += Vector2.up * Physics2D.gravity * (jumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
    void Jump2()
    {
        if (jumpAttempted)
        {
            if (Input.GetAxisRaw("Jump") == 1)
            {
                totalJumpTime += Time.fixedDeltaTime;
                if (totalJumpTime <= maxJumpTime)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpF);
                }
                else
                {
                    jumpAttempted = false;
                }
            }
        }
           
        if (isOnGround && Input.GetAxisRaw("Jump") == 0)
        {
            jumpAttempted = true;
            totalJumpTime = 0;
        }

        if (rb.velocity.y < 0)
        {
            //下降
            rb.velocity += Physics2D.gravity * fallMultiplier * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0 && jumpAttempted)
        {
            //上升
            float t = totalJumpTime / maxJumpTime * 1;
            float tempJumpM = jumpMultiplier;

            if (t > 0.5f)
            {
                tempJumpM = jumpMultiplier * (1 - t);
            }

            rb.velocity += Physics2D.gravity * tempJumpM * Time.fixedDeltaTime;
        }
    }
    //地面检测碰撞器
    bool OnGround()
    {
        Collider2D Coll = Physics2D.OverlapBox((Vector2)transform.position + pointOffset, 
            size, 0, groundLayerMask);
        if (Coll != null)
            return true;
        else
            return false;
    }
    //显示地面碰撞器
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position + pointOffset, size);
    }
}
