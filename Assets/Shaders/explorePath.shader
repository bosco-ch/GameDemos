Shader "UnLitShader/explorePath"
{
    SubShader
    {
        ZWrite Off
        ZTest Always
        Cull Off
        Pass
        {
            HLSLPROGRAM
            #pragma vertex  vertex
            #pragma fragment  fragment
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            CBUFFER_START(circle)
                float _Radius;
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
                input.UV = v.UV;
                float4 worldPos4 = float4(mul(unity_ObjectToWorld, v.pos).xyz, 1.0);
                input.WorldPos = worldPos4.xy;
                input.posHCS = mul(_OrthoMatrix, worldPos4);
                // input.posHCS = mul(UNITY_MATRIX_VP, worldPos4);
                // input.posHCS = TransformWorldToHClip(worldPos4);
                return input;
            }

            half4 fragment(fragmentInput f):SV_Target
            {
                float oldValue = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, f.UV).r;
                //判断是否在uv的圆形/椭圆范围内
                float2 diff = float2(f.WorldPos.x, f.WorldPos.y) - _PlayerPos.xy;
                float dist = dot(diff, diff);
                float radiusSq = _Radius * _Radius;
                float circle = step(dist, radiusSq);
                // circle = 1 - circle;
                float newR = max(circle, oldValue);
                return half4(newR, 0, 0, 1);
                // float circle;
                // float dis = distance(f.WorldPos, float2(_PlayerPos.x, _PlayerPos.y));
                // if (dis < _Radius)
                // {
                //     circle = 1;
                // }
                // else
                // {
                //     circle = 0;
                // }
                // return half4
                //     (circle, 0, 0, 1);

                // float2 diff = float2(f.WorldPos.x, f.WorldPos.y) - _PlayerPos.xy;
                // float circle = step(dot(diff, diff), _Radius * _Radius);
                return half4(circle, 0, 0, 1);
            }

            // half4 fragment(fragmentInput f):SV_Target
            // {
            //     float2 uv = f.UV; // 或 input.UV
            //     return half4(uv.y, 0, 0, 1); // 灰度显示 V
            // }
            ENDHLSL


        }
    }
}