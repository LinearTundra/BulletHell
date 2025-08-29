Shader "Custom/GlitchSpriteShader2D"
{
    Properties
    {
        [PerRendererData]_MainTex ("Sprite Texture", 2D) = "white" {}
        _GlitchIntensity ("Glitch Intensity", Range(0, 5)) = 0
        _ColorShift ("Color Shift", Range(0, 1)) = 0.05
        _Speed ("Speed", Range(0, 10)) = 2.0
        _Color ("Tint", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent" 
            "RenderType"="Transparent" 
            "IgnoreProjector"="True"
            "CanUseSpriteAtlas"="True" 
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;

            float _GlitchIntensity;
            float _ColorShift;
            float _Speed;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

    
                float glitch = frac(sin(dot(uv.xy ,float2(12.9898,78.233))) * 43758.5453);
                float offset = (glitch < _GlitchIntensity) ? (_ColorShift * sin(_Time.y * _Speed)) : 0.0;

        
                float2 rUV = uv + float2(offset, 0);
                float2 gUV = uv;
                float2 bUV = uv - float2(offset, 0);


                float shake = (glitch < _GlitchIntensity) ? (sin(_Time.y * 40.0) * 0.005) : 0;
                rUV.x += shake;
                gUV.x += shake;
                bUV.x += shake;

            
                float scanline = sin(uv.y * 800.0) * 0.05;
                float4 colR = tex2D(_MainTex, rUV);
                float4 colG = tex2D(_MainTex, gUV);
                float4 colB = tex2D(_MainTex, bUV);

                float4 result = float4(colR.r, colG.g, colB.b, colG.a);

               
                float noise = frac(sin(dot(uv * _Time.y , float2(24.9898,78.233))) * 43758.5453);
                result.rgb += (noise * 0.1 * _GlitchIntensity);

               
                result.rgb -= scanline * _GlitchIntensity;

                return result;
            }
            ENDCG
        }
    }
}
