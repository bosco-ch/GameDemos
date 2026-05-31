using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using Unity.Mathematics;
using UnityEngine;

public class RopeController : MonoBehaviour
{
    /// <summary>
    /// 用来创建可以摆动的绳索
    /// </summary>

    public GameObject chainHead;
    public int chainLength = 4;//绳索长度
    public float chainMass = 0.5f;
    public float chainGravityScale = 1;
    public float lineDrag = 0.5f;
    public float angleDrag = 0.5f;
    public float angleLimit = 45f;//物体摆动幅度
    GameObject[] chainChilds;
    Rigidbody2D headRb2D;//绳索头部的刚体组件

    // Start is called before the first frame update
    void Start()
    {
        if (chainHead == null)
        {
            Debug.LogError("请在Inspector面板中设置chainHead");
            return;
        }
        headRb2D = chainHead.GetComponent<Rigidbody2D>();
        if (headRb2D == null)
        {
            headRb2D = chainHead.AddComponent<Rigidbody2D>();
        }
        headRb2D.bodyType = RigidbodyType2D.Static;//将绳索头部设置为静态物体
        CreateChainChild();
    }

    // // Update is called once per frame
    // void Update()
    // {

    // }
    ///创建其子物体
    private void CreateChainChild()
    {
        chainChilds = new GameObject[chainLength];
        //母物体的位置
        Vector3 headPos = chainHead.transform.localPosition;
        for (int i = 0; i < chainLength; i++)
        {
            GameObject child = new GameObject($"ChainChild_{i}");
            child.transform.parent = chainHead.transform.parent;
            child.transform.localPosition = new Vector3(headPos.x, headPos.y - i - 1, 0);
            var childRb2D = child.AddComponent<Rigidbody2D>();
            childRb2D.gravityScale = chainGravityScale;
            childRb2D.mass = chainMass;
            childRb2D.drag = lineDrag;
            childRb2D.angularDrag = angleDrag;
            //添加贴图
            var sr = child.AddComponent<SpriteRenderer>();
            Sprite sprite = Resources.Load<Sprite>("Imgs/001");
            sr.sprite = sprite;
            //这边是铰链的设置
            var joint = child.AddComponent<HingeJoint2D>();
            joint.connectedBody = i == 0 ? headRb2D : chainChilds[i - 1].GetComponent<Rigidbody2D>();
            joint.autoConfigureConnectedAnchor = false;
            joint.anchor = new Vector2(0, 0.5f);
            joint.connectedAnchor = new Vector2(0, -0.5f);
            joint.useLimits = true;
            //设置铰链的角度限制
            JointAngleLimits2D limits = new JointAngleLimits2D();
            limits.min = -angleLimit;
            limits.max = angleLimit;
            joint.limits = limits;
            //设置铰链的距离关节
            var distanceJoint = child.AddComponent<DistanceJoint2D>();
            distanceJoint.connectedBody = joint.connectedBody;
            distanceJoint.autoConfigureDistance = false;
            distanceJoint.distance = 1f;
            distanceJoint.maxDistanceOnly = true;
            // distanceJoint.dampingRatio = 0.5f;
            if (i == chainLength - 1)
            {
                childRb2D.bodyType = RigidbodyType2D.Static;//将最后一个子物体设置为静态物体
            }
            chainChilds[i] = child;
        }
    }
}
