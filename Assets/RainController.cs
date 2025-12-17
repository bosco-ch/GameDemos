using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 雨点控制系统
/// </summary>
public class RainController : MonoBehaviour
{
    public ParticleSystem rainParticleSystem;
    public AudioSource audioSource;
    public float maxRainIntensity = 300f;
    public float minRainIntensity = 100f;
    public float maxRainVolume = 0.8f;
    public float minRainVolume = 0.2f;


    public float currentIntensity;
    public ParticleSystem.EmissionModule emissionModule;
    // Start is called before the first frame update
    void Start()
    {
        if (rainParticleSystem != null)
            emissionModule = rainParticleSystem.emission;
    }

    // Update is called once per frame
    void Update()
    {
        SetRainIntensity(0.8f);
    }

    public void SetRainIntensity(float intensity)
    {
        if (rainParticleSystem != null)
        {
            currentIntensity = Mathf.Lerp(minRainIntensity, maxRainIntensity, intensity);
            emissionModule.rateOverTime = currentIntensity;
        }
        if (audioSource != null)
            audioSource.volume
                = Mathf.Lerp(minRainVolume, maxRainVolume, intensity);
    }
}
