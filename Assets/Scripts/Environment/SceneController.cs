using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal.Internal;
//用于设置背景的移动
public class SceneController : MonoBehaviour
{
    public Camera camera;
    public Transform tarBackGround;//需要操作的对象
    public float singeSceneMoveDistance = 20f;//单个背景移动的距离，移动超过这个距离就要开始变换

    public float count = 3f;//需要生成的个数
    public Vector3 boatMoveDirection;
    public static Vector3 _boatMoveDirection;
    float backgroundWidth = 0f;
    public float moveSpeed = 2f;//移动速度  
    List<Transform> backgrounds;//背景数组
    // Start is called before the first frame update
    //用于防止对象重新生成导致挂载的脚本
    void Start()
    {
        if (tarBackGround == null)
        {
            Debug.LogError("请在Inspector面板中设置tarBackGroundObj");
            return;
        }
        if (camera == null)
        {
            camera = Camera.main;
        }
        backgrounds = new List<Transform>();
        backgrounds.Add(tarBackGround);
        getWidth();
        InitBakground();
    }

    void FixedUpdate()
    {
        this.transform.position += boatMoveDirection.normalized * moveSpeed * Time.fixedDeltaTime;
        if (boatMoveDirection.normalized != Vector3.zero)
            IsOutOfCameraView(boatMoveDirection.normalized);
    }
    void getWidth()
    {
        backgroundWidth = tarBackGround.GameObject().GetComponent<SpriteRenderer>().bounds.size.x;//获得对方的宽度

    }
    /// <summary>
    /// 初始化背景，生成足够数量的背景以覆盖整个场景,初始化左右两个背景
    /// </summary>
    void InitBakground()
    {
        float angle = -90;
        float pos = 0;
        Vector3 LastboatPos = tarBackGround.transform.position;
        for (int i = 1; i < count; i++)
        {
            if (i % 2 == 0)
            {
                pos = backgroundWidth * i;
                angle = 90;

            }
            else
            {
                pos = -backgroundWidth * i;
                angle = -90;

            }
            LastboatPos = new Vector3(LastboatPos.x + pos, LastboatPos.y, LastboatPos.z);
            GameObject BG = Instantiate(tarBackGround.gameObject, LastboatPos, Quaternion.Euler(0, 0, angle));
            BG.transform.parent = this.transform;
            backgrounds.Add(BG.transform);
        }
    }
    void SceneMove(Vector3 direction)//出现在摄像头外了 就要跳到后面去 但是要考虑到移动方向
    {   //获得摄像机与背景的水平距离
        float distance = Mathf.Abs(camera.transform.position.x - backgrounds[0].position.x);
        float allSceneWidth = backgroundWidth * count;//所有背景的宽度
        if (camera == null) camera = Camera.main;
        if (distance >= singeSceneMoveDistance)
        {
            //移动背景
            foreach (var bg in backgrounds)
            {
                bg.position += direction * allSceneWidth;
            }
        }
    }
    //这里的想法就是，一旦物体移出摄像机的所能看见的范围，就消失，
    void IsOutOfCameraView(Vector3 moveDirection = default)
    {//计算水平的距离就可以了

        //获取摄像机的中心位置（ 世界坐标）
        Vector3 camCenter = camera.transform.position;
        //获取摄像机的宽高
        float camHight = camera.orthographicSize * 2;
        float camWidth = camHight * camera.aspect;
        //计算摄像机的边界
        float leftbound = camCenter.x - camWidth / 2;
        float rightbound = camCenter.x + camWidth / 2;
        if (moveDirection.x > 0)
        {
            //正向移动
            foreach (var bg in backgrounds)
            {
                if (bg.position.x - backgroundWidth / 2 > rightbound)
                {
                    bg.position = new Vector3(bg.position.x - backgroundWidth * count, bg.position.y, bg.position.z);
                }
            }
        }
        else if (moveDirection.x < 0)
        {

            //逆向移动
            foreach (var bg in backgrounds)
            {
                if (bg.position.x + backgroundWidth / 2 < leftbound)
                {
                    bg.position = new Vector3(bg.position.x + backgroundWidth * count, bg.position.y, bg.position.z);
                }
            }
        }
    }
}