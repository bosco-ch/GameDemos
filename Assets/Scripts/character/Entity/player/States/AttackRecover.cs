using character.Entity.player.StateMachine;
using UnityEngine;

namespace character.Entity.player.states
{
    public class AttackRecover : MoveStateBase<PlayerAttackController>, IAttackStateMachine
    {
        private float _time = 0;
        private float _maxTime = 0;

        public AttackRecover(PlayerAttackController moveController) : base(moveController)
        {
        }

        public void onEnter()
        {
            _maxTime = MoveController.RecoverTime;
        }

        public void onUpdate()
        {
            _time += Time.deltaTime;
            if (_time > _maxTime)
            {
                MoveController.SetAttackState(AttackActionState.Idle);
            }
        }

        public void onExit()
        {
            _time = 0;
        }
    }
}