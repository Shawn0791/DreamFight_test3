using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private Walker1 manager;
    private Parameter parameter;

    private AnimatorStateInfo info;

    public AttackState(Walker1 manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        if (parameter.isChanged == false)
        {
            parameter.anim.Play("attack1");
        }
        else
        {
            parameter.anim.Play("attack2");
        }
    }

    public void OnUpdate()
    {
        info = parameter.anim.GetCurrentAnimatorStateInfo(0);

        if (info.normalizedTime >= 0.95f)
        {
            manager.TransitionState(StateType.Chase);
        }
    }

    public void OnExit()
    {

    }
}
