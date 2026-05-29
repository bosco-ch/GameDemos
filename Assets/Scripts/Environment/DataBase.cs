using UnityEngine;

namespace Environment
{
    public class DataBase : MonoBehaviour
    {
        public Transform player;
        public Vector3 lastPosition;
        private float Distance => Vector2.Distance(transform.position, player.position);
        private float _stopTime = 0f;
        private readonly float _maxTime = 3f;
        public bool IsComplete { get; private set; } = false;
        private Color _startColor;
        [SerializeField] private Color endColor;
        private SpriteRenderer _rg;
        private Color _currentColor;

        private void Awake()
        {
            _rg = GetComponent<SpriteRenderer>();
            _startColor = _rg.color;
        }

        private void Update()
        {
            if (Distance <= .5 && PlayerIsMove())
            {
                _stopTime += Time.deltaTime;
                _rg.color = Color.Lerp(_startColor, endColor, _stopTime / _maxTime);
                if (_stopTime >= _maxTime)
                {
                    IsComplete = true;
                }
            }
            else
            {
                if (!IsComplete)
                {
                    _rg.color = _startColor;
                    _stopTime = 0f;
                    IsComplete = false;
                }
            }

            lastPosition = player.position;
        }

        private bool PlayerIsMove()
        {
            return player.position == lastPosition;
        }
    }
}