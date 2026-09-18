using System;
using System.Collections;
using UnityEngine;

namespace PlayerMainScenes.LoadStates
{
    public class WarmUpState : ILoadState
    {
        private readonly GameProcessManager _gameProcessManager;
        private float _currentProgress;

        public WarmUpState(GameProcessManager gameProcessManager)
        {
            _gameProcessManager = gameProcessManager;
        }

        public void OnEnterState()
        {
            Debug.Log("开始预热着色器");
            if (_gameProcessManager.shaderVariantCollection == null)
            {
                throw new InvalidOperationException("shaderVariantCollection未赋值，请在GameProcessManager上配置此项目");
            }

            if (!_gameProcessManager.shaderVariantCollection.isWarmedUp)
            {
                _gameProcessManager.shaderVariantCollection.WarmUp();
            }

            _currentProgress = _gameProcessManager.CurrentProgress;
        }

        public void OnUpdateState()
        {
            if (_gameProcessManager.shaderVariantCollection.isWarmedUp)
                _gameProcessManager.SwitchState(LoadStateType.LoadSceneState);
        }


        public void OnExitState()
        {
            _currentProgress += _gameProcessManager.wdl.loadSceneStateWeight /
                                _gameProcessManager.WeightDuringLoadingTotal;
            _gameProcessManager.SetSliderValue(_currentProgress);
        }
    }
}