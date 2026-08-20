    using System;
    using UnityEngine;
    using UnityEngine.Rendering;

    namespace FogDemo
    {
        public class Fog : MonoBehaviour
        {
            [Header("Render About")] [SerializeField]
            private RenderTexture exploreRT;

            private Mesh _planeMesh;
            [SerializeField] private float _minx, _maxx, _miny, _maxy;
            [Header("Fog About")] [SerializeField] float brushRadius = 3;
            private Material _fogMaterial;

            [Header("Player About")] [SerializeField]
            private Transform playerPos;

            [SerializeField] private float movespeed = 1;

            private Matrix4x4 _orthoGraphic;

            private void Awake()
            {
                CalcularSize();
                CalculatarBounds();
            }

            // Start is called before the first frame update
            void Start()
            {
                CreateMaterial();
                SetMaterialParma();

                // RenderTexture.active = exploreRT;
                // GL.Clear(false, false, Color.black, 0);
                // RenderTexture.active = null;
            }

            // Update is called once per frame
            void Update()
            {
                PlayerMove();
                DrawRT();
            }

            /// <summary>
            /// 绘制RT
            /// </summary>
            void DrawRT()
            {
                CommandBuffer cmd = CommandBufferPool.Get("drawFog");
                RenderTexture tmp = RenderTexture.GetTemporary(exploreRT.descriptor);
                // Graphics.Blit(exploreRT, tmp);
                cmd.SetRenderTarget(tmp);
                cmd.ClearRenderTarget(true, false, Color.black);
                _fogMaterial.SetTexture("_MainTex", exploreRT);//读取一下上一个存的RT的值
                _fogMaterial.SetVector("_PlayerPos", playerPos.position);
                cmd.DrawMesh(_planeMesh, this.transform.localToWorldMatrix, _fogMaterial);
                Graphics.ExecuteCommandBuffer(cmd);
                Graphics.Blit(tmp, exploreRT);
                CommandBufferPool.Release(cmd);
                RenderTexture.ReleaseTemporary(tmp);
                // CommandBuffer cmd = CommandBufferPool.Get("drawFog");
                // // RenderTexture tmp = RenderTexture.GetTemporary(exploreRT.descriptor);
                // // Graphics.Blit(exploreRT, tmp);
                // cmd.SetRenderTarget(exploreRT);
                // cmd.ClearRenderTarget(false, true, Color.black);
                // _fogMaterial.SetTexture("_MainTex", exploreRT);
                // _fogMaterial.SetVector("_PlayerPos", playerPos.position);
                // cmd.DrawMesh(_planeMesh, this.transform.localToWorldMatrix, _fogMaterial);
                // Graphics.ExecuteCommandBuffer(cmd);
                // // Graphics.Blit(tmp, exploreRT);
                // CommandBufferPool.Release(cmd);
                // // RenderTexture.ReleaseTemporary(exploreRT);
            }

            void CreateMaterial()
            {
                _fogMaterial = new Material(Shader.Find($"Custom/fog"));
            }

            void SetMaterialParma()
            {
                _fogMaterial.SetFloat("_BrushRadius", brushRadius);
                _fogMaterial.SetMatrix("_OrthoGraphic", _orthoGraphic); //正交变化矩阵
            }

            ///计算边界
            void CalcularSize()
            {
                _planeMesh = this.GetComponent<MeshFilter>().mesh;
                var bounds = GeometryUtility.CalculateBounds(_planeMesh.vertices, this.transform.localToWorldMatrix);
                // var bounds = GeometryUtility.CalculateBounds(_planeMesh.vertices, Matrix4x4.identity);
                _minx = bounds.min.x;
                _maxx = bounds.max.x;
                _miny = bounds.min.y;
                _maxy = bounds.max.y;
                // // _miny = bounds.max.y;
                // Debug.Log($"bounds X:{_minx}~{_maxx} Y:{_miny}~{_maxy}");
                // Debug.Log($"plane pos:{transform.position} scale:{transform.lossyScale} rot:{transform.eulerAngles}");
            }

            ///框起来 将其映射到 UV坐标上去
            void CalculatarBounds()
            {
                _orthoGraphic = Matrix4x4.Ortho(
                    _minx,
                    _maxx,
                    _miny,
                    _maxy,
                    -100, 100
                );  
                // _orthoGraphic = GL.GetGPUProjectionMatrix(ortho, true);
                // _orthoGraphic = Matrix4x4.Ortho(
                //     -10,
                //     10,
                //     -10,
                //     10,
                //     -100, 100
                // );
            }

            void PlayerMove()
            {
                if (Input.GetKey(KeyCode.W))
                {
                    Move(Vector3.up);
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    Move(Vector3.left);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    Move(Vector3.down);
                }

                else if (Input.GetKey(KeyCode.D))
                {
                    Move(Vector3.right);
                }
            }

            void Move(Vector3 dir)
            {
                playerPos.position += dir * (movespeed * Time.deltaTime);
            }
        }
    }