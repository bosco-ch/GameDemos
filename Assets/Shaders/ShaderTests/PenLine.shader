Shader "Custom/PenLine"
{
    Properties
    {
        _ColorTint("线条颜色", Color) = (1,1,1,1)
        _MainTex("贴图",2D) = "white"{}
    }
    SubShader
    {
        ZWrite Off
        Cull Off
        ZTest Always
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            CBUFFER_START(DrawParams)
                half4 _ColorTint;
                float4 _Center;
                float _RadiusX;
                float _RadiusZ;
                float _Softness;
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
                Vary output;
                output.posHCS = TransformObjectToHClip(input.pos); //从物体空间转换到了裁剪空间
                output.UV = input.UV;
                return output;
            }
            half4 frag(Vary input) : SV_Target
            {
                float2 uv = input.UV;
                float oldValue = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv).r;
                float2 center = _Center.xy;
                float2 diff = uv - center;

                float dist =
                    (diff.x * diff.x) / (_RadiusX * _RadiusX) +
                    (diff.y * diff.y) / (_RadiusZ * _RadiusZ);
                // float circle = smoothstep(0.0, 1.0, dist);
                float circle = 0;
                if (dist < 1)
                {
                    circle = 1;
                }
                float result = max(oldValue, circle);
                return half4(result, 0, 0, 1);
            }
            ENDHLSL
        }
    }
}