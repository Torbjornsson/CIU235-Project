Shader "Custom/CellShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Main Color", Color) = (1, 1, 1, 1)
        [MaterialToggle] _UseColor ("Use color instead of texture", float) = 1
        _Treshold ("Cel treshold", Range(1., 20.)) = 5.
        _Ambient ("Ambient intensity", Range(0., 0.5)) = 0.1
        //_DiffuseTexture ("Diffuse Texture", 2D) = "white" {}
        //_DiffuseTint ( "Diffuse Tint", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "LightMode"="ForwardBase" }
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
           
            #include "UnityCG.cginc"
 
            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldNormal : NORMAL;
            };
 
            float _Treshold;
 
            float LightToonShading(float3 normal, float3 lightDir)
            {
                float NdotL = max(0.0, dot(normalize(normal), normalize(lightDir)));
                return floor(NdotL * _Treshold) / (_Treshold - 0.5);
            }
 
            sampler2D _MainTex;
            float4 _MainTex_ST;
 
            v2f vert (appdata_full v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.worldNormal = mul(v.normal.xyz, (float3x3) unity_WorldToObject);
                return o;
            }
 
            fixed4 _LightColor0;
            half _Ambient;
            
            fixed4 _Color;
            bool _UseColor;
 
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = _Color;
                if (!_UseColor) {
                    col = tex2D(_MainTex, i.uv);
                }
                col.rgb *= saturate(LightToonShading(i.worldNormal, _WorldSpaceLightPos0.xyz) + _Ambient) * _LightColor0.rgb;
                return col;
            }
            ENDCG
        }
        
        //Pass
        //{
        //    Tags { "RenderType"="Opaque" }
        //    LOD 200

        //    CGPROGRAM
        //    // Physically based Standard lighting model, and enable shadows on all light types
        //    #pragma surface surf Standard fullforwardshadows

        //    // Use shader model 3.0 target, to get nicer looking lighting
        //    #pragma target 3.0

        //    sampler2D _MainTex;

        //    struct Input
        //    {
        //        float2 uv_MainTex;
        //    };

        //    half _Glossiness;
        //    half _Metallic;
        //    fixed4 _Color;

        //    // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        //    // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        //    // #pragma instancing_options assumeuniformscaling
        //    UNITY_INSTANCING_BUFFER_START(Props)
        //        // put more per-instance properties here
        //    UNITY_INSTANCING_BUFFER_END(Props)

        //    void surf (Input IN, inout SurfaceOutputStandard o)
        //    {
        //        // Albedo comes from a texture tinted by color
        //        fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
        //        o.Albedo = c.rgb;
        //        // Metallic and smoothness come from slider variables
        //        o.Metallic = _Metallic;
        //        o.Smoothness = _Glossiness;
        //        o.Alpha = c.a;
        //    }
        //    ENDCG
        //}
        
        //pass
        //{       
        //    Tags { "LightMode"="ForwardBase"}

        //    CGPROGRAM

        //    #pragma target 3.0
        //    #pragma fragmentoption ARB_precision_hint_fastest

        //    #pragma vertex vertShadow
        //    #pragma fragment fragShadow
        //    #pragma multi_compile_fwdbase

        //    #include "UnityCG.cginc"
        //    #include "AutoLight.cginc"

        //    sampler2D _DiffuseTexture;
        //    float4 _DiffuseTint;
        //    float4 _LightColor0;
            
        //    float _Treshold;
        //    half _Ambient;
        //    fixed4 _Color;
        //    bool _UseColor;
            
        //    float LightToonShading(float3 normal, float3 lightDir)
        //    {
        //        float NdotL = max(0.0, dot(normalize(normal), normalize(lightDir)));
        //        return floor(NdotL * _Treshold) / (_Treshold - 0.5);
        //    }

        //    struct v2f
        //    {
        //        float4 pos : SV_POSITION;
        //        float3 lightDir : TEXCOORD0;
        //        float3 normal : TEXCOORD1;
        //        float2 uv : TEXCOORD2;
        //        LIGHTING_COORDS(3, 4)
        //    };

        //    v2f vertShadow(appdata_base v)
        //    {
        //        v2f o;

        //        o.pos = UnityObjectToClipPos(v.vertex);
        //        o.uv = v.texcoord;
        //        o.lightDir = normalize(ObjSpaceLightDir(v.vertex));
        //        o.normal = normalize(v.normal).xyz;

        //        TRANSFER_VERTEX_TO_FRAGMENT(o);

        //        return o; 
        //    }

        //    float4 fragShadow(v2f i) : COLOR
        //    {                   
        //        float3 L = normalize(i.lightDir);
        //        float3 N = normalize(i.normal);  

        //        float attenuation = LIGHT_ATTENUATION(i) * 2;
        //        float4 ambient = UNITY_LIGHTMODEL_AMBIENT * 2;

        //        float NdotL = saturate(dot(N, L));
        //        float4 diffuseTerm = NdotL * _LightColor0 * _DiffuseTint * attenuation;

        //        float4 diffuse = tex2D(_DiffuseTexture, i.uv);

        //        float4 finalColor = (ambient + diffuseTerm) * diffuse;

        //        return finalColor;
        //    }

        //    ENDCG
        //}
    }
    
    FallBack "Diffuse"
}
