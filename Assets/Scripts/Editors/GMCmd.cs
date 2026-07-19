using System;
using System.Collections.Generic;
using Manages;
using Saves;
using UI;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    public class GmCmd
    {
        [MenuItem("CMCmd/1.Read Table")]
        public static void ReadTable()
        {
            PackageTable packageTable = Resources.Load<PackageTable>("TableData/PackageTable");
            foreach (PackageTableItem item in packageTable.dataList)
            {
                // Debug.Log(item.name);
                Debug.Log(string.Format(
                    "id:{0},type:{1},star:{2},description:{3},skilldescription:{4},name:{5},imgpath:{6}", item.id,
                    item.type, item.star, item.description, item.skilldescription, item.name, item.imgpath));
            }
        }

        [MenuItem("CMCmd/2.Create Package Test Data")]
        public static void CreateTestData()
        {
            PackageLocalTable.Instance.item = new List<PackageTableDataItem>();
            for (int i = 0; i < 3; i++)
            {
                PackageTableDataItem item = new()
                {
                    uid = Guid.NewGuid().ToString(),
                    id = i,
                    num = i * 10 + i,
                    isNew = i % 2 == 0,
                    level = (i + 1)
                };
                PackageLocalTable.Instance.item.Add(item);
            }

            PackageLocalTable.Instance.SaveData();
        }

        [MenuItem("CMCmd/3.Load Package Test Data")]
        public static void LoadTestData()
        {
            PackageLocalTable.Instance.item = PackageLocalTable.Instance.LoadData();
            foreach (var item in PackageLocalTable.Instance.item)
            {
                Debug.Log(item.id);
            }
        }

        [MenuItem("CMCmd/4.Open Package Panel")]
        public static void OpenPackagePanel()
        {
            UIManage.Instance.ShowPanel<PackagesPanel>(UIPanelType.PackagePanel);
        }

        [MenuItem("CMCmd/5.Close Package Panel")]
        public static void ClosePackagePanel()
        {
            UIManage.Instance.HidePanel();
        }

        [MenuItem("CMCmd/6.Clear All dirt of panel")]
        public static void ClearPackagePanelDirt()
        {
            UIManage.Instance.RemoveAll();
        }

        [MenuItem("2d Vision/Save date")]
        public static void VisionGameSaveDate()
        {
            SaveManager.Instance.SaveGame();
        }

        [MenuItem("2d Vision/Load date")]
        public static void VisionGameLoadDate()
        {
            SaveManager.Instance.LoadData();
        }

        [MenuItem("CMCmd/7.Open Dialogue panel")]
        public static void OpenDialoguePanel()
        {
            UIManage.Instance.ShowPanel<DialoguePanel>(UIPanelType.DialoguePanel,
                "DialoguePanel");
        }
    }
}