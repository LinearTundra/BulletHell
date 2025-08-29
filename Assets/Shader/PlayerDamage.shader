Shader "Player/PlayerDamage" {

    Properties {
        _Color("test", color) = (1, 1, 1, 1)
    }

    SubShader {

        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass {

            CGPROGRAM
            #pragma vertex vert     // runs on every vertice
            #pragma fragment frag   // runs on every pixel

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            fixed4 _Color;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = _Color;
                return col;
            }
            ENDCG
        }
    }
}
