using Manages;
using UnityEngine;

namespace PlayerMainScenes.LoadStates
{
    public class CheckAssetState : ILoadState
    {
        private readonly GameProcessManager _gameProcessManager;
        private float _currentProcess;

        public CheckAssetState(GameProcessManager gameProcessManager)
        {
            _gameProcessManager = gameProcessManager;
        }

        public void OnEnterState()
        {
            Debug.Log("开始检查资源文件");
            _currentProcess = _gameProcessManager.CurrentProgress;
        }

        public void OnUpdateState()
        {
            if (ResourceManager.Instance.isFindAssetSuccess)
            {
                _gameProcessManager.SwitchState(LoadStateType.LoadAssetState);
            }
        }

        public void OnExitState()
        {
            _currentProcess += _gameProcessManager.wdl.checkAssetStateWeight /
                               _gameProcessManager.WeightDuringLoadingTotal;
            _gameProcessManager.SetSliderValue(_currentProcess);
        }
    }
}