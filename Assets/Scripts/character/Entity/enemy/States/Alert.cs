using character.Entity.bases;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace character.Entity.enemy.States
{
    public class Alert : IEnemyState
    {
        private readonly float _maxTime = 3f;
        private float _time;
        [UsedImplicitly] public static readonly UnityEvent OnAnimatorIdleEnter = new UnityEvent(); //播放动画
        public static readonly UnityEvent OnAlertEnter = new UnityEvent();
        private readonly EnemySmart2dAI _enemy;
        private Vector3 _targetPos;

        public Alert(EnemySmart2dAI enemy)
        {
            this._enemy = enemy;
        }

        public void OnEnter()
        {
            if (_enemy.Target != null)
            {
                _targetPos = _enemy.Target.position;
                OnAnimatorIdleEnter?.Invoke(); //播放警觉动画，比如出现感叹号，什么的
                OnAlertEnter?.Invoke();
            }
            else
            {
                _enemy.SwitchState(EnemyState.Patrol);
            }
        }

        public void OnExit()
        {
            _time = 0;
        }

        public void Update()
        {
            if (_time > _maxTime)
            {
                MoveUtil.MoveNavMesh(_enemy.agent, _enemy.PatrolPoints[0], _enemy.PatrolSpeed, out bool Done);
                {
                    if (Done)
                    {
                        _enemy.SwitchState(EnemyState.Patrol);
                    }
                }
            }
            else
            {
                MoveUtil.MoveNavMesh(_enemy.agent, targetpos: _targetPos, _enemy.AlertSpeed, out bool isdone);
                var hit = Detector.RayCast(
                    _enemy.transform.position,
                    _enemy.ViewForward,
                    _enemy.ForwardDis,
                    GameLayer.Player
                );
                if (hit)
                {
                    if (hit.collider.gameObject.layer == GameLayer.Player)
                    {
                        _enemy.Target = hit.collider.transform;
                        _enemy.SwitchState(EnemyState.Attack);
                        return;
                    }
                }

                if (isdone) //走到的时候开启圆形的警戒范围
                {
                    var circle = Detector.CheckPlayerInCircle(
                        _enemy.transform.position,
                        _enemy.AlertRange,
                        GameLayer.PlayerMask
                    );
                    if (circle != null)
                    {
                        _time = 0;
                        _enemy.Target = circle.transform;
                        _enemy.SwitchState(EnemyState.Attack);
                    }
                    else
                    {
                        _time += Time.deltaTime;
                    }
                }
            }
        }
    }
}