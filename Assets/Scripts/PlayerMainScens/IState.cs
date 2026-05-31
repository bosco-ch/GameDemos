//状态机 接口

public interface IState
{
    void OnEnter();
    void OnExit();
    void Update();
}