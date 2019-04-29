Shader "Custom/CellShader2"
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
    
        Tags { "RenderType"="Opaque" "LightMode"="ForwardAdd" }
        //Tags { "RenderType"="Opaque" "LightMode"="ForwardBase" }
        
        pass
        {       
            Tags { "LightMode"="ForwardAdd"}
            //Tags { "LightMode"="ForwardBase"}

            CGPROGRAM

            #pragma target 3.0
            #pragma fragmentoption ARB_precision_hint_fastest

            #pragma vertex vertShadow
            #pragma fragment fragShadow
            //#pragma multi_compile_fwdbase
            #pragma multi_compile_fwdadd_fullshadows

            #include "UnityCG.cginc"
            #include "AutoLight.cginc"

            //sampler2D _DiffuseTexture;
            //float4 _DiffuseTint;
            float4 _LightColor0;
            
            float _Treshold;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            half _Ambient;
            fixed4 _Color;
            bool _UseColor;
            
            float LightToonShading(float3 normal, float3 lightDir)
            {
                float NdotL = max(0.0, dot(normalize(normal), normalize(lightDir)));
                return floor(NdotL * _Treshold) / (_Treshold - 0.5);
            }

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 lightDir : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float2 uv : TEXCOORD2;
                //float3 worldNormal : NORMAL;
                LIGHTING_COORDS(3, 4)
            };

            v2f vertShadow(appdata_base v)
            {
                v2f o;

                o.pos = UnityObjectToClipPos(v.vertex);
                //o.uv = v.texcoord;
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.lightDir = normalize(ObjSpaceLightDir(v.vertex));
                o.normal = normalize(v.normal).xyz;
                
                //o.worldNormal = mul(v.normal.xyz, (float3x3) unity_WorldToObject);

                TRANSFER_VERTEX_TO_FRAGMENT(o);

                return o; 
            }

            float4 fragShadow(v2f i) : COLOR
            {                
                // Cell shading stuff
                fixed4 col = _Color;
                if (!_UseColor) {
                    col = tex2D(_MainTex, i.uv);
                }
                //col.rgb *= saturate(LightToonShading(i.worldNormal, _WorldSpaceLightPos0.xyz) + _Ambient) * _LightColor0.rgb;
                
                // Shadow stuff   
                //float3 L = normalize(i.lightDir);
                //float3 N = normalize(i.normal);  

                float attenuation = LIGHT_ATTENUATION(i);
                //float4 ambient = UNITY_LIGHTMODEL_AMBIENT * 2;

                //float NdotL = saturate(dot(N, L));
                float NdotL = saturate(LightToonShading(i.lightDir, i.normal));
                float4 diffuseTerm = NdotL * _LightColor0 * col * attenuation;

                //float4 diffuse = tex2D(_DiffuseTexture, i.uv);
                float4 diffuse = tex2D(_MainTex, i.uv);

                //float4 finalColor = (ambient + diffuseTerm) * diffuse;
                float4 finalColor = (_Ambient + diffuseTerm) * diffuse;

                return finalColor;
                //return col;
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
}
