using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneController : MonoBehaviour
{

    private LoadSceneController _instance;
    // Start is called before the first frame update
    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
