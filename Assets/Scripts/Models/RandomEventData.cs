using UnityEngine;
namespace Models
{
    [CreateAssetMenu(fileName = "NewEventData", menuName = "Random/EventData")]
    public class RandomEventData : ScriptableObject
    {
        public string eventName = "bird-fly";
        [Header("生成设置")] public GameObject eventPrefab; //需要生成的预制体
        [Range(0, 1f)] public float spawnChance = 0.5f; //每次发生的频率
        public Vector2 minCoolDown = new Vector2(10, 20); //冷却的时间
        public Vector3 spawnPosition => new Vector3(-7.57f,5.12f,0); //生成位置
        [Header("行为参数")]
        public float flightSpeed = 5f;
        public Vector2 spawnHeightRange = new Vector2(3f, 5f);
        public Animation animation; //关联动画
    }
}