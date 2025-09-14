using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;

public class HitManage : MonoBehaviour
{
    public float offset = 0.5f;//补偿值
    public double HitWindow = 0.3f;//有效的击打窗口·
    public float hitTime = 0f;
    List<MusicScore> score = new();
    public bool Determination = true;
    private int index = 0;
    float startTime = 0f;
    /// <summary>
    /// 设定：当前节奏点已经判定了/远离最后判定时机也不能再判定了 就不能再判定了 防止连续按键
    /// </summary>
    private void OnEnable()
    {
        BeatManager.OnMusicStart += OnLoadMusicBeatInfo;
        BeatManager.OnBeatTimeAction += OnUnlockDetermination;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && Determination)
        {
            Determination = false;//不能再按来判定了
            float hitNowTime = (float)AudioSettings.dspTime - startTime;
            //Debug.Log($"hitNowTime:{hitNowTime},差值:{Mathf.Abs(hitNowTime - (score[index].beatTimes + offset))}，是否在窗口：{Mathf.Abs(hitNowTime - (score[index].beatTimes + offset)) <= HitWindow}");
            if (Mathf.Abs(hitNowTime - (score[index].beatTimes + offset)) <= HitWindow)
            {
                Debug.Log("ok!!");
            }
        }
    }

    void OnLoadMusicBeatInfo(List<MusicScore> score)
    {
        startTime = (float)AudioSettings.dspTime;
        this.score = score;
        Debug.Log(this.score.Count);
    }
    void OnUnlockDetermination(int index)//下一个节奏点
    {
        if (index % 2 == 0)//beat点是AI的
        {
            Determination = true;
        }
        else
        {
            this.index = index;
        }
    }
}
