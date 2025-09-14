using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
public class Player : Character
{
    [Header("½üÕ½¹¥»÷")]
    public Vector2 attackSize = new Vector2(1f, 1f);
    private Vector2 attackAreaPos;
    public float offsetx = 1f;
    public float offsety = 1f;
    SpriteRenderer spriteRenderer;
    //public UnityEvent ShootEvent;
    //public string layerMask;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

    }
    void MeleeAttackAnimEvent(float attackNum)
    {
        attackAreaPos = transform.position;
        offsetx = spriteRenderer.flipX ? -Mathf.Abs(offsetx) : Mathf.Abs(offsetx);

        attackAreaPos.x += offsetx;
        attackAreaPos.y += offsety;
        Collider2D[] hitCollider = Physics2D.OverlapBoxAll(attackAreaPos, attackSize, 0f, LayerMask.GetMask(layer));
        //Debug.Log(attackNum);

        foreach (Collider2D collider in hitCollider)
        {
            collider.GetComponent<Character>().taskDamage(1 * attackNum);
        }
    }

    /*
    void shoot()
    {
        ShootEvent?.Invoke();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color
         = Color.red;
        Gizmos.DrawWireCube(attackAreaPos, attackSize);
    }
    */
}
