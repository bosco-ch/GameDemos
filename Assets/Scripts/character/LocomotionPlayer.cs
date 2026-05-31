using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class LocomotionPlayer : MonoBehaviour
{
    public float moveSpeed = 5f; // 移动速度
    public float rotateSpeed = 180f; // 旋转速度
    private CharacterController controller;
    private Animator animator;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        // 若没有CharacterController，自动添加
        if (controller == null)
            controller = gameObject.AddComponent<CharacterController>();
    }

    void Update()
    {
        // 键盘输入检测
        float horizontal = Input.GetAxisRaw("Horizontal"); // A/D或左右箭头
        float vertical = Input.GetAxisRaw("Vertical"); // W/S或上下箭头

        // 计算移动方向
        Vector3 moveDir = transform.forward * vertical + transform.right * horizontal;
        // 防止斜向移动速度过快
        // 应用移动
        if (moveDir.magnitude > 0.01f)
        {
            animator.SetBool("param_idletorunning", true);
            moveDir.Normalize();
            controller.Move(moveDir * moveSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("param_idletorunning", false);
        }
        // 模型旋转（按Q/E旋转，可选）
        if (Input.GetKey(KeyCode.Q))
            transform.Rotate(Vector3.up, -rotateSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.E))
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }
}