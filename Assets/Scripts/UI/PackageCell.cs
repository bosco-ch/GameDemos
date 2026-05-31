using System.Collections;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PackageCell : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private PackagesPanel _uiParent;
    private PackageTableItem _PackageTableItem;//配置的物品
    private PackageTableDataItem _PackageTableDataItem;//玩家进度的数据
    private Transform _uiPreSelectedBorder; //鼠标悬停时候展示的边框
    private Transform _uiSelectedBorder;
    private Image _previewImage;
    private Color _copyColor;
    private Coroutine _fabeCoroutine;
    private bool isSelected = true;
    private PanelModel currentPanel; //当前模式

    private Transform _uiItemIcon;
    private Transform _uiItemHead;
    private Transform _uiItemNew;
    private Transform _uiStar;
    private Transform _uiItemStarText;
    private void Awake()
    {
        _uiPreSelectedBorder = transform.Find("Selected");
        _previewImage = _uiPreSelectedBorder.GetComponent<Image>();
        _copyColor = _previewImage.color;
        _copyColor.a = 1f;
        _uiSelectedBorder = transform.Find("DelectSelected");

        _uiItemIcon = transform.Find("Top/Icon");
        _uiItemHead = transform.Find("Top/head");
        _uiItemNew = transform.Find("Top/New");
        _uiStar = transform.Find("Bottom/LevelStar");
        _uiItemStarText = transform.Find("Bottom/LevelText");
    }
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="uiParent"></param>
    /// <param name="packageTableDataItem">本地保存的进度数据</param>
    public void Reflush(PackagesPanel uiParent, PackageTableDataItem packageTableDataItem)
    {
        _PackageTableItem = GameManage.Instance.GetPackageTableItemByID(packageTableDataItem.id);
        _uiParent = uiParent;
        _PackageTableDataItem = packageTableDataItem;
        if (_PackageTableItem != null)
        {
            transform.name = $"{_PackageTableItem.uid}_{_PackageTableItem.id}_{_PackageTableItem.name}";
            _uiItemIcon.GetComponent<Image>().sprite = Resources.Load<Sprite>(_PackageTableItem.imgpath);
            _uiItemStarText.GetComponent<TextMeshProUGUI>().text = $"LV.{_PackageTableItem.star.ToString()}";
            _uiItemNew.gameObject.SetActive(_PackageTableItem.isNew);
            for (int i = 0; i < _PackageTableItem.star; i++)
            {
                // _uiStar.GetChild(i).GetComponent<LayoutElement>().preferredWidth = starRectWidth;
                _uiStar.GetChild(i).gameObject.SetActive(i < _PackageTableItem.star);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_fabeCoroutine != null)
        {
            StopCoroutine(_fabeCoroutine);
            _fabeCoroutine = null;
        }
        _copyColor.a = 1f;
        _previewImage.color = _copyColor;
        if (this._uiParent.ChooseUID == _PackageTableItem.uid)
        {
            return;
        }

        this._uiParent.ChooseUID = _PackageTableItem.uid;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _uiSelectedBorder.gameObject.SetActive(isSelected);
        isSelected = !isSelected;
        if (_uiParent.selecedItems.Contains(_PackageTableItem.uid))
            _uiParent.selecedItems.Remove(_PackageTableItem.uid);
        else
            _uiParent.selecedItems.Add(_PackageTableItem.uid);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_fabeCoroutine != null)
        {
            StopCoroutine(_fabeCoroutine);
        }

        _fabeCoroutine = StartCoroutine(FadeOut(1f, _copyColor.a));
    }

    IEnumerator FadeOut(float duration = 0.8f, float currentAlpha = 0f)
    {
        float elapeTime = 0;
        _copyColor.a = currentAlpha;
        while (elapeTime < duration)
        {
            _copyColor.a = Mathf.Lerp(_copyColor.a, 0f, elapeTime / duration);
            _previewImage.color = _copyColor;
            elapeTime += Time.deltaTime;
            yield return null;
        }
        _copyColor.a = 0;
        _previewImage.color = _copyColor;
        _fabeCoroutine = null;
    }
}