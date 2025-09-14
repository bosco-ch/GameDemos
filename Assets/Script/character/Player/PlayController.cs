using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class PlayController : MonoBehaviour
{
    [Header("人物属性")]
    public InputActions actions;
    private Rigidbody2D rgbody;    // Start is called before the first frame update
    private Animator animator;
    public bool _isSprint = false;
    public Vector2 movement;
    public int curretSpeed = 20;
    public Image canvas;
    //是否连续攻击
    public bool _meleeAttack = false;
    public bool isDead;
    [Header("武器")]
    //public InputActions actions;
    public GameObject shootWeapoon;
    public float ShootSpeed = 2f;
    public float WeaponScale = 1f;
    private void Update()
    {
        animator.SetBool("isAttack", _meleeAttack);
        animator.SetBool("isDead", isDead);
        //更新血条
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>().normalized;
        Vector2 velocity = movement * curretSpeed;
        rgbody.velocity = velocity;
        animator.SetFloat("speed", rgbody.velocity.sqrMagnitude);
        if (movement.x != 0)
        {
            GetComponent<SpriteRenderer>().flipX = movement.x < 0;
        }
        //Debug.Log("当前位置:" + movement);
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        //_isSprint = context.action.WasPressedThisFrame();
        if (context.performed)
        {
            _isSprint = true;

        }
        else if (context.canceled)
        {
            _isSprint = false;
        }
        Debug.Log("加速状态:" + _isSprint);
    }
    private void Awake()
    {
        actions = new InputActions();
        rgbody = GetComponent<Rigidbody2D>();
        // spriteRenderer = rgbody.GetComponent<SpriteRenderer>();
        animator = rgbody.GetComponent<Animator>();
        rgbody.gravityScale = 0;
        actions?.Enable();
        canvas = GetComponent<Image>();
        //正常行走
        //actions.GamePlay.Move.started += OnMove;
        actions.GamePlay.Move.performed += OnMove;
        actions.GamePlay.Move.canceled += OnMove;
        //加速
        actions.GamePlay.Sprint.performed += OnSprint;
        actions.GamePlay.Sprint.canceled += OnSprint;

        //攻击脚本
        actions.GamePlay.MeleeAttack.started += OnMeleeAttack;

        actions.GamePlay.Fire.started += OnShoot;
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        //OnShoot();
        animator.SetTrigger("isShoot");
    }

    private void OnMeleeAttack(InputAction.CallbackContext context)
    {
        animator.SetTrigger("melonAttack");
        _meleeAttack = true;
    }
    public void PlayHurt()
    {
        animator.SetTrigger("hurt");
    }
    public void PlayDead()
    {
        isDead = true;//限制玩家输入
        switchActionMap(actions.UI);
    }

    void switchActionMap(InputActionMap inputsmap)
    {
        actions.Disable();
        inputsmap.Enable();

    }


    public void OnShootAnimationEvent()
    {
        //实例化 武器
        GameObject fireball = Instantiate(shootWeapoon, transform.position, transform.rotation);
        fireball.transform.localScale *= WeaponScale;
        //获取 刚体
        Rigidbody2D rigidbody = fireball.GetComponent<Rigidbody2D>();
        //朝着 目标所示的方向移动
        float direction = transform.GetComponent<SpriteRenderer>().flipX ? -1 : 1;
        rigidbody.velocity = new Vector2(direction, 0) * ShootSpeed;
        StartCoroutine((Clear(fireball)));
    }
    IEnumerator Clear(GameObject gameObject)
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
    private void OnEnable() => actions?.Enable();
    private void OnDisable() => actions?.Disable();
}
