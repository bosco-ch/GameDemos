using System;
using System.Collections;
using System.Collections.Generic;
using PlayerMainScenes.States;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using IState = PlayerMainScenes.IState;

public enum StateType
{
    Idle,
    Patrol,
    Chase,
    Attack,
    React
}
[Serializable]
public class Paramters
{
    public float Health;
    public float Speed;
    public float chaseSpeed;
    public Transform Target;
    public float IdleTime;
    public Transform[] patrolPoints;//巡逻点
    public Transform[] chasePoints;//追逐点 
    public Animator animator;//管理动画
    public LayerMask targetMask;
    public Transform attackPoint;
    public float attackArea;
}

/// <summary>
/// 有限状态机
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]//自带刚体，以防遗漏
public class FSM : MonoBehaviour
{

    private IState _currentState;
    public Paramters paramters;
    private Dictionary<StateType, IState> _states = new();
    // Start is called before the first frame update
    void Start()
    {
        _states.Add(StateType.Idle, new IdleState(this));
        _states.Add(StateType.Patrol, new PatrolState(this));
        _states.Add(StateType.Chase, new ChaseState(this));
        _states.Add(StateType.Attack, new AttackState(this));
        _states.Add(StateType.React, new ReactState(this));
        TransitionState(StateType.Idle);
        paramters.animator = GetComponent<Animator>();
        if (paramters.animator == null)
        {
            Debug.Log("动画状态机对象没有找到");
        }
    }

    // Update is called once per frame
    void Update()
    {
        _currentState.Update();
    }
    //切换状态
    public void TransitionState(StateType stateType)
    {
        if (_currentState != null)
        {
            _currentState.OnExit();
        }
        _currentState = _states[stateType];
        _currentState.OnEnter();
    }
    //这里考虑到需要改变对象的面朝方向
    public void FlipTo(Transform target)
    {
        if (target == null) return;
        if (target != null)
        {
            if (transform.position.x > target.position.x)
            {
                transform.GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (transform.position.x < target.position.x)
            {
                // transform.GetComponent<SpriteRenderer>().flipX = false;
                transform.GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            paramters.Target = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            paramters.Target = null;
        }
    }
    // void OnDrawGizmos()
    // {
    //     Gizmos.DrawWireSphere(paramters.attackPoint.position, paramters.attackArea);
    // }

}
