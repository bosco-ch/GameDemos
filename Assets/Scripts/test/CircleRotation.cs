using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

// using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class CircleRotation : MonoBehaviour
{
    [Header("旋转组件")]
    public Transform transform;

    [Tooltip("扇区设置")]
    public int sectorNum = 6;
    private int sectorCount = 0;
    public float[] sectorAngles = new float[] { 60f, 60f, 60f, 60f, 60f, 60f };//需要自动计算余下的扇区
    public float redundancyAngles = 0f;//冗余角度，用于调整指针位置
    public Color[] sectorColors;
    public List<string> sectorContent = new() { "", "", "", "", "", "" };
    [Tooltip("圆形平面设置")]
    public float initialSpeed;//初始速度
    public float rotaionDuration = 5f;//旋转持续时间
    public float acceleration = 100;//减速度
    private float _time = 0f;
    public float decelerationTime = 5f;
    private Texture2D _sectorTexture;

    private float wheelRidus = 200f;
    public Font font;
    [Header("测试按钮")]
    public Button startButton;
    // Start is called before the first frame update
    void Start()
    {
        //提供冗余扇区的角度计算
        redundancyAngles = 360 - CalculateTargetAngle(sectorAngles);
        transform = GetComponent<Transform>();
        //给扇区上颜色
        sectorCount = sectorAngles.Count();
        if (redundancyAngles > 0)
        {
            sectorCount++;
        }
        // sectorColors = new Color[sectorCount];
        // for (int i = 0; i < sectorCount; i++)
        // {
        //     float hue = (float)i / sectorCount;
        //     sectorColors[i] = Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.8f, 1f); ;
        // }
        GenerateSectors();

        //运行
        startButton.onClick.AddListener(SpinTransfrom);
        initialSpeed = Random.Range(600f, 1000f);
        // StartCoroutine(RotateOver(initialSpeed));
    }

    // Update is called once per frame
    void Update()
    {
        // if(_time < rotaionDuration)
        // {
        //     // float angleThisFrame = rotationAngle * Time.deltaTime / rotaionDuration;
        //     float angleThisFrame = Mathf.SmoothStep(initialSpeed * Time.deltaTime,0,_time);
        //     Debug.Log("Angle this frame: " + angleThisFrame);
        //     transform.Rotate(0, 0, angleThisFrame);
        //     _time += Time.deltaTime;            
        // }
    }
    void SpinTransfrom()
    {
        initialSpeed = Random.Range(600f, 1000f);
        StartCoroutine(RotateOver(initialSpeed));
    }

    IEnumerator RotateOver(float initialSpeed = 720f)
    {//重置参数

        while (initialSpeed > 0)
        {
            float angleThisFram = initialSpeed * Time.deltaTime;
            initialSpeed -= acceleration * Time.deltaTime;
            if (initialSpeed < 0)
            {
                initialSpeed = 0;
                //并得出结果
            }
            transform.Rotate(0, 0, angleThisFram);
            Debug.Log(initialSpeed);
            yield return null;
        }
    }

    float CalculateTargetAngle(float[] sectorAngles)
    {
        float targetAngle = 0f;
        for (int i = 0; i < sectorAngles.Length; i++)
        {
            targetAngle += sectorAngles[i];
        }
        if (targetAngle > 360f)
        {
            Debug.LogError("扇区角度之和超过360度，请检查设置");
        }
        return targetAngle;
    }

    void GenerateSectorTexture()
    {
        int textureSize = 512;
        _sectorTexture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
        _sectorTexture.wrapMode = TextureWrapMode.Clamp;
        _sectorTexture.filterMode = FilterMode.Bilinear;
        //纹理中心坐标
        Vector2 center = new Vector2(textureSize / 2f, textureSize / 2f);
        float radius = textureSize / 2f - 2;//留出边框
        float anglePerSector = 360f / sectorNum;
        Color[] pixels = new Color[textureSize * textureSize];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.white;
        }
        _sectorTexture.SetPixels(pixels);
    }
    //创建子物体
    void GenerateSectors()
    {
        GameObject sectorParent = transform.Find("Sectors").gameObject;
        foreach (Transform child in sectorParent.transform)
        {
            Destroy(child.gameObject);
        }
        float startRad = 0f - sectorAngles[0];//起始角度，初始位置调整
        for (int i = 0; i < sectorCount; i++)
        {
            //起点以及终点位置
            startRad = startRad + sectorAngles[i];
            float endrad = (i + 1) * sectorAngles[i];
            GameObject sectorObj = new GameObject($"Sector_{i}");
            sectorObj.transform.SetParent(sectorParent.transform);
            Image sectorImage = sectorObj.AddComponent<Image>();
            //给每一个扇区添加图片组件
            sectorImage.color = Random.ColorHSV(
            0f, 1f,
            0.8f, 1f,
            0.8f, 1f
            );
            sectorImage.fillAmount = sectorAngles[i] / 360f;//填充比例
            sectorImage.fillMethod = Image.FillMethod.Radial360;
            sectorImage.sprite = createSprite(startRad, endrad);
            RectTransform rectTransform = sectorObj.GetComponent<RectTransform>();
            //使锚点与父物体锚点匹配
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = new Vector2(300, 300);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);//以底部为中心旋转
            rectTransform.anchoredPosition = Vector2.zero;
            //将其旋转到对应位置
            rectTransform.localEulerAngles = new Vector3(0, 0, -startRad);
            AddTextToSector(sectorObj, i);
        }
    }
    void AddTextToSector(GameObject obj, int index)
    {
        GameObject textObject = new GameObject($"SectorText{index}");
        textObject.transform.SetParent(obj.transform);
        Text text = textObject.AddComponent<Text>();
        //设置文本内
        text.text = string.IsNullOrEmpty(sectorContent[index]) ? "Again" : sectorContent[index];
        // text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");//这边需要转换成赋值的font 
        text.font = font;
        text.fontSize = 24;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.black;
        //计算文字距离
    }

    Sprite createSprite(float startRad, float endRad)
    {
        //讲角度转换成弧度
        float startAngle = startRad * Mathf.Deg2Rad;
        float endAngle = endRad * Mathf.Deg2Rad;
        //计算扇区的顶点 起始点，结束点
        Vector2[] vertices = new Vector2[3];
        vertices[0] = Vector2.zero;
        vertices[1] = new Vector2(Mathf.Cos(startAngle), Mathf.Sin(startAngle)) * 0.5f;
        vertices[2] = new Vector2(Mathf.Cos(endAngle), Mathf.Sin(endAngle)) * 0.5f;
        //定义一个三角形索引
        int[] triangles = new int[] { 0, 1, 2 };
        //创建一个新的Sprite
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.white);
        texture.Apply();

        Vector2[] uv = new Vector2[3] { Vector2.zero, Vector2.right, Vector2.up };
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), Vector2.zero);
        //通过mesh绘制扇区
        Mesh mesh = new Mesh();
        mesh.vertices = System.Array.ConvertAll(vertices, v => new Vector3(v.x, v.y, 0));//将二维转化成三维
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();
        return sprite;
    }
}
