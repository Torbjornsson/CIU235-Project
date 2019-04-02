// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/TestShader"
{
    //Properties
    //{
    //    _MainTex ("Texture", 2D) = "white" {}
    //}
    //SubShader
    //{
    //    Tags { "RenderType"="Opaque" }
    //    LOD 100

    //    Pass
    //    {
    //        CGPROGRAM
    //        #pragma vertex vert
    //        #pragma fragment frag
    //        // make fog work
    //        #pragma multi_compile_fog

    //        #include "UnityCG.cginc"

    //        struct appdata
    //        {
    //            float4 vertex : POSITION;
    //            float2 uv : TEXCOORD0;
    //        };

    //        struct v2f
    //        {
    //            float2 uv : TEXCOORD0;
    //            UNITY_FOG_COORDS(1)
    //            float4 vertex : SV_POSITION;
    //        };

    //        sampler2D _MainTex;
    //        float4 _MainTex_ST;

    //        v2f vert (appdata v)
    //        {
    //            v2f o;
    //            o.vertex = UnityObjectToClipPos(v.vertex);
    //            o.uv = TRANSFORM_TEX(v.uv, _MainTex);
    //            UNITY_TRANSFER_FOG(o,o.vertex);
    //            return o;
    //        }

    //        fixed4 frag (v2f i) : SV_Target
    //        {
    //            // sample the texture
    //            fixed4 col = tex2D(_MainTex, i.uv);
    //            // apply fog
    //            UNITY_APPLY_FOG(i.fogCoord, col);
    //            return col;
    //        }
    //        ENDCG
    //    }
    //}
    
    //Properties {
    //    _Color ("Main Color", Color) = (1,0.5,0.5,1)
    //    _ColorCount ("Color Count", int) = 32
    //}
    //SubShader {
    //    Pass {
    //        ////Color c = Color(1, 1, 1, 1)
    //        //_Color.r = 0
            
    //        //Material {
    //        //    Diffuse [c]
    //        //}
    //        //Lighting On
            
    //        CGPROGRAM
    //        #pragma vertex vert
    //        #pragma fragment frag
    //        #include "UnityCG.cginc"
            
    //        struct v2f {
    //            float4 pos : SV_POSITION;
    //            fixed3 color : COLOR0;
    //        };

    //        v2f vert (appdata_base v)
    //        {
    //            v2f o;
    //            o.pos = UnityObjectToClipPos(v.vertex);
    //            o.color = v.normal * 0.5 + 0.5;
    //            return o;
    //        }
            
    //        fixed4 frag (v2f i) : SV_Target
    //        {
    //            return fixed4 (i.color, 1);
    //            //fixed3 c = (_Color);
    //            //return fixed4 (c, 1);
    //        }
            
    //        ENDCG
    //    }
    //}
    
    //Properties
    //{
    //    // we have removed support for texture tiling/offset,
    //    // so make them not be displayed in material inspector
    //    [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
    //}
    //SubShader
    //{
    //    Pass
    //    {
    //        CGPROGRAM
    //        // use "vert" function as the vertex shader
    //        #pragma vertex vert
    //        // use "frag" function as the pixel (fragment) shader
    //        #pragma fragment frag

    //        // vertex shader inputs
    //        struct appdata
    //        {
    //            float4 vertex : POSITION; // vertex position
    //            float2 uv : TEXCOORD0; // texture coordinate
    //        };

    //        // vertex shader outputs ("vertex to fragment")
    //        struct v2f
    //        {
    //            float2 uv : TEXCOORD0; // texture coordinate
    //            float4 vertex : SV_POSITION; // clip space position
    //        };

    //        // vertex shader
    //        v2f vert (appdata v)
    //        {
    //            v2f o;
    //            // transform position to clip space
    //            // (multiply with model*view*projection matrix)
    //            o.vertex = UnityObjectToClipPos(v.vertex);
    //            // just pass the texture coordinate
    //            o.uv = v.uv;
    //            return o;
    //        }
            
    //        // texture we will sample
    //        sampler2D _MainTex;

    //        // pixel shader; returns low precision ("fixed4" type)
    //        // color ("SV_Target" semantic)
    //        fixed4 frag (v2f i) : SV_Target
    //        {
    //            // sample texture and return it
    //            fixed4 col = tex2D(_MainTex, i.uv);
    //            return col;
    //        }
    //        ENDCG
    //    }
    //}
    
    //Properties {
    //    _Color ("Main Color", Color) = (1,1,1,0.5)
    //    _MainTex ("Texture", 2D) = "white" { }
    //}
    //SubShader {
    //    Pass {

    //    CGPROGRAM
    //    #pragma vertex vert
    //    #pragma fragment frag

    //    #include "UnityCG.cginc"

    //    fixed4 _Color;
    //    sampler2D _MainTex;

    //    struct v2f {
    //        float4 pos : SV_POSITION;
    //        float2 uv : TEXCOORD0;
    //    };

    //    float4 _MainTex_ST;

    //    v2f vert (appdata_base v)
    //    {
    //        v2f o;
    //        o.pos = UnityObjectToClipPos(v.vertex);
    //        o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
    //        return o;
    //    }

    //    fixed4 frag (v2f i) : SV_Target
    //    {
    //        //fixed4 texcol = tex2D (_MainTex, i.uv);
    //        //return texcol * _Color;
    //        return _Color;
    //    }
    //    ENDCG

    //    }
    //}
    
    // after CGPROGRAM;
    //#include "AutoLight.cginc"
     
    //// in v2f struct;
    //LIGHTING_COORDS(0,1)
    // replace 0 and 1 with the next available TEXCOORDs in your shader, don't
    // put a semicolon at the end of this line.
     
    //// in vert shader;
    //TRANSFER_VERTEX_TO_FRAGMENT(o);
    // Calculates shadow and light attenuation and passes it to the frag shader.
     
    ////in frag shader;
    //float atten = LIGHT_ATTENUATION(i);
    // This is a float for your shadow/attenuation value, multiply your lighting
    // value by this to get shadows. Replace i with whatever you've defined your
    // input struct to be called (e.g. frag(v2f [b]i[/b]) : COLOR { ... ).
     
    
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Main Color", Color) = (1, 1, 1, 1)
        [MaterialToggle] _UseColor ("Use color instead of texture", float) = 1
        _Treshold ("Cel treshold", Range(1., 20.)) = 5.
        _Ambient ("Ambient intensity", Range(0., 0.5)) = 0.1
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
    
        //Pass {
 
        //    Tags {"LightMode" = "ForwardBase"}
 
        //    CGPROGRAM
 
        //        #pragma multi_compile_fwdbase
        //        #pragma vertex vert
        //        #pragma fragment frag
        //        #pragma fragmentoption ARB_precision_hint_fastest
        //        #include "UnityCG.cginc"
        //        #include "AutoLight.cginc"
 
        //        struct Input
        //        {
        //            float4 pos : SV_POSITION;
        //            float3 vlight : COLOR;
        //            float3 lightDir : TEXCOORD1;
        //            float3 vNormal : TEXCOORD2;
        //            LIGHTING_COORDS(3,4)
 
        //        };
 
        //        Input vert(appdata_full v)
        //        {
        //            Input o;
        //            o.pos = UnityObjectToClipPos(v.vertex);
 
        //            // Calc normal and light dir.
        //            o.lightDir = normalize(ObjSpaceLightDir(v.vertex));
        //            o.vNormal = normalize(v.normal).xyz;
 
        //            // Calc spherical harmonics and vertex lights. Ripped from compiled surface shader.
        //            float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
        //            float3 worldNormal = mul((float3x3)unity_ObjectToWorld, SCALED_NORMAL);
        //            o.vlight = float3(0, 0, 0);
 
        //            #ifdef LIGHTMAP_OFF
 
        //                float3 shlight = ShadeSH9(float4(worldNormal, 1.0));
        //                o.vlight = shlight;
 
        //                #ifdef VERTEXLIGHT_ON
 
        //                    o.vlight += Shade4PointLights (
        //                        unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
        //                        unity_LightColor[0].rgb, unity_LightColor[1].rgb, unity_LightColor[2].rgb, unity_LightColor[3].rgb,
        //                        unity_4LightAtten0, worldPos, worldNormal
        //                    );
 
        //                #endif // VERTEXLIGHT_ON
        //            #endif // LIGHTMAP_OFF
 
        //            TRANSFER_VERTEX_TO_FRAGMENT(o);
 
        //            return o;
        //        }
 
        //        float4 _LightColor0; // Contains the light color for this pass.
 
        //        half4 frag(Input IN) : COLOR
        //        {
        //            IN.lightDir = normalize ( IN.lightDir );
        //            IN.vNormal = normalize ( IN.vNormal );
 
        //            float atten = LIGHT_ATTENUATION(IN);
        //            float3 color;
        //            float NdotL = saturate( dot (IN.vNormal, IN.lightDir ));
 
        //            color = UNITY_LIGHTMODEL_AMBIENT.rgb * 2;
        //            color += IN.vlight;
        //            color += _LightColor0.rgb * NdotL * ( atten * 2);
                    
        //            return half4(color, 1.0f);
        //        }
                
        //    ENDCG
 
        //}
 
 
 
        // Take this pass out if you don't want extra per-pixel lights.
        // Just set the other lights' Render Mode to "Not Important",
        // and they will be calculated as Spherical Harmonics or Vertex Lights in the above pass instead.
 
        //Pass {
            //Tags {"LightMode" = "ForwardAdd"}
 
            //CGPROGRAM
 
            //    #pragma multi_compile_fwdadd
            //    #pragma vertex vert
            //    #pragma fragment frag
            //    #pragma fragmentoption ARB_precision_hint_fastest
            //    #include "UnityCG.cginc"
            //    #include "AutoLight.cginc"
 
            //    struct Input
            //    {
            //        float4 pos : SV_POSITION;
            //        float3 lightDir : TEXCOORD1;
            //        float3 vNormal : TEXCOORD2;
            //    };
 
            //    Input vert(appdata_full v)
            //    {
            //        Input o;
            //        o.pos = UnityObjectToClipPos(v.vertex);
 
            //        // Calc normal and light dir.
            //        o.lightDir = normalize(ObjSpaceLightDir(v.vertex));
            //        o.vNormal = normalize(v.normal).xyz;
 
            //        // Don't need any ambient or vertex lights in here as this is just an additive pass for each extra light.
            //        // Shadows won't work here, either.
                    
            //        return o;
            //    }
 
            //    float4 _LightColor0; // Contains the light color for this pass.
 
            //    half4 frag(Input IN) : COLOR
            //    {
            //        IN.lightDir = normalize ( IN.lightDir );
            //        IN.vNormal = normalize ( IN.vNormal );
 
            //        float3 color;
            //        float NdotL = saturate( dot (IN.vNormal, IN.lightDir ));
            //        color = _LightColor0.rgb * NdotL;
 
            //        return half4(color, 1.0f);
 
            //    }
 
            //    ENDCG
     
            //}
     
        }
        
    FallBack "Diffuse"
    //FallBack "VertexLit"
    
}
