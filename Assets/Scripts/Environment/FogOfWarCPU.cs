using System;
using UnityEngine;
using UnityEngine.UI;

namespace Environment
{
    /// <summary>
    /// CPU计算制作战场迷雾
    /// </summary>
    public class FogOfWarCPU : MonoBehaviour
    {
        private Texture2D _texture;
        [SerializeField] private Transform _player;
        private Image _image;

        [SerializeField] private int _fogWidth;
        [SerializeField] private int _fogHeight;
        private byte[] _fogBuffer;
        private int ViewRadius { get; set; } = 60;

        private void Awake()
        {
            CreateFogTextureCPU();
        }

        private void ResetFogBuffer()
        {
            for (int i = 0; i < _fogBuffer.Length; i++)
            {
                _fogBuffer[i] = 255; //设置为全黑色；   
            }

            Debug.Log(_fogBuffer.Length);
        }

        private void ReveadFog(int centerX, int centerY)
        {
            int redis = ViewRadius;
            var radiusSqr = redis * redis;
            for (int i = -redis; i < redis; i++)
            {
                for (int j = -redis; j < redis; j++)
                {
                    //圆形半径的区域
                    if (i * i + j * j < radiusSqr)
                    {
                        int px = centerX + i;
                        int py = centerY + j;
                        if (px >= 0 && px < _fogWidth && py >= 0 && py < _fogHeight)
                        {
                            _fogBuffer[py * _fogWidth + px] = 0;
                        }
                    }
                }
            }
        }

        private void CreateFogTextureCPU()
        {
            _fogBuffer = new byte[_fogWidth * _fogHeight];
            ResetFogBuffer();
            Color32[] colors = new Color32[_fogBuffer.Length];
            ReveadFog(200, 300);
            for (int i = 0; i < _fogBuffer.Length; i++)
            {
                colors[i] = new Color32(0, 0, 0, _fogBuffer[i]);
            }

            _texture = new Texture2D(_fogWidth, _fogHeight)
            {
                filterMode = FilterMode.Point
            };
            _texture.SetPixels32(colors);
            _texture.Apply();
            // GetComponent<SpriteRenderer>().sprite = Sprite.Create(
            //     _texture,
            //     new Rect(0, 0, _texture.width, _texture.height),
            //     new Vector2(0.5f, 0.5f)
            // );
            transform.position = new Vector3(0, 0, -1);
        }
    }
}