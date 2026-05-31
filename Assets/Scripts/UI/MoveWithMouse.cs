using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveWithMouse : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isDragging;//是否正在拖动
    public float dragSmooth = 200;//拖动平滑度 越大越跟手
    public float followSpeed = 200;
    public GameObject tarObject;//need to be dragged object 

    public bool is2dScene;//是否是2d场景
    EventSystem eventSystem;
    Camera camera;
    Rigidbody2D tarRb2D;
    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        Debug.Log("开始拖动");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        Debug.Log("结束拖动");
    }

    // Start is called before the first frame update
    void Start()
    {
        #region 初始化设置，eventsystem，碰撞体，2d射线检测组件
        camera = Camera.main;
        eventSystem = FindObjectOfType<EventSystem>();
        if (eventSystem == null)
        {
            Debug.Log("没有找到EventSystem，将对其进行设置");
            GameObject eventObject = new GameObject("EventSystem");
            eventSystem = eventObject.AddComponent<EventSystem>();
            // 添加StandaloneInputModule组件以支持鼠标输入
            var oldInputModule = eventSystem.GetComponent<StandaloneInputModule>();
            if (oldInputModule != null)
                Destroy(oldInputModule);

            var newInputModule = eventSystem.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();//如果没有旧的输入模块，则添加新的输入模块
            Debug.Log("已完成对inputModule的添加");
        }

        //这边需要给物体添加一个collider组件才能检测到鼠标事件
        if (tarObject == null)
        {
            Debug.LogError("请在Inspector面板中设置tarObject");
            return;
        }
        var tarObjectCollider = tarObject.GetComponent<BoxCollider2D>();
        if (tarObjectCollider == null)
        {
            Debug.Log("目标物体没有Collider2D组件，自动添加");
            tarObjectCollider = tarObject.AddComponent<BoxCollider2D>();
        }
        tarObjectCollider.isTrigger = false;//取消触发器
        tarObjectCollider.size = new Vector2(1f, 1f);
        //现在给摄像机 添加2d射线检测组件
        if (is2dScene && camera.GetComponent<Physics2DRaycaster>() == null)
        {
            Debug.Log("摄像机没有Physics2DRaycaster组件，自动添加");
            camera.gameObject.AddComponent<Physics2DRaycaster>();
        }
        #endregion
        tarRb2D = tarObject == null ? null : tarObject.GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame

    void FixedUpdate()
    {
        if (isDragging && tarObject != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = camera.WorldToScreenPoint(tarObject.transform.position).z;//保持物体在同一平面上
            Vector3 targetPosition = camera.ScreenToWorldPoint(mousePosition);
            Debug.Log($"鼠标位置：{mousePosition}，目标位置：{targetPosition}");
            // if (tarRb2D != null)
            // {
            //     // 计算差值
            //     Vector3 distance = mousePosition - camera.WorldToScreenPoint(tarObject.transform.position);
            //     tarRb2D.velocity = distance * followSpeed * Time.fixedDeltaTime;
            // }
            // else
            // {
            tarObject.transform.position = Vector3.Lerp(tarObject.transform.position, targetPosition, dragSmooth * Time.fixedDeltaTime);
            // }
        }
    }
    private void OnDrawGizmos()
    {
        if (tarObject != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(tarObject.transform.position, new Vector3(new Vector2(1f, 1f).x, new Vector2(1f, 1f).y, 0f));
        }
    }
}