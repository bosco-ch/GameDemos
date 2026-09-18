using UnityEngine;

namespace PlayerMainScenes.LoadStates
{
    public class LoadCompletedState : ILoadState
    {
        public void OnEnterState()
        {
            OnExitState();
        }

        public void OnUpdateState()
        {
            throw new System.NotImplementedException();
        }

        public void OnExitState()
        {
            Debug.Log("completed finish");
        }
    }
}