using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class VirtualJoystick : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IDragHandler
{
    public Transform joystick;//需要控制的点
    public Vector2 inputMovement;//输出的移动值
    public float joystickRadius = 50f;//摇杆的半径
    private Vector2 joystickPosition;//摇杆的初始位置
    // public Transform TargetPlayer;
    // private PlayMovement playMovement;
    // [Header("测试Event委托事件")]
    public event System.Action<Vector2> OnJoystickMove;
    private void Awake()
    {
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 dir = eventData.position - (Vector2)joystickPosition;
        if (dir.magnitude > joystickRadius)
        {
            inputMovement = dir.normalized;
        }
        else
        {
            inputMovement = dir / joystickRadius;
        }
        joystick.position = joystickPosition + new Vector2(inputMovement.x * joystickRadius, inputMovement.y * joystickRadius);
        OnJoystickMove?.Invoke(inputMovement);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        joystickPosition = joystick.position;
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputMovement = Vector2.zero;
        joystick.position = joystickPosition;
        inputMovement = Vector2.zero;
        OnJoystickMove?.Invoke(inputMovement);
    }
}
