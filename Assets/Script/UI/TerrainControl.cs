using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TerrainControl : MonoBehaviour
{
    [Header("生成地形的数据")]
    public int terrainMaxCount = 4;
    public float terrainDefaultDistance = 200f;
    [Header("摄像机位置")]
    public Transform Camera;
    [Header("地形")]
    public Terrain InitTerrain;
    // public float onloadTerrainDistance;//距离当前多远开始加载地形
    public float UnloadTerrainDistance = 201;//距离当前地形多远卸载地形
    [Header("地形环境")]
    public List<GameObject> prefebs;//需要生成的环境物体预制体们
    public int prefebsCount;//需要生成的预制体的总数量（也即是范围);
    public float yOffset = 0;//高度偏移量
    public float speed = 10;
    public UnityEngine.Vector3 MoveMent;
    [Header("材质")]
    [Header("地形字典")]
    public Material material;

    [Tooltip("测试用材质")]

    public Color grassColor1 = new Color(0.2f, 0.6f, 0.3f);   // 深绿色
    public Color grassColor2 = new Color(0.4f, 0.8f, 0.4f);   // 浅绿色
    public Color waterColor = new Color(0.15f, 0.5f, 0.8f, 0.7f); // 水域颜色
    Queue<Terrain> terrainQueue = new();
    // Start is called before the first frame update
    void Awake()
    {
        //terrainDefaultDistance = InitTerrain.terrainData.size.z;
        InitializeTerrainPool();
        CreateTerrain();
        MoveMent = new UnityEngine.Vector3(0, 0, -1);
    }
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        updateTerrain();
    }

    void PrefabsIntoTerrain(List<GameObject> prefab, Terrain terrain)
    {
        Vector3 terrainSize = terrain.terrainData.size;
        Vector3 terrainPos = terrain.transform.position;

        for (int i = 0; i < prefebsCount; i++)
        {
            float randomX = Random.Range(0, terrainSize.x);
            float randomZ = Random.Range(0, terrainSize.z);
            Vector3 worldPos = new Vector3(randomX, 0, randomZ) + terrainPos;
            //获取高度
            worldPos.y = terrain.SampleHeight(worldPos);
            worldPos.y += yOffset;

            GameObject newObject = Instantiate(prefab[0], worldPos, Quaternion.identity);
            //float randomScale = Random.Range(0.5f, 1.2f);
            float randomScale = 0.5f;
            newObject.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
            newObject.transform.parent = terrain.transform;
        }
    }

    Terrain loadTerrarin(int i)
    {
        TerrainData terrainData = new();
        terrainData = InitTerrain.terrainData;
        terrainData.name = $"terrain_{i}";
        GameObject terrainObj = Terrain.CreateTerrainGameObject(terrainData);
        terrainObj.GetComponent<TerrainCollider>().terrainData = terrainData;
        Terrain newTerrain = terrainObj.GetComponent<Terrain>();
        PrefabsIntoTerrain(prefebs, newTerrain);
        return terrainObj.GetComponent<Terrain>();
    }   
    /// <summary>
    /// 初始化地形池；
    /// </summary>
    /// <returns></returns>
    void InitializeTerrainPool()
    {
        terrainQueue.Enqueue(InitTerrain);
        for (int i = 0; i < terrainMaxCount; i++)
        {
            terrainQueue.Enqueue(loadTerrarin(i));
        }
    }

    void CreateTerrain()
    {
        Vector3 _position = InitTerrain.transform.position;
        for (int i = 0; i < terrainQueue.Count; i++)
        {
            Terrain _terrain = terrainQueue.Dequeue();
            _terrain.transform.position = new(_position.x, _position.y, _position.z + (i * terrainDefaultDistance));
            _terrain.materialTemplate = material;
            terrainQueue.Enqueue(_terrain);
        }
    }
    void updateTerrain()
    {
        Terrain _terrain = terrainQueue.Peek();//第一块terrain
         
        Vector3 _position = _terrain.transform.position;
        float distance = Mathf.Abs(Mathf.Abs(Camera.position.z) - Mathf.Abs(_terrain.transform.position.z));
        if (distance > UnloadTerrainDistance)
        {
            terrainQueue.Dequeue();
            _terrain.transform.position = new(_position.x, _position.y, _position.z + terrainDefaultDistance * (terrainMaxCount + 1));
            terrainQueue.Enqueue(_terrain);
        }
    }
}
