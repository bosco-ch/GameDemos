using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;

public class HitManage : MonoBehaviour
{
    public float offset = 0.5f;//����ֵ
    public double HitWindow = 0.3f;//��Ч�Ļ��򴰿ڡ�
    public float hitTime = 0f;
    List<MusicScore> score = new();
    public bool Determination = true;
    private int index = 0;
    float startTime = 0f;
    /// <summary>
    /// �趨����ǰ������Ѿ��ж���/Զ������ж�ʱ��Ҳ�������ж��� �Ͳ������ж��� ��ֹ��������
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
            Determination = false;//�����ٰ����ж���
            float hitNowTime = (float)AudioSettings.dspTime - startTime;
            //Debug.Log($"hitNowTime:{hitNowTime},��ֵ:{Mathf.Abs(hitNowTime - (score[index].beatTimes + offset))}���Ƿ��ڴ��ڣ�{Mathf.Abs(hitNowTime - (score[index].beatTimes + offset)) <= HitWindow}");
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
    void OnUnlockDetermination(int index)//��һ�������
    {
        if (index % 2 == 0)//beat����AI��
        {
            Determination = true;
        }
        else
        {
            this.index = index;
        }
    }
}
