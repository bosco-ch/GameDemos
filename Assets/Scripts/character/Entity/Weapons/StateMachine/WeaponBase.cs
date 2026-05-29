using character.Interfaces;
using Editors;
using UnityEngine;
using UnityEngine.Events;

namespace character.Entity.Weapons.StateMachine
{
    public enum WeaponType
    {
        None, // 不持有武器 / 空手
        Dagger, // 匕首
        SilencedPistol, // 消音手枪 
    }

    public abstract class WeaponBase
    {
        private readonly WeaponConfigSo _configData;
        protected WeaponBase(WeaponConfigSo configData)
        {
            _configData = configData;
        }

        public WeaponConfigSo Data => _configData;
        public AttackWayType AttackWayType => _configData.attackWayType;


        // protected abstract void AttackWay(Transform target);//实现自己的攻击方式
        /// <summary>
        /// 负面影响
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="mask"></param>
        public virtual void EmitAttack(Transform origin, LayerMask mask)
        {
            Detector.CheckPlayerInCircle(origin.position,
                Data.noiseRange,
                mask);
        }

        /// <summary>
        /// 攻击方式
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="direction"></param>
        /// <param name="mask"></param>
        public virtual void AttackWay(Transform origin, Vector2 direction, LayerMask mask)
        {
            var hit = Physics2D.Raycast(origin.position, direction, Data.attackRange, mask);
            if (hit)
            {
                if (hit.collider.TryGetComponent(out IDamageable damageTarget))
                {
                    damageTarget.TakeDamage(9999);
                }
            }
        }
    }
}