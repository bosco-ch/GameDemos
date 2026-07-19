Shader "Hidden/DrawExploreCircle"
{
    Properties
    {
        _PlayerWorldPos("Player XY", Vector) = (0,0,0,0)
        _ViewRadius("View Radius", Float) = 1
        _SmoothRange("Circle Smooth", Float) = 1
        _MapWorldSize("Map Size", Vector) = (20,20,0,0)
        _MapWorldOrigin("Map Bottom Left", Vector) = (-10,-10,0,0)
    }
    SubShader
    {
        Tags
        {
            "QUEUE"="Overlay"
            "RenderType"="Transparent"
        }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            CBUFFER_START(CircleData)
                float2 _PlayerWorldPos;
                float _ViewRadius;
                float _SmoothRange;
                float2 _MapWorldSize;
                float2 _MapWorldOrigin;
                float4 _MainTex_ST;
            CBUFFER_END

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            struct v2f
            {
                float4 posHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(float4 posOS : POSITION, float2 uv : TEXCOORD0)
            {
                v2f o;
                o.posHCS = TransformObjectToHClip(posOS.xyz);
                o.uv = TRANSFORM_TEX(uv, _MainTex);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                // 将Blit的0~1UV换算成地图真实世界坐标
                float2 worldPos = i.uv * _MapWorldSize + _MapWorldOrigin;
                half oldMask = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv).r;
                float dist = distance(worldPos, _PlayerWorldPos);
                float circleFactor = 1 - smoothstep(_ViewRadius, _ViewRadius + _SmoothRange, dist);
                half finalMask = max(oldMask, circleFactor);
                return half4(finalMask, 0, 0, 0);
            }
            ENDHLSL
        }
    }
}