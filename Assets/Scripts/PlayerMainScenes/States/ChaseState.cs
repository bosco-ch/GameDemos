using UnityEngine;

namespace PlayerMainScenes.States
{
    /// <summary>
    /// 追击状态
    /// </summary>
    public class ChaseState : IState
    {
        // Start is called before the first frame update
        FSM manager;//FSM管理器
        Paramters paramters;  //参数
        public ChaseState(FSM fSM)
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
            paramters.animator.Play("Idle", 0, 0);
        }


        public void Update()
        {
            manager.FlipTo(paramters.Target);
            if (paramters.Target)
                manager.transform.position = Vector2.MoveTowards(
                    manager.transform.position,//起始位置
                    paramters.Target.position,//目标位置
                    paramters.Speed * Time.deltaTime);
            if (paramters.Target == null
                || paramters.Target.position.x < paramters.chasePoints[0].position.x
                || paramters.Target.position.x > paramters.chasePoints[1].position.x)
            {
                manager.TransitionState(StateType.Idle);
            }
            Collider2D player = Physics2D.OverlapCircle(paramters.attackPoint.position, paramters.attackArea, paramters.targetMask);
            if (player != null && player.gameObject.transform == paramters.Target)
            {
                manager.TransitionState(StateType.Attack);
            }
        }
    }
}