using character.Entity.bases;
using UnityEngine;
using UnityEngine.Events;

namespace character.Entity.enemy.States
{
    public class Patrol : IEnemyState
    {
        private readonly EnemySmart2dAI _enemy;
        private Vector2 _rayCastDirection; //射线检测方向
        public readonly UnityEvent PatrolEvent = new UnityEvent();
        private int _isChangeDirection = 1; //这个是用来定那个往那个巡逻点点移动的
        private Vector3 _nextPoint;

        public Patrol(EnemySmart2dAI enemy)
        {
            _enemy = enemy;
        }

        public void OnEnter()
        {
            _nextPoint = _enemy.PatrolPoints[1];
            _rayCastDirection = (_nextPoint - _enemy.transform.position).normalized;
            _enemy.agent.speed = _enemy.PatrolSpeed;
            PatrolEvent?.Invoke();
        }

        public void OnExit()
        {
        }

        public void Update()
        {
            HandleRayCast(); //检测墙体 或者检测同事，免得撞到
            MoveUtil.MoveNavMesh(_enemy.agent, _nextPoint, _enemy.PatrolSpeed, out bool isdone);
            if (isdone)
            {
                _isChangeDirection++;
                _nextPoint = _enemy.PatrolPoints[_isChangeDirection % 2];
                _rayCastDirection = -_rayCastDirection;
            }
            HandleRayCastPlayer(); //检测玩家
        }

        /// <summary> 射线检测：墙/敌人/ </summary>
        void HandleRayCast()
        {
            //射线检测是一直在进行的，需要每帧都处理
            var hit = Detector.RayCast((Vector2)_enemy.transform.position + _rayCastDirection * 0.25f,
                _rayCastDirection,
                _enemy.ForwardDis,
                GameLayer.WallOrEnemy);
            if (!hit) return;
            if (hit.collider.gameObject.layer == GameLayer.Wall)
            {
                _nextPoint = hit.point;
            }
            else if (hit.collider.gameObject.layer == GameLayer.Enemy)
            {
                _nextPoint = hit.point;
            }
        }

        void HandleRayCastPlayer()
        {
            //射线检测是一直在进行的，需要每帧都处理
            var hit = Detector.RayCast(_enemy.transform.position + _enemy.ViewForward * 0.25f,
                _enemy.ViewForward,
                _enemy.ForwardDis,
                GameLayer.PlayerMask);
            if (!hit) return;
            _enemy.Target = hit.collider.transform;
            _enemy.SwitchState(EnemyState.Alert);
        }
    }
}