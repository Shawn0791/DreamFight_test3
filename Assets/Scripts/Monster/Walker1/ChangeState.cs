using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeState : IState
{
    private Walker1 manager;
    private Parameter parameter;

    private AnimatorStateInfo info;

    public ChangeState(Walker1 manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.anim.Play("change");
        parameter.isChanging = true;
    }

    public void OnUpdate()
    {
        info = parameter.anim.GetCurrentAnimatorStateInfo(0);

        if (info.normalizedTime >= 0.95f)
        {
            manager.TransitionState(StateType.Chase);
            Debug.Log(info.normalizedTime);
            parameter.isChanging = false;
        }
    }

    public void OnExit()
    {
        parameter.hp = parameter.maxHp = 300;
        parameter.attack = 30;
        parameter.moveSpeed = 4;
        parameter.chaseSpeed = 5;
    }
}
