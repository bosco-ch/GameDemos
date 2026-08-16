using UnityEngine;

namespace test
{
    public class RenderTexTest : MonoBehaviour
    {
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");

        [Header("画布")]
        public RenderTexture rt;
        [Header("画笔材质（PenLine Shader）")]
        public Material drawMat;
        [Header("面片显示材质（DrawWhiteLine Shader）")]
        public Material displayMaterial;
        [Header("主相机")]
        public Camera mainCam;
        private Vector2 _lastMouseScreenPos;
        private bool _isMouseDown;

        void Start()
        {
            if (rt == null || displayMaterial == null || mainCam == null)
            {
                Debug.LogError("参数缺失：RT / displayMaterial / mainCam 不能为空！", this);
                enabled = false;
                return;
            }

            displayMaterial.SetTexture(MainTex, rt);
            // 修复GL.Clear参数顺序 (clearDepth, clearColor, color)
            RenderTexture prevActive = RenderTexture.active;
            RenderTexture.active = rt;
            GL.Clear(false, true, Color.white);
            RenderTexture.active = prevActive;
        }

        void Update()
        {
            if (!enabled || rt == null || drawMat == null || mainCam == null)
            {
                Debug.Log("缺少");
                return;
                
            }
            Vector2 mouseScreenPos = Input.mousePosition;

            if (Input.GetMouseButtonDown(0))
            {
                _isMouseDown = true;
                _lastMouseScreenPos = mouseScreenPos;
            }

            if (Input.GetMouseButton(0) && _isMouseDown)
            {
                DrawLineOnRT(_lastMouseScreenPos, mouseScreenPos);
                _lastMouseScreenPos = mouseScreenPos;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isMouseDown = false;
            }
        }

        /// <summary>
        /// 屏幕坐标线段绘制到RenderTexture
        /// </summary>
        void DrawLineOnRT(Vector2 screenStart, Vector2 screenEnd)
        {
            RenderTexture prevActive = RenderTexture.active;
            RenderTexture.active = rt;

            drawMat.SetPass(0);
            GL.Begin(GL.LINES);

            // Screen [0,0]左下 → [ScreenWidth,ScreenHeight]右上
            // 映射到 RT NDC [-1,-1]左下 ~ [1,1]右上
            Vector2 ndcA = ScreenToNDC(screenStart);
            Vector2 ndcB = ScreenToNDC(screenEnd);

            GL.Vertex(new Vector3(ndcA.x, ndcA.y, 0));
            GL.Vertex(new Vector3(ndcB.x, ndcB.y, 0));

            GL.End();
            RenderTexture.active = prevActive;
        }

        // 屏幕坐标 → RT画布NDC [-1,1]
        Vector2 ScreenToNDC(Vector2 screenPos)
        {
            Vector2 viewportPos;
            viewportPos.x = screenPos.x / Screen.width;
            viewportPos.y = screenPos.y / Screen.height;

            Vector2 ndc;
            ndc.x = viewportPos.x * 2f - 1f;
            ndc.y = viewportPos.y * 2f - 1f;
            return ndc;
        }
    }
}