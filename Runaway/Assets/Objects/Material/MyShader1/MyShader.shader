Shader "Custom/MyShader"    // ToonShader
{
    Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _RampTex("RampTexture", 2D) = "white" {}
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" }

        CGPROGRAM
        #pragma surface surf _CustomRamp noambient

        sampler2D _MainTex;
        sampler2D _RampTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }

        float4 Lighting_CustomRamp(SurfaceOutput o, float3 lightDir, float atten)
        {
            float fDotl = dot(o.Normal, lightDir) * 0.5 + 0.5;

            float4 fRamp_Tex = tex2D(_RampTex, float2(fDotl, 0.1));

            return fRamp_Tex;
        }

        ENDCG
    }
        FallBack "Standard"
}
