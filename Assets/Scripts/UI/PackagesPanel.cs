using System.Collections.Generic;
using Manages;
using TMPro;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum PanelModel
{
    normal,
    deleted,
}

public class PackagesPanel : BasePanel
{
    public override UIPanelType panelType => UIPanelType.PackagePanel;

    public override void ClosePanel()
    {
        UIManage.Instance.HidePanel();
    }

    public override void OpenPanel()
    {
        UIManage.Instance.ShowPanel<PackagesPanel>(panelType);
    }

    // public GameObject PackagePanelPrefab;
    public GameObject packageItemPrefab;

    private Transform _uiClosePackageBtn;
    private Transform _uiDeleteBtn; //预删除按钮
    private Transform _uiDetailBtn; //所选物品的详情查看按钮
    private Transform _uiDeleteCancelBtn; //点击删除后，询问是否删除有返回（也就是取消删除的）
    private Transform _uiDeleteConfirmBtn; //确认删除
    private Transform _uiPrevPageBtn; //上一页
    private Transform _uiNextPageBtn; //下一页

    /// <summary>
    /// panel
    /// </summary>
    private Transform _uiDeleteConfirmPanel; //点击删除后的询问框

    private Transform _uiPackageRootPanel; //一整个背包panel
    private Transform _uiTopMenuPanel; //顶部菜单栏panel
    private Transform _uiDetailPanel; //物品详情

    /// <summary>
    /// scroll view transform
    /// </summary>
    private Transform _uiItemIcon; //滚动view中图标
    private Transform _uiItemHead; //滚动view中所属角色;
    private Transform _uiItemNew; //滚动view中的是否是新获得的武器
    private Transform _uiItemStarText; //滚动view中的等级文字显示;
    private Transform _uiStar; //滚动view中的的等级图画显示;
    private Transform _uiScrollView;

    // private delegate void ButtonBind();
    private Dictionary<Transform, UnityAction> _buttonDict = new();

    private string _chooseUid;

    public List<string> selecedItems;

    public PanelModel currentPanelModel = PanelModel.normal;

    public string ChooseUID
    {
        get => _chooseUid;
        set
        {
            _chooseUid = value;
            if (_chooseUid != null)
                ReflushDetail();
        }
    }

    private void Awake()
    {
        InitUIName();
        init();
    }

    private void Start()
    {
        InitDict();
        BindAction();
        ReFlushScrollView();
    }

    void InitUIName()
    {
        _uiClosePackageBtn = transform.Find("RightTop/Close");
        if (_uiClosePackageBtn == null)
        {
            Debug.LogError("未成功绑定 close");
        }

        _uiPackageRootPanel = transform;
        _uiDeleteBtn = transform.Find("Bottom/BottomMenus/DeleteBottom");
        _uiDeleteCancelBtn = transform.Find("Bottom/DeletePanel/Back");
        _uiPrevPageBtn = transform.Find("LeftCenter/LeftButton");
        _uiNextPageBtn = transform.Find("RightCenter/RightButton");
        _uiDeleteConfirmPanel = transform.Find("Bottom/DeletePanel");
        _uiDeleteConfirmPanel.gameObject.SetActive(false);
        _uiScrollView = transform.Find("Center/Scroll View");
        _uiDetailPanel = transform.Find("Center/DetailPanel");
    }

    void ReflushDetail()
    {
        if (!string.IsNullOrEmpty(_chooseUid))
        {
            var data = GameManage.Instance.GetPackageTableDataItemByUid(_chooseUid);
            _uiDetailPanel.GetComponent<PackageDetail>().ReFlush(data, this);
        }
    }

    void ReFlushScrollView()
    {
        //首先清理滚动容器中的所有物品
        RectTransform content = _uiScrollView.GetComponent<ScrollRect>().content;
        for (int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }
        var items = GameManage.Instance.GetPackageLocalTableData();
        foreach (var item in GameManage.Instance.GetPackageLocalTableData())
        {
            Transform packageUiItem = Instantiate(packageItemPrefab.transform, content);
            packageUiItem.GetComponent<PackageCell>().Reflush(this, item);
        }
    }

    void InitDict()
    {
        _buttonDict.Add(_uiClosePackageBtn, CloseToPanel);
        _buttonDict.Add(_uiDeleteBtn, OpenDeletedComfirmPanel);
        _buttonDict.Add(_uiDeleteCancelBtn, CloseDeletedComfirmPanel);
    }

    void BindAction()
    {
        foreach (var tr in _buttonDict)
        {
            GetButtonOrSet(tr.Key).onClick.AddListener(tr.Value);
        }
    }

    Button GetButtonOrSet(Transform tr)
    {
        Button button = tr.GetComponent<Button>();
        if (button == null)
        {
            button = tr.AddComponent<Button>();
        }

        return button;
    }

    void CloseToPanel()
    {
        currentPanelModel = PanelModel.normal;
        ClosePanel();
    }

    /// <summary>
    /// 点击删除触发，询问框
    /// </summary>
    void OpenDeletedComfirmPanel()
    {
        currentPanelModel = PanelModel.deleted; //当前为删除模式
        _uiDeleteConfirmPanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// 点击删除后，取消删除关闭询问框
    /// </summary>
    void CloseDeletedComfirmPanel()
    {
        currentPanelModel = PanelModel.normal;
        _uiDeleteConfirmPanel.gameObject.SetActive(false);
    }
}