using Manages;
using UnityEngine;

public class ButtonText : MonoBehaviour
{
    public void OnButtonClick()
    {
        UIManage.Instance.ShowPanel<MainPanel>(UIPanelType.MainMenuPanel);
    }
    public void 按下()
    {
        UIManage.Instance.ShowPanel<MainPanel>(UIPanelType.MainMenuPanel);
    }
}