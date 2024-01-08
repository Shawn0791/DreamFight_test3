using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flyable : MonoBehaviour
{
    public float fanForce;
    
    public GameObject Wings;
    public bool flyable;

    private Rigidbody2D rb;
    private ConstantForce2D cf;
    private Animator anim;
    private float time = 1;
    private bool fan = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cf = GetComponent<ConstantForce2D>();
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        if(GameManager.instance.gameMode == GameManager.GameMode.Normal)
        {
            Suspending();

            if (Input.GetKeyDown(KeyCode.F) && !flyable)
            {
                anim.SetTrigger("CreateWings");
            }
            else if (Input.GetKeyDown(KeyCode.F) && flyable)
            {
                flyable = !flyable;
                Wings.SetActive(flyable);
            }
        }
    }

    public void WingsFinish()
    {
        flyable = !flyable;
        Wings.SetActive(flyable);
    }

    void Flying()
    {
        if (Input.GetAxisRaw("Vertical") > 0.01f) 
        {
            cf.relativeForce = new Vector2(0, fanForce);
        }
        else if (Input.GetAxisRaw("Vertical") < -0.01f)
        {
            cf.relativeForce = new Vector2(0, -fanForce);
        }
        else
        {
            cf.relativeForce = new Vector2(0, 0);
        }
    }

    //挥动翅膀保持滞空
    void Suspending()
    {
        if (flyable)
        {
            time += Time.deltaTime;

            //允许飞翔
            Flying();
            //飞翔动画锁定
            anim.SetBool("Fly", true);
            //如果上升
            if (fan)
            {
                rb.velocity = new Vector2(rb.velocity.x, 1.5f + Mathf.Pow(time, 4));
                //上升速度超过设定改为下降
                if (rb.velocity.y > 4.6f)
                {
                    time = 0;
                    fan = false;
                }
            }
            //如果下降
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, -0.15f - Mathf.Pow(time, 3));
                //下降速度超过设定改为上升
                if (rb.velocity.y < -0.46f)
                {
                    time = 1;
                    fan = true;
                }
            }
        }
        else
        {
            time = 1;
            fan = true;
            //飞翔动画解锁
            anim.SetBool("Fly", false);
        }
    }
}
