using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMode : MonoBehaviour
{
    private float inputX, inputY;
    public Rigidbody2D rb;
    private Animator anim;
    private int combo;
    private float timer;
    private float veloccityX;
    private bool isJumping;

    public bool isOnGround;//敌人检测是否在地面
    public bool isAttack;
    public float attack;

    public float speed;
    public float attackSpeed;
    [Header("跳跃1")]
    public float jumpF;
    public float fallMultiplier;
    public float jumpMultiplier;
    [Header("地面检测")]
    public Vector2 pointOffset;
    public Vector2 size;
    public LayerMask groundLayerMask;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (GameManager.instance.gameMode == GameManager.GameMode.Normal)
        {
            movement();
            Attack();
            Jump1();
        }
        isOnGround = OnGround();
    }
    //2D移动
    void movement()
    {
        if (transform.GetComponent<PlayerData>().isHurt == false &&
            transform.GetComponent<PlayerData>().isDead == false &&
            !isAttack)//受击、死亡、攻击状态下不允许移动
        {
            //移动
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                rb.velocity = new Vector2(Mathf.SmoothDamp(rb.velocity.x,
                    speed * Time.fixedDeltaTime * 60, ref veloccityX, 0.1f), rb.velocity.y);
                anim.SetBool("isMoving", true);
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                rb.velocity = new Vector2(Mathf.SmoothDamp(rb.velocity.x,
                    speed * Time.fixedDeltaTime * -60, ref veloccityX, 0.1f), rb.velocity.y);
                anim.SetBool("isMoving", true);
            }
            else
            {
                rb.velocity = new Vector2(Mathf.SmoothDamp(rb.velocity.x, 0, ref veloccityX, 0.1f),
                    rb.velocity.y);
                anim.SetBool("isMoving", false);
            }
        }
        else if (transform.GetComponent<PlayerData>().isHurt == false &&
            transform.GetComponent<PlayerData>().isDead == false &&
            isAttack)//攻击状态下补偿速度
        {
            rb.velocity = new Vector2(transform.localScale.x * attackSpeed, rb.velocity.y);
        }


        //转向
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        if (horizontalMove != 0 && !isAttack && transform.GetComponent<PlayerData>
            ().isDead == false)  //死亡和攻击状态不允许转向
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

    //攻击
    void Attack()
    {
        //鼠标左键攻击
        if (Input.GetMouseButtonDown(0) &&
            !isAttack &&//没在攻击状态
            transform.GetComponent<PlayerData>().isHurt == false &&//没在受击状态
            transform.GetComponent<PlayerData>().isDead == false &&//没死
            transform.GetComponent<Flyable>().flyable == false) //不在飞翔
        {
            rb.velocity = Vector2.zero;//停止移动
            isAttack = true;
            //连击数
            combo++;
            if (combo > 2)
                combo = 1;
            timer = 1;

            anim.SetTrigger("Attack");
            anim.SetInteger("Combo", combo);
        }
        //延时连击
        if (timer != 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                combo = 0;
            }
        }
    }
    //攻击结束
    public void AttackFinish()
    {
        isAttack = false;
    }

    //攻击
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //造成伤害
        if (collision.CompareTag("Enemy"))
        {
            //获取interface
            InterFaces interfaces = collision.gameObject.GetComponent<InterFaces>();
            //计算方向
            Vector3 dir = transform.position - collision.transform.position;
            //传递伤害
            interfaces.GetHitBack(attack, dir, 500);
        }
    }
}
