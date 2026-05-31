using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [Range(0f, 2f)] public float defaultTimeScale = 1;
    [Header("×Óµ¯Ê±¼ä")]
    [SerializeField, Range(0f, 2f)] float bulletTimeScale;
    [SerializeField] private float timeRecoveryDuration;
    private GUIStyle GUIStyle;
    private void Awake()
    {
        Time.timeScale = defaultTimeScale;
    }
    private void Start()
    {
        GUIStyle = new GUIStyle();
        GUIStyle.fontSize = 18;
        GUIStyle.normal.textColor = Color.white;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 80), $"scale time = {Time.timeScale}", GUIStyle);
    }
    public void bulletTime()
    {
        Time.timeScale = bulletTimeScale;
        StartCoroutine(nameof(TimeRecoverContinue));
    }
    //public void recover
    IEnumerator TimeRecoverContinue()
    {
        float ratio = 0f;
        while (ratio < 1f)
        {
            ratio += Time.unscaledDeltaTime / timeRecoveryDuration;
            Time.timeScale = Mathf.Lerp(bulletTimeScale, defaultTimeScale, ratio);
            yield return null;
        }
    }

}
