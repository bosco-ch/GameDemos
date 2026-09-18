using System.Collections.Generic;
using Core;
using Editors;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Manages
{
    /// <summary>
    /// 异步加载资源
    /// </summary>
    public class ResourceManager : Singleton<ResourceManager>
    {
        private readonly Dictionary<string, AsyncOperationHandle> _resourcesCache =
            new();

        public List<DialogueSo> dialogueList;
        public bool isFindAssetSuccess = false;
        private float _needLoadAssetTotal = 0;
        public float NeedLoadAssetTotal => _needLoadAssetTotal;
        private float _haveLoadAssetTotal = 0;
        public float HaveLoadAssetTotal => _haveLoadAssetTotal;

        public float LoadAssetsPercent
        {
            get
            {
                if (_needLoadAssetTotal == 0)
                    return 0.0f;
                else
                {
                    // ReSharper disable once PossibleLossOfFraction
                    return _haveLoadAssetTotal / _needLoadAssetTotal;
                }
            }
        }

        [Header("配置信息")] [SerializeField] private PreLoadAssetSo preLoadAssetsSo;

        private void Start()
        {
            //填充labelcount
            // PreloadAsset<DialogueSo>("dialogue");
            AmountOfAsset();
        }

        private void Update()
        {
            // Debug.Log(NeedLoadAssetTotal);
        }

        private void AmountOfAsset()
        {
            var result = Addressables.LoadResourceLocationsAsync(preLoadAssetsSo.labels[0], typeof(DialogueSo));
            result.Completed += op =>
            {
                if (op.Status == AsyncOperationStatus.Succeeded)
                {
                    _needLoadAssetTotal = op.Result.Count;
                    isFindAssetSuccess = true;
                    Addressables.Release(op);
                    PreloadAssets<DialogueSo>(preLoadAssetsSo.labels[0]);
                }
                else
                {
                    _needLoadAssetTotal = 0;
                }
            };
        }

        /// <summary>
        /// 使用key 来加载
        /// </summary>
        /// <param name="keyName"></param>
        /// <typeparam name="T"></typeparam>
        private void PreloadAsset<T>(string keyName) where T : Object
        {
            if (_resourcesCache.ContainsKey(keyName))
            {
                return;
            }

            var result = Addressables.LoadAssetAsync<T>(keyName);
            result.Completed += op =>
            {
                if (op.Status == AsyncOperationStatus.Succeeded)
                    _resourcesCache.Add(keyName, result);
            };
        }

        /// <summary>
        /// 使用label标签来加载
        /// </summary>
        /// <param name="labelName"></param>
        /// <typeparam name="T"></typeparam>
        private void PreloadAssets<T>(string labelName)
        {
            if (_resourcesCache.ContainsKey(labelName))
            {
                return;
            }

            var result = Addressables.LoadAssetsAsync<T>(
                labelName
                , null
                , true);
            result.Completed += op =>
            {
                if (op.Status == AsyncOperationStatus.Succeeded)
                {
                    _resourcesCache.Add(labelName, op);
                    _haveLoadAssetTotal += op.Result.Count;
                }
            };
        }

        private void OnDestroy()
        {
            foreach (var cache in _resourcesCache)
            {
                if (cache.Value.IsValid())
                {
                    Addressables.Release(cache.Value);
                }
            }

            _resourcesCache.Clear();
        }
    }
}