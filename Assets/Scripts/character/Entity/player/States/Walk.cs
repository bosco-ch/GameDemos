using character.Entity.player.StateMachine;
using UnityEngine;
using UnityEngine.Events;

namespace character.Entity.player.states
{
    public class Walk: MoveStateBase<PlayerMoveController>, IMoveStateMachine
    {
        private UnityEvent _animatorEvent;
        public Walk(PlayerMoveController moveController) : base(moveController)
        {
        }

        public void OnEnter()
        {
            _animatorEvent?.Invoke();
        }
        public void OnUpdate()
        {
         
        }

        public void OnExit()
        {
        }
    }
}