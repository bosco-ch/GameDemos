using character.Interfaces;
using Manages;
using UnityEngine;

namespace Factories
{
    public class DestroyDataTask : ITask
    {
        private static Vector3 PlayerPosition => GameProcessManage.Instance.playerTransform.position; //玩家位置

        private static Vector3 TargetPosition
        {
            get
            {
                if (GameProcessManage.Instance.targetPosition != null)
                {
                    return GameProcessManage.Instance.targetPosition.position;
                }
                else
                {
                    return GameObject.Find("FinishPoint").transform.position;
                }
            }
        }
        //目标位置
        private float _holdTime = 0;
        public bool IsCompleted()
        {
            if (Vector3.Distance(PlayerPosition, TargetPosition) <= .3f)
            {
                _holdTime += Time.deltaTime;
                if (_holdTime >= 3f)
                    return true;
            }

            return false;
        }
    }
}