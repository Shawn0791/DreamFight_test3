using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    private Walker1 manager;
    private Parameter parameter;

    private int patrolPosition;
    private int sign;

    public PatrolState(Walker1 manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        if (parameter.isChanged == false)
        {
            parameter.anim.Play("walk1");
        }
        else
        {
            parameter.anim.Play("walk2");
        }
    }

    public void OnUpdate()
    {
        //持续朝向巡逻方向
        manager.FlipTo(parameter.patrolPoints[patrolPosition]);

        //移动到巡逻点
        manager.transform.position = Vector2.MoveTowards(manager.transform.position,
            parameter.patrolPoints[patrolPosition], parameter.moveSpeed * Time.deltaTime);

        //if (manager.transform.position.x > parameter.patrolPoints[patrolPosition].transform.position.x)
        //{
        //    sign = -1;
        //}
        //if(manager.transform.position.x < parameter.patrolPoints[patrolPosition].transform.position.x)
        //{
        //    sign = 1;
        //}
        //parameter.rb.velocity = new Vector2(sign * parameter.moveSpeed, parameter.rb.velocity.y);

        //接近目标切换为追击状态
        if (parameter.target != null )
        {
            manager.TransitionState(StateType.Chase);
        }

        //到达巡逻点
        if (Mathf.Abs(manager.transform.position.x - parameter.patrolPoints[patrolPosition].x) < 0.1f) 
        {
            //结束巡逻开始静止
            manager.TransitionState(StateType.Idle);
        }
    }

    public void OnExit()
    {
        //下一个巡逻点
        patrolPosition++;

        //防止超出数量
        if (patrolPosition >= parameter.patrolPoints.Count)
        {
            patrolPosition = 0;
        }
    }
}
