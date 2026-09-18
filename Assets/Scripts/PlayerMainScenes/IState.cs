//状态机 接口

namespace PlayerMainScenes
{
    public interface IState
    {
        void OnEnter();
        void OnExit();
        void Update();
    }
}