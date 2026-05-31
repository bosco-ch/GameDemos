using UnityEngine;

public class Player : Character
{
    [Header("��ս����")] public Vector2 attackSize = new Vector2(1f, 1f);
    private Vector2 _attackAreaPos;
    public float offsetX = 1f;
    public float offsetY = 1f;
    SpriteRenderer _spriteRenderer;
    //public UnityEvent ShootEvent;
    //public string layerMask;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void MeleeAttackAnimEvent(float attackNum)
    {
        _attackAreaPos = transform.position;
        offsetX = _spriteRenderer.flipX ? -Mathf.Abs(offsetX) : Mathf.Abs(offsetX);
        _attackAreaPos.x += offsetX;
        _attackAreaPos.y += offsetY;
        Collider2D[] hitCollider = Physics2D.OverlapBoxAll(_attackAreaPos, attackSize, 0f, LayerMask.GetMask(layer));
        //Debug.Log(attackNum);

        foreach (Collider2D collider in hitCollider)
        {
            collider.GetComponent<Character>().taskDamage(1 * attackNum);
        }
    }
}