using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public enum UIPanelType
{
    MainMenuPanel,
    PackagePanel,
    SettingPanel,
    LotteryPanel
}
public abstract class BasePanel : MonoBehaviour
{
    //面板类型
    public abstract UIPanelType panelType { get; } // 属性
    //父节点
    protected Transform UIRoot;
    public virtual void init()//virtual 可以让子类选择重写
    {
        UIRoot = GameObject.Find("Canvas").transform;
        //将面板挂宰uiroot下面
        transform.SetParent(UIRoot);
        // transform.position = Vector3.zero;
        // transform.localScale = Vector3.one;
        var rect = GetComponent<RectTransform>();
        rect.anchoredPosition = Vector3.zero;
        // rect.anchorMin = new Vector2(0, 0);
        // rect.anchorMax = new Vector2(1, 1);
        rect.sizeDelta = Vector2.zero;//用于适配父节点的拉伸

    }
    /// <summary>
    /// 展示面板
    /// </summary>
    public abstract void OpenPanel();
    /// <summary>
    /// 隐藏面板
    /// </summary>
    public abstract void ClosePanel();
    public virtual void DesroyPanel()
    {
        Destroy(gameObject);
    }
}
