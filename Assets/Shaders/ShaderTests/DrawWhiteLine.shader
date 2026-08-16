Shader "Custom/DrawWhiteLine"
{
    Properties
    {
        _ColorTint("_ColorTint",Color) = (1,1,1,1)
        _MainTex("_MainTex",2D) = "white"{}
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent" "RenderType"="Transparent"
        }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(DrawWhite)
                half4 _ColorTint;
                float4 _MainTex_ST;
            CBUFFER_END

            struct Attr
            {
                float4 pos : POSITION;
                float2 UV:TEXCOORD0;
            };

            struct Vary
            {
                float4 posHCS : SV_POSITION;
                float2 UV:TEXCOORD0;
            };

            Vary vert(Attr input)
            {
                Vary o;
                o.posHCS = TransformObjectToHClip(input.pos);
                o.UV = TRANSFORM_TEX(input.UV, _MainTex);
                return o;
            }

            half4 frag(Vary input) : SV_Target
            {
                // half4 canvas = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.UV);
                // return canvas * _ColorTint;
                // half exploreMask = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.UV).r;
                // return half4(_ColorTint.r, _ColorTint.g, _ColorTint.b, 1);
                half exploreMask = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.UV).r;
                return half4(1, 0, 0, 1 - exploreMask);
            }
            ENDHLSL
        }
    }
}