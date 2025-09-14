using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
/// <summary>
/// 制作抛物线
/// </summary>
public class CreatLine
{
    Mesh mesh;
    Material litMaterial;
    /// <summary>
    /// 球的飞行路线
    /// </summary>
    public static Dictionary<string, Vector3[]> line = new
          Dictionary<string, Vector3[]>
    {
        { "1",new  Vector3[]{  new Vector3(-1,1.3f,6),new Vector3(1.2f,1.3f,6),new Vector3(1.3f,-0.3f,6)}},//近低，贝塞尔的两点，以及ai 的位置
        { "2",new  Vector3[]{  new Vector3(-1,1.3f,8),new Vector3(0.5f, 1.3f, 15f),new Vector3(1.3f,-0.3f,18)}},//中低
        { "3",new  Vector3[]{  new Vector3(-1,1.3f,8),new Vector3(1.2f,1.3f,48),new Vector3(1.3f,-0.3f,50)}},//远低
        //{ "4",new  Vector3[]{  new Vector3(),new Vector3(),new Vector3()}},//近低
        //{ "5",new  Vector3[]{  new Vector3(),new Vector3(),new Vector3()}},//中低
        //{ "6",new  Vector3[]{  new Vector3(),new Vector3(),new Vector3()}},//远低
    };

    public CreatLine()
    {
        mesh = Resources.GetBuiltinResource<Mesh>("Sphere.fbx");
        //mRenderer;
        litMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
    }

    /// <summary>
    /// 创建点路径
    /// </summary>
    /// <returns></returns>

    public float pathLength(List<Vector3> points)
    {
        float distance = 0;
        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector3 v1 = points[i];
            Vector3 v2 = points[i + 1];
            distance += Vector3.Distance(v1, v2);
        }
        return distance;
    }
    /// <summary>
    /// 绘制贝尔曲线，
    /// </summary>
    /// <param name="t"></param>
    /// <param name="points"></param>
    /// <returns></returns>
    public Vector3 CubicBezier(float t, Vector3[] points)
    {
        // 贝塞尔公式：B(t) = (1-t)^3*P0 + 3(1-t)^2*t*P1 + 3(1-t)*t^2*P2 + t^3*P3

        float u = 1 - t;
        float uu = u * u;
        float uuu = uu * u;
        float tt = t * t;
        float ttt = tt * t;
        Vector3 point = uuu * points[0];
        point += 3 * uu * t * points[1];
        point += 3 * u * tt * points[2];
        point += ttt * points[3];
        return point;
    }
    /// <summary>
    /// 求贝塞尔曲线的切线
    /// </summary>
    /// <param name="t"></param>
    /// <param name="points"></param>
    /// <returns></returns>
    public Vector3 GetFirstDerivative(float t, Vector3[] points)
    {
        float u = 1 - t;
        float uu = u * u;
        float tt = t * t;
        Vector3 derivative = 3 * uu * (points[1] - points[0]);
        derivative += 6 * u * t * (points[2] - points[1]);
        derivative += 3 * tt * (points[3] - points[2]);
        return derivative;
    }

}
