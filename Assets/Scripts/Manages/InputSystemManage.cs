using System;
using UnityEngine;

namespace Manages
{
    public enum InputMode
    {
        None,
        GamePlay,
        UI
    }
    public class InputSystemManage : MonoBehaviour
    {
        public static InputSystemManage Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<InputSystemManage>();
                    if (_instance == null)
                    {
                        var obj = new GameObject("InputSystemManage");
                        _instance = obj.AddComponent<InputSystemManage>();
                    }
                }
                return _instance;
            }
        }

        private static InputSystemManage _instance;
        // private InputActions actions;
        private InputMode _inputMode;
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
        }

//切换到游戏场景
        public void Switch2Game()
        {
            this._inputMode = InputMode.GamePlay;
        }

//切换到UI场景
        public void Switch2UI()
        {
            this._inputMode = InputMode.UI;
        }

        public InputMode GetMode() => _inputMode;
    }
}