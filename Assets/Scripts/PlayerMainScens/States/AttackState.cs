

using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// 攻击状态
/// </summary>
public class AttackState : IState
{
    // Start is called before the first frame update
    FSM manager;//FSM管理器
    Paramters paramters;  //参数
    private AnimatorStateInfo info;

    public AttackState(FSM fSM)
    {
        this.manager = fSM;
        this.paramters = fSM.paramters;
    }
    public void OnEnter()
    {
        paramters.animator.Play("Attack");
    }
    public void OnExit()
    {

    }
    public void Update()
    {
        info = paramters.animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime > .95f)//通过这个来判断动画完成
        {
            Debug.Log("attack->chase");
            manager.TransitionState(StateType.Chase);//攻击完成后 切换为追击状态
        }
    }
}
