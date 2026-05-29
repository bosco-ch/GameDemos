using System;
using System.Collections.Generic;
using character.Entity.Weapons.StateMachine;
using Manages;
using UnityEngine;
using UnityEngine.Serialization;

namespace Saves
{
    [Serializable]
    public class GameSaveData
    {
        public int currentSceneIndex; //当前场景
        [FormerlySerializedAs("Player")] public Player player;
        [FormerlySerializedAs("Enemies")] public List<Enemy> enemies;
        public TaskType currentTaskType; //当前任务
        public bool otherEnemiesAlert;
        public bool enemiesIsDecreas;
    }

    [Serializable]
    public class Player
    {
        [FormerlySerializedAs("PlayerPosition")] public Vector3 playerPosition;
        [FormerlySerializedAs("PlayerRotation")] public Vector3 playerRotation;
        [FormerlySerializedAs("PlayerScale")] public Vector3 playerScale;
        [FormerlySerializedAs("PlayerCurrentWeaponIndex")] public int playerCurrentWeaponIndex; //当前武器下标
        [FormerlySerializedAs("PlayerCurrentHealth")] public float playerCurrentHealth;
    }

    [Serializable]
    public class Enemy
    {
        [FormerlySerializedAs("EnemyName")] public string enemyName;
        [FormerlySerializedAs("EnemyRotation")] public Vector3 enemyRotation;
        [FormerlySerializedAs("EnemyPosition")] public Vector3 enemyPosition;
        [FormerlySerializedAs("EnemyScale")] public Vector3 enemyScale;
    }
}