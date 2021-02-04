// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Unlit/SafetyEdge"
{
	Properties
	{
		_Tex_mask("Tex_mask", 2D) = "white" {}
		_Tex("Tex", 2D) = "white" {}
		_Color("Color", Color) = (0,0,0,0)
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_MSK_T("MSK_T", Float) = 10
		_U("U", Float) = 0
		_V("V", Float) = 0
		_Power("Power", Float) = 0
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_ani("ani", Float) = 1.12
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Tex;
		uniform float4 _Tex_ST;
		uniform float4 _Color;
		uniform sampler2D _Tex_mask;
		uniform float4 _Tex_mask_ST;
		uniform float _ani;
		uniform sampler2D _TextureSample2;
		uniform float4 _TextureSample2_ST;
		uniform sampler2D _TextureSample0;
		uniform float _MSK_T;
		uniform float _U;
		uniform float _V;
		uniform float _Power;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_Tex = i.uv_texcoord * _Tex_ST.xy + _Tex_ST.zw;
			o.Emission = ( tex2D( _Tex, uv_Tex ) * _Color ).rgb;
			float2 uv_Tex_mask = i.uv_texcoord * _Tex_mask_ST.xy + _Tex_mask_ST.zw;
			float2 uv_TextureSample2 = i.uv_texcoord * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
			float ifLocalVar42 = 0;
			if( _ani >= tex2D( _TextureSample2, uv_TextureSample2 ).r )
				ifLocalVar42 = 0.0;
			else
				ifLocalVar42 = 1.0;
			float temp_output_8_0 = ( 1.0 / _MSK_T );
			float2 temp_output_30_0 = (( floor( ( i.uv_texcoord / temp_output_8_0 ) ) * temp_output_8_0 )*2.0 + -1.0);
			float2 break32 = temp_output_30_0;
			float2 appendResult31 = (float2(length( temp_output_30_0 ) , ( ( atan2( break32.y , break32.x ) / 6.0 ) + 0.5 )));
			float4 appendResult19 = (float4(_U , _V , 0.0 , 0.0));
			float4 temp_output_20_0 = ( appendResult19 * _Time.y );
			float4 temp_output_34_0 = ( float4( appendResult31, 0.0 , 0.0 ) + temp_output_20_0 );
			float4 tex2DNode7 = tex2D( _TextureSample0, temp_output_34_0.xy );
			float4 temp_cast_3 = (_Power).xxxx;
			o.Alpha = ( ( tex2D( _Tex_mask, uv_Tex_mask ) * ifLocalVar42 ) * ( tex2DNode7 + pow( tex2DNode7 , temp_cast_3 ) ) ).r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16301
1920;54;1906;965;2446.729;872.8043;2.2559;True;False
Node;AmplifyShaderEditor.RangedFloatNode;9;-2863.052,1325.761;Float;False;Constant;_Float0;Float 0;4;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-2839.052,1440.761;Float;False;Property;_MSK_T;MSK_T;6;0;Create;True;0;0;False;0;10;50.9;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;8;-2597.052,1323.761;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-3371.789,1318.417;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;6;-2505.157,1167.566;Float;True;2;0;FLOAT2;0,0;False;1;FLOAT;0.47;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FloorOpNode;11;-2132.533,1260.878;Float;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-1774.81,1346.863;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;30;-3662.673,397.5681;Float;True;3;0;FLOAT2;0,0;False;1;FLOAT;2;False;2;FLOAT;-1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;32;-3213.41,-99.09227;Float;True;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.ATan2OpNode;24;-2845.954,-104.7126;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;25;-2479.163,-221.6609;Float;True;2;0;FLOAT;0;False;1;FLOAT;6;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-3523.426,848.6791;Float;False;Property;_V;V;8;0;Create;True;0;0;False;0;0;-0.04;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-3524.426,777.6791;Float;False;Property;_U;U;7;0;Create;True;0;0;False;0;0;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;28;-3081.723,-579.473;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;33;-4042.551,-95.32671;Float;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;16;-3512.426,1026.679;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;19;-3336.426,831.6791;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-3117.426,896.6791;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;31;-3795.616,-356.1859;Float;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;34;-2244.522,580.5223;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-834.3542,1183.779;Float;False;Property;_Power;Power;9;0;Create;True;0;0;False;0;0;1.44;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;40;-752.7058,-201.1865;Float;True;Property;_TextureSample2;Texture Sample 2;10;0;Create;True;0;0;False;0;None;0000000000000000f000000000000000;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;45;-336.7058,-274.1865;Float;False;Property;_ani;ani;11;0;Create;True;0;0;False;0;1.12;1.12;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;7;-1047.791,763.4117;Float;True;Property;_TextureSample0;Texture Sample 0;4;0;Create;True;0;0;False;0;d7587323b7bbc084ab37898fd841d0fa;46158b0ee9ad38241ab1fbf9a40f51a7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;44;-159.7058,49.81348;Float;False;Constant;_Float3;Float 3;11;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-231.7058,-161.1865;Float;False;Constant;_Float2;Float 2;11;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;35;-580.9159,1014.619;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;2;-727.2874,199.8032;Float;True;Property;_Tex_mask;Tex_mask;0;0;Create;True;0;0;False;0;None;55595d78e92f53544804aeb6155391c9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ConditionalIfNode;42;-52.70581,-272.1865;Float;True;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;4;-1438.162,523.313;Float;False;Property;_Color;Color;2;0;Create;True;0;0;False;0;0,0,0,0;0,0.5911949,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;37;-603.8499,842.168;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;314.2942,-78.18652;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;1;-1379.799,302.0569;Float;True;Property;_Tex;Tex;1;0;Create;True;0;0;False;0;None;55595d78e92f53544804aeb6155391c9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;27;-2876.892,368.3626;Float;False;Constant;_Float5;Float 5;2;0;Create;True;0;0;False;0;0.9;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;21;-2839.653,951.9957;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;29;-2619.078,285.24;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-3116.264,310.0358;Float;False;Property;_Float4;Float 4;5;0;Create;True;0;0;False;0;0.82;0.08;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;22;-1855.876,429.9583;Float;True;Property;_TextureSample1;Texture Sample 1;3;0;Create;True;0;0;False;0;5228a04ef529d2641937cab585cc1a02;5228a04ef529d2641937cab585cc1a02;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;38;-755.2159,652.3231;Float;False;Constant;_Float1;Float 1;10;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-167.5681,745.2336;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-1024.762,460.9131;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;253.2,198.5;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;New Amplify Shader 1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;8;0;9;0
WireConnection;8;1;10;0
WireConnection;6;0;5;0
WireConnection;6;1;8;0
WireConnection;11;0;6;0
WireConnection;12;0;11;0
WireConnection;12;1;8;0
WireConnection;30;0;12;0
WireConnection;32;0;30;0
WireConnection;24;0;32;1
WireConnection;24;1;32;0
WireConnection;25;0;24;0
WireConnection;28;0;25;0
WireConnection;33;0;30;0
WireConnection;19;0;17;0
WireConnection;19;1;18;0
WireConnection;20;0;19;0
WireConnection;20;1;16;2
WireConnection;31;0;33;0
WireConnection;31;1;28;0
WireConnection;34;0;31;0
WireConnection;34;1;20;0
WireConnection;7;1;34;0
WireConnection;35;0;7;0
WireConnection;35;1;36;0
WireConnection;42;0;45;0
WireConnection;42;1;40;1
WireConnection;42;2;43;0
WireConnection;42;3;43;0
WireConnection;42;4;44;0
WireConnection;37;0;7;0
WireConnection;37;1;35;0
WireConnection;39;0;2;0
WireConnection;39;1;42;0
WireConnection;21;0;5;0
WireConnection;21;1;20;0
WireConnection;29;0;26;0
WireConnection;29;1;27;0
WireConnection;22;1;34;0
WireConnection;13;0;39;0
WireConnection;13;1;37;0
WireConnection;3;0;1;0
WireConnection;3;1;4;0
WireConnection;0;2;3;0
WireConnection;0;9;13;0
ASEEND*/
//CHKSM=D74316A0043FFD4C51F773A5BD5D5F97B07E9C70