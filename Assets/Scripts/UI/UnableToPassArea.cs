using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using JetBrains.Annotations;
using UnityEngine;

public class UnableToPassArea : MonoBehaviour
{
    public SpriteRenderer areaSpriteRenderer;//区域的SpriteRenderer组件
    // Start is called before the first frame update
    public Size size;
    private void Awake()
    {
        areaSpriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        if(areaSpriteRenderer==null)
        {
            Debug.LogError("没有找到区域的SpriteRenderer组件，请检查");
            return;
        }   

    }

    // Update is called once per frame
    void Update()
    {

    }
}
