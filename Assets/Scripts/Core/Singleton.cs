using UnityEngine;

namespace Core
{
    /// <summary>
    /// 单例模式 基类 (所有单例最好继承一次，统一一下一些公告变量)
    /// </summary>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static readonly object _lock = new object();
        private static T _instance;

        public static T Instance //
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = FindObjectOfType<T>();
                            if (_instance == null)
                            {
                                var gb = new GameObject(typeof(T).ToString());
                                _instance = gb.AddComponent<T>();
                                DontDestroyOnLoad(gb);
                            }
                        }
                    }
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}