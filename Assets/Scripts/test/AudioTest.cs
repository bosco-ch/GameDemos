using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;

public class AudioTest : MonoBehaviour
{
    public AudioClip audioRain;
    public AudioClip audioBackGround;
    private AudioSource AudioSourceRain;
    private AudioSource AudioSourceBackground;
    private AudioSource AudioSourceThunder;

    public AudioClip[] audioThunder;
    public float minWaitTime = 1f; // 随机等待时间的最小值
    public float maxWaitTime = 10f; // 随机等待时间的最大值    

    // Start is called before the first frame update
    void Start()
    {
        AudioSourceRain = gameObject.AddComponent<AudioSource>();
        AudioSourceThunder = gameObject.AddComponent<AudioSource>();
        AudioSourceBackground = gameObject.AddComponent<AudioSource>();
        AudioSourceBackground.clip = audioBackGround;
        AudioSourceBackground.loop = true;
        AudioSourceBackground.Play();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(FadeIn());
            StartCoroutine(ThunderRandomly());
        }
    }
    IEnumerator FadeIn()
    {
        float targetVolume = 10f;
        float startVolume = 0.0f;
        float fadeDuration = 4.0f;
        AudioSourceRain.clip = audioRain;
        AudioSourceRain.loop = true;
        AudioSourceRain.volume = startVolume;
        AudioSourceRain.Play();
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime / 10;
            AudioSourceRain.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / fadeDuration);
            yield return null;
        }
    }
    IEnumerator ThunderRandomly()
    {

        yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
        int a = Random.Range(0, audioThunder.Length);
        AudioSourceThunder.clip = audioThunder[a];
        AudioSourceThunder.Play();
        yield return new WaitForSeconds(audioThunder[a].length + 1);
        StartCoroutine(ThunderRandomly());
    }
}
