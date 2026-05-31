using System.Collections;
using System.Collections.Generic;
using Manages;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : BasePanel
{
    public override UIPanelType panelType => UIPanelType.MainMenuPanel;

    private Transform _uiBag;
    private Transform _uiLottery;
    private Transform _uiQuit;
    private void Awake()
    {
        initName();
    }
    void Start()
    {
        BindAction();
    }

    public override void ClosePanel()
    {
        UIManage.Instance.HidePanel();
    }

    public override void OpenPanel()
    {
        UIManage.Instance.ShowPanel<MainPanel>(panelType);
    }

    // Start is called before the first frame update
    void initName()
    {
        _uiBag = transform.Find("Top/Bag");
        _uiLottery = transform.Find("Top/Lottery/");
        _uiQuit = transform.Find("Bottom/Quit");
    }

    void BindAction()
    {
        _uiLottery.GetComponent<Button>().onClick.AddListener(() =>
        {
            UIManage.Instance.ShowPanel<LotterryPanel>(UIPanelType.LotteryPanel);
        });
        _uiBag.GetComponent<Button>().onClick.AddListener(() =>
        {
            UIManage.Instance.ShowPanel<PackagesPanel>(UIPanelType.PackagePanel);
        });
        _uiQuit.GetComponent<Button>().onClick.AddListener(() =>
        {
            // UIManage.Instance.ShowPanel<PackagesPanel>(UIPanelType.PackagePanel);
            ClosePanel();
        });
    }
    // void BagOpen()
    // {
    //     UIManage.Instance.ShowPanel<PackagesPanel>(UIPanelType.PackagePanel);
    //     ClosePanel();
    // }
    // void LotteryOpen()
    // {
    //     UIManage.Instance.ShowPanel<LotterryPanel>(UIPanelType.LotteryPanel);
    //     ClosePanel();
    // }
}
