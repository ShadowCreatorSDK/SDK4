// Shader created with Shader Forge v1.40 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.40;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,cpap:True,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:4013,x:33505,y:32828,varname:node_4013,prsc:2|diff-6444-OUT,emission-7753-OUT;n:type:ShaderForge.SFN_TexCoord,id:5685,x:31678,y:32822,varname:node_5685,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Panner,id:783,x:31904,y:32822,varname:node_783,prsc:2,spu:-0.2,spv:0|UVIN-5685-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:3520,x:32095,y:32833,ptovrint:False,ptlb:Glow,ptin:_Glow,varname:node_3520,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-783-UVOUT;n:type:ShaderForge.SFN_Multiply,id:6900,x:32396,y:32862,varname:node_6900,prsc:2|A-3520-RGB,B-3995-RGB;n:type:ShaderForge.SFN_Color,id:3995,x:32136,y:33066,ptovrint:False,ptlb:node_3995,ptin:_node_3995,varname:node_3995,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_TexCoord,id:3441,x:32396,y:32569,varname:node_3441,prsc:2,uv:1,uaff:False;n:type:ShaderForge.SFN_Tex2d,id:118,x:32669,y:32453,ptovrint:False,ptlb:diffuse,ptin:_diffuse,varname:node_118,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-3441-UVOUT;n:type:ShaderForge.SFN_Multiply,id:6444,x:32928,y:32570,varname:node_6444,prsc:2|A-118-RGB,B-4245-RGB;n:type:ShaderForge.SFN_Color,id:4245,x:32669,y:32658,ptovrint:False,ptlb:diffuse_color,ptin:_diffuse_color,varname:node_4245,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Power,id:7085,x:32658,y:32867,varname:node_7085,prsc:2|VAL-6900-OUT,EXP-3159-OUT;n:type:ShaderForge.SFN_Slider,id:3159,x:32396,y:33079,ptovrint:False,ptlb:power,ptin:_power,varname:node_3159,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.1709402,max:1;n:type:ShaderForge.SFN_Multiply,id:1216,x:32929,y:32888,varname:node_1216,prsc:2|A-7085-OUT,B-9456-OUT;n:type:ShaderForge.SFN_Slider,id:9456,x:32559,y:33194,ptovrint:False,ptlb:lianggang,ptin:_lianggang,varname:node_9456,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Multiply,id:4536,x:33150,y:33080,varname:node_4536,prsc:2|A-3520-RGB,B-4929-RGB;n:type:ShaderForge.SFN_Color,id:4929,x:32933,y:33232,ptovrint:False,ptlb:node_4929,ptin:_node_4929,varname:node_4929,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Add,id:7753,x:33256,y:32931,varname:node_7753,prsc:2|A-1216-OUT,B-4536-OUT;proporder:118-4245-3520-3995-3159-9456-4929;pass:END;sub:END;*/

Shader "My Shader Forge/TwoUv_ZoGlow" {
    Properties {
        _diffuse ("diffuse", 2D) = "white" {}
        _diffuse_color ("diffuse_color", Color) = (1,1,1,1)
        _Glow ("Glow", 2D) = "white" {}
        _node_3995 ("node_3995", Color) = (1,1,1,1)
        _power ("power", Range(0, 1)) = 0.1709402
        _lianggang ("lianggang", Range(0, 1)) = 0
        _node_4929 ("node_4929", Color) = (0.5,0.5,0.5,1)
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _Glow; uniform float4 _Glow_ST;
            uniform sampler2D _diffuse; uniform float4 _diffuse_ST;
            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float4, _node_3995)
                UNITY_DEFINE_INSTANCED_PROP( float4, _diffuse_color)
                UNITY_DEFINE_INSTANCED_PROP( float, _power)
                UNITY_DEFINE_INSTANCED_PROP( float, _lianggang)
                UNITY_DEFINE_INSTANCED_PROP( float4, _node_4929)
            UNITY_INSTANCING_BUFFER_END( Props )
            struct VertexInput {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
                LIGHTING_COORDS(4,5)
                UNITY_FOG_COORDS(6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID( v );
                UNITY_TRANSFER_INSTANCE_ID( v, o );
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float4 _diffuse_var = tex2D(_diffuse,TRANSFORM_TEX(i.uv1, _diffuse));
                float4 _diffuse_color_var = UNITY_ACCESS_INSTANCED_PROP( Props, _diffuse_color );
                float3 diffuseColor = (_diffuse_var.rgb*_diffuse_color_var.rgb);
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float4 node_8010 = _Time;
                float2 node_783 = (i.uv0+node_8010.g*float2(-0.2,0));
                float4 _Glow_var = tex2D(_Glow,TRANSFORM_TEX(node_783, _Glow));
                float4 _node_3995_var = UNITY_ACCESS_INSTANCED_PROP( Props, _node_3995 );
                float _power_var = UNITY_ACCESS_INSTANCED_PROP( Props, _power );
                float _lianggang_var = UNITY_ACCESS_INSTANCED_PROP( Props, _lianggang );
                float4 _node_4929_var = UNITY_ACCESS_INSTANCED_PROP( Props, _node_4929 );
                float3 emissive = ((pow((_Glow_var.rgb*_node_3995_var.rgb),_power_var)*_lianggang_var)+(_Glow_var.rgb*_node_4929_var.rgb));
/// Final Color:
                float3 finalColor = diffuse + emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _Glow; uniform float4 _Glow_ST;
            uniform sampler2D _diffuse; uniform float4 _diffuse_ST;
            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float4, _node_3995)
                UNITY_DEFINE_INSTANCED_PROP( float4, _diffuse_color)
                UNITY_DEFINE_INSTANCED_PROP( float, _power)
                UNITY_DEFINE_INSTANCED_PROP( float, _lianggang)
                UNITY_DEFINE_INSTANCED_PROP( float4, _node_4929)
            UNITY_INSTANCING_BUFFER_END( Props )
            struct VertexInput {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
                LIGHTING_COORDS(4,5)
                UNITY_FOG_COORDS(6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID( v );
                UNITY_TRANSFER_INSTANCE_ID( v, o );
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float4 _diffuse_var = tex2D(_diffuse,TRANSFORM_TEX(i.uv1, _diffuse));
                float4 _diffuse_color_var = UNITY_ACCESS_INSTANCED_PROP( Props, _diffuse_color );
                float3 diffuseColor = (_diffuse_var.rgb*_diffuse_color_var.rgb);
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
