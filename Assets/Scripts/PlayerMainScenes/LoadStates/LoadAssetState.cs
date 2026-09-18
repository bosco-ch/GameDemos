using Manages;
using UnityEngine;

namespace PlayerMainScenes.LoadStates
{
    public class LoadAssetState : ILoadState
    {
        private readonly GameProcessManager _gameProcessManager;
        private float _value;
        private float _currentProgress;

        private float MaxProgress =>
            _gameProcessManager.wdl.loadSceneStateWeight /
            _gameProcessManager.WeightDuringLoadingTotal;

        public LoadAssetState(GameProcessManager gameProcessManager)
        {
            _gameProcessManager = gameProcessManager;
        }

        public void OnEnterState()
        {
            Debug.Log("开始加载资源文件");
            _currentProgress = _gameProcessManager.CurrentProgress;
        }

        public void OnUpdateState()
        {
            if (ResourceManager.Instance.HaveLoadAssetTotal < ResourceManager.Instance.NeedLoadAssetTotal)
            {
                _value = _currentProgress + ResourceManager.Instance.LoadAssetsPercent * MaxProgress;
                _gameProcessManager.SetSliderValue(_value);
            }
            else
            {
                _gameProcessManager.SwitchState(LoadStateType.FadeState);
            }
        }


        public void OnExitState()
        {
            _currentProgress += _gameProcessManager.wdl.loadAssetStateWeight /
                                _gameProcessManager.WeightDuringLoadingTotal;
            _gameProcessManager.SetSliderValue(_currentProgress);
        }
    }
}