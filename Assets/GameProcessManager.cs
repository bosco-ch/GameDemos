using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameProcessManager : MonoBehaviour
{
    public GameProcessManager intance;
    Queue Scence = new Queue();
    [SerializeField]
    public bool isFirstEnter = true;//��ϵͳ�����ļ����ж�
    private void Awake()
    {
        if (intance == null)
        {
            intance = this;
        }
        else
        {
            Destroy(intance);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        if (isFirstEnter)
        {
            //���ؿ���
        }
        else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
