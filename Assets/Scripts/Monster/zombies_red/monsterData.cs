using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class monsterData : MonoBehaviour
{
    private Animator anim;
    private Vector3 direction;
    private AnimatorStateInfo info;
    [SerializeField] private float hurtSpeed;
    [Header("Debug")]
    [SerializeField] private bool isChanged;
    [SerializeField] private bool isAttacking;
    [SerializeField] private bool isHurt;
    [SerializeField] private bool isChanging;
    [SerializeField] private int isHitNumber;
    [SerializeField] private bool superArmor;
    [SerializeField] private float superArmorTimer;

    public Transform player;
    public AIPath aiPath;
    [Header("追击属性")]
    public float trackDistance;
    public float changeDistance;
    public float maxSpeed;
    public float attackCD;
    [Header("怪物属性")]
    public float lightAttack;
    public float heavyAttack;
    public float maxHp;
    public float hp;

    void Start()
    {
        anim = transform.GetComponent<Animator>();
    }

    void Update()
    {
        Turn();
        Track();
        Change();
        Attack();
        HurtF();

        info = anim.GetCurrentAnimatorStateInfo(0);
    }

    //转向
    void Turn()
    {
        if (aiPath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (aiPath.desiredVelocity.x <= 0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    //攻击
    void Attack()
    {
        if (attackCD < 0)
            attackCD = 0;
        else if (attackCD > 0)
            attackCD -= Time.deltaTime;

        if (!isAttacking && 
            !isHurt && 
            attackCD == 0 &&
            player.GetComponent<PlayerData>().isDead == false &&
            (player.position - transform.position).sqrMagnitude <= 4)
        {
            isAttacking = true;
            transform.parent.GetComponent<AIPath>().canMove = false;
            anim.SetTrigger("attack");
            attackCD = 1.5f;
        }
    }
    //传递伤害和方向
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //传递伤害
        if (collision.CompareTag("Player") && isChanged == false)
        {
            collision.GetComponent<PlayerData>().TakeDamage(lightAttack);
        }
        else if (collision.CompareTag("Player") && isChanged == true)
        {
            collision.GetComponent<PlayerData>().TakeDamage(heavyAttack);
        }
        //传递方向
        if (collision.CompareTag("Player"))
        {
            if (transform.position.x > collision.transform.position.x)
                collision.GetComponent<PlayerData>().GetHit1(Vector3.left);
            if (transform.position.x < collision.transform.position.x)
                collision.GetComponent<PlayerData>().GetHit1(Vector3.right);
        }
    }
    //攻击结束
    void AttackFinish()
    {
        isAttacking = false;
        transform.parent.GetComponent<AIPath>().canMove = true;
    }

    //追击
    void Track()
    {
        if ((player.position - transform.position).sqrMagnitude < trackDistance *
            trackDistance && player.GetComponent<PlayerData>().isDead == false) //死亡后不追击
        {
            transform.parent.GetComponent<AIPath>().canSearch = true;
            //transform.parent.GetComponent<AIPath>().canMove = true;
            anim.SetBool("isMoving", true);
        }
        else
        {
            transform.parent.GetComponent<AIPath>().canSearch = false;
            //transform.parent.GetComponent<AIPath>().canMove = false;
            anim.SetBool("isMoving", false);
        }
    }
    //变身
    void Change()
    {
        if (!isChanged && (player.position - transform.position).sqrMagnitude
            < changeDistance * changeDistance)
        {
            isChanging = true;//变身途中不允许攻击
            isChanged = true;
            isAttacking = true;
            transform.parent.GetComponent<AIPath>().canMove = false;
            anim.SetTrigger("change");
        }
    }
    //变身完成
    void ChangeFinish()
    {
        isChanging = false;
        isAttacking = false;//变身结束后才允许攻击
        anim.SetBool("changed", true);
        transform.parent.GetComponent<AIPath>().canMove = true;
        //变身之后的移速
        transform.parent.GetComponent<AIPath>().maxSpeed = maxSpeed;
    }
    //受击掉血
    public void TakeDamage(float _damage)
    {
        //掉血
        hp -= _damage;
        //死亡
        if (hp <= 0)
        {
            anim.SetTrigger("isDead");
        }
    }
    //受击朝向
    public void GetHit(Vector3 direction)
    {
        isHitNumber++;//受击次数累计

        if (!isChanging && !superArmor) 
        {
            this.direction = direction;
            if (hp > 0)
            {
                transform.localScale = new Vector3(-direction.x, 1, 1);
                isHurt = true;
                anim.SetTrigger("isHurt");
            }
        }
    }
    //受击后退以及多次受击后的霸体
    void HurtF()
    {
        if (isHurt && !isChanging ) 
        {
            transform.parent.position = Vector3.MoveTowards(transform.parent.position,
                transform.parent.position + direction, hurtSpeed * Time.deltaTime);

            //受击结束
            if (info.normalizedTime >= .6f)
            {
                isHurt = false;
                isAttacking = false;
                transform.parent.GetComponent<AIPath>().canMove = true;
            }
        }
        //多次受击产生霸体
        if (isHitNumber >= 3 && !superArmor) 
        {
            superArmor = true;
            superArmorTimer = 3;
            isHitNumber = 0;
        }
        //霸体倒计时
        if (superArmorTimer > 0)
        {
            superArmorTimer -= Time.deltaTime;
        }
        if (superArmorTimer < 0)
        {
            superArmorTimer = 0;
            superArmor = false;
            isHitNumber = 0;
        }
    }

    //void PointData()
    //{
    //    //目标位置
    //    Vector2 target = new Vector2(player.transform.position.x, player.transform.position.y);
    //    //顶点坐标
    //    List<Vector2> Points = new List<Vector2>();
    //    Vector2 topL = new Vector2(transform.position.x - 5, transform.position.y + 5);
    //    Vector2 topR = new Vector2(transform.position.x + 5, transform.position.y + 5);
    //    Vector2 bottomL = new Vector2(transform.position.x - 5, transform.position.y - 5);
    //    Vector2 bottomR = new Vector2(transform.position.x + 5, transform.position.y - 5);
    //    Points.Add(topL);
    //    Points.Add(topR);
    //    Points.Add(bottomL);
    //    Points.Add(bottomL);
    //    //传递数值给ContainsPoint
    //    ContainsPoint(Points, target);

    //    //追击
    //    if (ContainsPoint(Points, target))
    //    {
    //        transform.position = Vector3.MoveTowards(transform.position,
    //            player.position, moveSpeed * Time.deltaTime);
    //        Debug.Log("yep");
    //    }
    //    else
    //    {
    //        transform.position = new Vector3(-125,-58,0);
    //    }
    //}

    ////判断目标是否在范围内
    //public static bool ContainsPoint(List<Vector2>polyPoints,Vector2 point)
    //{
    //    var j = polyPoints.Count-1;
    //    var inside = false;
    //    for (int i = 0; i < polyPoints.Count; j = i++)
    //    {
    //        var pi = polyPoints[i];
    //        var pj = polyPoints[j];
    //        if (((pi.y <= point.y && point.y < pj.y) || (pj.y <= point.y && point.y < pi.y)) &&
    //            (point.x < (pj.x - pi.x) * (point.y - pi.y) / (pj.y - pi.y) + pi.x))
    //            inside = !inside;
    //    }
    //    return inside;
    //}


}
