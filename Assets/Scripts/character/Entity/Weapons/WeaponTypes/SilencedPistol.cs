using character.Entity.Weapons.StateMachine;
using Editors;
using UnityEngine;

namespace character.Entity.Weapons.WeaponTypes
{
    public class SilencedPistol : WeaponBase
    {
        // public SilencedPistol()
        // {
        //     AttackRange = 3f;
        //     BaseAttack = 50f;
        //     NoiseRange = 2;
        //     IsSilent = false;
        //     Cooldown = 2f;
        // }
        //
        // public override WeaponType WeaponType => WeaponType.SilencedPistol;
        //
        // public override float WindUpTime => .2f;
        //
        // public override float AttackTime => .4f;
        //
        // public override float RecoverTime => .2f;
        //
        //
        // [SerializeField] private GameObject bulletPrefab;

        public SilencedPistol(WeaponConfigSo configData) : base(configData)
        {
        }

        public override void AttackWay(Transform origin, Vector2 direction, LayerMask mask)
        {
            Vector3 bulletRotate = origin.rotation.eulerAngles;
            bulletRotate.z += 60;
            //生成子弹
            var bullet = GameObject.Instantiate(Data.bulletPrefab, origin.position, Quaternion.Euler(bulletRotate));
            bullet.AddComponent<BulletController>().Initialize(3f, 2f, origin.transform.up);
        }
    }
}