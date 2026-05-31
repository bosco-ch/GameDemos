using System.Collections.Generic;
using UnityEngine;
//该对象不会被销毁
//用于长周期的 例如 天气变化，昼夜变化，音乐播放
public class DonotDestory : MonoBehaviour
{
    [Tooltip("天空球控制")]
    public List<Material> skyboxs;
    [Range(0f, 1f)] public float exposure = 1f;
    [Range(0f, 365f)] public float rotation = 1f;
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        //加载天空球
    }
}
