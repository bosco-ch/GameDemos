using UnityEngine;

namespace character.Entity.enemy.States
{
    public class   Chase : IEnemyState
    {
        private float dis = 0;
        private float MaxTime = 3f; //巡查时间
        private float time = 0;
        private readonly EnemySmart2dAI _enemy;

        public Chase(EnemySmart2dAI enemy)
        {
            _enemy = enemy;
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
            dis = Vector3.Distance(_enemy.transform.position, _enemy.Target.position);
            //朝着玩家出现过的位置移动，如果玩家还没移开，就开始攻击
            if (dis <= _enemy.AlertRange)
            {
                Vector3 direction = (_enemy.Target.position - _enemy.transform.position).normalized;
                var move = Time.deltaTime * _enemy.ChaseSpeed;
                _enemy.transform.position += (direction * move);
            }
            else
            {
            
            }
        }
    }
}