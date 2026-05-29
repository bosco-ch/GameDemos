using character.Entity.bases;

namespace character.Entity.enemy.States
{
    public class Backoff : IEnemyState
    {
        private EnemyBase _enemyBase;
        public Backoff(EnemyBase enemyBase)
        {
            this._enemyBase = enemyBase;
        }
        public void OnEnter()
        {
            throw new System.NotImplementedException();
        }

        public void OnExit()
        {
            throw new System.NotImplementedException();
        }

        public void Update()
        {
            throw new System.NotImplementedException();
        }
    }
}