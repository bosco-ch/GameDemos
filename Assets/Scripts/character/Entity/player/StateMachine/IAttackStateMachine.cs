namespace character.Entity.player.StateMachine
{
    public interface IAttackStateMachine
    {
        void onEnter();
        void onUpdate();
        void onExit();
    }
}