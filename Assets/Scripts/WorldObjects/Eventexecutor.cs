using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;

public class Eventexecutor : MonoBehaviour
{
    public static Eventexecutor Instance;
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ExecuteEvent(RandomEventData data)
    {
        Debug.Log($"执行事件:{data.eventName}");
        //在这里添加事件执行的具体逻辑
        //生成对象预制体
        // Instantiate(data.eventPrefab, GetOffCameraPosition(), Quaternion.identity);
        Instantiate(data.eventPrefab, data.spawnPosition, Quaternion.identity);
    }
    //返回摄像机外的位置
    public Vector3 GetOffCameraPosition()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {   
            Debug.LogError("Main Camera not found!");
            return Vector3.zero;
        }
        Vector3 offCameraPosition = mainCamera.ViewportToScreenPoint(new Vector3(-0.1f, 0.5f, mainCamera.nearClipPlane));
        return offCameraPosition;
    }
}
