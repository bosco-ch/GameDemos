using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class PackageDetail : MonoBehaviour
{
    private Transform _uiName;
    private Transform _uiIcon;
    private Transform _uiDescription;
    private Transform _uiDetailedDescription;
    private Transform _uiStarText;
    private PackageTableItem _packageDataItem;
    private PackageTableItem _packageLocalData;
    private PackagesPanel uiParent;

    private void Awake()
    {
        InitName();
    }
    void InitName()
    {
        _uiName = transform.Find("Top/Title");
        _uiIcon = transform.Find("Center/Icon");
        _uiDescription = transform.Find("Center/Description");
        _uiDetailedDescription = transform.Find("Bottom/Description");
        _uiStarText = transform.Find("LevelPanel/LevelText");
    }

    public void ReFlush(PackageTableItem data, PackagesPanel uiParent)
    {
        _packageLocalData = data;
        _packageDataItem = GameManage.Instance.GetPackageTableItemByID(data.id);
        this.uiParent = uiParent;
        _uiName.GetComponent<TextMeshProUGUI>().text = data.name;
        _uiIcon.GetComponent<Image>().sprite = Resources.Load<Sprite>(data.imgpath);
    }
}