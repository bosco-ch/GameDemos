using PlayerMainScenes;
using UnityEngine;
/// <summary>
/// 巡逻状态
/// </summary>
public class PatrolState : IState
{
    FSM manager;//FSM管理器 
    Paramters paramters;  //参数
    public int patrolPosition;//用于下标查找和切换巡逻点
    public PatrolState(FSM fSM)
    {
        this.manager = fSM;
        this.paramters = fSM.paramters;
    }
    public void OnEnter()
    {
        paramters.animator.Play("Walk", 0, 0);
    }

    public void OnExit()
    {
        patrolPosition++;
        if (patrolPosition >= paramters.patrolPoints.Length)//查看是否超出数组范围来进行判断
        {
            patrolPosition = 0;
        }
    }

    public void Update()
    {
        if (paramters.Target != null
     && manager.transform.position.x > paramters.chasePoints[0].position.x
     && manager.transform.position.x < paramters.chasePoints[1].position.x)
        {
            manager.TransitionState(StateType.React);
        }
        manager.FlipTo(paramters.patrolPoints[patrolPosition]);
        manager.transform.position = UnityEngine.Vector2.MoveTowards(manager.transform.position
        , paramters.patrolPoints[patrolPosition].transform.position
        , paramters.Speed * Time.deltaTime);
        if (Vector2.Distance(manager.transform.position
        , paramters.patrolPoints[patrolPosition].transform.position) < .1f)//表示达到目标位置
        {
            //这个时候切换为idle 状态
            manager.TransitionState(StateType.Idle);
        }
    }
}