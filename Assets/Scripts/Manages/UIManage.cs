using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Manages
{
    /// <summary>
    /// ui管理器
    /// 实现统一
    /// </summary>
    public class UIManage : Core.Singleton<UIManage>
    {
        //判断一下是否在GameRoot下面，不在的话就把自己放到GameRoot下面
        protected override void Awake()
        {
            base.Awake();

            if (transform.parent == null || transform.parent != GameRoot.Instance.transform)
            {
                transform.SetParent(GameRoot.Instance.transform);
            }
        }
        private const string PanelPrefabPath = "Prefab/Panel/";//面板的预制件位置
        private static Dictionary<UIPanelType, string> _dictPrefabPath = new()
        {
            {UIPanelType.LotteryPanel,"Lottery/LotteryPanel"  },
            {UIPanelType.MainMenuPanel,"MainPanel"  },
            {UIPanelType.PackagePanel,"Package/PackagePanel"  },
            {UIPanelType.SettingPanel,""  },
        };
        //加一个线程安全锁
        private Stack<BasePanel> _basePanelsStack = new();
        //预制件缓存
        private Dictionary<UIPanelType, BasePanel> _panelDict = new();
        //音效缓存
        private Dictionary<Button, AudioClip> _audioClipDict = new();
        public T ShowPanel<T>(UIPanelType panelType) where T : BasePanel
        {
            T currectPanel = null;
            //检查一下缓存，若是缓存已经存在，则之间显示
            if (_panelDict.TryGetValue(panelType, out BasePanel outPanel))
            {
                //outPanel.OpenPanel();
                currectPanel = outPanel as T;
                outPanel.gameObject.SetActive(true);
                _basePanelsStack.Push(outPanel);
                return outPanel as T;
            }
            //缓存中没有，则使用预制体
            GameObject _prefab = Resources.Load<GameObject>(PanelPrefabPath + _dictPrefabPath[panelType]);
            if (_prefab == null)
            {
                Debug.LogError($"path of {PanelPrefabPath + _dictPrefabPath[panelType]} is not exit");
                return null;
            }
            GameObject panel = Instantiate(_prefab);//实例化
            currectPanel = panel.GetComponent<T>();
            if (currectPanel == null)
            {
                Debug.LogError($"Script {panelType.ToString()} is not mounted on the {currectPanel}");
                Destroy(panel);//避免资源浪费
                return null;
            }
            currectPanel.init();
            _panelDict.Add(panelType, currectPanel);
            if (_basePanelsStack.Count > 0)
            {
                var topPanel = _basePanelsStack.Peek();
                topPanel.gameObject.SetActive(false);
            }
            currectPanel.gameObject.SetActive(true);
            _basePanelsStack.Push(currectPanel);
            //给面板里面所有按钮添加上音效
            foreach (var obj in currectPanel.GetComponentsInChildren<UnityEngine.UI.Button>(true))
            {
                obj.onClick.AddListener(() =>
                {
                    AudioManage.Instance.PlayOneShot();
                });
            }
            return currectPanel;
        }
        public void HidePanel()
        {
            // _panelDict.TryGetValue(panelType, out BasePanel outPanel);
            // if (outPanel != null)
            // {
            //     // outPanel.ClosePanel();
            //     outPanel.gameObject.SetActive(false);
            //     return;
            // }
            // else
            // {
            //     Debug.LogWarning($"{outPanel} not exit,dont need hide");
            // }
            if (_basePanelsStack.Count == 0) return;
            var panel = _basePanelsStack.Pop();
            panel.gameObject.SetActive(false);
            _basePanelsStack.TryPeek(out BasePanel topPanel);
            if (topPanel != null)
            {
                topPanel.gameObject.SetActive(true);
            }

        }
        /// <summary>
        /// 移除panel
        /// </summary>
        public void DestroyPanel(UIPanelType panelType)
        {
            _panelDict.TryGetValue(panelType, out BasePanel outPanel);
            if (outPanel != null)
            {
                Destroy(outPanel);
                _panelDict.Remove(panelType);
            }
            else
            {
                Debug.LogWarning($"{outPanel} is not load,dont need destory");
            }

        }
        /// <summary>
        /// 销毁所有panel
        /// </summary>
        public void DestroyAllPanel()
        {
            foreach (var panel in _panelDict)
            {
                Destroy(panel.Value.gameObject);
            }
        }
        /// <summary>
        /// 隐藏所有面板
        /// </summary>
        public void HideAllPanel()
        {
            foreach (var _key in _panelDict.Values)
            {
                _key.ClosePanel();
                _key.gameObject.SetActive(false);
            }
        }
        public void RemoveAll()
        {
            _panelDict.Clear();
        }
        public void OnTestFinish()
        {

        }
        void OnApplicationQuit()
        {
            Destroy(gameObject);
        }
    }
}
