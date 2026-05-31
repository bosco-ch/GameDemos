
using UI;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 单个抽奖详情页
/// </summary>
public class LotteryCell : MonoBehaviour
{
    private LotteryCell instance;

    private PackageTableItem _packageTable;
    private PackageTableDataItem _packageLocalTable;//本地数据
    private LotterryPanel _lotterryPanel;
    private Transform _uiIcon;
    public void Reflush(LotterryPanel lotterryPanel, PackageTableDataItem _packageLocalTable)
    {
        this._packageLocalTable = _packageLocalTable;
        this._lotterryPanel = lotterryPanel;
        this._packageTable = GameManage.Instance.GetPackageTableItemByID(_packageLocalTable.id);
        _uiIcon.GetComponent<Image>().sprite = Resources.Load<Sprite>(_packageTable.imgpath);
        var startlevel = transform.Find("Bottom/LevelStar");
        for (int i = 0; i < 5; i++)
        {
            startlevel.GetChild(i).gameObject.SetActive(i < _packageLocalTable.star);
        }
    }
    // Start is called before the first frame update
    private void Awake()
    {
        _uiIcon = this.transform.Find("Center/Icon");
    }


}
