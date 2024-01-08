using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprouterMoveBehaviour : StateMachineBehaviour
{
    private Transform player;
    private Rigidbody2D rb;
    private Transform sprouter;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        sprouter = animator.transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetComponent<Sprouter>().findTarget == true)
        {
            Turn();

            if (Vector2.Distance(player.transform.position, animator.transform.position) > 2)
            {
                if (player.transform.position.x < animator.transform.position.x)
                {
                    rb.velocity = new Vector2(-1 * animator.GetComponent<Sprouter>().speed, 0);
                }
                else
                {
                    rb.velocity = new Vector2(animator.GetComponent<Sprouter>().speed, 0);
                }
            }
            else
            {
                animator.SetBool("moving", false);
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
        if (player.transform.position.x > sprouter.position.x)
        {
            sprouter.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            sprouter.localScale = new Vector3(-1, 1, 1);
        }
    }
}
