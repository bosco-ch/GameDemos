using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class PackageLocalTable
{
    private static PackageLocalTable _instance;
    public List<PackageTableDataItem> item = new();
    public static PackageLocalTable Instance //静态属性
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PackageLocalTable();
            }

            return _instance;
        }
    }
    public void SaveData()
    {
        string json = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("PackageLocalTable", json); //保存数据
        PlayerPrefs.Save();
    }

    public List<PackageTableDataItem> LoadData()
    {
        if (PlayerPrefs.HasKey("PackageLocalTable"))
        {
            string json = PlayerPrefs.GetString("PackageLocalTable");
            PackageLocalTable data = JsonUtility.FromJson<PackageLocalTable>(json);
            item = data.item;
            return item;
        }
        else
        {
            return new List<PackageTableDataItem>();
        }
    }
}

[Serializable]
public class PackageTableDataItem
{
    public string uid;
    public int id;
    public int num;
    public bool isNew;
    public int level;
    public int star;
    public override string ToString()
    {
        return string.Format("Guid:{0},id:{1},id:{2}", uid, id, num);
    }
}