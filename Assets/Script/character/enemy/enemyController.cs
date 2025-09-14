using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyConlltor : MonoBehaviour
{

    [Header("属性")]
    [SerializeField] private float currentSpeed = 0;    //当前速度
    public Vector2 MovementInput { get; set; }
    private Rigidbody2D rb;
    private SpriteRenderer rbSprite;
    private Animator anim;
    public bool isMove = false;
    public bool isAttack = true;
    public float attackCoolDuration = 3;
    // Start is called before the first frame update
    private void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        rbSprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
        anim.SetBool("isMove", MovementInput.magnitude > 0);
    }

    private void Move()
    {
        if (MovementInput.magnitude > 0.1f && currentSpeed >= 0)
        {
            rb.velocity = MovementInput * currentSpeed;
            if (MovementInput.x > 0)
            {
                rbSprite.flipX = false;
            }
            else
            {
                rbSprite.flipX = true;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

    }
    public void Attack()
    {
        if (isAttack)
        {
            isAttack = false;
            anim.SetTrigger("isAttack");
            StartCoroutine(nameof(AttackCoroutine));
        }
    }
    IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(attackCoolDuration);
        isAttack = true;
    }

}
