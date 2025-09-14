using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("属性")]
    public Camera Camera;
    public Transform CameraTarget ;
    public GameObject CameraTargetGameObject;
    //private Vector2 TargetPostion;
    public double MaxOffset = 10f;
    public float Offset = 0f;
    public float CameraiDistance = -20f;
    //private int offsetX = 0, offsetY = 0;   
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (Offset < MaxOffset)
        //{
        //    Offset += 1f;
        //}
        //Debug.Log(Offset);
        //this.gameObject.transform.position = new Vector3(CameraTarget.transform.position.x + Offset, CameraTarget.transform.position.y, -4);
        this.gameObject.transform.position = new Vector3(CameraTarget.transform.position.x, CameraTarget.transform.position.y, CameraiDistance);
    }
}
