using character.Entity.bases;
using UnityEngine;
using UnityEngine.Events;

namespace character.Entity.enemy.States
{
    /// <summary>
    /// 丢失目标产生疑惑
    /// </summary>
    public class LostConfuse : IEnemyState
    {
        public UnityEvent ConfuesAnimator;
        private EnemySmart2dAI enemy;
        private float time;
        private float MaxTime = 5f;

        public LostConfuse(EnemySmart2dAI enemy)
        {
            this.enemy = enemy;
        }

        public void OnEnter()
        {
            Debug.Log("丢失目标，疑惑中");
            time = 0;
            ConfuesAnimator?.Invoke();
        }

        public void OnExit()
        {
            time = 0;
        }

        public void Update()
        {
            var hit = Detector.RayCast(
                enemy.transform.position,
                enemy.ViewForward,
                enemy.ForwardDis,
                GameLayer.PlayerMask
            );
            if (hit) //直接碰到了 触发攻击
            {
                enemy.SwitchState(EnemyState.Attack);
                return;
            }

            var collider = Detector.CheckPlayerInCircle(
                enemy.transform.position,
                enemy.AlertRange,
                GameLayer.PlayerMask
            );
            if (collider != null)
            {
                enemy.SwitchState(EnemyState.Attack);
                return;
            }

            time += Time.deltaTime;
            if (time > MaxTime)
            {
                //返回巡逻点
                MoveUtil.MoveNavMesh(enemy.agent, enemy.PatrolPoints[0], enemy.PatrolSpeed, out bool Done);
                {
                    if (Done)
                    {
                        enemy.SwitchState(EnemyState.Patrol);
                    }
                }
            }
        }
    }
}