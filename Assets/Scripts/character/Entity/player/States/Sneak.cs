using character.Entity.player.StateMachine;
using UnityEngine;

namespace character.Entity.player.states
{
    public class Sneak : MoveStateBase<PlayerMoveController>, IMoveStateMachine
    {
        private float _originSpeed;

        public Sneak(PlayerMoveController moveController) : base(moveController)
        {
        }

        public void OnEnter()
        {
            _originSpeed = MoveController.MoveSpeed;
            MoveController.MoveSpeed = .5f;
        }

        public void OnUpdate()
        {
        }

        public void OnExit()
        {
            MoveController.MoveSpeed = _originSpeed;
        }
    }
}