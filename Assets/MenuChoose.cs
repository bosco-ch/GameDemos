using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Rendering.Universal;
using UnityEditor.SceneManagement;
public class MenuChoose : MonoBehaviour
{
    public GameProcessManager instance;
    public Button ChooseMusicButton;
    public Button AgainOpeningButton;
    public Button SpecialButton;
    public Button BackButton;
    public GameObject DescriptionTextMesh;
    private TextMeshProUGUI _description;
    UnityEngine.AsyncOperation operation;

    private Transform _musicListPanel;//音乐列表页面
    private Transform _menuPanel;//菜单页面
    private Vector3? TemporaryStoragePosition = null;
    public enum MenuType
    {
        [Description("选择音乐")]
        chooseMusic,
        [Description("再次开启开场")]
        againOpening,
        [Description("特殊隐藏场")]
        Special,
    }
    private void Awake()
    {
        _menuPanel = transform.Find("MenuPanle");
    }
    // Start is called before the first frame update
    void Start()
    {
        _description = DescriptionTextMesh.GetComponent<TextMeshProUGUI>();
        SetupButtonDescription(ChooseMusicButton, "选择你喜欢的音乐加入游玩");
        SetupButtonDescription(AgainOpeningButton, "重新游玩开场（随机开场）");
        SetupButtonDescription(SpecialButton, "解锁隐藏特殊场");
        SetupButtonListenEvent(ChooseMusicButton, OnChooseMusic);
        SetupButtonListenEvent(BackButton, OnMusicListBackMenu);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void SetupButtonListenEvent<Button>(Button _button, Action action) where Button : UnityEngine.UI.Button
    {
        _button.onClick.AddListener(() =>
        {
            action?.Invoke();
        });
    }
    /// <summary>
    /// 选择音乐开始游戏
    /// </summary>
    void OnChooseMusic()
    {
        _musicListPanel = transform.Find("MusicListPanel");
        //panel左移动
        if (_musicListPanel == null && _menuPanel == null)
            Debug.Log("this null");
        else
        //target 位置
        {
            TemporaryStoragePosition = _musicListPanel.transform.position;
            PanelMove(_musicListPanel, _menuPanel.position);
        }
    }
    void OnMusicListBackMenu()
    {
        _musicListPanel = transform.Find("MusicListPanel");
        if (_musicListPanel == null && _menuPanel == null)
            Debug.Log("this null");
        else
        {
            PanelMove(_musicListPanel, TemporaryStoragePosition.GetValueOrDefault());
            TemporaryStoragePosition = null;
        }
    }
    /// <summary>
    /// 设置鼠标悬停在按钮上显示描述文本
    /// </summary>
    /// <param name="target"></param>
    /// <param name="menuType"></param>
    void SetupButtonDescription(Button target, string description)
    {
        //添加触发trigger
        UnityEngine.EventSystems.EventTrigger trigger = target.GetComponent<UnityEngine.EventSystems.EventTrigger>() ?? target.AddComponent<UnityEngine.EventSystems.EventTrigger>();
        //鼠标进入事件
        var eventEntry = new UnityEngine.EventSystems.EventTrigger.Entry();
        eventEntry.eventID = UnityEngine.EventSystems.EventTriggerType.PointerEnter;
        eventEntry.callback.AddListener((data) =>
        {
            _description.text = description;
        });
        trigger.triggers.Add(eventEntry);
        //鼠标退出事件
        var eventExit = new UnityEngine.EventSystems.EventTrigger.Entry();
        eventExit.eventID = UnityEngine.EventSystems.EventTriggerType.PointerExit;
        eventExit.callback.AddListener((data) => { _description.text = ""; });
        trigger.triggers.Add(eventExit);
    }
    void PanelMove(Transform origin, Vector3 target)
    {
        //Debug.Log($"{origin.position}至{target.position}");
        origin.DOMove(target, 1);
    }
    private IEnumerator OnLoadScence(int _index)
    {
        operation = SceneManager.LoadSceneAsync(_index);
        while (!operation.isDone)
        {
            float _process = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log(_process);
            yield return null;
        }
        operation.allowSceneActivation = false;
    }
}
