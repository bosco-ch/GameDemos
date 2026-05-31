using character.Entity.bases;
using UnityEngine.Events;

namespace character.Entity.enemy.States
{
    public class Idle : IEnemyState
    {
        private EnemySmart2dAI _enemy;

        public Idle(EnemySmart2dAI enemy)
        {
            this._enemy = enemy;
        }

        //动画订阅
        public static UnityEvent OnAnimatorIdleEnter;//播放动画

        public void OnEnter()
        {
            //进入Idle状态时的逻辑
            OnAnimatorIdleEnter?.Invoke();
        }

        public void OnExit()
        {
            //退出Idle状态时的逻辑
            //例如：停止Idle动画等
        }

        public void Update()
        {
            //在Idle状态下的更新逻辑
            //例如：检测玩家是否进入视野范围，决定是否切换到Chase状态等
            var hit = Detector.RayCast(_enemy.transform.position,
                _enemy.transform.forward,
                _enemy.ForwardDis,
                layer: 1 << 6
            );
            _enemy.SwitchState(EnemyState.Alert);
        }
    }
}