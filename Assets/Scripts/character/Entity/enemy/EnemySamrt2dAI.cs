using System.Collections.Generic;
using character.Entity.bases;
using character.Entity.enemy.States;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Vector3 = UnityEngine.Vector3;

namespace character.Entity.enemy
{
    public class EnemySmart2dAI : EnemyBase
    {
        public Transform Target { set; get; }
        // private EnemyBase enemyBase;
        [Header("移动相关")] [SerializeField] private bool isMove = true; //是否需要移动
        public float viewRange = 10f;
        public float closeRange = 2f; //距离过近强制后退
        [SerializeField] private float patrolRange = 4f; //巡逻时能移动的最远距离
        public float PatrolRange => patrolRange;
        [SerializeField] private float chaseSpeed = 1f; //追击速度
        public float ChaseSpeed => chaseSpeed;
        [SerializeField] private float alertSpeed = .8f;
        public float AlertSpeed => alertSpeed; //前往警戒点速度
        [SerializeField] private float patrolSpeed = .3f;
        public float PatrolSpeed => patrolSpeed;
        [SerializeField] private float backSpeed = 1.5f; //后退速度
        [SerializeField] private float fleeHealthThreshold = 0.3f; //逃跑血量阈值
        [SerializeField] private float rotateSpeed = 5f; //转身速度
        private float RotateSpeed => rotateSpeed;
        [Header("敌人视角")] [SerializeField] private float alertRange = 1.5f; //警戒范围
        public float AlertRange => alertRange;
        [SerializeField] private float forwardDis = 2f; //前身能看到的直线距离
        public float ForwardDis => forwardDis; //前身能看到的距离
        [SerializeField] private float forwardRange = 3f; //前身能看到的弧形范围
        public float ForwardRange => forwardRange; //前身能看到的范围
        [SerializeField] private float backDis = 1f; //背身感知的直线距离
        public float BackDis => backDis; //背身感知的直线距离
        [SerializeField] private float backRange; //背身能感觉到的弧形范围;
        public float BackRange => backRange; //背身能感觉到的弧形范围;
        private IEnemyState _iState;
        private readonly Dictionary<EnemyState, IEnemyState> _currentState = new();
        public Vector3 ViewForward => transform.right;
        protected EnemyState CurrentState;
        private Vector3 _moveFaceDirection;
        [SerializeField] private List<Vector3> patrolPoints;
        public NavMeshAgent agent;
        public List<Vector3> PatrolPoints => patrolPoints;

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            if (agent == null)
            {
                Debug.Log("agent is null");
                return;
            }
            // agent.updateRotation = false;
            agent.updateUpAxis = false;
            agent.stoppingDistance = .5f;
        }

        void Start()
        {
            _currentState.Add(EnemyState.Idle, new Idle(this));
            _currentState.Add(EnemyState.Alert, new Alert(this));
            _currentState.Add(EnemyState.Chase, new Chase(this));
            _currentState.Add(EnemyState.BackOff, new Backoff(this));
            _currentState.Add(EnemyState.Attack, new Attack((this)));
            _currentState.Add(EnemyState.Patrol, new Patrol(this));
            _currentState.Add(EnemyState.LostConfuse, new LostConfuse(this));
            _moveFaceDirection = ViewForward;
            //主动计算得出两个巡逻点的位置
            patrolPoints.Add(transform.position); //【0】
            //面朝方向的地方
            Vector3 anotherPos = transform.position + (patrolRange - .2f) * (transform.right);
            patrolPoints.Add(anotherPos);
            if (isMove)
                SwitchState(EnemyState.Patrol);
        }

        protected override void Update()
        {
            base.Update();
            _iState?.Update();
            MoveFaceDirection();
        }

        void MoveFaceDirection()
        {
            if (agent.velocity.magnitude > 0.1f) // 正在移动
            {
                Vector3 moveDir = agent.velocity.normalized;
                // 计算目标角度（2D 朝上为 0°）
                float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
                // 平滑旋转
                agent.transform.rotation = Quaternion.Lerp(
                    agent.transform.rotation,
                    Quaternion.Euler(0, 0, angle),
                    RotateSpeed * Time.deltaTime
                );
            }
        }

        public void SwitchState(EnemyState state)
        {
            _currentState[state]?.OnExit();
            _iState = _currentState[state];
            _iState.OnEnter();
        }


        private void OnDrawGizmos()
        {
            // Debug.Log("run!");
            //前身能看到的距离
            Gizmos.color = Color.red;
            Debug.DrawRay(transform.position, ViewForward, Color.magenta);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position - transform.right * backDis);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, this.alertRange);
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, AttackRange);
        }
    }
}