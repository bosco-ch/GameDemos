using System.Collections.Generic;
using UnityEngine;

public class BeatDetector : MonoBehaviour
{
    public float BPM = 143f;//��ʼ BPMֵ
    public float thresholdMultiplier = 1.2f;
    public float thresholdDecay = 0.02f;
    public int sampleWindow = 2048;//������С��
    public int spectrumChannel = 64; //���õĵ�Ƶ����;��ǰ64��Ƶ�Σ���
    public float beatCooldown = 0.1f;//��ֹ����������ȴʱ�䣻
    public static BeatDetector Instance { get; private set; }
    private AudioSource audioSource;
    //private List<double> beatIntervals = new List<double>();//���� ���� BPM�ļ��
    private OfflineBeatDetector beatDetector = new();
    private List<float> beatTimes;

    [Header("������")]
    int x = 0;
    float secondsPerBeat;
    private void Awake()
    {
        secondsPerBeat = 60f / BPM;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        audioSource = this.GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //audioSource.PlayScheduled(AudioSettings.dspTime + 5f);
        audioSource.loop = true;
        beatTimes = new List<float>();
        if (audioSource.clip != null)
        {
            beatTimes = beatDetector.GenerateTimeLine(audioSource, secondsPerBeat);//��¼���ĵ�
        }
        else
        {
            Debug.Log("clip null");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(audioSource.time);
        if (audioSource.time > beatTimes[x])
        {
            x += 1;
        }
    }

    public float ReturnAudioTime()
    {
        return audioSource.time;
    }
    private void OnGUI()
    {
        GUI.Label(new Rect(0f, 0f, 10, 10), audioSource.time.ToString());
    }
}
