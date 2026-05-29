using character.Entity.Weapons.StateMachine;
using UnityEngine;

namespace Editors
{
    /// <summary>
    /// 各类武器表现
    /// </summary>
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Character/Weapon SO")]
    public class WeaponConfigSo : ScriptableObject
    {
        [Header("基本属性")] public WeaponType weaponType;
        public float baseAttack;
        public float attackRange;
        public float cooldown;

        [Header("attack time")] public float windUpTime;
        public float attackTime;
        public float recoverTime;

        [Header("noise")] public float noiseRange;
        public bool isSilent;
        [Header("with buttet")] public GameObject bulletPrefab;
        public float bulletSpeed;
        public float bulletDestoryTime;
        [Header("Attack Way Type")] public AttackWayType attackWayType;
    }
    public enum AttackWayType
    {
        Melee, // 近战（小刀）
        Ranged // 远程（手枪/狙击）
    }
}