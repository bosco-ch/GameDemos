using System;
using character.Interfaces;
using Manages;
using UnityEngine;

namespace character.Entity.bases
{
    public enum EnemyState
    {
        Idle,
        Alert,
        Chase,
        Attack,
        BackOff, //后退\
        Patrol,
        LostConfuse, //丢失目标产生疑惑
    }

    public class EnemyBase : MonoBehaviour, IAttackEntity, IDamageable
    {
        [Header("基础属性")] [SerializeField] private float moveSpeed;
        public float maxHealth;
        public float currentHealth;
        [Header("攻击属性")] public bool isDead;
        public bool isStunned;
        [SerializeField] private float attackCooldown = 1.5f; //攻击冷却时间
        public float AttackCoolDown => attackCooldown;

        [SerializeField] private float attackRange = 1.8f;
        public float AttackRange => attackRange;

        private bool _isAttack = false;

        //死亡通知
        public new static Action Destroy;
        public bool IsAttacking
        {
            get => _isAttack;
            set => _isAttack = value;
        }
        public bool IsDead => isDead;
        protected Rigidbody2D Rb;

        public void SwitchAttackState()
        {
            _isAttack = !_isAttack;
        }

        void Awake()
        {
            Rb = GetComponent<Rigidbody2D>();
        }

        protected virtual void Update()
        {
            if (isDead)
                return;
        }

        public void TakeDamage(float damage)
        {
            FrameManage.Instance.TriggerFreezeTime();
            if (isDead)
                return;
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Die();
            }
        }

        public void knockback(Vector2 direction, float force)
        {
            throw new System.NotImplementedException();
        }

        public void Attack()
        {
            throw new System.NotImplementedException();
        }

        void Die()
        {
            isDead = true;
            Destroy?.Invoke();
        }
    }
}