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
        //�ж�Ŀ�����λ����Ϣ
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
            audioSource.PlayOneShot(audioSource.clip);//��Ҫ��ai���·��� ai���������һ�����ߵ� �滻һ�� �������һ�������������˶� ֱ����ʧ
            //RuleControl.Instance.PlayerIsHitSuccess = false;
            Debug.Log("notBad");
        }
        else
        {
            Debug.Log("Miss");//��Ҫ��ai���·��� ���������߷�������˶� ֱ����ʧ
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
        // ������ʾ���򣨿�200����30��
        Rect rect1 = new Rect(0, 0, 200, 30);
        Rect rect2 = new Rect(0, 30, 200, 30); // ��һ����ǩ�·���ʾ�ڶ���

        // ����������ʽ����ɫ���֣�
        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.white;

        // ��ȫ��ȡʵ��ʱ��
        float audioTime = BeatManager.Instance ? BeatManager.Instance.audioTime : -1;
        float currentTime = BeatManager.Instance ? BeatManager.Instance.currentTime : -1;

        // ��ʾ����ʶ���ı�
        GUI.Label(rect1, $"Audio: {audioTime:F3}", style);
        GUI.Label(rect2, $"Current: {currentTime:F3}", style);
    }
    //private void OnDrawGizmos()
    //{

    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawSphere(this.transform.position, notBadTime);
    //}
}
