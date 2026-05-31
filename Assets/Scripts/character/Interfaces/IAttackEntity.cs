namespace character.Interfaces
{
    ///can attack
    public interface IAttackEntity
    {
        void Attack();
        bool IsAttacking { get; }
    }
}
