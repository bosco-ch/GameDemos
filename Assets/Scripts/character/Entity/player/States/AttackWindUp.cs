using character.Entity.player.StateMachine;
using UnityEngine;
using UnityEngine.Events;

namespace character.Entity.player.states
{
    /// <summary>
    /// 攻击前摇
    /// </summary>
    public class AttackWindUp : MoveStateBase<PlayerAttackController>, IAttackStateMachine
    {
        private float _time = 0;
        private float _maxTime = 0;

        public AttackWindUp(PlayerAttackController moveController) : base(moveController)
        {
        }

        public void onEnter()
        {
            //武器变更为小刀形状
            // AnimationEvent?.Invoke();
            _time = 0;
            _maxTime = MoveController.AttackWindUp;
        }

        public void onUpdate()
        {
            _time += Time.deltaTime;
            if (_time > _maxTime)
            {
                MoveController.SetAttackState(AttackActionState.Attack);
            }
        }

        public void onExit()
        {
            _time = 0;
        }
    }
}