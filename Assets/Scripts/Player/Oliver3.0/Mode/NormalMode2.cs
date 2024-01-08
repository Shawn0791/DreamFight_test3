using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMode2 : MonoBehaviour
{
    private float inputX, inputY;
    public Rigidbody2D rb;
    private Animator anim;
    public bool isAttack;
    private int combo;
    private float timer;

    public float speed;
    public float attackSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        movement();
        Attack();
    }
    //2.5D移动
    void movement()
    {
        if (transform.GetComponent<PlayerData>().isHurt==false&&
            transform.GetComponent<PlayerData>().isDead == false&&
            !isAttack)//受击、死亡、攻击状态下不允许移动
        {
            inputX = Input.GetAxisRaw("Horizontal");
            inputY = Input.GetAxisRaw("Vertical");
            Vector2 input = new Vector2(inputX, inputY).normalized;
            rb.velocity = input * speed;
        }
        else if(transform.GetComponent<PlayerData>().isHurt == false &&
            transform.GetComponent<PlayerData>().isDead == false &&
            isAttack)//攻击状态下补偿速度
        {
            rb.velocity = new Vector2(transform.localScale.x * attackSpeed, rb.velocity.y);
        }



        //移动动画
        if (inputX != 0||inputY!=0)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }

        //转向
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        if (horizontalMove != 0 && !isAttack&& transform.GetComponent<PlayerData>
            ().isDead == false)  //死亡和攻击状态不允许转向
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);
        }
    }
    //攻击
    void Attack()
    {
        //鼠标左键攻击
        if (Input.GetMouseButtonDown(0) && !isAttack && transform.GetComponent<PlayerData>()
            .isHurt == false && transform.GetComponent<PlayerData>().isDead == false) 
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
}
