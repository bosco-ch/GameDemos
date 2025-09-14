using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// ��Ϸ���̻�������
/// </summary>
public class EnvControl : MonoBehaviour
{
    public List<GameObject> activePrefabs;
    public List<GameObject> prefabs;
    public int maxCreateCount = 100;
    public float offsetX = 16;//��ʵ��߾����ڼ������ɵĻ����������� Ҫ���ǵ�ͬʱ���ɵĻ��� ���ƫ�������� ������ -offsetx ~-offsety
    public float offsetMinY = 19f;//�����z���ɵ�λ��
    public float offsetMaxY = 25f;//��Զ��z���ɵ�λ��
    public float offsetMinZ = 7f;//�����z���ɵ�λ��
    public float offsetMaxZ = 16;//��Զ��z���ɵ�λ��
    Vector3 moveDirection = new Vector3(0, 0, -1);
    public float speed = 10;
    public bool isCreateCloud = false;
    private bool isSpawnCloud = false;
    private Coroutine coroutine;//Э�̵�����
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
    /// ������
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
        // ��������Ƕ�
        float angle = Random.Range(0f, 2f * Mathf.PI);

        // ʹ�ö��κ������ƾ���ֲ�
        float t = Random.value;
        float distance = 10f * (1 - Mathf.Pow(t, 2f));

        // ����λ��
        float x = Mathf.Cos(angle) * distance;
        float y = Mathf.Sin(angle) * distance;
        return new Vector3(x, y, 0) + vector3;
    }
}
