using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.Events;
public class BallRun : MonoBehaviour
{
    public bool direction { get; private set; }
    CreatLine creatLine;
    public Vector3[] _points = new Vector3[4];
    private int index = 0;
    public float beatTimeProcess;
    private List<MusicScore> musicScores;
    public Transform playerTransform;
    public Transform AITransform;
    public Transform ParentTransform => this.transform.parent;
    bool startt = false;

    private void OnEnable()
    {
        BeatManager.OnBeatTimeAction += OnChangeDirection;
        BeatManager.OnMusicStart += OnLoadMusicBeatInfo;
    }
    void OnDisable() => BeatManager.OnBeatTimeAction -= OnChangeDirection;

    private void Start()
    {
        creatLine = new CreatLine();
        _points[0] = playerTransform.position;
        _points[3] = AITransform.position;
    }
    private void Awake()
    {
#if UNITY_IOS || UNITY_ANDROID// 移动端
        Application.targetFrameRate = 45;
#else
        Application.targetFrameRate = 60;
#endif
    }
    
    private void Update()
    {
        if (startt)
        {
            string linekey = musicScores[index].flyLevel;
            Vector3[] lintpoints = CreatLine.line[linekey];
            if (direction)
            {
                _points[1] = ParentTransform.TransformPoint(lintpoints[0]);
                _points[2] = ParentTransform.TransformPoint(lintpoints[1]);
                _points[3] = ParentTransform.TransformPoint(lintpoints[2]);
                beatTimeProcess = BeatManager.Instance.beatTimeProcess;
                StartCoroutine(MoveCoroutine(musicScores[index + 1].beatTimes - musicScores[index].beatTimes));
            }
            else
                beatTimeProcess = 1 - BeatManager.Instance.beatTimeProcess;
            this.transform.position = creatLine.CubicBezier(beatTimeProcess, _points);
            //使y轴沿着切线方向
            Vector3 tangent = creatLine.GetFirstDerivative(beatTimeProcess, _points).normalized;
            if (tangent != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(tangent);
                targetRotation *= tangent.y >= 0 ? Quaternion.Euler(90, 0, 180) : Quaternion.Euler(90, 0, 0);
                this.transform.rotation = targetRotation;
            }
        }
    }
    //控制ai飞机
    IEnumerator MoveCoroutine(float moveDuration)
    {
        Vector3 start = AIPlayerController.Instance.transform.localPosition;
        Vector3 end = CreatLine.line[musicScores[index].flyLevel][2];
        if (Vector3.Distance(start, end) < 0.1f)
        {
            AIPlayerController.Instance.transform.localPosition = end;
            yield return null;
        }
        float elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;
            t = Mathf.SmoothStep(0f, 1f, t);
            AIPlayerController.Instance.transform.localPosition = Vector3.Lerp(start, end, t);
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        AIPlayerController.Instance.transform.localPosition = end;
    }
    void OnChangeDirection(int index)
    {
        this.index = index;
        beatTimeProcess = direction ? 0 : 1;//强制对齐
        direction = !direction;
        //ai先
    }
    void OnLoadMusicBeatInfo(List<MusicScore> musicScores)
    {
        startt = true;
        this.musicScores = musicScores;
    }
    //private void OnDrawGizmos()//可视化
    //{
    //    creatLine = new CreatLine();
    //    for (int i = 0; i < 4; i++)
    //    {
    //        _points[i] = transforms[i].position;
    //    }
    //    Vector3[] drawLinePoints = new Vector3[segment + 1];
    //    for (int i = 0; i <= segment && _points.Length > 0; i++)
    //    {
    //        float t = (float)i / segment;
    //        drawLinePoints[i] = creatLine.CubicBezier(t, _points);
    //    }
    //    Gizmos.color = Color.yellow;
    //    for (int i = 0; i < segment; i++)
    //    {
    //        Gizmos.DrawLine(drawLinePoints[i], drawLinePoints[i + 1]);
    //    }
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(_points[0], _points[1]);
    //    Gizmos.DrawLine(_points[2], _points[3]);
    //}
}
