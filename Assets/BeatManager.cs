using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class BeatManager : MonoBehaviour
{
    public static BeatManager Instance;
    [Header("节奏调整")]
    public float offset = 0f;//对延迟进行调整
    [Header("关卡相关")]
    public string levelInfoFileName;//目标关卡文件
    MusicInfo musicInfo;
    AudioSource audioSource;
    List<MusicScore> musicScores;
    private int index = 0;
    [Header("目标对象")]
    public Transform PlayerTransform;
    [Header("开放信息")]
    public float audioTime;//当前音乐进度
    public float currentTime;//
    public float beatTimeProcess;//一个节奏间的进度
    public float currentBitTime;//当前节奏时间点
    //开放事件
    public static event Action<int> OnBeatTimeAction;//节奏点触发事件
    public static event Action<List<MusicScore>> OnMusicStart;//游戏正式开始
    private double startTime;
    private double currentDSP;
    float lastBeatTime = 0;
    float nextBeatTime = 0;
    //public GameObject gameObject;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        musicScores = new List<MusicScore>();
        audioSource = GetComponent<AudioSource>();
        MusicGameStart();
        //不可销毁
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (index < musicScores.Count)
        {
            if (audioSource.time >= musicScores[index + 1].beatTimes)
            {
                index++;
                OnBeatTimeAction?.Invoke(index);
            }
            beatTimeProcess = (audioSource.time - musicScores[index].beatTimes) / (musicScores[index + 1].beatTimes - musicScores[index].beatTimes);
        }
    }
    void MusicGameStart()
    {
        musicInfo = JsonTool.ReadJson<MusicInfo>($"Charts/{levelInfoFileName}");
        audioSource.clip = Resources.Load<AudioClip>(musicInfo.ResFileUrl);
        OnMusicStart?.Invoke(musicInfo.ScoreList);
        audioSource.Play();
        audioSource.loop = false;
        musicScores = musicInfo.ScoreList;
        //开启检测歌曲结束的协程
        StartCoroutine(nameof(CheckMusicEnd));
        //StartCoroutine(nameof(MusicBitTimeProcess));
        startTime = AudioSettings.dspTime;
    }

    //基于采样点来判断歌曲是否播放完毕
    bool AudioFinshed()
    {
        //预留一个区间的误差
        //const float bufferSample = 0.1f;
        //Debug.Log(audioSource.clip.length + "         " + audioSource.timeSamples);
        //return audioSource.clip.length <= audioSource.timeSamples + bufferSample;
        // 容错：音频未加载或长度为0
        if (audioSource.clip == null || audioSource.clip.length <= 0) return true;
        //Debug.Log($"audiosourceTime{audioSource.time}, audioSource.clip.length - 0.1f{audioSource.clip.length - 0.1f}");
        // 考虑时间容差（0.1秒）避免浮点误差
        return audioSource.time >= audioSource.clip.length - 0.1f;
    }
    //检测歌曲是否结束
    IEnumerator CheckMusicEnd()
    {
        if (audioSource == null) yield break;
        while (!AudioFinshed())
        {
            yield return new WaitForSeconds(0.05f);
        }
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        yield return null;
    }

    //不使用update() :感觉会影响性能开销
    IEnumerator MusicBitTimeProcess()
    {
        //float lastBeatTime = 0;
        //float nextBeatTime = 0;
        //if (index >= musicScores.Count - 1)
        //{
        //    //结束游戏
        //}
        //currentDSP = AudioSettings.dspTime;
        ////Debug.Log((nowTime - startTime) + " " + audioSource.time);
        //double elapsedTime = (currentDSP - startTime);
        //while (index < musicScores.Count - 1 && elapsedTime >= musicScores[index].bitTimes)//使用dsptime的精度过高。大于每一帧的时间间隔
        //{
        //    lastBeatTime = musicScores[index].bitTimes;
        //    index++;
        //    nextBeatTime = musicScores[index].bitTimes;
        //    OnBeatTimeAction?.Invoke(index);//下一个节奏点；   
        //    Debug.Log(index + " " + elapsedTime + " " + (nextBeatTime - lastBeatTime) + " " + (float)(elapsedTime - lastBeatTime) / (nextBeatTime - lastBeatTime));
        //}
        //if (index < musicScores.Count)
        //{
        //    bitTimeProcess = (float)(elapsedTime - lastBeatTime) / (nextBeatTime - lastBeatTime);
        //    bitTimeProcess = Mathf.Clamp01(bitTimeProcess);//确保在0，1之间
        //}
        //else
        //{
        //    //游戏停止
        //}
        //double c = AudioSettings.dspTime;
        //Debug.Log(c - startTime - audioSource.time);
        while (audioSource.isPlaying && index <= musicScores.Count - 1)
        {
            currentDSP = AudioSettings.dspTime;
            double elapsedTime = (currentDSP - startTime);
            if (elapsedTime > musicScores[index].beatTimes)
            {
                lastBeatTime = musicScores[index].beatTimes;
                index++;
                nextBeatTime = musicScores[index].beatTimes;
                OnBeatTimeAction?.Invoke(index);//下一个节奏点；   
                Debug.Log(index + "_" + elapsedTime + "_" + lastBeatTime + "_" + nextBeatTime);
            }
            beatTimeProcess = ((float)elapsedTime - lastBeatTime) / (nextBeatTime - lastBeatTime);
            Debug.Log($"{beatTimeProcess}");
            yield return new WaitForSeconds(0.1f);
        }
    }
}
