using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SceneMoveController : MonoBehaviour
{
    Vector3 boatMoveDirection;
    [Range(-0.1f, 0.1f)] public float boatMoveSpeed = .1f;//船的移动速度      
    float backgroundWidth;
    SpriteRenderer spriteRenderer;

    [Header("shader控制背景移动的速度")]
    public Vector2 Direction;
    Vector2 _direction
    {
        set
        {
            value = value.normalized;
            value *= boatMoveSpeed;
            ScrollShader();
        }
        get => Direction;
    }
    static readonly int ScrollSpeedID = Shader.PropertyToID("_Direction");
    #region 旧版本的背景移动方式
    // Start is called before the first frame update
    // void Start()
    // {
    //     getWidth();
    // }

    // private void getWidth()
    // {
    //     backgroundWidth = this.transform.GetComponent<SpriteRenderer>().bounds.size.x;//获得对方的宽度

    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     boatMoveDirection = SceneController._boatMoveDirection;
    //     Debug.Log(boatMoveDirection);
    //     if (boatMoveDirection != Vector3.zero)
    //     {
    //         SceneMove();
    //     }
    // }
    // private void SceneMove()
    // {
    //     boatMoveDirection = boatMoveDirection.normalized; //确保移动方向是单位向量
    //     if (boatMoveDirection.x > 0) //玩家向右移动
    //     {
    //         this.transform.position -= new Vector3(boatMoveDirection.x * Time.deltaTime * boatMoveSpeed, 0, 0); //背景向右移动
    //     }
    //     else if (boatMoveDirection.x < 0) //玩家向左移动
    //     {
    //         this.transform.position += new Vector3(-boatMoveDirection.x * Time.deltaTime * boatMoveSpeed, 0, 0); //背景向左移动
    //     }
    //     //获取背景与摄像机的水平距离
    //     float distanceToCamera = Mathf.Abs(this.transform.position.x - Camera.main.transform.position.x);
    //     // Debug.Log($"摄像机与背景的距离: {distanceToCamera}, 背景宽度: {backgroundWidth}");
    //     if (distanceToCamera > backgroundWidth)
    //     {
    //         if (boatMoveDirection.x > 0) // 
    //         {
    //             this.transform.position += new Vector3(backgroundWidth, 0, 0); //将背景移动到摄像机的右侧
    //         }
    //         else if (boatMoveDirection.x < 0) // 
    //         {
    //             this.transform.position -= new Vector3(backgroundWidth, 0, 0); //将背景移动到摄像机的左侧
    //         }
    //     }
    // }
    #endregion
    //现在使用shader来控制这些移动
    void Awake()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        _direction = new Vector2(0.1f, 0); //初始值，向右移动
    }
    void ScrollShader()
    {
        spriteRenderer.material.SetVector(ScrollSpeedID, Direction);
    }
    void OnValidate()
    {
        _direction = Direction;
    }
}
