namespace character.Entity.player.StateMachine
{
    public interface IMoveStateMachine
    {
        void OnEnter();
        void OnUpdate();
        void OnExit();
    }
}