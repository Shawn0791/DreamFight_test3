using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMode : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private float flipY;
    private Vector2 mousePos;

    private float veloccityX;
    public bool isOnGround;//敌人检测是否在地面
    private bool isJumping;

    private Camera gunCamera;

    [Header("移动")]
    public float accelerateTime;
    public float decelerateTime;
    public float moveSpeed;
    [Header("跳跃1")]
    public float jumpF;
    public float fallMultiplier;
    public float jumpMultiplier;
    [Header("地面检测")]
    public Vector2 pointOffset;
    public Vector2 size;
    public LayerMask groundLayerMask;


    void Start()
    {
        gunCamera = GameObject.FindGameObjectWithTag("GunCam").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        flipY = transform.localScale.y;
    }

    void FixedUpdate()
    {
        if (GameManager.instance.gameMode == GameManager.GameMode.Normal)
        {
            movement();
            Jump1();
            Flip();
        }

        //地面检测
        isOnGround = OnGround();
    }

    void movement()
    {
        //移动
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            rb.velocity = new Vector2(Mathf.SmoothDamp(rb.velocity.x,
                moveSpeed * Time.fixedDeltaTime * 60, ref veloccityX, accelerateTime), rb.velocity.y);
            anim.SetBool("isMoving", true);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            rb.velocity = new Vector2(Mathf.SmoothDamp(rb.velocity.x,
                moveSpeed * Time.fixedDeltaTime * -60, ref veloccityX, accelerateTime), rb.velocity.y);
            anim.SetBool("isMoving", true);
        }
        else
        {
            rb.velocity = new Vector2(Mathf.SmoothDamp(rb.velocity.x, 0, ref veloccityX, decelerateTime),
                rb.velocity.y);
            anim.SetBool("isMoving", false);
        }
    }
    
    //翻转
    void Flip()
    {
        //获取鼠标位置
        mousePos = gunCamera.ScreenToWorldPoint(Input.mousePosition);

        //翻转人物
        if (mousePos.x < transform.position.x)
            transform.localScale = new Vector3(-flipY, flipY, 1);
        else
            transform.localScale = new Vector3(flipY, flipY, 1);
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
            //设置动画
            anim.SetBool("JumpDown", false);
            anim.SetBool("JumpUp", false);
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
        //设置动画
        if (!isOnGround)
        {
            float latestY = transform.position.y;
            if (rb.velocity.y > 0)
            {
                //设置动画
                anim.SetBool("JumpDown", false);
                anim.SetBool("JumpUp", true);
            }
            else if (rb.velocity.y < 0)
            {
                //设置动画
                anim.SetBool("JumpDown", true);
                anim.SetBool("JumpUp", false);
            }
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
