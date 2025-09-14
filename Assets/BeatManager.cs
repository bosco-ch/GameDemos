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
    [Header("�������")]
    public float offset = 0f;//���ӳٽ��е���
    [Header("�ؿ����")]
    public string levelInfoFileName;//Ŀ��ؿ��ļ�
    MusicInfo musicInfo;
    AudioSource audioSource;
    List<MusicScore> musicScores;
    private int index = 0;
    [Header("Ŀ�����")]
    public Transform PlayerTransform;
    [Header("������Ϣ")]
    public float audioTime;//��ǰ���ֽ���
    public float currentTime;//
    public float beatTimeProcess;//һ�������Ľ���
    public float currentBitTime;//��ǰ����ʱ���
    //�����¼�
    public static event Action<int> OnBeatTimeAction;//����㴥���¼�
    public static event Action<List<MusicScore>> OnMusicStart;//��Ϸ��ʽ��ʼ
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
        //��������
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
        //����������������Э��
        StartCoroutine(nameof(CheckMusicEnd));
        //StartCoroutine(nameof(MusicBitTimeProcess));
        startTime = AudioSettings.dspTime;
    }

    //���ڲ��������жϸ����Ƿ񲥷����
    bool AudioFinshed()
    {
        //Ԥ��һ����������
        //const float bufferSample = 0.1f;
        //Debug.Log(audioSource.clip.length + "         " + audioSource.timeSamples);
        //return audioSource.clip.length <= audioSource.timeSamples + bufferSample;
        // �ݴ���Ƶδ���ػ򳤶�Ϊ0
        if (audioSource.clip == null || audioSource.clip.length <= 0) return true;
        //Debug.Log($"audiosourceTime{audioSource.time}, audioSource.clip.length - 0.1f{audioSource.clip.length - 0.1f}");
        // ����ʱ���ݲ0.1�룩���⸡�����
        return audioSource.time >= audioSource.clip.length - 0.1f;
    }
    //�������Ƿ����
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

    //��ʹ��update() :�о���Ӱ�����ܿ���
    IEnumerator MusicBitTimeProcess()
    {
        //float lastBeatTime = 0;
        //float nextBeatTime = 0;
        //if (index >= musicScores.Count - 1)
        //{
        //    //������Ϸ
        //}
        //currentDSP = AudioSettings.dspTime;
        ////Debug.Log((nowTime - startTime) + " " + audioSource.time);
        //double elapsedTime = (currentDSP - startTime);
        //while (index < musicScores.Count - 1 && elapsedTime >= musicScores[index].bitTimes)//ʹ��dsptime�ľ��ȹ��ߡ�����ÿһ֡��ʱ����
        //{
        //    lastBeatTime = musicScores[index].bitTimes;
        //    index++;
        //    nextBeatTime = musicScores[index].bitTimes;
        //    OnBeatTimeAction?.Invoke(index);//��һ������㣻   
        //    Debug.Log(index + " " + elapsedTime + " " + (nextBeatTime - lastBeatTime) + " " + (float)(elapsedTime - lastBeatTime) / (nextBeatTime - lastBeatTime));
        //}
        //if (index < musicScores.Count)
        //{
        //    bitTimeProcess = (float)(elapsedTime - lastBeatTime) / (nextBeatTime - lastBeatTime);
        //    bitTimeProcess = Mathf.Clamp01(bitTimeProcess);//ȷ����0��1֮��
        //}
        //else
        //{
        //    //��Ϸֹͣ
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
                OnBeatTimeAction?.Invoke(index);//��һ������㣻   
                Debug.Log(index + "_" + elapsedTime + "_" + lastBeatTime + "_" + nextBeatTime);
            }
            beatTimeProcess = ((float)elapsedTime - lastBeatTime) / (nextBeatTime - lastBeatTime);
            Debug.Log($"{beatTimeProcess}");
            yield return new WaitForSeconds(0.1f);
        }
    }
}
