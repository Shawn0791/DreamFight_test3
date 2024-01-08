using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{
    Idle,Patrol,Chase,Attack,Change,Null
}

[Serializable]
public class Parameter
{
    public float maxHp;
    public float hp;
    public float attack;
    public float moveSpeed;
    public float chaseSpeed;
    public float idleTime;

    [Header("Attack Data")]
    public Transform target;
    public LayerMask targetLayer;
    public Transform attackPoint;
    public float attackArea;
    public Transform viewPoint;
    public Vector3 viewArea;

    [Header("DeBug")]
    public Animator anim;
    public Rigidbody2D rb;
    public SpriteRenderer sp;

    public List<Vector2> patrolPoints = new List<Vector2>();
    public List<Vector2> chasePoints = new List<Vector2>();
    public Vector2 patrolPoint1;
    public Vector2 patrolPoint2;
    public Vector2 chasePoint1;
    public Vector2 chasePoint2;

    public Transform player;
    public bool isChanged;
    public bool isChanging;
}

public class Walker1 : MonoBehaviour,InterFaces
{
    public Parameter parameter;

    private IState currentState;
    private Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();

    void Start()
    {
        states.Add(StateType.Idle, new IdleState(this));
        states.Add(StateType.Patrol, new PatrolState(this));
        states.Add(StateType.Chase, new ChaseState(this));
        states.Add(StateType.Attack, new AttackState(this));
        states.Add(StateType.Change, new ChangeState(this));
        states.Add(StateType.Null, new NullState(this));

        TransitionState(StateType.Idle);

        parameter.anim = GetComponent<Animator>();
        parameter.rb = GetComponent<Rigidbody2D>();
        parameter.sp = GetComponent<SpriteRenderer>();
        parameter.player = GameObject.FindGameObjectWithTag("Player").transform;

        parameter.patrolPoint1 = new Vector2(transform.position.x - 20, transform.position.y);
        parameter.patrolPoint2 = new Vector2(transform.position.x + 20, transform.position.y);
        parameter.chasePoint1 = new Vector2(transform.position.x - 25, transform.position.y);
        parameter.chasePoint2 = new Vector2(transform.position.x + 25, transform.position.y);
        parameter.patrolPoints.Add(parameter.patrolPoint1);
        parameter.patrolPoints.Add(parameter.patrolPoint2);
        parameter.chasePoints.Add(parameter.chasePoint1);
        parameter.chasePoints.Add(parameter.chasePoint2);
    }


    void Update()
    {
        FindTarget();
        currentState.OnUpdate();
    }

    //切换状态
    public void TransitionState(StateType type)
    {
        if (currentState != null)
            currentState.OnExit();
        currentState = states[type];
        currentState.OnEnter();

        Debug.Log(type);
    }

    //角色朝向
    public void FlipTo(Vector2 target)
    {
        if (target != null)
        {
            if (transform.position.x > target.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (transform.position.x < target.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
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
            Vector3 dir = transform.position - parameter.target.position;
            //传递伤害
            interfaces.GetHitBack(parameter.attack, dir, 500);
        }
    }

    //发现敌人
    void FindTarget()
    {
        if (Physics2D.OverlapBox(parameter.viewPoint.position, parameter.viewArea, 0, parameter.targetLayer))
        {
            parameter.target = parameter.player;
        }
        else if (Vector3.Distance(transform.position, parameter.player.position) > 30) 
        {
            parameter.target = null;
        }
    }

    //绘制范围
    private void OnDrawGizmos()
    {
        //攻击范围
        Gizmos.DrawWireSphere(parameter.attackPoint.position, parameter.attackArea);
        //视野范围
        Gizmos.DrawWireCube(parameter.viewPoint.position,parameter.viewArea);
    }

    //受击掉血
    public void GetHit(float damage)
    {
        //造成伤害
        parameter.hp -= damage;
        //判断死亡
        if (parameter.hp <= 0)
        {
            Dead();
        }
        else
        {
            //停顿
            StartCoroutine(Stop());
        }
        //判断变身
        if (parameter.hp <= 0.5f * parameter.maxHp && parameter.isChanged == false)
        {
            StartCoroutine(ChangeToRed());
            parameter.isChanged = true;
        }
    }

    //受击掉血且击退
    public void GetHitBack(float damage, Vector3 dir, float force)
    {
        //造成伤害
        parameter.hp -= damage;
        parameter.anim.SetTrigger("Hurt");
        //闪白
        StartCoroutine(HurtShader());
        //后退
        parameter.rb.AddForce(-dir * force);
        //判断死亡
        if (parameter.hp <= 0)
            Dead();
        //判断变身
        if (parameter.hp <= 0.5f * parameter.maxHp && parameter.isChanged == false)
        {
            ChangeToRed();
            parameter.isChanged = true;
        }
    }

    //受击闪白
    IEnumerator HurtShader()
    {
        parameter.sp.material.SetFloat("_FlashAmount", 1);
        yield return new WaitForSeconds(0.1f);
        parameter.sp.material.SetFloat("_FlashAmount", 0);
    }

    void Dead()
    {
        if (parameter.isChanged == false)
        {
            parameter.anim.Play("dead1");
        }
        else
        {
            parameter.anim.Play("dead2");
        }
        enabled = false;
        transform.gameObject.layer = LayerMask.NameToLayer("Dead");
    }
    //受击停顿
    IEnumerator Stop()
    {
        if (parameter.isChanging == false)
        {
            TransitionState(StateType.Null);
            yield return new WaitForSeconds(0.1f);
            parameter.target = parameter.player;
            TransitionState(StateType.Chase);
        }
    }

    //变身
    IEnumerator ChangeToRed()
    {
        yield return new WaitForSeconds(0.1f);
        TransitionState(StateType.Change);
    }
}
