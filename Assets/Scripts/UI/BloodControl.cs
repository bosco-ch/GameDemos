using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class BloodControl : MonoBehaviour
{
    public Image img;
    public float changeDuration = 0.5f; //血条变化总时间
    public float timer = 3f;
    // public float _lerp_speed = 10;
    private float targetFillAmount;
    // private float currentFillAmount;
    public void bloodShow(float currect, float max)
    {
        // Debug.Log($"BloodControl的{currect / max}，，，，，{img.fillAmount},,,,,{Time.deltaTime}");
        // _lerp_speed = Mathf.Clamp01(timer / changeDuration);
        // img.fillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, t);
        // img.fillAmount = Mathf.Lerp(img.fillAmount, currect / max, _lerp_speed * Time.deltaTime);
        targetFillAmount = currect / max;
    }
    void Update()
    {

        if (img.fillAmount != targetFillAmount)
        {
            // timer += Time.deltaTime;
            // float t = Mathf.Clamp01(timer / changeDuration);
            // img.fillAmount = Mathf.Lerp(img.fillAmount, targetFillAmount, t);
            img.fillAmount = Mathf.Lerp(img.fillAmount, targetFillAmount, timer * Time.deltaTime);
            Debug.Log(img.fillAmount);
        }
    }

}

