using System.Collections.Generic;
using System.Linq;
using character.Entity.player.StateMachine;
using character.Entity.player.states;
using character.Entity.Weapons.StateMachine;
using character.Entity.Weapons.WeaponTypes;
using Editors;
using UnityEngine;

namespace character.Entity.player
{
    /// <summary>
    /// 角色攻击模块
    /// </summary>
    public class PlayerAttackController : MonoBehaviour
    {
        [Header("LineRenderer Components")] private LineRenderer _lineRenderer;
        private Vector3 _linRenderEnd; //辅助瞄准线结束位置
        [Header("Attack Settings")]
        [SerializeField]
        private List<WeaponConfigSo> weaponSoList;
        private WeaponBase _weaponBase;
        public WeaponBase WeaponBase => _weaponBase;
        private float AttackRange => _weaponBase.Data.attackRange; //攻击范围
        public float AttackWindUp => _weaponBase.Data.windUpTime; //前摇
        public float AttackTime => _weaponBase.Data.attackTime; //攻击中
        public float RecoverTime => _weaponBase.Data.recoverTime; //后摇
        public int WeaponIndex
        {
            get => _weaponIndex;
            set => _weaponIndex = value;
        }
    
        private int _weaponIndex = 1;
        private readonly List<WeaponBase> _weaponTypes = new List<WeaponBase>();

        [Header("State Machine")]
        private readonly Dictionary<AttackActionState, IAttackStateMachine> _attackMachine = new();

        private AttackActionState _attackActionCurrentState; //攻击状态

        public AttackActionState AttackActionState
        {
            get => _attackActionCurrentState;
            private set
            {
                if (_attackActionCurrentState == value) return;
                if (!_attackMachine.Keys.Contains(value)) return;
                _attackMachine[_attackActionCurrentState]?.onExit();
                _attackActionCurrentState = value;
                _attackMachine[value].onEnter();
            }
        }
        [Header("Rotation")] private Vector3 _mousePos;
        private float _angleOffset;
        private Camera _camera;

        void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.startColor = Color.red;
            _lineRenderer.endColor = Color.red;
            _lineRenderer.widthMultiplier = .05f;
            _attackMachine.Add(AttackActionState.WindUp, new AttackWindUp(this));
            _attackMachine.Add(AttackActionState.Idle, new AttackIdle(this));
            _attackMachine.Add(AttackActionState.Recover, new AttackRecover(this));
            _attackMachine.Add(AttackActionState.Attack, new AttackAttacking(this));
        }

        void Start()
        {
            foreach (var data in weaponSoList)
            {
                if (data.attackWayType == AttackWayType.Melee) //短手
                {
                    _weaponTypes.Add(new Dagger(data));
                }
                else if (data.attackWayType == AttackWayType.Ranged) //长手
                {
                    _weaponTypes.Add(new SilencedPistol(data));
                }
            }

            _camera = Camera.main;
            EquipWeapon(index: _weaponIndex); //替换 武器
            SetAttackState(AttackActionState.Idle); //enter idle start
        }

        private void Update()
        {
            RotateControl();
            _attackMachine[_attackActionCurrentState].onUpdate();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _weaponIndex = (_weaponIndex + 1) % _weaponTypes.Count;
                EquipWeapon(_weaponIndex);
            }

            if (Input.GetMouseButtonDown(0))
            {
                StartAttack();
            }
        }

        /// <summary>
        /// 渲染相关的都放在这里
        /// </summary>
        private void LateUpdate()
        {
            SniperLine();
        }

        //装备武器
        void EquipWeapon(int index)
        {
            if (_attackActionCurrentState == AttackActionState.Idle) //just can change weapon when idle
            {
                _weaponBase = _weaponTypes[index];
            }
        }

        void StartAttack()
        {
            if (_attackActionCurrentState == AttackActionState.Idle)
            {
                SetAttackState(AttackActionState.WindUp); //attack windup first;
            }
        }

        /// <summary>
        /// 跟随鼠标方向转头
        /// </summary>
        void RotateControl()
        {
            //角色朝向
            _mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
            _mousePos.z = 0;
            var lookDir = _mousePos - transform.position;
            _angleOffset = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg * Time.timeScale - 90;
            transform.rotation = Quaternion.Euler(0, 0, _angleOffset);
        }

        /// <summary>
        /// 瞄准辅助线
        /// </summary>
        void SniperLine()
        {
            _linRenderEnd = transform.position + transform.up * AttackRange;
            var hit = Physics2D.Raycast(
                transform.position,
                transform.up,
                AttackRange,
                GameLayer.WallOrEnemy
            );
            if (hit)
            {
                _linRenderEnd = hit.point;
            }
            _linRenderEnd.z = -.1f;
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, _linRenderEnd);
        }

        public void SetAttackState(AttackActionState actionState)
        {
            AttackActionState = actionState;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + transform.up * .1f, .25f);
        }
    }
}