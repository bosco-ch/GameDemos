
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
/// 反应状态
/// </summary>
public class ReactState : IState
{
    // Start is called before the first frame update
    FSM manager;//FSM管理器
    Paramters paramters;  //参数
    private AnimatorStateInfo info;//获取动画的播放进度
    public ReactState(FSM fSM)
    {
        this.manager = fSM;
        this.paramters = fSM.paramters;
    }
    public void OnEnter()
    {
        paramters.animator.Play("React");//播放反应动画

    }
    public void OnExit()
    {
        
    }
    public void Update()
    {
        info = paramters.animator.GetCurrentAnimatorStateInfo(0);//获取播放的进度
        if (info.normalizedTime > .95f)//通过这个来判断动画完成
        {
            manager.TransitionState(StateType.Chase);
        }
    }
}
