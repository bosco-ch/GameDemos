using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Core;
using TMPro;
using UnityEngine.UI;

public class GameProcessManager : Singleton<GameProcessManager>
{
    // Queue Scence = new Queue();
    private string gameScene = "2DVisionDemo";
    [SerializeField] public bool isFirstEnter = true; //??????????????ж?
    private AsyncOperation nextScene;
    [SerializeField] private Slider slider;
    [SerializeField] private Image image;
    [Header("着色器预热")] public ShaderVariantCollection shaderVariantCollection;
    [SerializeField] private float transitionDuration = 3f;
    [SerializeField] private TMP_Text text;
    private bool _isLoadScene = false;

    private void Start()
    {
        // load next scene
        nextScene = SceneManager.LoadSceneAsync(gameScene);
        text.text = "now load next scene";
        Debug.Log($"nextScene是否为null：{nextScene == null}");
        if (nextScene != null)
            nextScene.allowSceneActivation = false;
        else
            Debug.Log("没有找到");
        StartCoroutine(nameof(LoadSceneGame));
    }

    // Update is called once per frame
    void Update()
    {
    }

    private IEnumerator LoadSceneGame()
    {
        float t = 0;
        slider.value = 0;
        Debug.Log("开启协程");
        if (shaderVariantCollection != null && !shaderVariantCollection.isWarmedUp)
        {
            text.text = "warmup shader";
            shaderVariantCollection.WarmUp();
        }
        else
        {
            Debug.Log(
                $"shaderVariantCollection{(shaderVariantCollection == null ? "为" : "不为")}null" +
                $"，且{(shaderVariantCollection.isWarmedUp ? "已经" : "还未")}预热完成");
        }

        yield return null;
        Debug.Log("开启场景加载");
        while (nextScene.progress < 0.9f)
        {
            slider.value = nextScene.progress / 0.9f;
            yield return null;
        }

        Debug.Log("背景淡化");
        while (t < transitionDuration)
        {
            t += Time.deltaTime;
            float alpha = 1 - Mathf.Clamp01(t / transitionDuration);
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            yield return null;
        }

        if (slider != null)
        {
            slider.value = 1f;
            text.text = " Load success ";
        }

        slider.value = 1.0f;

        Debug.Log("开启跳转");
        nextScene.allowSceneActivation = true;
    }
}