using character.Entity.player.StateMachine;
using UnityEngine;

namespace character.Entity.player.states
{
    public class AttackAttacking : MoveStateBase<PlayerAttackController>, IAttackStateMachine
    {
        private float _time = 0;
        private float _maxTime = 0;

        public AttackAttacking(PlayerAttackController moveController) : base(moveController)
        {
        }

        public void onEnter()
        {
            _maxTime = MoveController.AttackTime;
            MoveController.WeaponBase.AttackWay(MoveController.transform,
                MoveController.transform.right,
                GameLayer.EnemyMask
            );
        }

        public void onUpdate()
        {
            _time += Time.deltaTime;
            if (_time > _maxTime)
            {
                MoveController.SetAttackState(AttackActionState.Recover);
            }
        }

        public void onExit()
        {
            _time = 0;
        }
    }
}