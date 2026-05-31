using System.Collections;
using Core;
using DG.Tweening;
using UnityEngine;

namespace Manages
{
    ///摄像机单例
    public class CameraManage : Singleton<CameraManage>
    {
        private Transform _target;
        [Header("设置")] [SerializeField] private float smoothTime = 0.3f;

        [SerializeField] private Vector3 offset =
            new Vector3(0, 0, -10);

        private Tweener _tweener;
        private Transform _cameraTransform;
        private Vector3 _originalCameraPosition;

        protected override void Awake()
        {
            base.Awake();
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += (_, _) =>
            {
                _cameraTransform = Camera.main?.transform;
            };
        }

        private void Start()
        {
            _cameraTransform = Camera.main?.transform;
        }

        void LateUpdate()
        {
            if (_target == null)
            {
                Debug.LogWarning("不存在值");
                return;
            }

            Vector3 targetPos = _target.position + offset;
            // targetPos.z = transform.position.z;
            _tweener?.Kill();
            //跟随
            _tweener = _cameraTransform.DOMove(targetPos, smoothTime).SetEase(Ease.OutCubic).SetUpdate(UpdateType.Late);
        }

        public void SetTarget(Transform targetTransform)
        {
            _target = targetTransform;
        }

        public void ResetCameraPos()
        {
            if (_target == null) return;
            Vector3 pos = _target.position + offset;
            pos.z = transform.position.z;
            transform.position = pos;
            _tweener?.Kill();
        }

        public bool isShake = false;

//屏幕震动
        public IEnumerator ShakeCoroutine(float shakeMagnitude, float duration)
        {
            _originalCameraPosition = _cameraTransform.position;
            isShake = true;
            float elaped = 0;
            while (elaped < duration)
            {
                elaped += Time.deltaTime;

                float x = Random.Range(-1, 1) * shakeMagnitude;
                float y = Random.Range(-1, 1) * shakeMagnitude;
                _cameraTransform.localPosition = _originalCameraPosition + new Vector3(x, y, 0);
                yield return null;
            }

            _cameraTransform.localPosition = _originalCameraPosition;
            isShake = false;
        }


        public void ShakeScreen(float shakeMagnitude, float duration)
        {
            _originalCameraPosition = _cameraTransform.position;
            isShake = true;
            _cameraTransform.DOShakePosition(duration, shakeMagnitude, 10, 90)
                .OnComplete(() =>
                {
                    // 震动结束 → 恢复位置 + 标记结束
                    _cameraTransform.localPosition = _originalCameraPosition;
                    isShake = false;
                });
        }
    }
}