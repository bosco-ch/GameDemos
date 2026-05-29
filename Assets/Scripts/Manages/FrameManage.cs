using System.Collections;
using Core;
using UnityEngine;

namespace Manages
{
    /// <summary>
    /// 帧时间设置
    /// </summary>
    public class FrameManage : Singleton<FrameManage>
    {
        [Header("时间")] private bool _isFreeze = false;
        private readonly float _freezeTime = .5f;
        [SerializeField] [Range(0, 1)] private float timeScaleDuringTime = .5f; //0 完全冻结，1极慢的动作
        private float _originalScaleTime;

        [Header("屏幕震动")] private readonly float _shakeMagnitude = .1f; //震动强度

        public void TriggerFreezeTime(float? durationTime = null, float? scale = null)
        {
            if (_isFreeze) return;
            float dur = durationTime ?? _freezeTime;
            float sca = scale ?? timeScaleDuringTime;
            StartCoroutine(FreezeTime(dur, sca));
            //这里开始震动
            if (CameraManage.Instance.isShake) return;
            // StartCoroutine(CameraManage.Instance.ShakeCoroutine(_shakeMagnitude, dur));
            CameraManage.Instance.ShakeScreen(_shakeMagnitude, dur);
        }

        private IEnumerator FreezeTime(float freezeTime, float scaleTime)
        {
            _isFreeze = true;
            _originalScaleTime = Time.timeScale;
            Time.timeScale = scaleTime;
            Time.fixedDeltaTime = 0.02f * scaleTime; //物理同步
            yield return new WaitForSecondsRealtime(freezeTime);
            //恢复
            Time.timeScale = _originalScaleTime;
            Time.fixedDeltaTime = 0.02f;
            _isFreeze = false;
        }
    }
}