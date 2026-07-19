using System;
using System.Collections.Generic;
using Editors;
using Manages;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UI
{
    public class DialoguePanel : BasePanel
    {
        public override UIPanelType panelType => UIPanelType.DialoguePanel;

        private void Awake()
        {
            DialogueManager.Instance.LoadDialogueData();
        }

        public override void OpenPanel()
        {
            UIManage.Instance.ShowPanel<DialoguePanel>(panelType, "DialoguePanel");
        }

        public override void ClosePanel()
        {
            UIManage.Instance.HidePanel();
        }
    }
}