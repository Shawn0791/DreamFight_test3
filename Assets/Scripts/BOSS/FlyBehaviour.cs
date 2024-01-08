using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBehaviour : StateMachineBehaviour
{
    private Transform player;
    private float fireCastCD;
    private float waitTimer;
    private float groundTimer;
    private bool playerOnGround;
    private Transform skull;
    private int rand;
    private Rigidbody2D rb;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = player.GetComponent<Rigidbody2D>();
        skull = animator.transform;

        //展开翅膀
        animator.transform.Find("Wings4").gameObject.SetActive(true);
        //重力消失
        skull.GetComponent<Rigidbody2D>().gravityScale = 0;
        //设置等待时间
        waitTimer = 2;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SkillCD();
        Turn();

        //如果等待时间结束
        if (waitTimer <= 0)
        {
            //先判断距离是否贴身，如果距离小于3（火环半径）就放火环
            if (Vector2.Distance(player.position, skull.position) <= 3 &&
                fireCastCD <= 0)
            {
                animator.SetTrigger("FireCast");
                fireCastCD = 5;
            }
            else
            {
                //如果X差距小于8（骷髅拳头长度）Y近
                if (Mathf.Abs(player.transform.position.x - skull.position.x) <= 8 &&
                    Mathf.Abs(player.transform.position.y - skull.position.y) <= 2)
                {
                    //拳1/拳2/后退
                    rand = Random.Range(0, 8);
                    if (rand < 3 && rand >= 0)//几率 3/8 使用拳1
                        animator.SetTrigger("FistAttack1");
                    else if (rand < 5 && rand >= 3)//几率 1/4 使用拳2
                        animator.SetTrigger("FistAttack2");
                    else//几率 3/8 使用后撤
                    {
                        Vector2 target;
                        //计算目标点
                        if (player.transform.position.x < skull.position.x)
                            target = new Vector2(skull.position.x + 8, skull.position.y);
                        else
                            target = new Vector2(skull.position.x - 8, skull.position.y);
                        //传递目标点并且开始后撤
                        skull.GetComponent<BlackSkull>().target = target;
                        skull.GetComponent<BlackSkull>().dash = true;
                    }
                }
                //如果X近Y远（在正上方或者正下方）
                else if (Mathf.Abs(player.transform.position.x - skull.position.x) <= 8 &&
                    Mathf.Abs(player.transform.position.y - skull.position.y) > 2)
                {
                    //飞剑/火攻/飞平
                    rand = Random.Range(0, 5);
                    if (rand == 0 || rand == 1) //几率 2/5 飞剑
                    {
                        animator.SetTrigger("CreateSword");
                    }
                    else if (rand == 2)//几率 1/5 火攻
                    {
                        animator.SetTrigger("FireAttack");
                    }
                    else//几率 2/5 飞平
                    {
                        //计算目标点
                        Vector2 target = new Vector2(skull.position.x, player.transform.position.y);
                        //传递目标点并且开始飞平
                        skull.GetComponent<BlackSkull>().target = target;
                        skull.GetComponent<BlackSkull>().dash = true;
                    }
                }
                //如果X远Y近（在正前方）
                else if (Mathf.Abs(player.transform.position.x - skull.position.x) > 8 &&
                    Mathf.Abs(player.transform.position.y - skull.position.y) <= 2)
                {
                    //飞剑/火攻/飞近
                    rand = Random.Range(0, 5);
                    if (rand == 0 || rand == 1) //几率 2/5 飞剑
                    {
                        animator.SetTrigger("CreateSword");
                    }
                    else if (rand == 2)//几率 1/5 火攻
                    {
                        animator.SetTrigger("FireAttack");
                    }
                    else//几率 2/5 飞近
                    {
                        //计算目标点
                        Vector2 target ;
                        if (player.transform.position.x < skull.transform.position.x)
                            target = new Vector2(player.transform.position.x + 7, skull.position.y);
                        else
                            target = new Vector2(player.transform.position.x - 7, skull.position.y);

                        //传递目标点并且开始飞近
                        skull.GetComponent<BlackSkull>().target = target;
                        skull.GetComponent<BlackSkull>().dash = true;
                    }
                }
                //如果X远Y远（在斜上方或者斜下方）
                else if (Mathf.Abs(player.transform.position.x - skull.position.x) > 8 &&
                    Mathf.Abs(player.transform.position.y - skull.position.y) > 2)
                {
                    //飞剑/火攻/贴脸
                    rand = Random.Range(0, 5);
                    if (rand == 0 || rand == 1) //几率 2/5 飞剑
                    {
                        animator.SetTrigger("CreateSword");
                    }
                    else if (rand == 2)//几率 1/5 火攻
                    {
                        animator.SetTrigger("FireAttack");
                    }
                    else//几率 2/5 贴脸
                    {
                        ////计算目标点
                        Vector2 target = player.transform.position;

                        ////传递目标点并且开始贴脸
                        skull.GetComponent<BlackSkull>().target = target;
                        skull.GetComponent<BlackSkull>().dash = true;
                    }
                }

                Debug.Log(groundTimer);
                //如果目标落地
                if (player.GetComponent<NormalMode>().isOnGround == true ||
                    player.GetComponent<GunMode>().isOnGround == true)
                {
                    playerOnGround = true;
                    if (groundTimer > 3)
                    {
                        //退出飞翔状态
                        animator.SetBool("Fly", false);
                        //翅膀消失
                        animator.transform.Find("Wings4").gameObject.SetActive(false);
                        //重力出现
                        skull.GetComponent<Rigidbody2D>().gravityScale = 1;
                    }
                }
                else
                {
                    groundTimer = 0;
                    playerOnGround = false;
                }
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    void SkillCD()
    {
        //火环CD
        if (fireCastCD > 0)
            fireCastCD -= Time.deltaTime;
        //进入阶段等待时间
        if (waitTimer > 0)
            waitTimer -= Time.deltaTime;
        //目标落地时间
        if (playerOnGround)
            groundTimer += Time.deltaTime;
    }

    //转向
    void Turn()
    {
        if (player.transform.position.x > skull.position.x)
        {
            skull.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            skull.localScale = new Vector3(1, 1, 1);
        }
    }
}
