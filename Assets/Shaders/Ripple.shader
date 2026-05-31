Shader "Custom/CircleOutline"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline Width", Range(0, 0.1)) = 0.02
        _FillColor ("Fill Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "Sprite"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

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
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _OutlineColor;
            float _OutlineWidth;
            float4 _FillColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 采样原始纹理
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                // 计算到中心的距离
                float2 center = float2(0.5, 0.5);
                float dist = distance(i.uv, center);
                // 判断是否在圆环范围内
                float isOutline = step(0.5 - _OutlineWidth, dist) * step(dist, 0.5);
                float isFill = step(dist, 0.5 - _OutlineWidth);
                // 混合颜色
                fixed4 finalCol = col;
                finalCol.rgb = lerp(finalCol.rgb, _OutlineColor.rgb, isOutline);
                finalCol.rgb = lerp(finalCol.rgb, _FillColor.rgb, isFill);
                finalCol.a = max(col.a, isOutline + isFill);
                return finalCol;
            }
            ENDCG
        }
    }
    FallBack "Sprites/Default"
}