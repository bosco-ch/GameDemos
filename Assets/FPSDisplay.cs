using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 帧率提示
/// </summary>
public class FPSDisplay : MonoBehaviour
{
    [Header("帧率设置")]
    private float updateInterval = 0.5f;//更新间隔时间
    private Color color = Color.green;//默认颜色
    private int fontSize = 24;

    private float accum = 0; // 累计的帧时间
    private int frames = 0; // 帧数
    private float currectFPSD;
    private GUIStyle style;
    // Start is called before the first frame update
    void Start()
    {
        //初始化 gui
        style = new GUIStyle()
        {
            fontSize = fontSize,
            normal = { textColor = color }
        };
    }

    // Update is called once per frame
    void Update()
    {
        accum += Time.unscaledDeltaTime;
        frames++;
        if (accum >= updateInterval)
        {
            currectFPSD = frames / accum;
            currectFPSD = Mathf.Round(currectFPSD * 10) / 10;//保留一位小数
            accum = 0;
            frames = 0;

        }
    }

    private void OnGUI()
    {
        GUI.Label(new(10, 10, 10, 10), $"_fps:{currectFPSD}", style);
    }
}
