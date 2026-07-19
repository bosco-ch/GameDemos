using System.Collections.Generic;
using character.Interfaces;
using Environment;
using UnityEngine;

namespace Factories
{
    public class CollectDataTask : ITask
    {
        private readonly List<GameObject> datas;
        public CollectDataTask()
        {
            datas = new List<GameObject>();
            //找到当前场景所有要收集的数据
            var data = GameObject.FindGameObjectsWithTag($"DataCollect");
            foreach (var d in data)
            {
                datas.Add(d);
            }
        }
        
        public bool IsCompleted()
        {
            bool iscomplete = true;
            foreach (var d in datas)
            {
                iscomplete = (d.GetComponent<DataBase>().IsComplete && iscomplete);
            }

            return iscomplete;
        }
    }
}