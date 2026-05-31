using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class LocomationPlayer2D : MonoBehaviour
{
    public float speed = 10;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector2 moveInput = new Vector2(horizontal, vertical).normalized;
        // 计算目标位置
        Vector2 targetPos = (Vector2)transform.position + moveInput * speed * Time.deltaTime;

        // 检测目标位置是否有障碍物
        // bool isObstacle = Physics2D.OverlapCircle(targetPos, collisionCheckRadius, obstacleLayer);

        // 无障碍物则移动
        // if (!isObstacle)
        // {
        transform.position = targetPos;
        // }
    }
}
