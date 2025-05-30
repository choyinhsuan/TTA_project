Shader "Custom/GrayToColorEffect"
{
    Properties
    {
        _MainTex ("MainTex", 2D) = "white" {}
        _NoiseTex ("Noise", 2D) = "white" {}
        _Radius ("Radius", Range(0, 1)) = 0.5
        _Center ("Center", Vector) = (0.5, 0.5, 0, 0)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float4 _MainTex_TexelSize;
            float4 _Center;
            float _Radius;

            fixed4 frag(v2f_img i) : SV_Target
            {
                float2 uv = i.uv;
                float2 center = _Center.xy;

                // 計算從中心到此點的距離
                float dist = distance(uv, center);

                // 加上雜訊（不規則擴散）
                float noise = tex2D(_NoiseTex, uv).r;
                dist -= noise * 0.1;

                float fade = smoothstep(_Radius, _Radius - 0.1, dist);

                // 原圖像
                fixed4 col = tex2D(_MainTex, uv);

                // 灰階版本（稍微有彩度，略暗）
                float gray = dot(col.rgb, float3(0.3, 0.59, 0.11));
                fixed3 desaturated = lerp(col.rgb, float3(gray, gray, gray), 0.5); // 淡彩
                fixed3 grayColor = desaturated * 0.7; // 稍暗

                // 混合：fade = 0 是灰暗，fade = 1 是彩色
                col.rgb = lerp(grayColor, col.rgb, fade);
                return col;
            }
            ENDCG
        }
    }
}
