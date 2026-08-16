using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

namespace Environment
{
    public class FogExploreMask : MonoBehaviour
    {
        private static readonly int ExploreRt = Shader.PropertyToID("_ExploreRt");
        [Header("RT画布设置")]
        [SerializeField] private int rtResolution = 1024; //the  size of rt
        [SerializeField] private RenderTexture renderTexture;
        [SerializeField] private MeshRenderer fogPlaneRenderer;
        [SerializeField] private Transform worldMap;
        [Header("地图范围")]
        [SerializeField] private Vector2 mapWorldMin;
        [SerializeField] private Vector2 mapWorldMax;
        [Header("视野参数")]
        [SerializeField] private float radiusView = 3f;
        [SerializeField] private float drawDistanceThreshold = 2f; //update when  distance > drawDistanceThreshold
        private Material _writeMaterial; //写入材质
        private Mesh _circleMesh;
        private CommandBuffer _cmdBuffer;
        private Vector2 _lastDrawPos;

        // Start is called before the first frame update
        void Start()
        {
            mapWorldMax = new Vector2(0.5f * worldMap.localScale.x, 0.5f * worldMap.localScale.z);
            mapWorldMin = new Vector2(-0.5f * worldMap.localScale.x, -0.5f * worldMap.localScale.z);
            CreateRT();
            CreateCircleMesh();
            InitWriteMaterial();
            _cmdBuffer = new();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateScreenPos();
        }

        void UpdateWorldMapSize()
        {
        }

        /// <summary>
        /// 初始化RT
        /// </summary>
        void CreateRT()
        {
            renderTexture = new RenderTexture(rtResolution, rtResolution, 0, GraphicsFormat.R8_UNorm);
            renderTexture.wrapMode = TextureWrapMode.Clamp;
            renderTexture.filterMode = FilterMode.Bilinear;
            //设置初始全部为黑色；
            RenderTexture.active = renderTexture;
            GL.Clear(false, true, Color.black);
            fogPlaneRenderer.material.SetTexture(ExploreRt, renderTexture);
        }

        void CreateCircleMesh()
        {
            int segment = 32; //将圆分成32份；
            Vector3[] vertices = new Vector3[segment + 1];
            int[] triangles = new int[segment * 3]; //三个顶点

            for (int i = 0; i < segment; i++)
            {
                float angle = i * Mathf.PI * 2 / segment; //计算弧度
                vertices[i + 1] = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
            }

            for (int i = 0; i < segment; i++)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = 0;
            }

            _circleMesh = new Mesh();
            _circleMesh.vertices = vertices;
            _circleMesh.triangles = triangles;
        }

        /// <summary>
        /// 初始化材质
        /// </summary>
        void InitWriteMaterial()
        {
            Shader shader = Shader.Find($"Unlit/FogWriteCircle");
            _writeMaterial = new Material(shader);
        }

        public void UpdateScreenPos() //mousepos 
        {
            _lastDrawPos =
                Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f));
            Vector2 UV = WorldPosToUV(_lastDrawPos);
            if (UV.x < 0 || UV.x > 1 || UV.y < 0 || UV.y > 1)
                return;
            Debug.Log(UV);//[-1,1]
            float worldWidth = mapWorldMax.x - mapWorldMin.x;
            float worldHeight = mapWorldMax.y - mapWorldMin.y;

            float radiusU = radiusView / worldWidth;
            float radiusV = radiusView / worldHeight;
            _cmdBuffer.Clear();
            _cmdBuffer.SetRenderTarget(renderTexture);

            //转移到裁剪空间NDC
            Vector4 ndcCenter = new(UV.x * 2f - 1f, UV.y * 2f - 1f, 0, 1);
            Matrix4x4 matrix = Matrix4x4.TRS(
                ndcCenter,
                Quaternion.identity,
                new Vector3(radiusU * 2f, radiusV * 2f, 1f)
            );
            _cmdBuffer.DrawMesh(_circleMesh, matrix, _writeMaterial, 0, 0);
            Graphics.ExecuteCommandBuffer(_cmdBuffer);
        }

        private Vector2 WorldPosToUV(Vector2 lastDrawPos)
        {
            float u = Mathf.InverseLerp(mapWorldMin.x, mapWorldMax.x, lastDrawPos.x);
            float v = Mathf.InverseLerp(mapWorldMin.y, mapWorldMax.y, lastDrawPos.y);
            return new Vector2(u, v);
        }

        private void OnDestroy()
        {
            if (renderTexture == null) Destroy(renderTexture);
            if (_writeMaterial == null) Destroy(_writeMaterial);
            if (_circleMesh == null) Destroy(_circleMesh);
            _cmdBuffer.Release();
        }
    }
}