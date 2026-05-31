using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PackageTable", menuName = "Script/PackageTable")]//按钮的名称，创建的文件的名称
public class PackageTable : ScriptableObject
{
    public List<PackageTableItem> dataList = new();
}
[System.Serializable]
public class PackageTableItem
{
    public string uid;
    public int id;
    public bool isNew = true;
    public int type;
    public int star;
    public string description;
    public string skilldescription;
    public string name;
    public string imgpath;
}