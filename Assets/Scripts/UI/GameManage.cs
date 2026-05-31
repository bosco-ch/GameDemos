using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

namespace UI
{
    public class GameManage : Singleton<GameManage>
    {
        protected override void Awake()
        {
            base.Awake();
            if (this.transform.parent == null || this.transform.parent != GameRoot.Instance.transform)
            {
                this.transform.SetParent(GameRoot.Instance.transform);
            }
        }
        private PackageTable packageTable;
        // Start is called before the first frame update
        // void Start()
        // {
        //     UIManage.Instance.ShowPanel<MainPanel>(UIPanelType.MainMenuPanel);
        // }

        public List<PackageTableItem> GetPackageTable()
        {
            if (packageTable == null)
            {
                packageTable = Resources.Load<PackageTable>("TableData/PackageTable");
            }

            return packageTable.dataList;
        }

        public List<PackageTableDataItem> GetpackageTableItems()
        {
            List<PackageTableDataItem> _items = PackageLocalTable.Instance.LoadData();
            _items.OrderBy(b => b.level).ThenByDescending(b => b.id);
            return _items;
        }

        public PackageTableItem GetPackageTableItemByID(int id)
        {
            return GetPackageTable().Find(b => b.id == id);
        }

        public PackageTableItem GetPackageTableDataItemByUid(string uid)
        {
            return GetPackageTable().FirstOrDefault(b => b.uid == uid);
        }

        public void DeletedTableItem(List<string> uid)
        {
        }

        public void AddPackageTableItem(PackageTableDataItem item)
        {
            PackageLocalTable.Instance.item.Add(item);
            PackageLocalTable.Instance.SaveData();
        }

        //进度信息
        public List<PackageTableDataItem> GetPackageLocalTableData()
        {
            return PackageLocalTable.Instance.LoadData();
        }

        public PackageTableDataItem RandomPackageTable()
        {
            var items = GetPackageTable();
            PackageTableDataItem ptdi = new();
            System.Random random = new();
            int id = random.Next(1, items.Count);
            Debug.Log(id);
            var item = GetPackageTableItemByID(id);
            int star = random.Next(1, 3);
            ptdi = new PackageTableDataItem()
            {
                id = id,
                uid = item.uid,
                isNew = true,
                star = star,
                level = star
            };
            AddPackageTableItem(ptdi);
            return ptdi;
        }
    }
}