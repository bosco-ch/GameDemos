using System;
using System.Collections.Generic;
using System.IO;
using character.Entity.player;
using Core;
using Manages;
using UnityEngine;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Saves
{
    public class SaveManager : Singleton<SaveManager>
    {
        private string _savePath; //保存路径

        private GameObject _enemies;

        protected override void Awake()
        {
            base.Awake();
            _savePath = Path.Combine(Application.persistentDataPath, "save.json");
            _enemies = GameObject.Find("Enemies");
        }

        public void SaveGame()
        {
            List<Enemy> list = new List<Enemy>();
            //获取所有的活跃的敌人
            for (int i = 0;
                 i < _enemies.transform.childCount;
                 i++)
            {
                list.Add(new Enemy()
                {
                    enemyPosition = _enemies.transform.GetChild(i).position,
                    enemyRotation = _enemies.transform.GetChild(i).rotation.eulerAngles,
                    enemyScale = _enemies.transform.GetChild(i).localScale,
                });
            }

            var data = new GameSaveData()
            {
                currentSceneIndex = SceneManager.GetActiveScene().buildIndex,
                player = new Player()
                {
                    playerPosition = GameProcessManage.Instance.playerTransform.position,
                    playerRotation = GameProcessManage.Instance.playerTransform.rotation.eulerAngles,
                    playerScale = GameProcessManage.Instance.playerTransform.localScale,
                    playerCurrentHealth =
                        GameProcessManage.Instance.playerTransform.GetComponent<PlayerBase>().CurrentHealth,
                    playerCurrentWeaponIndex =
                        GameProcessManage.Instance.playerTransform.GetComponent<PlayerAttackController>().WeaponIndex
                },
                currentTaskType = GameProcessManage.Instance.TaskType,
                enemies = list,
                otherEnemiesAlert = GameProcessManage.Instance.OtherEnemiesAlert,
                enemiesIsDecreas = GameProcessManage.Instance.EnemiesIsDecreas
            };

            var jsonString = JsonUtility.ToJson(data, prettyPrint: true);
            File.WriteAllText(_savePath, jsonString);
            Debug.Log("保存成功");
        }

        public void LoadData()
        {
            if (!File.Exists(_savePath))
            {
                Debug.LogWarning("不存在文件");
            }

            try
            {
                string jsonString = File.ReadAllText(_savePath);
                var data = JsonUtility.FromJson<GameSaveData>(jsonString);
                //将数据恢复到原来的样子
                Manages.SceneManager.Instance.next = data.currentSceneIndex;
                GameProcessManage.Instance.playerTransform.position = data.player.playerPosition;
                GameProcessManage.Instance.playerTransform.rotation =
                    Quaternion.Euler(data.player.playerRotation);
                GameProcessManage.Instance.playerTransform.localScale = data.player.playerScale;
                GameProcessManage.Instance.playerTransform.GetComponent<PlayerAttackController>().WeaponIndex =
                    data.player.playerCurrentWeaponIndex;
                GameProcessManage.Instance.playerTransform.GetComponent<PlayerBase>().LoadHealth(
                    data.player.playerCurrentHealth);
                for (int i = 0; i < data.enemies.Count; i++)
                {
                    _enemies.transform.GetChild(i).position = data.enemies[i].enemyPosition;
                    _enemies.transform.GetChild(i).rotation = Quaternion.Euler(data.enemies[i].enemyRotation);
                    _enemies.transform.GetChild(i).localScale = data.enemies[i].enemyScale;
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.ToString());
            }

            Debug.Log("加载完成");
        }
    }
}