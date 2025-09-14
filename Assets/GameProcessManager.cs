using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameProcessManager : MonoBehaviour
{
    public GameProcessManager intance;
    Queue Scence = new Queue();
    [SerializeField]
    public bool isFirstEnter = true;//由系统配置文件来判断
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
            //加载开场
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
