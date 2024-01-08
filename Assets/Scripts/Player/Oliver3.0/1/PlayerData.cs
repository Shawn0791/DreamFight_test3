using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerData : MonoBehaviour,InterFaces
{
    public Transform HP_UI;
    public Text textNumber;
    public GameObject front_final;
    public Image[] joint;
    public GameObject[] joint_active;
    public GameObject[] joint_used;

    [Header("角色属性")]
    public float maxHp;
    public float hp;
    public float maxMp;
    public float mp;
    public float maxExp;
    public float exp;
    public float defense;
    public int persentNumber;

    [SerializeField] private float hurtSpeed;
    private float levelStage;
    private float maxMpUsed; 
    private Animator anim;
    public bool isHurt;
    public bool isDead;
    private Vector3 direction;
    private AnimatorStateInfo info;
    private SpriteRenderer sp;
    private Rigidbody2D rb;


    private void Start()
    {
        anim = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        info = anim.GetCurrentAnimatorStateInfo(0);
        if (mp < 0)
            mp = 0;


        Level1();
    }

    //受击掉血
    public void GetHit(float damage)
    {
        //掉血
        if (damage > defense)
        {
            hp -= (damage - defense);
            //传递红血数量
            HP_UI.GetComponent<HPandMP>().HP_2.fillAmount = (hp + (damage - defense) / 2) / maxHp;
        }

        //判断死亡
        IfDeadOrNot();
    }

    //受击掉血且击退
    public void GetHitBack(float damage, Vector3 dir, float force)
    {
        //isHurt = true;
        anim.SetTrigger("isHurt");
        //后退
        //transform.position = Vector3.MoveTowards(transform.position, transform.position - dir, force);
        rb.AddForce(-dir.normalized * force);
        //闪白
        StartCoroutine(HurtShader());
        //掉血
        if (damage > defense)
        {
            hp -= (damage - defense);
            //传递红血数量
            HP_UI.GetComponent<HPandMP>().HP_2.fillAmount = (hp + (damage - defense) / 2) / maxHp;
        }

        //判断死亡
        IfDeadOrNot();
    }

    //受击闪白
    IEnumerator HurtShader()
    {
        sp.material.SetFloat("_FlashAmount", 1);
        yield return new WaitForSeconds(0.1f);
        sp.material.SetFloat("_FlashAmount", 0);
    }

    //判断死亡
    void IfDeadOrNot()
    {
        //判断死亡
        if (hp <= 0 && isDead == false)
        {
            //改为普通状态
            anim.SetBool("Gun", false);
            anim.SetBool("Normal", true);
            //去除枪械
            transform.Find("Weapon").gameObject.SetActive(false);
            //死亡动画
            isDead = true;
            anim.SetBool("isDead", true);
            //防止死亡后尸体移动
            GameManager.instance.gameMode = GameManager.GameMode.Dead;
            //防止再次被打击
            transform.gameObject.layer = LayerMask.NameToLayer("Dead");

            //重载场景
            GameManager.PlayerDied();
        }
    }

    //掌控力等级
    int i = 0;
    private void Level1()
    {
        //跳转阶段
        if (exp > maxExp && maxExp < 51200) 
        {
            exp = 0;
            maxExp *= 2;
            //阶段+1
            levelStage += 1;
            //增加血蓝上限
            maxHp = 100 + 20 * (levelStage - maxMpUsed);
            maxMp = 100 * (levelStage - maxMpUsed);
            //恢复血蓝
            hp = maxHp;
            mp = maxMp;
            //激活状态
            joint_active[i].SetActive(true);
            i++;
        }
        else if (exp >= maxExp && maxExp == 51200)
        {
            exp = 0;
            maxExp = 99999;
            //激活状态
            joint_active[9].SetActive(true);
            //阶段
            levelStage = 10;
            //增加血蓝上限
            maxHp = 100 + 20 * (levelStage - maxMpUsed);
            maxMp = 100 * (levelStage - maxMpUsed);
            //恢复血蓝
            hp = maxHp;
            mp = maxMp;
            //变色，原（0.4622,0.2551,0.4289,1）
            front_final.SetActive(true);
            textNumber.color = new Color(1, 0, 0, 1);
        }
        //显示百分比
        if (maxExp <= 51200)
        {
            float _number = exp / maxExp * 10 + levelStage * 10;
            persentNumber = (int)_number;
            textNumber.text = Convert.ToString(persentNumber);
            //单节进度
            joint[i].fillAmount = exp / maxExp;
        }
        else if (maxExp == 99999) 
        {
            textNumber.text = ("100");
        }
    }

    //死亡重生
    public void DeadRestart()
    {

    }

    //醒来
    public void GetUp()
    {
        GetComponent<NormalMode>().enabled = true;
    }

    //废弃的受击掉血
    public void TakeDamage(float _damage)
    {
        //掉血
        if (_damage > defense)
        {
            hp -= (_damage - defense);
            //传递红血数量
            HP_UI.GetComponent<HPandMP>().HP_2.fillAmount = (hp + (_damage - defense) / 2) / maxHp;
        }
        //死亡
        if (hp <= 0 && isDead == false)
        {
            isDead = true;
            anim.SetBool("isDead", true);
            //防止死亡后尸体移动
            transform.GetComponent<NormalMode2>().rb.velocity = Vector2.zero;
        }
    }
    //废弃的受击朝向
    public void GetHit1(Vector3 direction)
    {
        this.direction = direction;
        if (hp > 0)
        {
            transform.localScale = new Vector3(-direction.x, 1, 1);
            isHurt = true;
            anim.SetTrigger("isHurt");
        }
    }
    //废弃的受击后退
    void HurtF()
    {
        if (isHurt)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                transform.position + direction, hurtSpeed * Time.deltaTime);
            if (info.normalizedTime >= .8f)
            {
                isHurt = false;
                transform.GetComponent<NormalMode2>().isAttack = false;
            }

        }
    }
    //废弃的level方法
    private void Level()
    {
        //跳转阶段
        if (exp >= maxExp && maxExp == 100) 
        {
            exp = 0;
            maxExp = 200;
            //激活状态
            joint_active[0].SetActive(true);
            //阶段
            levelStage = 1;
            //恢复血蓝
            hp = maxHp;
            mp = maxMp;
        }
        //10-20阶段
        else if (exp >= maxExp && maxExp == 200)
        {
            exp = 0;
            maxExp = 400;
            //激活状态
            joint_active[1].SetActive(true);
            //阶段
            levelStage = 2;
            //恢复血蓝
            hp = maxHp;
            mp = maxMp;
        }
        //20-30阶段
        else if (exp >= maxExp && maxExp == 400)
        {
            exp = 0;
            maxExp = 800;
            //激活状态
            joint_active[2].SetActive(true);
            //阶段
            levelStage = 3;
            //恢复血蓝
            hp = maxHp;
            mp = maxMp;
        }
        //30-40阶段
        else if (exp >= maxExp && maxExp == 800)
        {
            exp = 0;
            maxExp = 1600;
            //激活状态
            joint_active[3].SetActive(true);
            //阶段
            levelStage = 4;
            //恢复血蓝
            hp = maxHp;
            mp = maxMp;
        }
        //40-50阶段
        else if (exp >= maxExp && maxExp == 1600)
        {
            exp = 0;
            maxExp = 3200;
            //激活状态
            joint_active[4].SetActive(true);
            //阶段
            levelStage = 5;
            //恢复血蓝
            hp = maxHp;
            mp = maxMp;
        }
        //50-60阶段
        else if (exp >= maxExp && maxExp == 3200)
        {
            exp = 0;
            maxExp = 6400;
            //激活状态
            joint_active[5].SetActive(true);
            //阶段
            levelStage = 6;
            //恢复血蓝
            hp = maxHp;
            mp = maxMp;
        }
        //60-70阶段
        else if (exp >= maxExp && maxExp == 6400)
        {
            exp = 0;
            maxExp = 12800;
            //激活状态
            joint_active[6].SetActive(true);
            //阶段
            levelStage = 7;
            //恢复血蓝
            hp = maxHp;
            mp = maxMp;
        }
        //70-80阶段
        else if (exp >= maxExp && maxExp == 12800)
        {
            exp = 0;
            maxExp = 25600;
            //激活状态
            joint_active[7].SetActive(true);
            //阶段
            levelStage = 8;
            //恢复血蓝
            hp = maxHp;
            mp = maxMp;
        }
        //80-90阶段
        else if (exp >= maxExp && maxExp == 25600)
        {
            exp = 0;
            maxExp = 51200;
            //激活状态
            joint_active[8].SetActive(true);
            //阶段
            levelStage = 9;
            //恢复血蓝
            hp = maxHp;
            mp = maxMp;
        }
        //90-100阶段
        else if (exp >= maxExp && maxExp == 51200)
        {
            exp = 0;
            maxExp = 0;
            //激活状态
            joint_active[9].SetActive(true);
            //阶段
            levelStage = 10;
            //恢复血蓝
            hp = maxHp;
            mp = maxMp;
            //变色，原（0.4622,0.2551,0.4289,1）
            front_final.SetActive(true);
            textNumber.color = new Color(1, 0, 0, 1);

        }
        
        //显示百分数
        //0-10阶段
        if (maxExp==100)
        {
            float _number = exp / 100 * 10;
            persentNumber = (int)_number;
            textNumber.text = Convert.ToString(persentNumber);
            //单节进度
            joint[0].fillAmount = exp / maxExp;
        }
        //10-20阶段
        else if (maxExp==200)
        {
            float _number = exp / 200 * 10 + 10;
            persentNumber = (int)_number;
            textNumber.text = Convert.ToString(persentNumber);
            //单节进度
            joint[1].fillAmount = exp / maxExp;
        }
        //20-30阶段
        else if (maxExp == 400)
        {
            float _number = exp / 400 * 10 + 20;
            persentNumber = (int)_number;
            textNumber.text = Convert.ToString(persentNumber);
            //单节进度
            joint[2].fillAmount = exp / maxExp;
        }
        //30-40阶段
        else if (maxExp == 800)
        {
            float _number = exp / 800 * 10 + 30;
            persentNumber = (int)_number;
            textNumber.text = Convert.ToString(persentNumber);
            //单节进度
            joint[3].fillAmount = exp / maxExp;
        }
        //40-50阶段
        else if (maxExp == 1600)
        {
            float _number = exp / 1600 * 10 + 40;
            persentNumber = (int)_number;
            textNumber.text = Convert.ToString(persentNumber);
            //单节进度
            joint[4].fillAmount = exp / maxExp;
        }
        //50-60阶段
        else if (maxExp == 3200)
        {
            float _number = exp / 3200 * 10 + 50;
            persentNumber = (int)_number;
            textNumber.text = Convert.ToString(persentNumber);
            //单节进度
            joint[5].fillAmount = exp / maxExp;
        }
        //60-70阶段
        else if (maxExp == 6400)
        {
            float _number = exp / 6400 * 10 + 60;
            persentNumber = (int)_number;
            textNumber.text = Convert.ToString(persentNumber);
            //单节进度
            joint[6].fillAmount = exp / maxExp;
        }
        //70-80阶段
        else if (maxExp == 12800)
        {
            float _number = exp / 12800 * 10 + 70;
            persentNumber = (int)_number;
            textNumber.text = Convert.ToString(persentNumber);
            //单节进度
            joint[7].fillAmount = exp / maxExp;
        }
        //80-90阶段
        else if (maxExp == 25600)
        {
            float _number = exp / 25600 * 10 + 80;
            persentNumber = (int)_number;
            textNumber.text = Convert.ToString(persentNumber);
            //单节进度
            joint[8].fillAmount = exp / maxExp;
        }
        //90-100阶段
        else if (maxExp == 51200)
        {
            float _number = exp / 51200 * 10 + 90;
            persentNumber = (int)_number;
            textNumber.text = Convert.ToString(persentNumber);
            //单节进度
            joint[9].fillAmount = exp / maxExp;
        }
        //满级
        else if (maxExp == 0)
        {
            textNumber.text = ("100");
        }
    }
}
