using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Show : MonoBehaviour
{
    private float FPS;
    private float deltaTime;    
    public GUIStyle style;  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        FPS = 1.0f / deltaTime; 
        if(style == null)
        {
            style = new GUIStyle();
            style.fontSize = 30;
            style.normal.textColor = Color.white;
        }

    }   

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 50), 
        "FPS: " + Mathf.Ceil(FPS).ToString(), 
        new GUIStyle
        {
            fontSize = 30,
            normal = new GUIStyleState { textColor = Color.black }
        });
    }                
}
