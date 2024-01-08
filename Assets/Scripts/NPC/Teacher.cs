using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teacher : MonoBehaviour
{

    public float HurtSpeed;
    private Vector3 direction;
    [SerializeField] private bool isHurt;
    [SerializeField] private bool isAttack;
    [SerializeField] private bool visible;
    [SerializeField] private bool unTurn;
    private AnimatorStateInfo info;
    private Animator anim;
    private Rigidbody2D rb;

    public float moveSpeed;
    public float maxHp;
    public float hp;
    public float lightDamage;
    public float heavyDamage;

    private GameObject Player;
    public GameObject deadbody_L;
    public GameObject deadbody_R;

    [SerializeField] private float attackCD;
    [SerializeField] private float rulerCD;
    [SerializeField] private float rushCD;

    [Header("相机震动参数")]
    public float shakeTime;
    public int lightPause;
    public float lightStrength;
    public int heavyPause;
    public float heavyStrength;

    private Vector3 lastpos = new Vector3();
    //技能类型
    private string attackType;

    private void Start()
    {
        anim = transform.GetComponent<Animator>();
        rb = transform.GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        skillCD();
        HurtAnim();
        HurtF();
        info = anim.GetCurrentAnimatorStateInfo(0);
    }
    //移动动画
    void HurtAnim()
    {
        if (lastpos != transform.position)
        {
            anim.SetBool("isWalk", true);
            lastpos = transform.position;
        }
        else
            anim.SetBool("isWalk", false);
    }
    //受击后退
    void HurtF()
    {
        if (isHurt)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                transform.position + direction, HurtSpeed * Time.deltaTime);
            if (info.normalizedTime >= .6f)
                isHurt = false;
        }
    }
    //受击朝向
    public void GetHit(Vector3 direction)
    {
        this.direction = direction;
        if (hp > 0)
        {
            transform.localScale = new Vector3(-direction.x, 1, 1);
            isHurt = true;
            isAttack = false;
            anim.SetTrigger("isHurt");
        }
    }
    //受伤掉血
    public void TakeDamage(float _amount)
    {
        hp -= _amount;
        //死亡
        if (hp <= 0)
        {
            anim.SetTrigger("down");
        }
    }
    
    //尸体实例化
    public void Dead()
    {
        if(direction.x<0)
            GameObject.Instantiate(deadbody_L, transform.position,
                transform.rotation);
        if (direction.x > 0)
            GameObject.Instantiate(deadbody_R, transform.position,
                transform.rotation);
        Destroy(gameObject);
    }
    //发现主角
    public void FindPlayer(Transform _player)
    {
        if (GameManager.instance.gameMode == GameManager.GameMode.Normal)
            move();
        //转向
        if (unTurn == false)//攻击过程中禁止转身
        {
            if (_player.position.x >= transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }      

        //平A
        if (Mathf.Abs(transform.position.x - _player.transform.position.x) < 6.5 &&
            Mathf.Abs(transform.position.y - _player.transform.position.y) < 0.5&&
            attackCD <= 0&&isAttack==false)
        {
            isAttack = true;
            attackCD = 5;
            attackType = "Heavy";
            anim.SetTrigger("attack");
        }

        //远程
        if (6.5<Mathf.Abs(transform.position.x - _player.transform.position.x)&&
            Mathf.Abs(transform.position.x - _player.transform.position.x) < 16.5&&
            Mathf.Abs(transform.position.y - _player.transform.position.y) < 0.5 &&
            rulerCD <= 0&&isAttack==false)
        {
            isAttack = true;
            unTurn = true;
            rulerCD = 8;
            attackType = "Light";
            anim.SetTrigger("ruler_fly");
        }

        //冲刺
        if (6.5 < Mathf.Abs(transform.position.x - _player.transform.position.x) &&
            Mathf.Abs(transform.position.x - _player.transform.position.x) < 8 &&
            Mathf.Abs(transform.position.y - _player.transform.position.y) < 1 &&
            rushCD <= 0 && isAttack == false)
        {
            isAttack = true;
            unTurn = true;
            rushCD = 5;
            attackType = "Light";
            anim.SetTrigger("rush");
        }

    }
    //攻击
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isAttack)
            {
                //相机震动
                if (attackType == "Light")
                {
                    AttackSense.Instance.HitPause(lightPause);
                    AttackSense.Instance.CameraShake(shakeTime, lightStrength);
                }
                else if (attackType == "Heavy")
                {
                    AttackSense.Instance.HitPause(heavyPause);
                    AttackSense.Instance.CameraShake(shakeTime, heavyStrength);
                }

                //传递伤害
                if (attackType == "Light")
                    collision.gameObject.GetComponent<PlayerWSADMove>().TakeDamage(lightDamage);
                if (attackType == "Heavy")
                    collision.gameObject.GetComponent<PlayerWSADMove>().TakeDamage(heavyDamage);

                //传递伤害方向
                if (transform.position.x > collision.transform.position.x)
                    collision.GetComponent<PlayerWSADMove>().GetHit(Vector3.left);
                if (transform.position.x < collision.transform.position.x)
                    collision.GetComponent<PlayerWSADMove>().GetHit(Vector3.right);
            }
        }
    }
    //视野
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            visible = true;
    }
    //防止视野造成伤害
    void debug1()
    {
        if (isAttack == true)
            transform.Find("view").gameObject.SetActive(false);
        else
            transform.Find("view").gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        if(visible==true)
            FindPlayer(Player.GetComponent<Transform>());
    }
    //跟随主角
    void move()
    {
        if (!isAttack)
        {
            if (Mathf.Abs(transform.position.x - Player.transform.position.x) > 5f)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                             Player.transform.position, moveSpeed * Time.deltaTime);
            }
            else
            {
                if (transform.position.x > Player.transform.position.x)
                {
                    transform.position = Vector3.MoveTowards(transform.position,
                        new Vector3(Player.transform.position.x + 5, Player.transform.
                        position.y, Player.transform.position.z),moveSpeed * Time.deltaTime);
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(Player.transform.position.x - 5, Player.transform.position.y,
                Player.transform.position.z),moveSpeed * Time.deltaTime);
                }
            }
        }
    }

    //离开视野
    private void OnTriggerExit2D(Collider2D collision)
    {
        visible = false;
    }

    //攻击结束
    void AttackOver()
    {
        isAttack = false;
        unTurn = false;
    }

    //技能冷却
    void skillCD()
    {
        if (attackCD > 0)
            attackCD -= Time.deltaTime;
        if (rulerCD > 0)
            rulerCD -= Time.deltaTime;
        if (rushCD > 0)
            rushCD -= Time.deltaTime;
    }

    //向主角冲刺
    void rush()
    {
        transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, 5);
    }
}
