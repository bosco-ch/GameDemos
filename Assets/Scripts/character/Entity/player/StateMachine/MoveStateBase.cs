using UnityEngine.Events;

namespace character.Entity.player.StateMachine
{
    public class MoveStateBase<T>
    {
        public UnityEvent AnimationEvent { get; }

        protected T MoveController;

        public MoveStateBase(T moveController)
        {
            this.MoveController = moveController;
        }
    }
}