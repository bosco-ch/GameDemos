using System.Collections;
using UnityEngine;

namespace PlayerMainScenes.LoadStates
{
    public class FadeState : ILoadState
    {
        private float _time;
        private float _transitionDuration;
        private readonly GameProcessManager _gameProcessManager;
        private float _currentProgress;
        private float _alpha;

        public FadeState(GameProcessManager gameProcessManager)
        {
            this._gameProcessManager = gameProcessManager;
        }

        public void OnEnterState()
        {
            Debug.Log("开始淡化背景");
            _currentProgress = _gameProcessManager.CurrentProgress;
        }

        public void OnUpdateState()
        {
            _time += Time.deltaTime;
            _alpha = 1 - Mathf.Clamp01(_time / _transitionDuration);
            if (_time >= _transitionDuration)
            {
                _gameProcessManager.SwitchState(LoadStateType.CompleteState);
            }
            else
            {
                _gameProcessManager.SetImgAlpha(_alpha);
            }
        }

        public void OnExitState()
        {
            _currentProgress += _gameProcessManager.wdl.fadeStateWeight /
                                _gameProcessManager.WeightDuringLoadingTotal;
            _gameProcessManager.SetSliderValue(_currentProgress);
        }
    }
}