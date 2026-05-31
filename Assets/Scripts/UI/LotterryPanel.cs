using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Manages;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LotterryPanel : BasePanel
{
    private Transform _uiTenLottery;
    private Transform _uiOneLottery;
    private Transform _uiClose;
    private Transform _uiCenter;
    public GameObject cellPrefab;
    public override UIPanelType panelType => UIPanelType.LotteryPanel;

    // Start is called before the first frame update
    private void Awake()
    {
        _uiTenLottery = transform.Find("Bottom/TenLottery");
        _uiOneLottery = transform.Find("Bottom/OneLottery");
        _uiCenter = this.transform.Find("Center");
        _uiClose = transform.Find("TopRight/Close");
    }
    void Start()
    {
        BindAction();
    }
    void BindAction()
    {
        _uiTenLottery.GetComponent<Button>().onClick.AddListener(TenLottery);
        _uiOneLottery.GetComponent<Button>().onClick.AddListener(OneLottery);
        var closeButton = _uiClose.GetComponent<Button>();
        if (closeButton == null)
        {
            closeButton = _uiClose.gameObject.AddComponent<Button>();
        }
        closeButton.onClick.AddListener(ClosePanel);
    }

    void TenLottery()
    {

        for (int i = _uiCenter.childCount - 1; i >= 0; i--)
        {
            Destroy(_uiCenter.GetChild(i).gameObject);
        }
        for (int i = 0; i < 10; i++)
        {
            PackageTableDataItem item = GameManage.Instance.RandomPackageTable();
            InitCell(item);
        }
    }
    void OneLottery()
    {
        for (int i = _uiCenter.childCount - 1; i >= 0; i--)
        {
            Destroy(_uiCenter.GetChild(i).gameObject);
        }
        PackageTableDataItem item = GameManage.Instance.RandomPackageTable();
        InitCell(item);

    }
    void InitCell(PackageTableDataItem item)
    {
        GameObject gb = Instantiate(cellPrefab, _uiCenter);
        gb.GetComponent<LotteryCell>().Reflush(this, item);
    }
    public override void OpenPanel()
    {
        UIManage.Instance.ShowPanel<LotterryPanel>(panelType);
    }

    public override void ClosePanel()
    {
        UIManage.Instance.HidePanel();
    }
}
