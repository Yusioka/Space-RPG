// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SyntyStudios/NoFogUnlit"
{
	Properties
	{
		_Planet_Texture("Planet_Texture", 2D) = "white" {}
		_Rim_Texture("Rim_Texture", 2D) = "white" {}
		_Glow_Texture("Glow_Texture", 2D) = "white" {}
		_Planet_Tint("Planet_Tint", Color) = (1,0.821501,0.5955882,0)
		_Planet_Light("Planet_Light", Range( 0 , 10)) = 1
		_Rim_Intensity("Rim_Intensity", Range( 0 , 10)) = 1
		_Glow_Tint("Glow_Tint", Color) = (1,0,0,0)
		_Glow_Intensity("Glow_Intensity", Range( 0 , 10)) = 1
		[Toggle(_SOFT_BLEND_SWITCH_ON)] _Soft_Blend_Switch("Soft_Blend_Switch", Float) = 0
		_Opacity("Opacity", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature _SOFT_BLEND_SWITCH_ON
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Rim_Texture;
		uniform float4 _Rim_Texture_ST;
		uniform float _Rim_Intensity;
		uniform sampler2D _Planet_Texture;
		uniform float4 _Planet_Texture_ST;
		uniform float4 _Planet_Tint;
		uniform float _Planet_Light;
		uniform float4 _Glow_Tint;
		uniform float _Glow_Intensity;
		uniform sampler2D _Glow_Texture;
		uniform float4 _Glow_Texture_ST;
		uniform float _Opacity;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_Rim_Texture = i.uv_texcoord * _Rim_Texture_ST.xy + _Rim_Texture_ST.zw;
			float4 tex2DNode11 = tex2D( _Rim_Texture, uv_Rim_Texture );
			float4 lerpResult16 = lerp( float4( 0,0,0,0 ) , ( tex2DNode11 * _Rim_Intensity ) , tex2DNode11);
			float2 uv_Planet_Texture = i.uv_texcoord * _Planet_Texture_ST.xy + _Planet_Texture_ST.zw;
			float4 tex2DNode2 = tex2D( _Planet_Texture, uv_Planet_Texture );
			float2 uv_Glow_Texture = i.uv_texcoord * _Glow_Texture_ST.xy + _Glow_Texture_ST.zw;
			float4 lerpResult8 = lerp( ( tex2DNode2 * ( _Planet_Tint * _Planet_Light ) ) , ( _Glow_Tint * _Glow_Intensity ) , tex2D( _Glow_Texture, uv_Glow_Texture ));
			o.Emission = ( lerpResult16 + lerpResult8 ).rgb;
			float lerpResult27 = lerp( 0.0 , i.uv_texcoord.y , tex2DNode2.a);
			float lerpResult33 = lerp( (float)0 , lerpResult27 , _Opacity);
			#ifdef _SOFT_BLEND_SWITCH_ON
				float staticSwitch37 = lerpResult33;
			#else
				float staticSwitch37 = tex2DNode2.a;
			#endif
			o.Alpha = staticSwitch37;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit alpha:fade keepalpha fullforwardshadows exclude_path:deferred nofog 

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
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
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
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
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
Version=18909
-2466;109;2052;1359;171.444;271.8215;1;True;True
Node;AmplifyShaderEditor.RangedFloatNode;24;-284.9829,786.3144;Inherit;False;Property;_Planet_Light;Planet_Light;4;0;Create;True;0;0;0;False;0;False;1;4.57;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;3;-716.9359,558.2112;Inherit;False;Property;_Planet_Tint;Planet_Tint;3;0;Create;True;0;0;0;False;0;False;1,0.821501,0.5955882,0;0.4338235,0.2712057,0.1658737,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;10;-511.0779,-629.2368;Inherit;False;Property;_Glow_Intensity;Glow_Intensity;7;0;Create;True;0;0;0;False;0;False;1;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;26;109.7262,410.7887;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-778.5517,-50.49416;Inherit;True;Property;_Planet_Texture;Planet_Texture;0;0;Create;True;0;0;0;False;0;False;-1;0cff01051e6502246b75931fe5e9451d;ba3a6aa43b889004997b5af67aec7700;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;7;-531.1704,-876.9771;Inherit;False;Property;_Glow_Tint;Glow_Tint;6;0;Create;True;0;0;0;False;0;False;1,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;12;572.4872,-1119.156;Inherit;False;Property;_Rim_Intensity;Rim_Intensity;5;0;Create;True;0;0;0;False;0;False;1;1.36;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;11;-60.28147,-1146.619;Inherit;True;Property;_Rim_Texture;Rim_Texture;1;0;Create;True;0;0;0;False;0;False;-1;00d0df43882e89644bb0e0901843d05c;78204ff12f1124f478bf66b4e04eb8a6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-279.4765,513.5957;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.IntNode;36;614.3379,308.0548;Inherit;False;Constant;_Int0;Int 0;9;0;Create;True;0;0;0;False;0;False;0;0;False;0;1;INT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;546.369,435.6475;Inherit;False;Property;_Opacity;Opacity;9;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;27;492.0007,-0.1115851;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-228.0779,-844.2368;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;6;-697.6086,-452.4364;Inherit;True;Property;_Glow_Texture;Glow_Texture;2;0;Create;True;0;0;0;False;0;False;-1;5337fcdcdfb4ee84da7302c25621db92;51bd24b0eaa199743b6a89adc78ac60e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;910.4872,-1255.156;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-235.9129,193.6765;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;8;117.5642,-302.17;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;16;555.8936,-768.3997;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;33;919.5259,164.9441;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;21;935.2114,-581.4596;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;37;857.9426,-105.0399;Inherit;False;Property;_Soft_Blend_Switch;Soft_Blend_Switch;8;0;Create;True;0;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;False;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1304.161,-241.2001;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;SyntyStudios/NoFogUnlit;False;False;False;False;False;False;False;False;False;True;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.43;True;True;0;False;Transparent;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;23;0;3;0
WireConnection;23;1;24;0
WireConnection;27;1;26;2
WireConnection;27;2;2;4
WireConnection;9;0;7;0
WireConnection;9;1;10;0
WireConnection;14;0;11;0
WireConnection;14;1;12;0
WireConnection;4;0;2;0
WireConnection;4;1;23;0
WireConnection;8;0;4;0
WireConnection;8;1;9;0
WireConnection;8;2;6;0
WireConnection;16;1;14;0
WireConnection;16;2;11;0
WireConnection;33;0;36;0
WireConnection;33;1;27;0
WireConnection;33;2;35;0
WireConnection;21;0;16;0
WireConnection;21;1;8;0
WireConnection;37;1;2;4
WireConnection;37;0;33;0
WireConnection;0;2;21;0
WireConnection;0;9;37;0
ASEEND*/
//CHKSM=24AD7949F6AE3D47FDEFB050BEBC227932D0A527