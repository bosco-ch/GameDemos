using System.Collections;
using character.Entity.bases;
using character.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace character.Entity.enemy.States
{
    public class Attack : IEnemyState
    {
        private readonly EnemySmart2dAI _enemy;
        private IDamageable _damageable;
        private Coroutine _attackCoroutine;
        private Transform _targetCache;
        private UnityEvent AttackAnimator;
        public Attack(EnemySmart2dAI enemy)
        {
            _enemy = enemy;
        }

        public void OnEnter()
        {
            // 安全停止旧协程
            StopAttackCoroutine();
            // 无目标直接退出
            if (_enemy.Target == null)
            {
                _enemy.SwitchState(EnemyState.Alert);
                return;
            }
            _targetCache = _enemy.Target;
            _damageable = _targetCache.GetComponent<IDamageable>();
            _enemy.IsAttacking = true;
            _attackCoroutine = _enemy.StartCoroutine(AttackTarget());
            AttackAnimator?.Invoke();
        }

        public void OnExit()
        {
            StopAttackCoroutine();
            _enemy.IsAttacking = false;
        }

        public void Update()
        {
            // 检测玩家是否在攻击范围
            var hit = Detector.CheckPlayerInCircle(
                _enemy.transform.position,
                _enemy.AttackRange,
                GameLayer.PlayerMask
            );

            if (hit)
            {
                _targetCache = hit.transform;
                _enemy.Target = _targetCache;
                // 协程没启动 → 启动
                if (_attackCoroutine == null)
                {
                    _attackCoroutine = _enemy.StartCoroutine(AttackTarget());
                }
            }
            else
            {
                // 玩家丢失 → 停止攻击
                StopAttackCoroutine();
                Debug.Log("丢失目标");
                _enemy.SwitchState(EnemyState.LostConfuse);
            }
        }

        private IEnumerator AttackTarget()
        {
            var damageable = _damageable;
            var target = _targetCache;
            while (target != null && damageable != null)
            {
                damageable.TakeDamage(10);
                yield return new WaitForSeconds(_enemy.AttackCoolDown);
            }
            // 结束后置空
            _attackCoroutine = null;
        }

        private void StopAttackCoroutine()
        {
            if (_attackCoroutine != null)
            {
                _enemy.StopCoroutine(_attackCoroutine);
                _attackCoroutine = null;
            }
        }
    }
}