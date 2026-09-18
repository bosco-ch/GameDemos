using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Core;
using Editors;
using PlayerMainScenes;
using PlayerMainScenes.LoadStates;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;
using ResourceManager = Manages.ResourceManager;

public enum LoadStateType
{
    WarmUpState,
    LoadSceneState,
    CheckAssetState,
    LoadAssetState,
    FadeState,
    CompleteState,
}

public class GameProcessManager : Singleton<GameProcessManager>
{
    private readonly string _gameScene = "2DVisionDemo";
    [SerializeField] public bool isFirstEnter = true;
    public AsyncOperation NextScene;
    [Header("着色器预热")] public ShaderVariantCollection shaderVariantCollection;
    [SerializeField] private float transitionDuration = 3f;
    [SerializeField] private TMP_Text text;
    [Header("UI")] [SerializeField] private Slider slider;
    [SerializeField] public Image image;

    [Header("权重分配/Weight distribution during loading")]
    public WeightDuringLoading wdl;

    public float CurrentProgress => slider.value;

    public float WeightDuringLoadingTotal => wdl.checkAssetStateWeight
                                             + wdl.fadeStateWeight
                                             + wdl.loadAssetStateWeight
                                             + wdl.loadSceneStateWeight
                                             + wdl.warmUpStateWeight;

    // public delegate void OnSliderAction(float value);

    private LoadStateType _currentStateType;
    private readonly Dictionary<LoadStateType, ILoadState> LoadState = new Dictionary<LoadStateType, ILoadState>();
    // [Header("配置信息")]

    private void Start()
    {
        // load next scene
        NextScene = SceneManager.LoadSceneAsync(_gameScene);
        text.text = "now load next scene";
        Debug.Log($"nextScene是否为null：{NextScene == null}");
        if (NextScene != null)
            NextScene.allowSceneActivation = false;
        else
            Debug.Log("没有找到");
        // StartCoroutine(nameof(LoadSceneGame));
        InitLoadState();
        SwitchState(LoadStateType.WarmUpState);
        StartCoroutine(nameof(LoadScene));
    }

    void InitLoadState()
    {
        LoadState.Add(LoadStateType.FadeState, new FadeState(this));
        LoadState.Add(LoadStateType.LoadAssetState, new LoadAssetState(this));
        LoadState.Add(LoadStateType.LoadSceneState, new LoadSceneState(this));
        LoadState.Add(LoadStateType.WarmUpState, new WarmUpState(this));
        LoadState.Add(LoadStateType.CheckAssetState, new CheckAssetState(this));
        LoadState.Add(LoadStateType.CompleteState, new LoadCompletedState());
    }

    public void SwitchState(LoadStateType stateType)
    {
        LoadState[_currentStateType]?.OnExitState();
        _currentStateType = stateType;
        LoadState[_currentStateType]?.OnEnterState();
    }

    private IEnumerator LoadScene()
    {
        while (_currentStateType != LoadStateType.CompleteState)
        {
            LoadState[_currentStateType].OnUpdateState();
            yield return null;
        }

        NextScene.allowSceneActivation = true;
    }

    # region 不用

    // private IEnumerator LoadSceneGame()
    // {
    //     float t = 0;
    //     slider.value = 0;
    //     Debug.Log("开启协程");
    //     if (shaderVariantCollection != null && !shaderVariantCollection.isWarmedUp)
    //     {
    //         text.text = "warmup shader";
    //         shaderVariantCollection.WarmUp();
    //     }
    //     else
    //     {
    //         Debug.Log(
    //             $"shaderVariantCollection{(shaderVariantCollection == null ? "为" : "不为")}null" +
    //             $"，且{(shaderVariantCollection.isWarmedUp ? "已经" : "还未")}预热完成");
    //     }
    //
    //     slider.value = 0.2f;
    //     Debug.Log("开启场景加载");
    //     do
    //     {
    //         slider.value = 0.2f + NextScene.progress * 0.3f;
    //         yield return null;
    //     } while (NextScene.progress < 0.9f);
    //
    //     slider.value = 0.5f;
    //     ////
    //     Debug.Log("资源数量检查");
    //     while (!ResourceManager.Instance.isFindAssetSuccess)
    //     {
    //         Debug.Log("资源加载完毕");
    //         yield return null;
    //     }
    //
    //     Debug.Log("加载资源文件");
    //     float uiUpdateTimer = 0;
    //     float uiUpdateInterval = .4f;
    //     do
    //     {
    //         uiUpdateTimer += Time.deltaTime;
    //         if (uiUpdateTimer >= uiUpdateInterval)
    //         {
    //             slider.value = 0.5f + ResourceManager.Instance.LoadAssetsPercent * 0.3f;
    //             uiUpdateTimer = 0f;
    //             Debug.Log("加载此时总进度为：" + slider.value + ",当前模块进度为：" + ResourceManager.Instance.LoadAssetsPercent);
    //         }
    //
    //         // Debug.Log("资源加载进度" + ResourceManager.Instance.LoadAssetsPercent);
    //         yield return null;
    //     } while (ResourceManager.Instance.HaveLoadAssetTotal < ResourceManager.Instance.NeedLoadAssetTotal);
    //
    //     slider.value = 0.8f;
    //     ////
    //     Debug.Log("背景淡化");
    //     while (t < transitionDuration)
    //     {
    //         t += Time.deltaTime;
    //         float alpha = 1 - Mathf.Clamp01(t / transitionDuration);
    //         image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
    //         yield return null;
    //     }
    //
    //     text.text = " Load success ";
    //     slider.value = 1.0f;
    //     Debug.Log("开启跳转");
    //     // nextScene.allowSceneActivation = true;
    // }

    #endregion

    public void SetSliderValue(float value)
    {
        slider.value = value;
    }

    public void SetImgAlpha(float alpha)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
    }
}