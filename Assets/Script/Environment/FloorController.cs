using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FloorController : MonoBehaviour
{
    [Header("��������")]
    public GameObject floorPrefab;
    public Vector3 floorPosition = new(0, 16.4f, -18);
    //public MeshFilter meshFilter;
    [Header("���������")]
    public Transform MainCamera;
    [Header("����")]
    public float maxUnloadFloorDistance = 20;
    private Queue<GameObject> floorPool;
    private Vector3 floorLastPosition;
    public int floorCount = 6;//��������
    public float speed = 10;
    public Vector3 Direction = new(0, 0, -1);//�ƶ�����
    [Header("����Ԥ����")]
    public List<GameObject> prefabs;
    public int treeCount;
    public float minDistance = 2f;

    /// <summary>
    /// �ڴ��������ʱ�򣬿�ʼ�쾰ɫ��ÿ��ж��һ�飬�����ɵ�ʱ�򣬸�����Ϸ�Ľ��������þ���
    /// </summary>
    private void Awake()
    {
        floorPool = new Queue<GameObject>();
        CreateFloorPool();
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        foreach (GameObject floor in floorPool)
        {
            floor.transform.position += Direction * speed * Time.fixedDeltaTime;
        }
        MoveAllFloor();
    }
    public void CreateFloorPool()
    {
        for (int i = 0; i < floorCount; i++)
        {
            floorLastPosition = floorPosition + i * new Vector3(0, 0, 20);
            //prefabs.transform.position = ;
            GameObject floor
                = Instantiate(floorPrefab, floorLastPosition, Quaternion.identity);
            floor.name = ($"floor_{i}");
            floorPool.Enqueue(floor);
        }

        //floorLastPosition -= new Vector3(0, 0, maxUnloadFloorDistance);
    }
    void MoveAllFloor()
    {
        GameObject firstFloor = floorPool.Peek();
        if (Mathf.Abs(firstFloor.transform.position.z - MainCamera.transform.position.z) > maxUnloadFloorDistance)
        {
            floorLastPosition = firstFloor.transform.position + floorCount * new Vector3(0, 0, maxUnloadFloorDistance);
            floorPool.Dequeue();
            firstFloor.transform.position = floorLastPosition;
            floorPool.Enqueue(firstFloor);
        }
    }
}
