using System;
using System.Collections.Generic;
using character.Entity.player;
using character.Entity.player.StateMachine;
using character.Entity.Weapons.StateMachine;
using UnityEngine;
using UnityEngine.AI;
using character.Entity.Weapons.WeaponTypes;
using character.Interfaces;
using Manages;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;

#region 角色各种状态

public enum AttackActionState
{
    Idle, //站立不动
    Recover, //后摇
    WindUp, //抬手攻击
    Attack, //发动攻击
}

public enum MoveState
{
    // 停止观察
    Observe,

    // 正常走路
    Walk,

    // 快速跑动
    Run,

    // 静步慢走
    Sneak,

    // 闪现
    Blink,
}

public enum ActionState
{
    None, //不做出攻击
    NormalAttack, //正常攻击
    Skill,
    Dash,
    Defend // 防御/举盾格挡
}

#endregion

public class PlayerBase : MonoBehaviour, IAttackEntity, IDamageable
{
    [Header("人物属性")] [SerializeField] float maxHealth; //最大血量
    [SerializeField] float currentHealth; //当前血量
    public float CurrentHealth => currentHealth;
    private ActionState _actionCurrentState; //采取的措施，攻击，防御，隐藏

    public ActionState ActionCurrentState
    {
        get => _actionCurrentState;
        set => _actionCurrentState = value;
    }

    private bool _isDead;
    public bool IsDead => _isDead;
    private NavMeshAgent _agent;
    public bool IsAttacking => _playerAttackController.AttackActionState != AttackActionState.Idle;
    private PlayerAttackController _playerAttackController;
    public static Action Destroy;

    private void Awake()
    {
        currentHealth = maxHealth;
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = true;
        _agent.updateUpAxis = false;
        //
        _playerAttackController = GetComponent<PlayerAttackController>();
        if (_playerAttackController == null)
        {
            _playerAttackController = transform.AddComponent<PlayerAttackController>();
        }
    }

    void Start()
    {
        CameraManage.Instance.SetTarget(transform);
    }

    // Start is called before the first framea update
    public void Attack()
    {
    }

    public void TakeDamage(float damage)
    {
        Debug.Log(currentHealth);
        if (_isDead) return;
        if (_actionCurrentState != ActionState.Defend)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public void knockback(Vector2 direction, float force)
    {
    }

    public void Die()
    {
        Destroy?.Invoke();
        _isDead = true;
    }

    public void LoadHealth(float saveHealth)
    {
        currentHealth = Mathf.Clamp(saveHealth, 0, maxHealth);
    }
}