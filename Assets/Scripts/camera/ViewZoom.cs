using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class ViewZoom
    : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera Camera;
    public Rigidbody2D rigidbody1;
    public float minFOV;
    public float maxFOV;
    public float speed;
    public float caremaToPlay = 5;
    private UnityEngine.Vector3 vector3;
    void Start()
    {
        Camera = GetComponent<Camera>();
        //rigidbody1 = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //mouse scrollwheel
        float mouseSize = Input.GetAxisRaw("Mouse ScrollWheel");
        float fov = Camera.fieldOfView - mouseSize * speed;
        Camera.fieldOfView = Mathf.Clamp(fov, minFOV, maxFOV);
        vector3 = new UnityEngine.Vector3(rigidbody1.transform.position.x, rigidbody1.transform.position.y, rigidbody1.transform.position.z - 5);
        Camera.transform.position = vector3;
    }
    private void FixedUpdate()
    {

        //Debug.Log(mouseSize);
    }
}
