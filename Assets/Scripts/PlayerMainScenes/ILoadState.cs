namespace PlayerMainScenes
{
    public interface ILoadState
    {
        void OnEnterState();
        void OnUpdateState();
        void OnExitState();
    }
}