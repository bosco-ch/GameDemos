using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace Tool
{
    public static class AutoAddToAddressable
    {
        public enum GroupType
        {
            Default,
            DialogueConfig, // 对话SO分组
            CharacterConfig, // 角色配置
            UISprite, // UI贴图
            Prefab
        }

        /// <summary>
        /// 异步加载Addressable,代码添加资源
        /// </summary>
        /// <param name="assetPath">资源路径</param>
        /// <param name="key"></param>
        /// <param name="groupType">分组名</param>
        /// <param name="label">标签</param>
        public static void AddToAddressable(string assetPath, string key, GroupType groupType,
            string label)
        {
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                Debug.LogError("需要先创建AddressableAssetSetting");
                return;
            }

            string groupName = getGroupName(groupType);
            AddressableAssetGroup group = settings.FindGroup(groupName);
            if (group == null)
            {
                group = settings.CreateGroup(groupName, false, false, false, settings.DefaultGroup.Schemas);
            }

            string guid = AssetDatabase.AssetPathToGUID(assetPath);
            if (String.IsNullOrEmpty(guid))
            {
                Debug.LogWarning($"无法找到{assetPath}");
                return;
            }

            //将资源移入分组,设置加载用的key
            AddressableAssetEntry entry = settings.CreateOrMoveEntry(guid, group);
            entry.address = key;
            entry.labels.Add(label);
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);
            EditorUtility.SetDirty(settings);
        }

        static string getGroupName(GroupType groupType)
        {
            return groupType switch
            {
                GroupType.Default => "Default",
                GroupType.DialogueConfig => "DialogueConfig",
                GroupType.CharacterConfig => "CharacterConfig",
                GroupType.UISprite => "UISprite",
                GroupType.Prefab => "Prefab",
                _ => throw new ArgumentOutOfRangeException(nameof(groupType), groupType, null)
            };
        }
    }
}