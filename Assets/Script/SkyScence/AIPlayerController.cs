using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;

public class AIPlayerController : MonoBehaviour
{
    public static AIPlayerController Instance;
    private void Start()
    {
        if (Instance != null && Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
 
}
