using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IState
{
    private Walker1 manager;
    private Parameter parameter;

    public ChaseState(Walker1 manager)
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
        //朝向目标
        if(parameter.target!= null)
        {
            manager.FlipTo(parameter.target.position);
        }

        //向目标靠近
        if (parameter.target)
        {
            manager.transform.position = Vector2.MoveTowards(manager.transform.position,
                parameter.target.position, parameter.chaseSpeed * Time.deltaTime);
        }

        //丢失目标恢复静止
        if (parameter.target == null )
        {
            manager.TransitionState(StateType.Idle);
        }

        //检测到目标则切换为攻击状态
        if (Physics2D.OverlapCircle(parameter.attackPoint.position, parameter.attackArea, parameter.targetLayer))
        {
            manager.TransitionState(StateType.Attack);
        }
    }

    public void OnExit()
    {

    }
}
