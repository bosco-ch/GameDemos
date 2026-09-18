using System;
using System.Collections;
using UnityEngine;

namespace PlayerMainScenes.LoadStates
{
    public class LoadSceneState : ILoadState
    {
        private readonly GameProcessManager _gameProcessManager;
        private float _currentProgress;
        private float _value;
        private float MaxProgress =>
            _gameProcessManager.wdl.loadSceneStateWeight /
            _gameProcessManager.WeightDuringLoadingTotal;

        public LoadSceneState(GameProcessManager gameProcessManager)
        {
            _gameProcessManager = gameProcessManager;
        }

        public void OnEnterState()
        {
            Debug.Log("开始加载场景文件");
            _currentProgress = _gameProcessManager.CurrentProgress;
            if (_gameProcessManager.NextScene.progress >= .9f)
            {
                _gameProcessManager.SwitchState(LoadStateType.CheckAssetState);
            }
        }

        public void OnUpdateState()
        {
            if (_gameProcessManager.NextScene.progress < .9f)
            {
                _value = _currentProgress + _gameProcessManager.NextScene.progress * MaxProgress;
                _gameProcessManager.SetSliderValue(_value);
            }
            else
            {
                _gameProcessManager.SwitchState(LoadStateType.CheckAssetState);
            }
        }


        public void OnExitState()
        {
            _currentProgress += _gameProcessManager.wdl.loadSceneStateWeight /
                                _gameProcessManager.WeightDuringLoadingTotal;
            _gameProcessManager.SetSliderValue(_currentProgress);
        }
    }
}