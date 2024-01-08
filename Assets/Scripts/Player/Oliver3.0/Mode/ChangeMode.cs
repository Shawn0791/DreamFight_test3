using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMode : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    public GameObject player;
    public GameObject[] guns;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim.SetBool("Normal", true);
    }

    public void ChangeToNormal()
    {
        //动画切换
        anim.SetBool("Normal", true);
        anim.SetBool("Gun", false);
        //脚本切换
        transform.GetComponent<NormalMode>().enabled = true;
        transform.GetComponent<GunMode>().enabled = false;
        //武器关闭
        for (int i = 0; i < guns.Length; i++)
        {
            guns[i].SetActive(false);
        }
    }

    //切换状态
    public void ChangeByID(int id)
    {
        //1号技能：左轮
        if (id == 1) 
        {
            //切换脚本
            player.GetComponent<NormalMode>().enabled = false;
            player.GetComponent<GunMode>().enabled = true;
            //切换动画器开关
            anim.SetBool("Normal", false);
            anim.SetBool("Gun", true);
            //左轮显示
            for (int i = 0; i < guns.Length; i++)
            {
                guns[i].SetActive(false);
            }
            guns[0].SetActive(true);
        }

        //2号技能：巴雷特
        if (id == 2)
        {
            //切换脚本
            player.GetComponent<NormalMode>().enabled = false;
            player.GetComponent<GunMode>().enabled = true;
            //切换动画器开关
            anim.SetBool("Normal", false);
            anim.SetBool("Gun", true);
            //左轮显示
            for (int i = 0; i < guns.Length; i++)
            {
                guns[i].SetActive(false);
            }
            guns[1].SetActive(true);
        }

        //3号技能：M4
        if (id == 3)
        {
            //切换脚本
            player.GetComponent<NormalMode>().enabled = false;
            player.GetComponent<GunMode>().enabled = true;
            //切换动画器开关
            anim.SetBool("Normal", false);
            anim.SetBool("Gun", true);
            //左轮显示
            for (int i = 0; i < guns.Length; i++)
            {
                guns[i].SetActive(false);
            }
            guns[2].SetActive(true);
        }

        //4号技能：M249
        if (id == 4)
        {
            //切换脚本
            player.GetComponent<NormalMode>().enabled = false;
            player.GetComponent<GunMode>().enabled = true;
            //切换动画器开关
            anim.SetBool("Normal", false);
            anim.SetBool("Gun", true);
            //左轮显示
            for (int i = 0; i < guns.Length; i++)
            {
                guns[i].SetActive(false);
            }
            guns[3].SetActive(true);
        }

        //5号技能：霰弹枪
        if (id == 5)
        {
            //切换脚本
            player.GetComponent<NormalMode>().enabled = false;
            player.GetComponent<GunMode>().enabled = true;
            //切换动画器开关
            anim.SetBool("Normal", false);
            anim.SetBool("Gun", true);
            //左轮显示
            for (int i = 0; i < guns.Length; i++)
            {
                guns[i].SetActive(false);
            }
            guns[4].SetActive(true);
        }

        //6号技能：M1216
        if (id == 6)
        {
            //切换脚本
            player.GetComponent<NormalMode>().enabled = false;
            player.GetComponent<GunMode>().enabled = true;
            //切换动画器开关
            anim.SetBool("Normal", false);
            anim.SetBool("Gun", true);
            //左轮显示
            for (int i = 0; i < guns.Length; i++)
            {
                guns[i].SetActive(false);
            }
            guns[5].SetActive(true);
        }

    }


    public void PlayerChange2D()
    {
        //人变鸡
        //sr.sprite = mode[1];
        anim.SetBool("Chicken", true);
        anim.SetBool("Normal", false);
        anim.SetBool("Shadow2", false);
        //脚本切换
        transform.GetComponent<ChickenMode>().enabled = true;
        transform.GetComponent<ShadowMode>().enabled = false;
        transform.GetComponent<NormalMode2>().enabled = false;
        //恢复人物重力
        rb.gravityScale = 1;
    }

    //射线检测轮盘（不使用）
    void HoldQ1()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            //显示轮盘

            //游戏时间缩放0.1倍

            //屏幕中心点
            Vector2 ScreenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            Vector2 origin = Camera.main.ScreenToWorldPoint(ScreenCenter);

            //鼠标位置
            Vector2 finish = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //射线检测
            RaycastHit2D hitInfo = Physics2D.Raycast(origin, finish - origin);
            if (hitInfo.collider != null)
            {
                //显示名字
                //Debug.Log(hitInfo.collider.gameObject.name);
                //击中为红色
                Debug.DrawLine(origin, hitInfo.point, Color.red);
            }
            else
                Debug.DrawLine(origin, finish, Color.green);//未中为绿色
        }
    }
}
