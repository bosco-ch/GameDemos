using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FullScreen : MonoBehaviour
{
    public FullScreenPassRendererFeature feature;
    public bool isEnabled = false;
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            feature.SetActive(isEnabled);
            isEnabled = !isEnabled;
        }
    }
}
