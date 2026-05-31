using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
public class PlayMovement : MonoBehaviour
{
    public Transform PlayerTransform;
    public Animator animator;
    [Header("Movement Settings")]
    public float moveSpeed = 4f;
    public SpriteRenderer vaildMoveAreaRenderer;//有效移动的区域对象
    public Bounds moveBounds;//移动边界
    private Vector2 minPos = new Vector2();
    private Vector2 maxPos = new Vector2();
    private Vector2 faceDirection = Vector2.zero;//面朝的方向
    public bool isRunning = false;//是否在奔跑
    [Header("move input")]
    public Vector2 moveInput;
    private PlayerInput action;
    [Header("JoyStrick")]
    public VirtualJoystick virtualJoystick;//获取摇杆的值   
    //临时
    private bool _isHold = false;
    // Start is called before the first frame update
    void OnEnable()
    {
        action.actions["Move"].Enable();
    }
    void OnDisable()
    {
        action.actions["Move"].Disable();
    }
    private void Awake()
    {
        action = GetComponent<PlayerInput>();
        var _moveAction = action.actions["Move"];
        _moveAction.performed += PlayerMove_input;
        _moveAction.canceled += PlayerMove_stop;
        //JoyStrick
        virtualJoystick.OnJoystickMove += JoyStickMove;
        PlayerTransform = transform;
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        if (vaildMoveAreaRenderer == null)
        {
            vaildMoveAreaRenderer = GameObject.FindWithTag("Area").GetComponent<SpriteRenderer>();
        }
        if (vaildMoveAreaRenderer == null)
        {
            Debug.LogError("没有找到有效移动区域的SpriteRenderer组件，请检查");
        }
        moveBounds = vaildMoveAreaRenderer.bounds;
        clampToBounds();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isHold)
        {
            animator.SetBool("IsMove", true);
            animator.SetFloat("VerticalSpeed", moveInput.x, .1f, Time.deltaTime);
            animator.SetFloat("HorizontalSpeed", moveInput.y, .1f, Time.deltaTime);
            animator.SetFloat("FaceVertical", faceDirection.x);
            animator.SetFloat("FaceHorizontal", faceDirection.y);
            Vector2 transformPos = new Vector2(moveInput.x, moveInput.y) * moveSpeed * Time.deltaTime;
            Vector2 newPos = new Vector2(transform.position.x + transformPos.x, transform.position.y + transformPos.y);
            float clampedX = Mathf.Clamp(newPos.x, minPos.x, maxPos.x);
            float clampedY = Mathf.Clamp(newPos.y, minPos.y, maxPos.y);
            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }
    }
    void PlayerMove_input(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if (_isHold = moveInput.sqrMagnitude > .01f)
            faceDirection = moveInput;
        else
            moveInput = Vector2.zero;
    }
    void PlayerMove_stop(InputAction.CallbackContext context)
    {
        animator.SetBool("IsMove", false);
        _isHold = false;
        moveInput = Vector2.zero;
    }

    void clampToBounds()
    {
        minPos = moveBounds.min;// + new Vector3(.5f, .5f, 0);
        maxPos = moveBounds.max;//- new Vector3(.5f, .5f, 0);
    }
    // void OnDrawGizmos()
    // {
    //     if (moveBounds != null)
    //     {
    //         Gizmos.color = Color.green;
    //         Gizmos.DrawWireCube(moveBounds.center, moveBounds.size);
    //     }
    // }
    void destory()
    {
        action.actions["Move"].performed -= PlayerMove_input;
        action.actions["Move"].canceled -= PlayerMove_stop;
    }
    ///JoyStrick
    /// 摇杆移动事件
    public void JoyStickMove(Vector2 moveVector)
    {
        moveInput = moveVector;
        if (_isHold = moveInput.sqrMagnitude > .01f)
        {
            faceDirection = moveInput;
            animator.SetBool("IsMove", true);

        }
        else
        {
            moveInput = Vector2.zero;
            animator.SetBool("IsMove", false);
        }
        Debug.Log("摇杆移动事件触发，移动向量：" + moveVector + _isHold);
    }
}
