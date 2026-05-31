using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;

public class RandomEventManager : MonoBehaviour
{
    public static RandomEventManager Instance;
    [SerializeField] private List<RandomEventData> _possibleEvents = new();
    private Dictionary<RandomEventData, float> _eventCooldowns = new();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //更新 冷却时间
        List<RandomEventData> keys = new List<RandomEventData>(_eventCooldowns.Keys);
        foreach (var key in keys)
        {
            _eventCooldowns[key] -= Time.deltaTime;
            if (_eventCooldowns[key] <= 0)
            {
                _eventCooldowns.Remove(key);
            }
        }
    }
    public void TryTriggerRandomEvent()
    {
        foreach (var randomEvent in _possibleEvents)
        {
            if (_eventCooldowns.ContainsKey(randomEvent) && _eventCooldowns[randomEvent] > 0)
            {
                continue; //事件在冷却中
            }
            //计算触发概率
            float roll = UnityEngine.Random.Range(0f, 1f);
            //概率检查
            if (UnityEngine.Random.Range(0f, 1f) <= randomEvent.spawnChance)
            {
                TriggerEvent(randomEvent);
                //重置冷却时间
                float cooldown = UnityEngine.Random.Range(randomEvent.minCoolDown.x, randomEvent.minCoolDown.y);
                _eventCooldowns[randomEvent] = cooldown;
                break; //只触发一个事件 
            }
        }
    }
    private void TriggerEvent(RandomEventData randomEvent)
    {
        Debug.Log($"触发事件:{randomEvent.eventName}");
        Eventexecutor.Instance.ExecuteEvent(randomEvent);    
    }
}


    