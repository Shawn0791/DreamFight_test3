using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherWalkBehaviour : StateMachineBehaviour
{
    private Transform player;
    private Transform teacher2;
    private Rigidbody2D rb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        teacher2 = animator.transform;
        rb = animator.GetComponent<Rigidbody2D>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Turn();

        if(Vector2.Distance(player.position, animator.transform.position) < 2)
        {
            animator.SetBool("walking", false);
        }
        else
        {
            if (player.transform.position.x < teacher2.position.x)
            {
                rb.velocity = new Vector2(-5, 0);
            }
            else
            {
                rb.velocity = new Vector2(5, 0);
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
