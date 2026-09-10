using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System.Linq;
using Tool;

namespace Editors
{
    public class DialogueExcelImport : EditorWindow
    {
        private string csvPath = ""; //导入文件路径
        private string outputPath = @"Assets/Resources/TableData"; //导出文件位置路径

        [MenuItem("Tools/Import/(对话内容导入)Dialogue Excel")]
        static void ShowWindow()
        {
            GetWindow<DialogueExcelImport>("对话表格导入");
        }

        void OnGUI()
        {
            csvPath = EditorGUILayout.TextField("CSV Path", csvPath);
            outputPath = EditorGUILayout.TextField("Output Path", outputPath);
            if (GUILayout.Button("Import"))
            {
                importCsv();
            }
        }

        private void importCsv()
        {
            // string fullpath = Application.dataPath + csvPath.Substring(0, csvPath.LastIndexOf('.'));
            string fullpath = csvPath;
            if (!File.Exists(fullpath))
            {
                EditorUtility.DisplayDialog("error", $"路径{fullpath}不存在", "ok");
                return;
            }

            var lines = File.ReadAllLines(fullpath);
            //存字典可能比较快一点
            Dictionary<int, DialogueSo> dialoguesDict = new();
            for (int i = 1; i < lines.Length; i++)
            {
                string[] line = SplitString(lines[i]);

                DialogueSo dia = CreateInstance<DialogueSo>();
                dia.index = int.Parse(line[0]);
                dia._speakName = line[1];
                dia.content = line[2];
                Dialogue[] diaChild = Enumerable.Range(0, 4).Select(_ => new Dialogue()).ToArray();
                if (line[4] != "" && !string.IsNullOrEmpty(line[4]))
                {
                    diaChild[0].choice = int.Parse(line[4].Substring(3, 1));
                }

                if (line[5] != "" && !string.IsNullOrEmpty(line[5]))
                {
                    diaChild[1].choice = int.Parse(line[5].Substring(3, 1));
                }

                if (line[6] != "" && !string.IsNullOrEmpty(line[6]))
                {
                    diaChild[2].choice = int.Parse(line[6].Substring(3, 1));
                }

                if (!string.IsNullOrEmpty(line[7]) && line[7] != "")
                {
                    diaChild[3].choice = int.Parse(line[7].Substring(3, 1));
                }

                dia._dialogues = diaChild;
                dialoguesDict.Add(int.Parse(line[0]), dia);
            }

            //嵌套关系需要重新填充一遍
            foreach (var dialogue in dialoguesDict)
            {
                string savePath = outputPath + "/" + dialogue.Key + ".asset";
                if (File.Exists(savePath))
                {
                    File.Delete(savePath);
                }

                AssetDatabase.CreateAsset(dialogue.Value, savePath);
                AutoAddToAddressable.AddToAddressable(savePath, $"dialogue_{dialogue.Key.ToString()}",
                    AutoAddToAddressable.GroupType.DialogueConfig, "dialogue");
            }

            AssetDatabase.SaveAssets();

            // //嵌套关系需要重新填充一遍
            foreach (var dialogue in dialoguesDict)
            {
                foreach (var d in dialogue.Value._dialogues)
                {
                    // dialogue.Value._dialogues[d.choice]._nextDialogue = dialoguesDict[d.choice];
                    if (dialoguesDict.TryGetValue(d.choice, out var b))
                    {
                        dialogue.Value._dialogues.First(t => t.choice == d.choice)._nextDialogue = b;
                    }
                    else
                    {
                        Debug.Log("不存在");
                    }
                }
            }

            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("完成", "已完成", ok: "OK");
            //需要给打上异步加载的标记

            return;
        }

        private string[] SplitString(string line)
        {
            bool isQuoted = false; //判断是否是引号包裹的状态
            string current = "";
            List<string> results = new();
            foreach (char s in line)
            {
                if (s == ',' && isQuoted == false)
                {
                    results.Add(current);
                    current = "";
                }
                else if (s == '"' && isQuoted == false)
                {
                    isQuoted = true;
                }
                else if (s == '"' && isQuoted == true)
                {
                    // results.Add(current);
                    current = "";
                    isQuoted = false;
                }
                else
                {
                    current += s;
                }
            }

            results.Add(current);
            return results.ToArray();
        }
    }
}