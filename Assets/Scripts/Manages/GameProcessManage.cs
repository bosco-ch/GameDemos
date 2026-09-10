using character.Entity.bases;
using character.Entity.enemy.States;
using character.Interfaces;
using Core;
using UI;
using UnityEngine;

namespace Manages
{
    public enum TaskType //目标任务类型
    {
        DestroyData, // 摧毁数据
        KillTarget, //杀死目标
        CollectData //收集数据
    }

    /// <summary>
    ///游戏进程 
    /// </summary>
    public class GameProcessManage : Singleton<GameProcessManage>
    {
        public bool OtherEnemiesAlert => otherEnemiesAlert;
        public bool EnemiesIsDecreas => enemiesIsDecreas;
        [SerializeField] private bool otherEnemiesAlert = false; //是否触发警觉；
        [SerializeField] private bool enemiesIsDecreas = false; //敌人是否有被消灭的；
        private IWinCheck _winCheck;
        private IDamageable _enemyDamageable;
        public Transform playerTransform;
        public Transform targetPosition;
        private TaskType _taskType;
        private ITask _task;
        private bool isOpenFirst = true; //第一次进入游戏


        public TaskType TaskType
        {
            get => _taskType;
            private set
            {
                _task = Factories.TaskFactory.CreateTask(value);
                _taskType = value;
            }
        }

        private void Update()
        {
            if (_task != null && _task.IsCompleted())
            {
                Win();
                _task = null;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Time.timeScale = 0;
                FirstOpen();
            }
        }

        protected override void Awake()
        {
            base.Awake();
            playerTransform = GameObject.FindObjectOfType<PlayerBase>().transform;
            TaskType = TaskType.CollectData;
            Alert.OnAlertEnter.AddListener(SetAlert);
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += (_, _) =>
            {
                playerTransform = GameObject.FindObjectOfType<PlayerBase>().transform;
            };
            //绑定action
            EnemyBase.Destroy += () => { enemiesIsDecreas = true; };
            PlayerBase.Destroy += () =>
            {
                SceneManager.Instance.next = SceneManager.Instance.GetCurrentSceneIndex();
                SceneManager.Instance.ReloadCurrentScene();
            };
        }

        /// <summary>
        /// 其实这边应该做成策略的，但是觉得解耦太多了
        /// </summary>
        private void Win()
        {
            if (!enemiesIsDecreas && !otherEnemiesAlert)
                _winCheck = new CompleteNoAlertOrKill();
            else if (!enemiesIsDecreas)
                _winCheck = new KillNoBodyWin();
            else if (!otherEnemiesAlert)
                _winCheck = new NobodyAlertWin();
            else
                _winCheck = new KillEveryBody();
            _winCheck.Win();
        }

        void SetAlert()
        {
            otherEnemiesAlert = true;
        }

        private void FirstOpen()
        {
            UIManage.Instance.ShowPanel<DialoguePanel>(UIPanelType.DialoguePanel);
        }

        void closePanel()
        {
            UIManage.Instance.HidePanel();
        }
    }

    /// <summary>
    /// 不杀死如何人 获得胜利
    /// </summary>
    public class KillNoBodyWin : IWinCheck
    {
        public void Win()
        {
            Debug.Log("3个星");
        }
    }

    /// <summary>
    /// 暗杀但是没有触发其他人的警觉 获得胜利
    /// </summary>
    public class NobodyAlertWin : IWinCheck
    {
        public void Win()
        {
            Debug.Log("2个星");
        }
    }

    /// <summary>
    /// 不惜一切代价完成了任务
    /// </summary>
    public class KillEveryBody : IWinCheck
    {
        public void Win()
        {
            Debug.Log("一个星");
        }
    }

    public class CompleteNoAlertOrKill : IWinCheck
    {
        public void Win()
        {
            Debug.Log("三个星");
        }
    }
}