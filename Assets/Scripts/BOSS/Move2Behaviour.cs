using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move2Behaviour : StateMachineBehaviour
{
    private Transform player;
    private Transform skull;
    private float moveTimer;
    private Rigidbody2D rb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        skull = animator.transform;
        rb = animator.GetComponent<Rigidbody2D>();
        moveTimer = 3;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (moveTimer > 0)
            moveTimer -= Time.deltaTime;
        else
            animator.SetBool("Move2", false);


        //转向
        if (player.transform.position.x > skull.position.x)
        {
            skull.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            skull.localScale = new Vector3(1, 1, 1);
        }

        //接近
        if (Mathf.Abs(player.transform.position.x - skull.position.x) > 8 &&
            Mathf.Abs(player.transform.position.x - skull.position.x) <= 17 &&
            Mathf.Abs(player.transform.position.y - skull.position.y) <= 3)
        {
            if (player.transform.position.x < skull.position.x)
            {
                rb.velocity = new Vector2(-5, 0);
            }
            else
            {
                rb.velocity = new Vector2(5, 0);
            }

        }
        else
        {
            animator.SetBool("Move2", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}
