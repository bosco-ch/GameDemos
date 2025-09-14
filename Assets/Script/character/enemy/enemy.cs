using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D;

public class enemy : Character
{
    [Header("基本属性")]
    [SerializeField] public GameObject PlayerObject;
    private Transform player;
    [SerializeField] private float chaseDistance = 13f;//追击距离
    [SerializeField] private float attackDistance = 0.8f;//攻击距离
    public UnityEvent<Vector2> onMovementInput;
    public UnityEvent onAttack;
    //public float damage;
    public float offsetx;
    [SerializeField] private Vector2 attackAreaPos;
    [SerializeField] private Vector2 attackAreaSize;
    public SpriteRenderer spriteRenderer;
    //public string layerMask;

    // private void OnTriggerStay2D(Collider2D other)
    // {
    //     // other.
    //     if (other.CompareTag("Player"))
    //     {
    //         other.GetComponent<Character>().taskDamage(10);
    //     }
    // }
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = PlayerObject.GetComponent<Transform>();
    }
    public void MeleeAttackAnimEvent(int attackNum)
    {
        switch (attackNum)
        {
            case 1:
                offsetx = 1.12f; attackAreaSize = new Vector2(2.54f, 3f); break;
            case 2:
                offsetx = 0.46f; attackAreaSize = new Vector2(1.92f, 3.23f); break;
            case 3:
                offsetx = 1.67f; attackAreaSize = new Vector2(3.72f, 3.23f); break;
        }
        attackAreaPos = transform.position;
        offsetx = spriteRenderer.flipX ? -Mathf.Abs(offsetx) : Mathf.Abs(offsetx);
        attackAreaPos.x += offsetx;
        Collider2D[] hitCollider = Physics2D.OverlapBoxAll(attackAreaPos, attackAreaSize, 0f, LayerMask.GetMask(LayerMask.LayerToName(PlayerObject.layer)));
        foreach (var collider in hitCollider)
        {
            Character character = collider.GetComponent<Character>();
            Debug.Log(character);
            if (character != null)
                character.taskDamage(attackNum * 10);
        }
    }

    public void OnDrawGizmos()
    {
        // offsetx = 1.12f; attackAreaSize = new Vector2(2.54f, 3f);
        // attackAreaPos = transform.position;
        // offsetx = spriteRenderer.flipX ? -Mathf.Abs(offsetx) : Mathf.Abs(offsetx);
        // attackAreaPos.x += offsetx;
        Gizmos.color
            = Color.red;
        Gizmos.DrawWireCube(attackAreaPos, attackAreaSize);

    }
    private void Update()
    {
        // Debug.Log(player.position);
        if (player == null)
        {
            return;
        }
        float distance = Vector2.Distance(player.position, this.gameObject.transform.position);
        // Debug.Log(distance);
        //Debug.Log(distance);
        if (distance < chaseDistance)
        {
            if (distance <= attackDistance)
            {
                //攻击
                onMovementInput?.Invoke(Vector2.zero);
                onAttack?.Invoke();
            }
            else
            {
                //追击
                Vector2 direction = player.position - this.gameObject.transform.position;
                onMovementInput?.Invoke(direction.normalized);//传递给enmeyController
            }
        }
        else
        {
            //放弃追击
            onMovementInput?.Invoke(Vector2.zero);
        }
    }
}
