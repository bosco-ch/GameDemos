using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// 游戏进程环境控制
/// </summary>
public class EnvControl : MonoBehaviour
{
    public List<GameObject> activePrefabs;
    public List<GameObject> prefabs;
    public int maxCreateCount = 100;
    public float offsetX = 16;//其实这边就是在计算生成的环境物体的面积 要考虑到同时生成的环境 这个偏移量是有 正负的 -offsetx ~-offsety
    public float offsetMinY = 19f;//最近的z生成的位置
    public float offsetMaxY = 25f;//最远的z生成的位置
    public float offsetMinZ = 7f;//最近的z生成的位置
    public float offsetMaxZ = 16;//最远的z生成的位置
    Vector3 moveDirection = new Vector3(0, 0, -1);
    public float speed = 10;
    public bool isCreateCloud = false;
    private bool isSpawnCloud = false;
    private Coroutine coroutine;//协程的引用
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("envcontrol start");
    }

    // Update is called once per frame
    void Update()
    {
        if (isCreateCloud)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(CreateCloud());
        }
        else
        {
            if (coroutine == null)
            {

            }
            else
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }
    }

    /// <summary>
    /// 生成云
    /// </summary>
    /// <returns></returns>
    IEnumerator CreateCloud()
    {
        prefabs[0].transform.position = GenerateQuadraticPosition(new Vector3(0, 25, 7));
        Instantiate(prefabs[0]);
        activePrefabs.Add(prefabs[0]);
        yield return new WaitForSeconds(1f);
    }
    Vector3 GenerateQuadraticPosition(Vector3 vector3)
    {
        // 生成随机角度
        float angle = Random.Range(0f, 2f * Mathf.PI);

        // 使用二次函数控制距离分布
        float t = Random.value;
        float distance = 10f * (1 - Mathf.Pow(t, 2f));

        // 计算位置
        float x = Mathf.Cos(angle) * distance;
        float y = Mathf.Sin(angle) * distance;
        return new Vector3(x, y, 0) + vector3;
    }
}
