using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Chewer : MonoBehaviour,InterFaces
{
    public AIPath aiPath;
    public Transform player;
    [Header("属性")]
    public float trackDistance;
    public float attackDistance;
    public float attackCD;
    public float maxHp;
    public float hp;
    public float attack;

    private Animator anim;
    [SerializeField]private bool isAttacking;
    [SerializeField]private bool isHurt;
    [SerializeField]private float cd;
    [SerializeField] private bool isDead;
    private SpriteRenderer sp;
    private Transform trackedPoint;
    private Rigidbody2D rb;

    private void Start()
    {
        anim = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        trackedPoint= GameObject.FindGameObjectWithTag("TrackedPoint").transform;
        GetComponent<AIDestinationSetter>().target = player.transform;
    }

    void Update()
    {
        if (!isDead)
        {
            Track();
            Attack();
            Turn();
        }
    }

    //基于AIPath的追击
    void Track()
    {
        //设定距离内可追击
        if ((trackedPoint.position - transform.position).sqrMagnitude < trackDistance * trackDistance &&
            (trackedPoint.position - transform.position).sqrMagnitude >= attackDistance &&
            player.GetComponent<PlayerData>().isDead == false)  //死亡后不追击
        {
            aiPath.canSearch = true;
            //transform.GetComponent<AIPath>().canMove = true;
            anim.SetBool("isMoving", true);
        }
        else
        {
            aiPath.canSearch = false;
            //transform.GetComponent<AIPath>().canMove = false;
            anim.SetBool("isMoving", false);
        }
    }

    //基于AIPath的攻击
    void Attack()
    {
        //攻击冷却
        if (cd > 0)
            cd -= Time.deltaTime;

        if (!isAttacking &&//不在攻击
            !isHurt &&//不在受击状态
            cd <= 0 &&//cd冷却完成
            player.GetComponent<PlayerData>().isDead == false &&//角色没死
            (trackedPoint.position - transform.position).sqrMagnitude <= attackDistance)//距离足够
        {
            isAttacking = true;
            aiPath.canMove = false;
            anim.SetTrigger("Attack");
            cd = attackCD;
        }
    }
    //造成伤害
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //获取interface
            InterFaces interfaces = collision.gameObject.GetComponent<InterFaces>();
            //计算方向
            Vector3 dir = transform.position - trackedPoint.position;
            //传递伤害
            interfaces.GetHitBack(attack, dir, 350);
        }
    }

    //攻击结束
    void AttackFinish()
    {
        isAttacking = false;
        aiPath.canMove = true;
    }

    //基于AIPath的转向
    void Turn()
    {
        if (aiPath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (aiPath.desiredVelocity.x <= 0.01f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    //受击掉血
    public void GetHit(float damage)
    {
        //造成伤害
        hp -= damage;
        //判断死亡
        if (hp <= 0)
        {
            Dead();
        }
        else
        {
            //停顿
            StartCoroutine(Stop());
        }
    }

    //受击掉血且击退
    public void GetHitBack(float damage, Vector3 dir, float force)
    {
        //造成伤害
        hp -= damage;
        anim.SetTrigger("Hurt");
        //闪白
        StartCoroutine(HurtShader());
        //后退
        //transform.position = Vector3.MoveTowards(transform.position, transform.position - dir, force);
        rb.AddForce(-dir * force);
        //判断死亡
        if (hp <= 0)
            Dead();
    }

    //受击闪白
    IEnumerator HurtShader()
    {
        sp.material.SetFloat("_FlashAmount", 1);
        yield return new WaitForSeconds(0.1f);
        sp.material.SetFloat("_FlashAmount", 0);
    }
    //受击停顿
    IEnumerator Stop()
    {
        aiPath.canMove = false;
        yield return new WaitForSeconds(0.1f);
        aiPath.canMove = true;
    }
    //死亡
    void Dead()
    {
        anim.SetTrigger("Dead");
        aiPath.canSearch = false;
        aiPath.canMove = false;
        transform.GetComponent<AIDestinationSetter>().target = null;
        transform.GetComponent<Rigidbody2D>().gravityScale = 3;
        isDead = true;
        gameObject.layer = LayerMask.NameToLayer("Dead");
    }
}
