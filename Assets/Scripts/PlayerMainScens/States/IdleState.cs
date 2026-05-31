
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 闲置状态
/// </summary>
public class IdleState : IState
{
    FSM manager;//FSM管理器
    Paramters paramters;  //参数

    private float timer;//设置对象在到巡逻点的时候 需要在原地观察一段时间
    public IdleState(FSM fSM)
    {
        this.manager = fSM;
        this.paramters = fSM.paramters;
    }
    public void OnEnter()
    {
        paramters.animator.Play("Idle", 0, 0);
    }

    public void OnExit()
    {
        timer = 0;
    }

    public void Update()
    {
        if (paramters.Target != null
        && paramters.Target.transform.position.x > paramters.chasePoints[0].position.x
        && paramters.Target.position.x < paramters.chasePoints[1].position.x)
        {
            manager.TransitionState(StateType.React);
        }
        timer += Time.deltaTime;
        if (timer >= paramters.IdleTime)
        {
            manager.TransitionState(StateType.Patrol);
        }

    }
}
