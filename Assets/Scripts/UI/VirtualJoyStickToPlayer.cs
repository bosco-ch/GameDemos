using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
public class VirtualJoyStickToPlayer : MonoBehaviour
{
    public VirtualJoystick virtualJoyStick;//获取摇杆的值
    // public VirtualJoystick virtualJoystick;
    // [Header("Event委托事件")]
    public event System.Action<Vector2> OnJoystickMove;
    public event System.Action OnJoystickUp;
    private void Awake()
    {
        // OnJoystickMove?.Invoke(virtualJoyStick.inputMovement);
        // OnJoystickUp?.Invoke();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // OnJoystickMove?.Invoke(virtualJoyStick.inputMovement);
    }
}
