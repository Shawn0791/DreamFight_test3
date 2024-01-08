using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprouter : MonoBehaviour,InterFaces
{
    private Animator anim;
    private Transform player;

    [Header("视野数据")]
    public bool findTarget;
    public Transform viewPoint;
    public Vector3 viewArea;
    public LayerMask targetLayer;
    [Header("基本数据")]
    public float maxHp;
    public float hp;
    public float attack;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        FindTarget();
    }

    void InterFaces.GetHit(float damage)
    {

    }

    void InterFaces.GetHitBack(float damage, Vector3 dir, float force)
    {
        anim.SetTrigger("damage");
    }

    //绘制范围
    private void OnDrawGizmos()
    {
        //视野范围
        Gizmos.DrawWireCube(viewPoint.position, viewArea);
    }

    //发现敌人
    void FindTarget()
    {
        if (Physics2D.OverlapBox(viewPoint.position, viewArea, 0, targetLayer)) 
        {
            findTarget = true;
        }
        else if (Vector3.Distance(transform.position, player.position) > 25)
        {
            findTarget = false;
        }
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
}
