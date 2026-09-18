using UnityEngine;

namespace PlayerMainScenes.States
{
    /// <summary>
    /// 闲置状态
    /// </summary>
    public class IdleState : IState
    {
        readonly FSM _manager; //FSM管理器
        private readonly Paramters _paramters; //参数

        private float timer; //设置对象在到巡逻点的时候 需要在原地观察一段时间

        public IdleState(FSM fSm)
        {
            this._manager = fSm;
            this._paramters = fSm.paramters;
        }

        public void OnEnter()
        {
            _paramters.animator.Play("Idle", 0, 0);
        }

        public void OnExit()
        {
            timer = 0;
        }

        public void Update()
        {
            if (_paramters.Target
                && _paramters.Target.transform.position.x > _paramters.chasePoints[0].position.x
                && _paramters.Target.position.x < _paramters.chasePoints[1].position.x)
            {
                _manager.TransitionState(StateType.React);
            }

            timer += Time.deltaTime;
            if (timer >= _paramters.IdleTime)
            {
                _manager.TransitionState(StateType.Patrol);
            }
        }
    }
}