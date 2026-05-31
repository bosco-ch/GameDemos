using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    public InputActions action;
    public float redius = 0.25f;
    public float perfectTime = 0.05f;
    public float greatTime = 0.1f;
    public float notBadTime = 1f;
    public float coldTime = 1f;
    private AudioSource audioSource;
    private void Awake()
    {
        action = new();
    }
    private void OnEnable()
    {
        action.Enable();
    }
    private void OnDisable()
    {
        action.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        action.GamePlay.Fire.started += OnHit;
        action.GamePlay.Fire.performed += OnHit;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnHit(InputAction.CallbackContext contect)
    {
        //判断目标球的位置信息
        //float hitDistance = Vector3.Distance(targetTransform.position, this.transform.position);
        float diff = Mathf.Abs(BeatManager.Instance.audioTime - BeatManager.Instance.currentTime);
        if (diff < perfectTime)
        {
            //RuleControl.Instance.PlayerIsHitSuccess = true;
            audioSource.PlayOneShot(audioSource.clip);

            Debug.Log("perfect");
        }
        else if (diff < greatTime)
        {
            //RuleControl.Instance.PlayerIsHitSuccess = true;
            audioSource.PlayOneShot(audioSource.clip);
            Debug.Log("great");

        }
        else if (diff < notBadTime)
        {
            audioSource.PlayOneShot(audioSource.clip);//需要由ai重新发球 ai发球就是下一个疾走点 替换一下 球对象做一个贝塞尔曲线运动 直至消失
            //RuleControl.Instance.PlayerIsHitSuccess = false;
            Debug.Log("notBad");
        }
        else
        {
            Debug.Log("Miss");//需要由ai重新发球 球沿着切线方向继续运动 直至消失
            //RuleControl.Instance.PlayerIsHitSuccess = false;
        }
        StartCoroutine(nameof(HitColdTime));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(this.transform.position, redius);
    }
    IEnumerator HitColdTime()
    {
        action.GamePlay.Fire.Disable();
        yield return new WaitForSeconds(coldTime);
        action.GamePlay.Fire.Enable();
    }
    private void OnGUI()
    {
        // 增大显示区域（宽200，高30）
        Rect rect1 = new Rect(0, 0, 200, 30);
        Rect rect2 = new Rect(0, 30, 200, 30); // 第一个标签下方显示第二个

        // 创建字体样式（白色大字）
        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.white;

        // 安全获取实例时间
        float audioTime = BeatManager.Instance ? BeatManager.Instance.audioTime : -1;
        float currentTime = BeatManager.Instance ? BeatManager.Instance.currentTime : -1;

        // 显示带标识的文本
        GUI.Label(rect1, $"Audio: {audioTime:F3}", style);
        GUI.Label(rect2, $"Current: {currentTime:F3}", style);
    }
    //private void OnDrawGizmos()
    //{

    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawSphere(this.transform.position, notBadTime);
    //}
}
