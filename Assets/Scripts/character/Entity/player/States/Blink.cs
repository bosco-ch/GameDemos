using character.Entity.player.StateMachine;

namespace character.Entity.player.states
{
    public class Blink : MoveStateBase<PlayerMoveController>, IMoveStateMachine
    {
          
        public Blink(PlayerMoveController moveController) : base(moveController)
        {
        }
        public void OnEnter()
        {
            throw new System.NotImplementedException();
        }

        public void OnUpdate()
        {
            throw new System.NotImplementedException();
        }

        public void OnExit()
        {
            throw new System.NotImplementedException();
        }
    }
}