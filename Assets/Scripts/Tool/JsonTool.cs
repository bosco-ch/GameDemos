using System.IO;
using UnityEditor;
using UnityEngine;
///json generate tool
public class JsonTool
{
    
    //读取文件并转告换成泛型对象
    public static T ReadJson<T>(string jsonFilePath) where T : class, new()
    {
        //string ResourcesPath = System.IO.Path.Combine(Application.dataPath, jsonFilePath);
        string jsonContent = Resources.Load<TextAsset>(jsonFilePath).text;
        return JsonUtility.FromJson<T>(jsonContent);
    }
    public static string ToJson<T>(T obj)
    {
        string jsonString = JsonUtility.ToJson(obj, true);
        return jsonString;
    }
    public static bool SaveFile(string jsonString, string fileName,string saveTarget = "Resources/bits")
    {
        //save to Assert/Resources/config
        string ResourcesPath = System.IO.Path.Combine(Application.dataPath, saveTarget);
        if (!string.IsNullOrEmpty(ResourcesPath) && !Directory.Exists(ResourcesPath))
        {
            Directory.CreateDirectory(ResourcesPath);
        }
        try
        {
            //json
            File.WriteAllText(ResourcesPath + $"/{fileName}.json", jsonString);
            return true;
        }
        catch
        {
            throw;
        }
    }
}
