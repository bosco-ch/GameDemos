using System.Collections;
using Core;
using UnityEngine;

namespace Manages
{
    public class SceneManager : Singleton<SceneManager>
    {
        public int next; //下一个场景的下标；
        private AsyncOperation _asyncLoad;
        public float CurrentProgress => _asyncLoad.progress;
        /// <summary>
        /// 根据下标加加载场景
        /// </summary>
        public void LoadSceneByIndex()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(next);
        }
        /// <summary>
        /// 预加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        public void PreloadNextScene(string sceneName)
        {
            StartCoroutine(PreloadSceneCoroutine(sceneName));
        }
        /// <summary>
        /// 手动进入下一个场景
        /// </summary>
        public void EnterPreloadedScene()
        {
            if (_asyncLoad != null)
            {
                _asyncLoad.allowSceneActivation = true;
            }
        }
        /// <summary>
        /// 预加载场景协程方法
        /// </summary>
        private IEnumerator PreloadSceneCoroutine(string sceneName)
        {
            _asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
            if (_asyncLoad != null)
            {
                _asyncLoad.allowSceneActivation = false; //进入等待
                if (!(_asyncLoad.progress > .95f))
                {
                    yield return null;
                }
            }
        }

        /// <summary>
        /// 获取当前场景下标
        /// </summary>
        /// <returns></returns>
        public int GetCurrentSceneIndex()
        {
            return UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        }

        /// <summary>
        /// 重新刷新当前场景
        /// </summary>
        public void ReloadCurrentScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(next);
        }
    }
}