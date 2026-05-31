Shader "Custom/RainPuddle2D_Fixed"
{
    Properties
    {
        _MainTex ("地面纹理", 2D) = "white" {}
        _NoiseTex ("噪波纹理", 2D) = "white" {}
        _PuddleTex ("积水纹理", 2D) = "white" {}
        _PuddleThreshold ("积水阈值", Range(0,1)) = 0.3
        _PuddleSmooth ("边缘平滑度", Range(0,0.2)) = 0.05
        _PuddleTiling ("噪波平铺次数", Float) = 1
        _PuddleColor ("积水颜色", Color) = (0.2,0.4,0.8,0.8)
        _PuddleAlpha ("积水透明度", Range(0,1)) = 0.8
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" "RenderPipeline"="UniversalPipeline" }
        LOD 100
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv_main : TEXCOORD0; // 地面纹理UV
                float2 uv_noise : TEXCOORD1; // 噪波纹理UV
                float2 uv_puddle : TEXCOORD2; // 积水纹理UV
                float4 vertex : SV_POSITION;
            };

            // 声明参数
            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;
            sampler2D _PuddleTex;
            float4 _PuddleTex_ST;
            float _PuddleThreshold;
            float _PuddleSmooth;
            float _PuddleTiling;
            float4 _PuddleColor;
            float _PuddleAlpha;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                // 【关键1】2D Sprite 直接用原始UV，保证地面纹理1:1显示
                o.uv_main = v.uv;
                // 噪波图UV：仅平铺，不偏移，保证和地面纹理对齐
                o.uv_noise = v.uv * _PuddleTiling;
                // 积水纹理UV：和地面纹理UV一致
                o.uv_puddle = v.uv * 2;
                return o;
            }
            

            fixed4 frag (v2f i) : SV_Target
            {
                // 1. 采样地面纹理（完全移除容错判断，保证100%采样）
                fixed4 col_ground = tex2D(_MainTex, i.uv_main);
                // 仅当完全未赋值地面纹理时，才显示灰色（极低概率触发）
                if (_MainTex_ST.x == 0 && _MainTex_ST.y == 0)
                {
                    col_ground = fixed4(0.5, 0.5, 0.5, 1.0);
                }

                // 2. 采样噪波纹理（仅取R通道，保证灰度值准确）
                fixed noise_value = tex2D(_NoiseTex, i.uv_noise).r;

                // 3. 计算积水蒙版（逻辑不变：深色=积水，浅色=陆地）
                fixed puddle_mask = smoothstep(
                    _PuddleThreshold - _PuddleSmooth,
                    _PuddleThreshold + _PuddleSmooth,
                    1 - noise_value
                );
                // 限制蒙版范围：0=纯陆地，1=纯积水
                puddle_mask = saturate(puddle_mask * _PuddleAlpha);

                // 4. 采样积水纹理（移除冗余判断，避免干扰）
                fixed4 col_puddle = _PuddleColor;
                // 只有赋值了积水纹理，才采样（通过判断纹理ST是否为默认值）
                if (_PuddleTex_ST.x != 0 && _PuddleTex_ST.y != 0)
                {
                    col_puddle = tex2D(_PuddleTex, i.uv_puddle) * _PuddleColor;
                }

                // 5. 【核心修复】混合逻辑：保证puddle_mask=0时，100%显示地面纹理
                fixed4 final_col = col_ground * (1 - puddle_mask) + col_puddle * puddle_mask;
                // 强制不透明，避免Alpha干扰
                final_col.a = 1.0;

                return final_col;
            }
            ENDCG
        }
    }
    // 回退到2D专用Shader，适配性更好
    FallBack "Sprites/Default"
}