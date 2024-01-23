Shader "Unlit/testShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Resolution ("Resolution", Vector) = (192, 108, 0, 0)
        _PixPerUnit ("Pixels Per Unit", float) = 64
    }
    SubShader
    {
        Tags{"Queue" = "Transparent"}

        Pass
        {
            ZTest Off
            Blend SrcAlpha OneMinusSrcAlpha

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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 screenPos : TEXCOORD1;
                float3 viewPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float2 _Resolution;
            float _PixPerUnit;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.screenPos = ComputeScreenPos(o.vertex);
                o.viewPos = UnityObjectToViewPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                // fixed4 col = tex2D(_MainTex, floor(i.uv * _Resolution ) / _Resolution);
                // return col;
                // return float4(floor(i.uv * _Resolution ) / _Resolution, 0, 1);
                // return float4(i.uv, 0, 1);
                // return float4(i.vertex.x / 1000, i.vertex.y / 1000, 0, 1);
                // return float4(floor(i.screenPos * _Resolution) / _Resolution, 0, 1);

                // return float4(i.screenPos, 0, 1);

                float3 pixelatedViewPos = float3(floor(i.viewPos.xy * _PixPerUnit) / _PixPerUnit, i.viewPos.z);
                float4 pixelatedClipPos = mul(UNITY_MATRIX_P, float4(pixelatedViewPos, 0));

                return float4(i.viewPos.xy, 0, 1);
                // return float4(pixelatedViewPos.xy, 0, 1);
                // return float4(pixelatedClipPos.xyz, 1);
                // return float4(i.vertex.xyz / 1000, 1);]
            }
            ENDCG
        }
    }
}
