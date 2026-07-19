using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace Environment
{
    /// <summary>
    /// 使用GPU来制作，主要是影响shader
    /// </summary>
    [RequireComponent(typeof(MeshRenderer))]
    public class FogOfWarGPU : MonoBehaviour
    {
        private static readonly int PlayerPos = Shader.PropertyToID("_PlayerPos");
        private static readonly int ViewRadius = Shader.PropertyToID("_ViewRadius");
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");
        private static readonly int CirclePlayerWorldPos = Shader.PropertyToID("_PlayerWorldPos");
        private static readonly int CircleViewRadius = Shader.PropertyToID("_ViewRadius");
        private static readonly int CircleSmoothRange = Shader.PropertyToID("_SmoothRange");
        private static readonly int CircleMapSize = Shader.PropertyToID("_MapWorldSize");
        private static readonly int CircleMapOrigin = Shader.PropertyToID("_MapWorldOrigin");
        private static readonly int Radius = Shader.PropertyToID("_Radius");
        private static readonly int Opecity = Shader.PropertyToID("_Opacity");
        private static readonly int OrthoMatrix = Shader.PropertyToID("_OrthoMatrix");

        [SerializeField] Transform player;

        [Header("RenderTexture About")] [SerializeField]
        RenderTexture exploreRT;

        [SerializeField] Material drawCircleMat;
        [SerializeField] float brushRadius = 1f;
        [SerializeField] float smoothRange = 2f;
        private Material _explorePathMat; //走过的路径
        [SerializeField] private float opecity = .3f;
        [Header("Map About")] [SerializeField] private Transform fogPlane;
        [SerializeField] float mapMinX, mapMaxX, mapMinY, mapMaxY;
        private Material _fogMaterial;
        private Matrix4x4 _ortho;
        private Mesh _fogMesh;
        private int count = 0;
        // [Header("Test")] [SerializeField] private List<Transform> _points;

        private void Awake()
        {
            _fogMaterial = new Material(GetComponent<MeshRenderer>().material);
            _fogMaterial.SetTexture(MainTex, exploreRT);
        }

        private void Start()
        {
            RenderTexture.active = exploreRT;
            GL.Clear(false, false, Color.black);
            RenderTexture.active = null;
            _explorePathMat = new(Shader.Find($"UnLitShader/explorePath"));
            CalculateMapSize();
            SetExplorePath();
            SetFogMaterial();
        }

        private void Update()
        {
            _fogMaterial.SetVector(PlayerPos, player.position);
            UpdatePath();
        }

        void UpdatePath()
        {
            var cmd
                = CommandBufferPool.Get("explorePathDraw" + count++);
            RenderTexture tmp =
                RenderTexture.GetTemporary(exploreRT.descriptor);
            Graphics.Blit(exploreRT, tmp);
            cmd.SetRenderTarget(tmp);
            _explorePathMat.SetTexture(MainTex, exploreRT);
            _explorePathMat.SetVector(PlayerPos, player.position);
            _explorePathMat.SetMatrix(OrthoMatrix, _ortho);
            cmd.SetViewProjectionMatrices(Matrix4x4.identity, _ortho);
            cmd.DrawMesh(_fogMesh, fogPlane.localToWorldMatrix, _explorePathMat);
            Graphics.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
            Graphics.Blit(tmp, exploreRT);
            RenderTexture.ReleaseTemporary(tmp);
        }

        void SetExplorePath()
        {
            _explorePathMat.SetFloat(Radius, brushRadius);
            _explorePathMat.SetFloat(Opecity, opecity);
            _ortho = Matrix4x4.Ortho(
                mapMinX,
                mapMaxX,
                mapMinY,
                mapMaxY,
                -100, 100); //选择一个矩形的，取景框 用来框住要渲染的东西
        }

        void SetFogMaterial()
        {
            _fogMaterial.SetFloat(CircleViewRadius, brushRadius);
            _fogMaterial.SetFloat(CircleSmoothRange, smoothRange);
        }

        private void OnDestroy()
        {
            Destroy(_fogMaterial);
        }

        void CalculateMapSize()
        {
            _fogMesh = fogPlane.GetComponent<MeshFilter>().mesh;
            var bounds = GeometryUtility.CalculateBounds(_fogMesh.vertices, fogPlane.localToWorldMatrix);
            mapMinX = bounds.min.x;
            mapMaxX = bounds.max.x;
            mapMinY = bounds.min.y;
            mapMaxY = bounds.max.y;
        }
    }
}