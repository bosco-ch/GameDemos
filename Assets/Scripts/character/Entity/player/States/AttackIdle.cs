using character.Entity.player.StateMachine;
using UnityEngine;

namespace character.Entity.player.states
{
    public class AttackIdle : MoveStateBase<PlayerAttackController>,IAttackStateMachine
    {
        private float _time = 0;
        private float _maxTime = 0;
        public AttackIdle(PlayerAttackController moveController) : base(moveController)
        {
        }
        public void onEnter()
        {
        }

        public void onUpdate()
        {
        }

        public void onExit()
        {
        }
    }
}