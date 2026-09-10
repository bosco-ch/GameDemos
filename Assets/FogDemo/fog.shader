Shader "Custom/fog"
{
    Properties
    {
        _ColorInt("迷雾颜色",Color) = (0,0,0,0)
    }
    SubShader
    {
        ZWrite Off
        ZTest Off
        Cull Off
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            CBUFFER_START(fog)
                float _BrushRadius;
                float4x4 _OrthoGraphic;
                float3 _PlayerPos;
                half4 _ColorInt;
            CBUFFER_END

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 WorldPos :TEXCOORD1;
            };

            v2f vert(appdata v)
            {
                v2f o;
                float4 worldPos4 = mul(unity_ObjectToWorld, v.vertex);
                // float4 worldPos4 = v.vertex;
                o.WorldPos = float3(worldPos4.x, worldPos4.y, 0);
                o.vertex = mul(_OrthoGraphic, float4(o.WorldPos, 1.0));
                float2 uvFix = v.uv;
                 uvFix.x = 1 - uvFix.x;
                o.uv = uvFix;
                return o;

                // v2f o;
                // // v.vertex：已经被 DrawMesh 的 localToWorldMatrix 变换完毕 = 世界坐标！
                // float3 worldPos = v.vertex.xyz;
                // o.WorldPos = float3(worldPos.x, worldPos.y, 0);
                // // 直接送入正交投影，固定Z=0规避纵深裁剪
                // o.vertex = mul(_OrthoGraphic, float4(o.WorldPos.xy, 0, 1.0));
                // o.uv = v.uv;
                // return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                float oldValue = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv).r;
                float dis = distance(i.WorldPos, _PlayerPos);
                float a = 0;
                if (dis < _BrushRadius)
                {
                    a = 1;
                }
                _ColorInt.r = max(oldValue, a);
                // _ColorInt.r = a;
                return _ColorInt;
                // return half4(.5, 0, 0, 1);
            }
            ENDHLSL
        }
    }
}