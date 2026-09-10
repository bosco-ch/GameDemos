Shader "UnLitShader/FogOfWar"
{
    Properties
    {
        _PlayerPos("PlayerPos", Vector) = (0,0,0,0)
        _FogTintColor("Fog Tint Color", Color) = (1,1,1,1)
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
        Cull Back
        ZTest Off
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
                float4 texcoord:TEXCOORD1;
                float2 uv:TEXCOORD0;
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
                input.WorldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                input.posHCS = TransformObjectToHClip(v.vertex);
                input.uvExplore = v.uv;
                return input;
            }

            half4 frag_Main(fragmentInput f):SV_Target
            {
                half4 finalFog = _FogTintColor;
                half exploreMask =
                    SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(1-f.uvExplore.x,f.uvExplore.y)).r;
                if (exploreMask > 0.9f)
                {
                    finalFog.a = 1 - exploreMask;
                }
                return finalFog;
            }
            ENDHLSL
        }
    }
}