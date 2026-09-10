Shader "UnLitShader/explorePath"
{
    SubShader
    {
        ZWrite Off
        ZTest Always
        Cull Front
        Tags
        {
            "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline"
        }
        Pass
        {
            HLSLPROGRAM
            #pragma vertex  vertex
            #pragma fragment  fragment
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            CBUFFER_START(UnityPerMaterial)
                float _Radius;
                float _SmoothRadius;
                float2 _PlayerPos;
                float _Opacity; //可见程度
                float4x4 _OrthoMatrix;
            CBUFFER_END

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            struct vertexInput
            {
                float4 pos:POSITION;
                float2 UV:TEXCOORD0;
            };

            struct fragmentInput
            {
                float4 posHCS:SV_POSITION;
                float2 UV:TEXCOORD0;
                float2 WorldPos:TEXCOORD1;
            };

            fragmentInput vertex(vertexInput v)
            {
                fragmentInput input;
                float2 fixUV = v.UV;
                fixUV.y = 1 - fixUV.y;
                input.UV = fixUV;
                // input.UV = v.UV;
                float4 worldPos4 = float4(mul(unity_ObjectToWorld, v.pos).xyz, 1);
                input.WorldPos = worldPos4.xy;
                input.posHCS = mul(_OrthoMatrix, worldPos4);
                return input;
            }

            half4 fragment(fragmentInput f):SV_Target
            {
                //这边x,y连续反转我是不理解的，最多也就一个y翻转
                float oldValue
                    = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(1-f.UV.x,1-f.UV.y)).r;
                float2 diff = float2(f.WorldPos.x, f.WorldPos.y) - _PlayerPos.xy; //避开使用平方根
                // float dist = dot(diff, diff);
                // float radiusSq = _Radius * _Radius;
                // // float circle = step(dist, radiusSq);
                // float circle = 1 - smoothstep(0, radiusSq, dist);
                // float newR = max(circle, oldValue);
                // return half4(newR, 0, 0, 1);
                float dis = length(diff);
                float circle = 1 - smoothstep(0, _Radius, dis);
                return half4(max(circle, oldValue), 0, 0, 1);
            }
            ENDHLSL
        }
    }
}