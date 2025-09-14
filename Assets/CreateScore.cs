using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;
public class CreateScore : MonoBehaviour
{
    public Slider audioTimeSlider;
    public TMP_Text textMeshPro;
    public InputActions action;
    public CanvasGroup canvasGroup;
    public float CountDownNumber = 3f;

    private MusicInfo MS;
    private float startTime;
    private float endTime;
    private AudioSource audioSource;
    private List<float> bitsTimes;
    private
        float pointFirstDownTime;
    private float durationTime;
    // Start is called before the first frame update
    private bool isCountDown = false;
    private bool isDragging = false;
    private MusicScore score;
    private void Awake()
    {
        Application.targetFrameRate = 60;
        bitsTimes = new();
        MS = new MusicInfo();
        action = new();
        //score = new MusicScore();
        //slider = GetComponent<Slider>();
        action.GamePlay.Bit.started += RecordStartTime;
        action.GamePlay.Bit.canceled += RecordEndTime;
        audioSource = GetComponent
            <AudioSource>();
    }

    void Start()
    {
        audioTimeSlider.minValue = 0;
        audioTimeSlider.maxValue = Mathf.Floor(audioSource.clip.length);
        audioTimeSlider.onValueChanged.AddListener(OnSliderValueChange);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDragging)
        {
            audioTimeSlider.value = audioSource.time;
        }
        if (isCountDown && CountDownNumber > 0)
        {
            CountDownNumber -=
                Time.deltaTime;
            string countDownNumberString = Mathf.CeilToInt(CountDownNumber).ToString();
            if (countDownNumberString == "0")
            {
                textMeshPro.text = "Go!!";
                StartCoroutine(WaitforAudioEnd(audioSource.clip.length));
                PlayAudio();
            }
            else
            {
                textMeshPro.text = countDownNumberString;
            }
        }
    }

    void RecordStartTime(InputAction.CallbackContext callbackContext)
    {
        if (audioSource.isPlaying)
        {
            startTime = audioSource.time;
            bitsTimes.Add(startTime);

        }
        else
        {
            isCountDown = CheckMusicInfo();
        }
    }
    void RecordEndTime(InputAction.CallbackContext callbackContext)
    {
        score = new();
        endTime = audioSource.time;
        bitsTimes.Add(endTime);
        durationTime = endTime - startTime;
        score.durationTime = durationTime;
        score.beatTimes = startTime;
        MS.ScoreList.Add(score);
        score = null;
    }
    public void changeAudioTime(float time)
    {
        if (time <= audioSource.clip.length && time >= 0)
            audioSource.time = time;
    }
    public void OnSliderValueChange(float value)
    {
        if (!isDragging && audioSource != null && audioSource.clip != null)
        {
            audioSource.time = Mathf.Clamp(value, 0, audioSource.clip.length);
        }
    }
    public void PlayAudio()
    {
        audioSource.Play();
    }
    bool CheckMusicInfo()
    {
        var MusicID = Guid.NewGuid().ToString();
        var infos = GameObject.FindGameObjectsWithTag("MusicInfo");
        foreach (var info in infos)
        {
            var infoText = info.GetComponent<TMP_InputField>()?.text;
            if (info.name == "MusicName")
            {
                if (infoText == "")
                {
                    textMeshPro.text = "Please Enter Music Name";
                    return false;
                }
                MS.Name = infoText;
            }
            else if (info.name == "FileUrl")
            {
                if (info.GetComponent<TMP_InputField>().text == "")
                {
                    textMeshPro.text = "Please Enter File Url";
                    return false;
                }
                //MS.BPM = float.Parse(infoText);
            }
            else if (info.name == "Author")
            {
                if (info.GetComponent<TMP_InputField>().text == "")
                {
                    textMeshPro.text = "Please Enter Music BPM";
                    return false;
                }
                MS.Author = infoText;
            }
        }
        MS.ID = MusicID;
        return true;
    }
    private void OnEnable()
    {
        action.GamePlay.Bit.Enable();
    }
    private void OnDisable()
    {
        action.GamePlay.Bit.Disable();
    }
    IEnumerator WaitforAudioEnd(float audioDuration)
    {
        yield return new WaitForSeconds(audioDuration);
        Debug.Log("audio is completed");
        onCompleted();
    }
    void onCompleted()
    {
        string dataString = JsonTool.ToJson<MusicInfo>(MS);
        JsonTool.SaveFile(dataString, MS.Name + "_" + MS.ID);
    }
}
