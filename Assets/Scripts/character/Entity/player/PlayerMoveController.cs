using System;
using System.Collections.Generic;
using System.Linq;
using character.Entity.player.StateMachine;
using character.Entity.player.states;
using UnityEngine;

namespace character.Entity.player
{
    /// <summary>
    /// player 移动模块
    /// </summary>
    public class PlayerMoveController : MonoBehaviour
    {
        private InputActions _actions;
        private Vector2 _moveDirection;
        [SerializeField] private float moveSpeed = 1.5f;

        public float MoveSpeed
        {
            get => moveSpeed;
            set => moveSpeed = value;
        }

        private bool _isRunning; //是否跑步
        private bool _isSneaking; //是都静步
        [SerializeField] private float moveNoise = 1f; //移动时候造成的噪音

        public float MoveNoise
        {
            get => moveNoise;
            set => moveNoise = value;
        }

        private MoveState _currentMoveState;

        private MoveState MoveState
        {
            get => _currentMoveState;
            set
            {
                if (value == _currentMoveState) return;
                if (!_moveStateMachine.Keys.Contains(value)) return;
                _moveStateMachine[_currentMoveState]?.OnExit();
                _currentMoveState = value;
                _moveStateMachine[_currentMoveState]?.OnEnter();
            }
        } //移动状态控制

        private readonly Dictionary<MoveState, IMoveStateMachine> _moveStateMachine = new();

        void Awake()
        {
            _actions = new InputActions();
            // _actions.Enable();
            _actions.Player.Enable();
        }

        void Start()
        {
            _moveStateMachine.Add(MoveState.Observe, new Observe(this)); //缓慢提升提升观察视角
            _moveStateMachine.Add(MoveState.Walk, new Walk(this)); //正常
            _moveStateMachine.Add(MoveState.Run, new Run(this)); //会增加噪音范围
            _moveStateMachine.Add(MoveState.Sneak, new Sneak(this)); //降低噪音范围
            _moveStateMachine.Add(MoveState.Blink, new Blink(this)); //闪现
        }

        private void Update()
        {
            _moveDirection = _actions.Player.Move.ReadValue<Vector2>();
            _isRunning = _actions.Player.Run.IsPressed();
            _isSneaking = _actions.Player.Sneak.IsPressed();
            if (_moveDirection != Vector2.zero)
            {
                //正常移动移动
                if (_isRunning && _isSneaking) return;
                else if (_isRunning)
                    SetMoveState(MoveState.Run);
                else if (_isSneaking)
                    SetMoveState(MoveState.Sneak);
                else
                    SetMoveState(MoveState.Walk);
            }
            else
            {
                SetMoveState(MoveState.Observe);
            }

            MoveUtil.Move2DWithoutBody(transform, _moveDirection,
                moveSpeed);
            _moveStateMachine[MoveState]?.OnUpdate();
        }

        ///移动噪音范围调整
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, moveNoise);
        }

        private void SetMoveState(MoveState state)
        {
            MoveState = state;
        }

        private void OnDisable()
        {
            _actions.Player.Disable();
        }

        private void OnEnable()
        {
            _actions.Player.Enable();
        }
    }
}