    Shader "Custom/Skin Shader" {
        Properties {
            _Color ("Main Color", Color) = (1,1,1,1)
            _MainTex ("Diffuse (RGB) Alpha (A)", 2D) = "white" {}
            _SpecularTex ("Specular (R) Gloss (G) SSS Mask (B)", 2D) = "gray" {}
            _BumpMap ("Normal (Normal)", 2D) = "bump" {}
            _AnisoDirection ("Anisotropic Direction (RGB) Anisotropic Mask (A)", 2D) = "bump" {}
            _AnisoOffset ("Anisotropic Highlight Offset", Range(-1,1)) = -0.2
            _BRDFTex ("BRDF Lookup (RGB)", 2D) = "gray" {}
            _CurvatureScale ("Curvature Scale (metres)", Float) = 0.01
            _Cutoff ("Alpha Cut-Off Threshold", Range(0,1)) = 0.5
            _Fresnel ("Fresnel Value", Float) = 0.28
            _BumpBias ("Normal Map Blur Bias", Float) = 2.0
            _Gamma ("Gamma", Float) = 2.2
        }
     
        SubShader{
            Tags { "Queue" = "Geometry" "RenderType" = "TransparentCutout" }
     
            CGPROGRAM
     
                #pragma surface surf SkinShader fullforwardshadows exclude_path:prepass
                #pragma target 3.0
                #pragma only_renderers d3d9
     
                struct SurfaceOutputSkinShader {
                    fixed3 Albedo;
                    fixed3 Normal;
                    fixed4 AnisoDir;
                    fixed3 Emission;
                    fixed3 Specular;
                    fixed Alpha;
                    float Curvature;
                };
     
                struct Input
                {
                    float2 uv_MainTex;
                    float3 worldPos;
                    float3 worldNormal;
                    INTERNAL_DATA
                };
     
                sampler2D _MainTex, _SpecularTex, _BumpMap, _AnisoDirection, _CurvatureMap, _BRDFTex;
                float _BumpBias, _CurvatureScale, _Cutoff, _AnisoOffset, _Fresnel, _Gamma;
     
                inline float CalcFresnel (float3 viewDir, float3 h, float fresnelValue)
                {
                    float fresnel = pow(1.0 - dot(viewDir, h), 5.0);
                    fresnel += fresnelValue * (1.0 - fresnel);
                    return fresnel;
                }
     
                inline float CalcAniso (float3 normal, float3 anisoDir, float3 h, float offset)
                {
                    return max(0, sin(radians( (dot(normalize(normal + anisoDir), h) + offset) * 180 ) ));
                }
     
                inline float CalcSpec ( float spec, float specLevel, float gloss, float fresnel )
                {
                    return pow(spec, gloss * 128) * specLevel * fresnel;
                }
     
                inline float3 gamma ( float3 color, float gamma ) {
                    return pow ( color, gamma );
                }
     
                inline float3 ungamma ( float3 color, float gamma ) {
                    return pow ( color, 1.0 / gamma );
                }
     
                void surf (Input IN, inout SurfaceOutputSkinShader o)
                {
                    float4 albedo = tex2D ( _MainTex, IN.uv_MainTex );
                    o.Albedo = gamma ( albedo.rgb, _Gamma );
                    o.Alpha = albedo.a;
     
                    o.AnisoDir = tex2D ( _AnisoDirection, IN.uv_MainTex );
                    o.AnisoDir.rgb = o.AnisoDir.rgb * 2 - 1;
     
                    o.Normal = UnpackNormal ( tex2D ( _BumpMap, IN.uv_MainTex ) );
     
                    o.Specular = tex2D ( _SpecularTex, IN.uv_MainTex ).rgb;
     
                    float3 normalBlur = UnpackNormal ( tex2Dlod ( _BumpMap, float4 ( IN.uv_MainTex, 0.0, _BumpBias ) ) );
                    o.Curvature = length ( fwidth ( WorldNormalVector ( IN, normalBlur ) ) ) / length ( fwidth ( IN.worldPos ) ) * _CurvatureScale;
                }
     
                inline fixed4 LightingSkinShader (SurfaceOutputSkinShader s, fixed3 lightDir, fixed3 viewDir, fixed atten)
                {
                    clip ( s.Alpha - _Cutoff );
     
                    viewDir = normalize ( viewDir );
                    lightDir = normalize ( lightDir );
                    s.Normal = normalize ( s.Normal );
                    float NdotL = dot ( s.Normal, lightDir );
                    float3 h = normalize ( lightDir + viewDir );
                    _LightColor0.rgb = gamma ( _LightColor0.rgb, _Gamma );
     
                    float specBase = saturate ( dot( s.Normal, h ) );
     
                    float aniso = CalcAniso ( s.Normal, s.AnisoDir.rgb, h, _AnisoOffset );
     
                    float fresnel = CalcFresnel ( viewDir, h, lerp ( 0.2, _Fresnel, s.Specular.b ) );
     
                    float spec = CalcSpec ( lerp ( specBase, aniso, s.AnisoDir.a ), s.Specular.r, s.Specular.g, fresnel );
     
                    float3 brdf = gamma ( tex2D ( _BRDFTex, float2 ( ( NdotL * 0.5 + 0.5 ) * atten, s.Curvature ) ).rgb, _Gamma );
     
                    fixed4 c;
                    float3 skinResult = s.Albedo * brdf * _LightColor0.rgb;
                    float3 clothResult = s.Albedo * saturate ( NdotL ) * _LightColor0.rgb;
                    float3 finalResult = lerp ( clothResult, skinResult, s.Specular.b );
                    finalResult = lerp ( finalResult, _LightColor0.rgb, saturate ( spec * atten ) );
                    c.rgb = ungamma( finalResult, _Gamma );
                    #ifndef USING_DIRECTIONAL_LIGHT
                    c.rgb *= atten;
                    #endif
                    c.a = s.Curvature;
                    return c;
                }
            ENDCG
        }
        FallBack "Transparent/Cutout/VertexLit"
    }
