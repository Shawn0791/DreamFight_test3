using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherIdleBehaviour : StateMachineBehaviour
{
    private Transform player;
    private Transform teacher2;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        teacher2 = animator.transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Turn();

        if (animator.GetComponent<Teacher2>().attack == true)
        {
            //玩家远距离就追
            if (Vector2.Distance(player.position, animator.transform.position) > 7)
            {
                animator.SetBool("walking", true);
            }
            //玩家中距离就扔或者追
            else if (Vector2.Distance(player.position, animator.transform.position) > 2&&
                Vector2.Distance(player.position, animator.transform.position) <=7)
            {
                animator.SetTrigger("throw");
            }
            //玩家近距离就打
            else
            {
                animator.SetTrigger("attack1");
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    //转向
    void Turn()
    {
        if (player.transform.position.x > teacher2.position.x)
        {
            teacher2.localScale = new Vector3(0.5f, 0.5f, 1);
        }
        else
        {
            teacher2.localScale = new Vector3(-0.5f, 0.5f, 1);
        }
    }
}
