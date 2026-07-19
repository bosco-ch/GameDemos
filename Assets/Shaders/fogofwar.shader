Shader "UnLitShader/FogOfWar"
{
    Properties
    {
        _PlayerPos("PlayerPos", Vector) = (0,0,0,0)
        _FogTintColor("Fog Tint Color", Color) = (1,1,1,1)
        // 1. 将默认值改为 "black"，防止编辑模式下因默认白色贴图导致全屏穿透或被黄色大圆遮挡
        _MainTex("MainTex", 2D) = "black"{}
        _UnexploredAlpha("UnexploredAlpha", Range(0,1)) = .8
        _ViewRadius("ViewRadius", Float) = 1
        _SmoothRange("SmoothRange", Float) = 1 // 羽化宽度
        _TileSize("TileSize", Float) = 1
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "RenderPipeline"="UniversalPipeline"
        }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZTest Always
        Pass
        {
            HLSLPROGRAM
            #pragma vertex  vert_Main
            #pragma fragment frag_Main
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(PerMaterial_FogOfWar)
                half4 _FogTintColor;
                float4 _MainTex_ST;
                float _UnexploredAlpha;
                float2 _PlayerPos;
                float _ViewRadius;
                float _SmoothRange;
                float _TileSize;
            CBUFFER_END

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            struct vertexInput
            {
                float4 vertex:POSITION;
                float4 texcoord:TEXCOORD0;
            };

            struct fragmentInput
            {
                float4 posHCS:SV_POSITION;
                float2 uvExplore:TEXCOORD0;
                float3 WorldPos:TEXCOORD1;
            };

            fragmentInput vert_Main(vertexInput v)
            {
                fragmentInput input;
                input.posHCS = TransformObjectToHClip(v.vertex);
                input.uvExplore = TRANSFORM_TEX(v.texcoord, _MainTex);
                input.WorldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return input;
            }

            half4 frag_Main(fragmentInput f):SV_Target
            {
                // float dis = distance(f.WorldPos.xy, _PlayerPos); // 2D游戏建议只算XY距离
                // float visionFactor = smoothstep(_ViewRadius, _ViewRadius + _SmoothRange, dis);
                // half exploreMask = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, f.uvExplore).r;
                // half maxFogAlpha = lerp(_UnexploredAlpha, 1.0, visionFactor);
                // half currentFog = visionFactor * maxFogAlpha;
                // half exploredFog = (1.0 - exploreMask) * _UnexploredAlpha;
                half4 finalFog = _FogTintColor;
                // if (exploredFog.r >= 0.9)
                // {
                //     finalFog.a = 0;
                // }
                // else
                // {
                //     finalFog.a = max(currentFog, exploredFog);
                // }
                half exploreMask = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, f.uvExplore).r;
                if (exploreMask > 0.9f)
                {
                    finalFog.a = 1 - exploreMask;
                }
                return finalFog;
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/Universal Render Pipeline/UnlitTransparent"
}