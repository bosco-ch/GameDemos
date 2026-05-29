using character.Entity.player.StateMachine;
using UnityEngine;

namespace character.Entity.player.states
{
    /// <summary>
    /// 观察模式，可以慢慢扩大视野
    /// </summary>
    public class Observe : MoveStateBase<PlayerMoveController>, IMoveStateMachine
    {
        public Observe(PlayerMoveController moveController) : base(moveController)
        {
        }

        public void OnEnter()
        {
            Debug.Log("观察周边");
        }

        public void OnUpdate()
        {
            //慢慢扩大观察范围
        }

        public void OnExit()
        {
            Debug.Log("退出观察");
        }
    }
}