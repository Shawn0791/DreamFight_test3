using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBehaviour : StateMachineBehaviour
{
    private int rand;
    private Transform player;
    private float waitTimer;
    private Transform skull;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        skull = animator.transform;

        //设置等待时间
        waitTimer = 1;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SkillCD();
        Turn();

        if (waitTimer <= 0)
        {
            //目标Y高（先判断目标是否升空）
            if (player.position.y - animator.transform.position.y > 3 &&
                player.GetComponent<Animator>().GetBool("Fly") == true &&
                animator.GetBool("Fly") == false)
            {
                animator.SetBool("Fly", true);
            }
            else
            {
                //如果X差距小于8（骷髅拳头长度）Y近
                if (Mathf.Abs(player.transform.position.x - skull.position.x) <= 8 &&
                    Mathf.Abs(player.transform.position.y - skull.position.y) <= 2)
                {
                    //拳1/拳2/剑/瞬移
                    rand = Random.Range(0, 7);
                    if (rand == 0 || rand == 1) //几率 2/7 使用拳1
                        animator.SetTrigger("FistAttack1");
                    else if (rand == 2)//几率 1/7 使用拳2
                        animator.SetTrigger("FistAttack2");
                    else if (rand == 3 || rand == 4)//几率 2/7 使用剑
                    {
                        animator.SetTrigger("SwordAttack");
                    }
                    else if (rand == 5)//几率 1/7 使用后撤步
                    {
                        Vector2 target;
                        //计算目标点
                        if (player.transform.position.x < skull.position.x)
                            target = new Vector2(skull.position.x + 5, skull.position.y);
                        else
                            target = new Vector2(skull.position.x - 5, skull.position.y);
                        //传递目标点并且开始后撤
                        skull.GetComponent<BlackSkull>().target = target;
                        skull.GetComponent<BlackSkull>().dash = true;
                    }
                    else//几率 1/7 瞬移到身后中距离
                    {
                        Vector2 target;
                        //计算目标点
                        if (player.transform.position.x < skull.position.x)
                            target = new Vector2(player.transform.position.x - 5, player.transform.position.y);
                        else
                            target = new Vector2(player.transform.position.x + 5, player.transform.position.y);
                        //传递目标点并且开始瞬移
                        skull.GetComponent<BlackSkull>().target = target;
                        animator.SetTrigger("Move1_up");
                        skull.GetComponent<BlackSkull>().ShadowMove();
                    }
                }
                //目标X近Y远（在上下方）
                if (Mathf.Abs(player.transform.position.x - skull.position.x) <= 8 &&
                    Mathf.Abs(player.transform.position.y - skull.position.y) > 2)
                {
                    //飞剑/瞬移（倾向瞬移）
                    rand = Random.Range(0, 3);
                    if (rand == 0)//几率 1/3 飞剑
                    {
                        animator.SetTrigger("CreateSword");
                    }
                    else//几率 2/3 贴脸
                    {
                        Vector2 target;
                        //计算目标点
                        if (player.transform.position.x < skull.position.x)
                            target = new Vector2(player.transform.position.x - 1, player.transform.position.y);
                        else
                            target = new Vector2(player.transform.position.x + 1, player.transform.position.y);
                        //传递目标点并且开始瞬移
                        skull.GetComponent<BlackSkull>().target = target;
                        animator.SetTrigger("Move1_up");
                        skull.GetComponent<BlackSkull>().ShadowMove();
                    }
                }
                //目标X远Y近（在正前方）
                if (Mathf.Abs(player.transform.position.x - skull.position.x) > 8 &&
                    Mathf.Abs(player.transform.position.y - skull.position.y) <= 2)
                {
                    //飞剑/瞬移（平衡）
                    rand = Random.Range(0, 2);
                    if (rand == 0)//几率 1/2 飞剑
                    {
                        animator.SetTrigger("CreateSword");
                    }
                    else//几率 1/2 瞬移到近距离身后
                    {
                        Vector2 target;
                        //计算目标点
                        if (player.transform.position.x < skull.position.x)
                            target = new Vector2(player.transform.position.x - 3, player.transform.position.y);
                        else
                            target = new Vector2(player.transform.position.x + 3, player.transform.position.y);
                        //传递目标点并且开始瞬移
                        skull.GetComponent<BlackSkull>().target = target;
                        animator.SetTrigger("Move1_up");
                        skull.GetComponent<BlackSkull>().ShadowMove();
                    }
                }
                //目标X远Y远（在斜上或者斜下）
                if (Mathf.Abs(player.transform.position.x - skull.position.x) > 8 &&
                    Mathf.Abs(player.transform.position.y - skull.position.y) > 2)
                {
                    //飞剑/瞬移（倾向飞剑）
                    rand = Random.Range(0, 3);
                    if (rand == 0 || rand == 1)//几率 2/3 飞剑
                    {
                        animator.SetTrigger("CreateSword");
                    }
                    else//几率 1/3 瞬移到中距离身后
                    {
                        Vector2 target;
                        //计算目标点
                        if (player.transform.position.x < skull.position.x)
                            target = new Vector2(player.transform.position.x - 5, player.transform.position.y);
                        else
                            target = new Vector2(player.transform.position.x + 5, player.transform.position.y);
                        //传递目标点并且开始瞬移
                        skull.GetComponent<BlackSkull>().target = target;
                        animator.SetTrigger("Move1_up");
                        skull.GetComponent<BlackSkull>().ShadowMove();
                    }
                }
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    void SkillCD()
    {
        if (waitTimer > 0)
            waitTimer -= Time.deltaTime;
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
