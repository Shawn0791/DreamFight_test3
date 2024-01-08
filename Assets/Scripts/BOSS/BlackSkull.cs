using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackSkull : MonoBehaviour
{
    public GameObject fireCreaterPrefab;
    public GameObject fireCastPrefab;
    public GameObject blackSwordPrefab;
    public GameObject shadowMove_downPrefab;
    public GameObject ChewerPrefab;
    public GameObject Walker1Prefab;
    public Transform monsterCreatePoint;
    public GameObject FistAttack1Prefab;


    [Header("激光信息")]
    public Transform Attack2_DPoint;
    public GameObject attackD;
    public float maxDist;
    public GameObject LaserAttackVFX;
    public GameObject LaserHurtVFX;
    public float rotateSpeed;
    private bool lasering;
    private float laserTimer;
    float start1 = 150;
    float start2 = 200;

    public Transform[] FirePoints;
    public Transform[] SwordPoints;

    public bool shadowCreateStart;
    public bool dash;
    public GameObject skullShadowPrefab;
    public Vector2 target;

    [Header("角色信息")]
    public float maxHp;
    public float hp;
    public float attack;

    private Transform player;
    private Animator anim;
    private SpriteRenderer sp;
    private Rigidbody2D rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        StartCoroutine(DashMove());
        ShadowCreater();
        Attack2_D();
    }

    //攻击
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //造成伤害
        if (collision.CompareTag("Player"))
        {
            //获取interface
            InterFaces interfaces = collision.gameObject.GetComponent<InterFaces>();
            //计算方向
            Vector3 dir = transform.position - collision.transform.position;
            //传递伤害
            interfaces.GetHitBack(attack, dir, 500);
        }
    }

    public void GetHit(float damage)
    {
        //造成伤害
        hp -= damage;
        //判断死亡
        if (hp <= 0)
        {
            Dead();
        }
    }

    public void GetHitBack(float damage, Vector3 dir, float force)
    {
        if (anim.GetBool("Rage") == false)
        {
            //造成伤害
            hp -= damage;
            anim.SetTrigger("Hurt");
            //闪白
            StartCoroutine(HurtShader());
            //后退
            rb.AddForce(-dir * force);
        }
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

    void Dead()
    {
        anim.SetTrigger("Dead");
        gameObject.layer = LayerMask.NameToLayer("Dead");
    }

    //火焰追踪
    public IEnumerator CreatFire()
    {
        for (int i = 0; i < FirePoints.Length; i++)
        {
            //生成一个火焰
            GameObject fire = ObjectPool.Instance.GetObject(fireCreaterPrefab);
            //放到设定位置
            fire.transform.position = FirePoints[i].position;

            yield return new WaitForSeconds(0.75f);
        }
    }

    //魔剑追踪
    public IEnumerator CreateSword()
    {
        for (int i = 0; i < SwordPoints.Length; i++)
        {
            //生成一把剑
            GameObject sword= ObjectPool.Instance.GetObject(blackSwordPrefab);
            //放到指定位置
            sword.transform.position = SwordPoints[i].position;

            yield return new WaitForSeconds(0.3f);
        }
    }

    //抗拒火环
    public void FireCast()
    {
        GameObject cast= ObjectPool.Instance.GetObject(fireCastPrefab);
        cast.transform.position = new Vector2(transform.position.x, transform.position.y + 1);
    }

    //暗影移动（浮出地面）
    public void ShadowMove()
    {
        //生成下降残影（沉入地面）
        GameObject down = ObjectPool.Instance.GetObject(shadowMove_downPrefab);
        down.transform.position = transform.position;
        down.transform.localScale = transform.localScale;
        //自身瞬移到目标点
        transform.position = target;
        //面向玩家
        Turn();
    }
    
    //冲刺
    IEnumerator DashMove()
    {
        if (dash)
        {
            //移动
            transform.position = Vector2.MoveTowards(transform.position, target, 1);
            yield return new WaitForSeconds(0.5f);
            dash = false;
        }
    }

    //生成残影
    public void ShadowCreater()
    {
        if (shadowCreateStart)
        {
            GameObject shadow = ObjectPool.Instance.GetObject(skullShadowPrefab);
            shadow.transform.position = transform.position;
        }
    }

    //召唤Chewer六只
    public IEnumerator CreateChewer()
    {
        for (int i = 0; i < 6; i++)
        {
            GameObject chewer = ObjectPool.Instance.GetObject(ChewerPrefab);
            chewer.transform.position = monsterCreatePoint.position;

            yield return new WaitForSeconds(0.16f);
        }
    }

    //召唤Walker1四只
    public IEnumerator CreateWalker1()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject walker1 = ObjectPool.Instance.GetObject(Walker1Prefab);
            walker1.transform.position = monsterCreatePoint.position;

            yield return new WaitForSeconds(0.25f);
        }
    }

    //幻影三连锤
    public IEnumerator TripleFistAttack()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject fist = ObjectPool.Instance.GetObject(FistAttack1Prefab);
            fist.transform.position = new Vector2(player.transform.position.x + 5, player.transform.position.y + 4.5f);

            yield return new WaitForSeconds(0.5f);
        }
    }

    //瞬移
    public void DashAppear()
    {
        //自身瞬移到目标点
        transform.position = target;
        //面向玩家
        Turn();
        //出现
        anim.SetTrigger("DashAppear");
    }

    //口吐激光（Attack D）
    private void Attack2_D()
    {
        if (lasering)
        {

            //激光时间
            if (laserTimer > 0)
            {
                laserTimer -= Time.deltaTime;

                //射线检测
                RaycastHit2D hitInfo = Physics2D.Raycast(Attack2_DPoint.position, Attack2_DPoint.up, maxDist);
                LaserAttackVFX.SetActive(true);
                attackD.SetActive(true);
                //如果击中
                if (hitInfo.collider != null)
                {
                    //击中特效
                    LaserHurtVFX.SetActive(true);
                    LaserHurtVFX.transform.position = hitInfo.point;
                    LaserHurtVFX.transform.rotation = Attack2_DPoint.rotation;
                }
                else
                {
                    //击中特效
                    LaserHurtVFX.SetActive(false);
                }
                //计算角度
                Vector2 dir = player.position - new Vector3(transform.position.x, transform.position.y - 1, 0);
                float rad = Mathf.Acos(Vector2.Dot(transform.up, dir.normalized));
                float ang = rad * Mathf.Rad2Deg;
                //转动
                if (player.transform.position.x < transform.position.x)
                {
                    Attack2_DPoint.rotation = Quaternion.Euler(0, 0, start1 -= 20 * Time.deltaTime);
                }
                else if (player.transform.position.x > transform.position.x) 
                {
                    Attack2_DPoint.rotation = Quaternion.Euler(0, 0, start2 += 20 * Time.deltaTime);
                }
            }
            else
            {
                lasering = false;
                LaserAttackVFX.SetActive(false);
                LaserHurtVFX.SetActive(false);
                attackD.SetActive(false);
                start1 = 150;
                start2 = 200;
            }
        }
    }
    public void StartLaser()
    {
        lasering = true;
        laserTimer = 5;
    }


    ////转向
    void Turn()
    {
        if (player.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
