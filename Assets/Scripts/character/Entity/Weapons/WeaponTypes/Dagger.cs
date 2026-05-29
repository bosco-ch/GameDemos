using character.Entity.Weapons.StateMachine;
using character.Interfaces;
using Editors;
using UnityEngine;

namespace character.Entity.Weapons.WeaponTypes
{
    public class Dagger : WeaponBase
    {
        // public Dagger()
        // {
        //     AttackRange = .25f;
        //     BaseAttack = 10f;
        //     NoiseRange = 0;
        //     IsSilent = false;
        //     Cooldown = 1f;
        // }
        //
        // public override WeaponType WeaponType => WeaponType.Dagger;
        // public override float WindUpTime => .1f;
        // public override float AttackTime => .2f;
        //
        // public override float RecoverTime => 0.1f;
        //
        //
        private readonly float _offset = .1f;

        private readonly Collider2D[] _hitColliders = new Collider2D[10];

        // public override void ApplyDamage(IDamageable damageTar, float damage)
        // {
        //     damageTar.TakeDamage(damage);
        // }
        //背身伤害一刀毙命
        public Dagger(WeaponConfigSo configData) : base(configData)
        {
        }

        public void BackAttack(IDamageable damageTar)
        {
            damageTar.TakeDamage(9999);
        }

        public override void AttackWay(Transform origin, Vector2 direction, LayerMask mask)
        {
            int count = Physics2D.OverlapCircleNonAlloc(
                origin.position + origin.up * _offset,
                Data.attackRange,
                _hitColliders,
                mask
            );
            for (int i = 0; i < count; i++)
            {
                Collider2D col = _hitColliders[i];
                if (col.CompareTag($"Enemy"))
                {
                    col.GetComponent<IDamageable>().TakeDamage(9999);
                }
            }
        }
    }
}