using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogExploreMask1 : MonoBehaviour
{
    [Header("地图设置")] public int mapWidth = 1920; //建议与实际地图分辨率匹配
    public int mapHeight = 1080;
    public float pathRadius = 1000f; //角色留下路径的半径（世界单位）
    public float pathSoftness = 0.1f; //边缘软化
    public Transform mapPlane;

    [SerializeField] private float mapMinX;
    [SerializeField] private float mapMinZ;
    [SerializeField] private float mapMaxX;
    [SerializeField] private float mapMaxZ;
    public float mapWorldSizeX;
    public float mapWorldSizeZ;

    [Header("RT设置")] [SerializeField] private RenderTexture pathRT; //需要修改的RT
    private Material paintMat; // 用来往 RT 上画圆的材质
    private Camera pathCam; // 专门用来画路径的正交相机

    void Start()
    {
        GetMapBounds();
        GetMapSize();
        // pathRT = new RenderTexture(mapWidth, mapHeight, 0, RenderTextureFormat.R8);
        pathRT.filterMode = FilterMode.Bilinear;
        pathRT.wrapMode = TextureWrapMode.Clamp;
        pathRT.Create();
        // 清空为黑色（未走过）
        RenderTexture.active = pathRT;
        GL.Clear(true, true, Color.black);
        RenderTexture.active = null;
        // 创建画路径用的材质（简单的圆形刷）
        paintMat = new Material(Shader.Find("Custom/PenLine"));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 测试：按空格键在地图中心画一个路径
            Vector3 worldPos = Input.mousePosition; //直接在屏幕上获取鼠标位置
            PaintPath(worldPos);
        }
    }

    void GetMapSize()
    {
        mapWorldSizeX = 10 * mapPlane.localScale.x;
        mapWorldSizeZ = 10 * mapPlane.localScale.z;
    }

    public RenderTexture GetPathRT() => pathRT;

    public void PaintPath(Vector3 worldPos)
    {
        // 把世界坐标转换为 RT 的 UV
        Vector2 uv = WorldToUV(worldPos);
        // 用一个小圆刷往 RT 上叠加白色
        // 这里用 Graphics.Blit 配合自定义 Shader 更高效
        RenderTexture temp = RenderTexture.GetTemporary(pathRT.width, pathRT.height, 0, pathRT.format);
        paintMat.SetVector("_Center", new Vector4(uv.x, uv.y, 0, 0));
        paintMat.SetFloat("_RadiusX", pathRadius / mapWorldSizeX); // 归一化半径
        paintMat.SetFloat("_RadiusZ", pathRadius / mapWorldSizeZ); // 归一化半径
        paintMat.SetFloat("_Softness", pathSoftness);
        Graphics.Blit(pathRT, temp, paintMat);
        Graphics.CopyTexture(temp, pathRT);
        RenderTexture.ReleaseTemporary(temp);
    }

    Vector2 WorldToUV(Vector3 worldPos)
    {
        // 根据你的地图范围换算
        float u = 1f - worldPos.x / mapWidth;
        float v = 1f - worldPos.y / mapHeight;
        return new Vector2(u, v);
    }

    void GetMapBounds()
    {
        if (mapPlane == null)
        {
            Debug.LogError("请赋值mapPlane！");
            return;
        }

        Bounds bounds = mapPlane.GetComponent<MeshRenderer>().bounds;
        mapMinX = bounds.min.x;
        mapMinZ = bounds.min.z;
        mapMaxX = bounds.max.x;
        mapMaxZ = bounds.max.z;
        mapWorldSizeX = mapMaxX - mapMinX;
        mapWorldSizeZ = mapMaxZ - mapMinZ;
    }
}