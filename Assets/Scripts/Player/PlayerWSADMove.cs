using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWSADMove : MonoBehaviour
{
    //单例
    public static PlayerWSADMove WSAD;
    [Header("人物移速")]
    public float speed;
    public string scenePassword;
    [Header("受击后退速度")]
    public float HurtSpeed;
    [Header("血量")]
    public float maxHp;
    public float hp;

    private Vector3 direction;
    //刚体必须公开，其他脚本需要调用
    public Rigidbody2D rb;
    private Animator anim;
    private AnimatorStateInfo info;
    private float inputX, inputY;
    private float stopX, stopY;
    private float oldspeed;

    [SerializeField]private bool isAttack;
    [SerializeField]private bool isHurt;
    [SerializeField] private bool alive=true;

    public float attackDamage;
    public GameObject deadbody_L;
    public GameObject deadbody_R;

    [Header("相机震动参数")]
    public float shakeTime;
    public int lightPause;
    public float lightStrength;
    public int heavyPause;
    public float heavyStrength;

    private void Awake()
    {
        if (WSAD == null)
        {
            WSAD = this;
        }
        else
        {
            if (WSAD != this)
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);

    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        oldspeed = speed;
    }

    void Update()
    {
        Attack();
        HurtF();

        //CG中切断人物控制
        if (GameManager.instance.gameMode == GameManager.GameMode.Normal)
            movement();
        info = anim.GetCurrentAnimatorStateInfo(0);
    }
    //人物移动
    void movement()
    {
        if (!isAttack&&alive==true)//攻击时不允许移动
        {
            //上下左右
            inputX = Input.GetAxisRaw("Horizontal");
            inputY = Input.GetAxisRaw("Vertical");
            Vector2 input = new Vector2(inputX, inputY).normalized;
            rb.velocity = input * speed;

            if (input != Vector2.zero)
            {
                anim.SetBool("isMoving", true);
                stopX = inputX;
                stopY = inputY;
            }
            else
            {
                anim.SetBool("isMoving", false);
            }

            anim.SetFloat("InputX", stopX);
            anim.SetFloat("InputY", stopY);

            //shift加速
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                speed *= 2;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
                speed = oldspeed;
        }
        
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
            anim.SetTrigger("hurt");
        }
    }

    //受伤掉血
    public void TakeDamage(float _amount)
    {
        hp -= _amount;
        //死亡
        if (hp <= 0&&alive==true)
        {
            alive = false;
            isAttack = false;
            //防止攻击过程中死亡导致一直移动
            anim.SetBool("isMoving", false);
            rb.velocity = Vector2.zero;
            anim.SetTrigger("down");
        }
    }

    //尸体实例化
    //private void Dead()
    //{
    //    if (direction.x < 0)
    //        GameObject.Instantiate(deadbody_L, transform.position,
    //            transform.rotation);
    //    if (direction.x > 0)
    //        GameObject.Instantiate(deadbody_R, transform.position,
    //            transform.rotation);
    //}

    //飞踢
    void Attack()
    {
        if (Input.GetMouseButtonDown(0) && !isAttack)
        {
            isAttack = true;
            anim.SetTrigger("kick");
        }
    }
    //攻击结束
    void AttackOver()
    {
        isAttack = false;
    }
    //飞踢传递信息
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //相机震动
            AttackSense.Instance.HitPause(lightPause);
            AttackSense.Instance.CameraShake(shakeTime, lightStrength);
            
            //传递伤害
            collision.gameObject.GetComponent<Teacher>().TakeDamage(attackDamage);
            
            //传递伤害方向
            if (transform.position.x>collision.transform.position.x)
                collision.GetComponent<Teacher>().GetHit(Vector3.left);
            if(transform.position.x < collision.transform.position.x)
                collision.GetComponent<Teacher>().GetHit(Vector3.right);


        }
    }
}
