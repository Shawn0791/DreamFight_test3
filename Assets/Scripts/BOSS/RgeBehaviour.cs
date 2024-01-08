using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RgeBehaviour : StateMachineBehaviour
{
    private int rand;
    private float waitTimer;

    private Transform player;
    private Transform skull;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        skull = animator.transform;

        waitTimer = 0.8f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SkillCD();
        Turn();

        if (waitTimer <= 0)
        {
            //X近Y近
            if (Mathf.Abs(player.transform.position.x - skull.position.x) <= 7 &&
                Mathf.Abs(player.transform.position.y - skull.position.y) <= 3)
            {
                //A/B/C/D
                rand = Random.Range(0, 7);
                Debug.Log("near" + rand);
                if (rand == 0 || rand == 1)//概率 2/7 A
                {
                    animator.SetTrigger("Attack2_A");
                }
                else if (rand == 2 || rand == 3)//概率 2/7 B
                {
                    animator.SetTrigger("Attack2_B");
                }
                else if (rand == 4)//概率 1/7 C
                {
                    animator.SetTrigger("Attack2_C");
                }
                else//概率 2/7 D
                {
                    animator.SetTrigger("Attack2_D");
                }

                waitTimer = 2;
            }
            //X中Y近
            if (Mathf.Abs(player.transform.position.x - skull.position.x) > 7 &&
                Mathf.Abs(player.transform.position.x - skull.position.x) <= 17 &&
                Mathf.Abs(player.transform.position.y - skull.position.y) <= 3)
            {
                //D/三连锤/召W/接近
                rand = Random.Range(0, 5);
                Debug.Log("middle" + rand);
                if (rand == 0)//概率 1/5 D
                {
                    animator.SetTrigger("Attack2_D");
                }
                else if (rand == 1)//概率 1/5 三连锤
                {
                    animator.SetTrigger("TripleFistAttack");
                }
                else if (rand == 2)//概率 1/5 召W
                {
                    animator.SetTrigger("CreateWalker1");
                }
                else//概率 2/5 接近
                {
                    animator.SetBool("Move2", true);
                }

                waitTimer = 2;
            }
            //X远Y近
            if (Mathf.Abs(player.transform.position.x - skull.position.x) > 17 &&
                Mathf.Abs(player.transform.position.y - skull.position.y) <= 3)
            {
                //D/三连锤/瞬移
                rand = Random.Range(0, 5);
                Debug.Log("far" + rand);
                if (rand == 0)//几率 1/5 D
                {
                    animator.SetTrigger("Attack2_D");
                }
                else if (rand == 1 || rand == 2)//几率 2/5 三连锤
                {
                    animator.SetTrigger("TripleFistAttack");
                }
                else//几率 2/5 瞬移
                {
                    //计算目标点
                    Vector2 target;
                    if (player.transform.position.x < skull.position.x)
                        target = new Vector2(player.transform.position.x - 5,player.transform.position.y);
                    else
                        target = new Vector2(player.transform.position.x + 5, player.transform.position.y);
                    //传递目标点并瞬移
                    skull.GetComponent<BlackSkull>().target = target;
                    animator.SetTrigger("DashDisappear");
                }

                waitTimer = 2;
            }
            //X近Y远
            if (Mathf.Abs(player.transform.position.x - skull.position.x) <= 17 &&
                Mathf.Abs(player.transform.position.y - skull.position.y) > 3)
            {
                //D/三连锤/召C/瞬移
                rand = Random.Range(0, 6);
                if (rand == 0)//几率 1/6 D
                {
                    animator.SetTrigger("Attack2_D");
                }
                else if (rand == 1 || rand == 2)//几率 2/6 三连锤
                {
                    animator.SetTrigger("TripleFistAttack");
                }
                else if (rand == 3)//几率 1/6 瞬移
                {
                    //计算目标点
                    Vector2 target;
                    if (player.transform.position.x < skull.position.x)
                        target = new Vector2(player.transform.position.x - 5, player.transform.position.y);
                    else
                        target = new Vector2(player.transform.position.x + 5, player.transform.position.y);
                    //传递目标点并瞬移
                    skull.GetComponent<BlackSkull>().target = target;
                    animator.SetTrigger("DashDisappear");
                }
                else//几率 2/6 召C
                {
                    animator.SetTrigger("CreateChewer");
                }

                waitTimer = 2;
            }
            //X远Y远
            if (Mathf.Abs(player.transform.position.x - skull.position.x) > 17 &&
                Mathf.Abs(player.transform.position.y - skull.position.y) > 3)
            {
                //D/瞬移/三连锤
                rand = Random.Range(0, 3);
                if (rand == 0)//几率 1/3 D
                {
                    animator.SetTrigger("Attack2_D");
                }
                else if (rand == 1)//几率 1/3 瞬移
                {
                    //计算目标点
                    Vector2 target;
                    if (player.transform.position.x < skull.position.x)
                        target = new Vector2(player.transform.position.x - 5, player.transform.position.y);
                    else
                        target = new Vector2(player.transform.position.x + 5, player.transform.position.y);
                    //传递目标点并瞬移
                    skull.GetComponent<BlackSkull>().target = target;
                    animator.SetTrigger("DashDisappear");
                }
                else//几率 1/3 三连锤
                {
                    animator.SetTrigger("CreateChewer");
                }

                waitTimer = 2;
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
