using character.Entity.bases;
using character.Interfaces;
using UnityEngine;

namespace Factories
{
    public class KillTargetTask : ITask
    {
        private EnemyBase BeKillTarget => GameObject.FindWithTag("Boss").GetComponent<EnemyBase>();
        public bool IsCompleted()
        {
            return BeKillTarget.isDead;
        }
    }
}