using character.Entity.player.StateMachine;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

namespace character.Entity.player.states
{
    public class Run : MoveStateBase<PlayerMoveController>, IMoveStateMachine
    {
        private float _originSpeed;
        private float _noiseRange = 1.5f;

        public Run(PlayerMoveController moveController) : base(moveController)
        {
        }
        public void OnEnter()
        {
            _originSpeed = MoveController.MoveSpeed;
            MoveController.MoveSpeed = 3f;
            MoveController.MoveNoise += _noiseRange;
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