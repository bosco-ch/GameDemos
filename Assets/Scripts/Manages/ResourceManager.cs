using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Manages
{
    /// <summary>
    /// 异步加载资源
    /// </summary>
    public class ResourceManager : Singleton<ResourceManager>
    {
        private readonly Dictionary<string, object> _resourcesCache = new Dictionary<string, object>();

        public async Task PreloadAsset<T>(string key, T t) where T : Object
        {
            if (_resourcesCache.ContainsKey(key))
            {
                return;
            }

            var result = await Addressables.LoadAssetAsync<T>(key).Task;
            _resourcesCache.Add(key, result);
        }

        public void ClearAllResource()
        {
            foreach (var cache in _resourcesCache)
            {
                Addressables.Release(cache.Value);
            }

            _resourcesCache.Clear();
        }
    }
}