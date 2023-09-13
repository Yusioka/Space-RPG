// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SyntyStudios/EnvTriplanar"
{
	Properties
	{
		_Main_Texture("Main_Texture", 2D) = "gray" {}
		_Main_Normals("Main_Normals", 2D) = "bump" {}
		[HideInInspector]_Side_Triplanar("Side_Triplanar", 2D) = "black" {}
		[HideInInspector]_White_Triplanar("White_Triplanar", 2D) = "white" {}
		_Rock_Mask_Triplanar("Rock_Mask_Triplanar", 2D) = "black" {}
		[HideInInspector]_GroundMask("GroundMask", 2D) = "white" {}
		_Noise("Noise", 2D) = "black" {}
		_Normal_Rocks("Normal_Rocks", 2D) = "bump" {}
		_Normal_Triplanar("Normal_Triplanar", 2D) = "bump" {}
		[HideInInspector]_TextureSample0("Texture Sample 0", 2D) = "bump" {}
		_Emissive_01("Emissive_01", 2D) = "white" {}
		_Ground_Colour("Ground_Colour", Color) = (0.4603157,0.4718575,0.4852941,0)
		_Rock_Colour("Rock_Colour", Color) = (0.3161765,0.2836289,0.2836289,0)
		_Road_Colour("Road_Colour", Color) = (0.3161765,0.2836289,0.2836289,0)
		_Spec_Smoothness("Spec_Smoothness", Range( 0 , 1)) = 0.5
		_Spec_Metallic("Spec_Metallic", Range( 0 , 1)) = 0.5
		_Ground_Pebble_Scale("Ground_Pebble_Scale", Range( 0 , 0.1)) = 0.05
		_Ground_Detail_Scale("Ground_Detail_Scale", Range( 0 , 0.5)) = 0.1
		_Ground_Falloff("Ground_Falloff", Range( 5 , 500)) = 5
		_Ground_Detail_Power("Ground_Detail_Power", Range( 0 , 1)) = 0.5
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform sampler2D _Main_Normals;
		uniform float4 _Main_Normals_ST;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform sampler2D _Normal_Triplanar;
		uniform float _Ground_Detail_Scale;
		uniform float _Ground_Detail_Power;
		uniform sampler2D _White_Triplanar;
		uniform sampler2D _Side_Triplanar;
		uniform float _Ground_Falloff;
		uniform sampler2D _GroundMask;
		uniform float4 _GroundMask_ST;
		uniform sampler2D _Normal_Rocks;
		uniform float _Ground_Pebble_Scale;
		uniform sampler2D _Rock_Mask_Triplanar;
		uniform sampler2D _Noise;
		uniform sampler2D _Main_Texture;
		uniform float4 _Main_Texture_ST;
		uniform float4 _Rock_Colour;
		uniform float4 _Road_Colour;
		uniform float4 _Ground_Colour;
		uniform sampler2D _Emissive_01;
		uniform float4 _Emissive_01_ST;
		uniform float _Spec_Metallic;
		uniform float _Spec_Smoothness;


		inline float3 TriplanarSampling28( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = tex2D( topTexMap, tiling * worldPos.zy * float2(  nsign.x, 1.0 ) );
			yNorm = tex2D( topTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			zNorm = tex2D( topTexMap, tiling * worldPos.xy * float2( -nsign.z, 1.0 ) );
			xNorm.xyz  = half3( UnpackNormal( xNorm ).xy * float2(  nsign.x, 1.0 ) + worldNormal.zy, worldNormal.x ).zyx;
			yNorm.xyz  = half3( UnpackNormal( yNorm ).xy * float2(  nsign.y, 1.0 ) + worldNormal.xz, worldNormal.y ).xzy;
			zNorm.xyz  = half3( UnpackNormal( zNorm ).xy * float2( -nsign.z, 1.0 ) + worldNormal.xy, worldNormal.z ).xyz;
			return normalize( xNorm.xyz * projNormal.x + yNorm.xyz * projNormal.y + zNorm.xyz * projNormal.z );
		}


		inline float4 TriplanarSampling38( sampler2D topTexMap, sampler2D midTexMap, sampler2D botTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			float negProjNormalY = max( 0, projNormal.y * -nsign.y );
			projNormal.y = max( 0, projNormal.y * nsign.y );
			half4 xNorm; half4 yNorm; half4 yNormN; half4 zNorm;
			xNorm  = tex2D( midTexMap, tiling * worldPos.zy * float2(  nsign.x, 1.0 ) );
			yNorm  = tex2D( topTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			yNormN = tex2D( botTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			zNorm  = tex2D( midTexMap, tiling * worldPos.xy * float2( -nsign.z, 1.0 ) );
			return xNorm * projNormal.x + yNorm * projNormal.y + yNormN * negProjNormalY + zNorm * projNormal.z;
		}


		inline float3 TriplanarSampling89( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = tex2D( topTexMap, tiling * worldPos.zy * float2(  nsign.x, 1.0 ) );
			yNorm = tex2D( topTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			zNorm = tex2D( topTexMap, tiling * worldPos.xy * float2( -nsign.z, 1.0 ) );
			xNorm.xyz  = half3( UnpackNormal( xNorm ).xy * float2(  nsign.x, 1.0 ) + worldNormal.zy, worldNormal.x ).zyx;
			yNorm.xyz  = half3( UnpackNormal( yNorm ).xy * float2(  nsign.y, 1.0 ) + worldNormal.xz, worldNormal.y ).xzy;
			zNorm.xyz  = half3( UnpackNormal( zNorm ).xy * float2( -nsign.z, 1.0 ) + worldNormal.xy, worldNormal.z ).xyz;
			return normalize( xNorm.xyz * projNormal.x + yNorm.xyz * projNormal.y + zNorm.xyz * projNormal.z );
		}


		inline float4 TriplanarSampling95( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = tex2D( topTexMap, tiling * worldPos.zy * float2(  nsign.x, 1.0 ) );
			yNorm = tex2D( topTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			zNorm = tex2D( topTexMap, tiling * worldPos.xy * float2( -nsign.z, 1.0 ) );
			return xNorm * projNormal.x + yNorm * projNormal.y + zNorm * projNormal.z;
		}


		inline float4 TriplanarSampling73( sampler2D topTexMap, sampler2D midTexMap, sampler2D botTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			float negProjNormalY = max( 0, projNormal.y * -nsign.y );
			projNormal.y = max( 0, projNormal.y * nsign.y );
			half4 xNorm; half4 yNorm; half4 yNormN; half4 zNorm;
			xNorm  = tex2D( midTexMap, tiling * worldPos.zy * float2(  nsign.x, 1.0 ) );
			yNorm  = tex2D( topTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			yNormN = tex2D( botTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			zNorm  = tex2D( midTexMap, tiling * worldPos.xy * float2( -nsign.z, 1.0 ) );
			return xNorm * projNormal.x + yNorm * projNormal.y + yNormN * negProjNormalY + zNorm * projNormal.z;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Main_Normals = i.uv_texcoord * _Main_Normals_ST.xy + _Main_Normals_ST.zw;
			float3 tex2DNode182 = UnpackNormal( tex2D( _Main_Normals, uv_Main_Normals ) );
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float3 tex2DNode177 = UnpackNormal( tex2D( _TextureSample0, uv_TextureSample0 ) );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 triplanar28 = TriplanarSampling28( _Normal_Triplanar, ase_worldPos, ase_worldNormal, 100.0, _Ground_Detail_Scale, 1.0, 0 );
			float3 tanTriplanarNormal28 = mul( ase_worldToTangent, triplanar28 );
			float3 lerpResult179 = lerp( tex2DNode177 , tanTriplanarNormal28 , _Ground_Detail_Power);
			float4 triplanar38 = TriplanarSampling38( _White_Triplanar, _Side_Triplanar, _Side_Triplanar, ase_worldPos, ase_worldNormal, _Ground_Falloff, 1.0, float3( 1,1,1 ), float3(0,0,0) );
			float2 uv_GroundMask = i.uv_texcoord * _GroundMask_ST.xy + _GroundMask_ST.zw;
			float4 tex2DNode150 = tex2D( _GroundMask, uv_GroundMask );
			float lerpResult166 = lerp( triplanar38.w , (float)1 , tex2DNode150.g);
			float3 lerpResult79 = lerp( tex2DNode177 , lerpResult179 , lerpResult166);
			float3 triplanar89 = TriplanarSampling89( _Normal_Rocks, ase_worldPos, ase_worldNormal, 100.0, _Ground_Pebble_Scale, 1.0, 0 );
			float3 tanTriplanarNormal89 = mul( ase_worldToTangent, triplanar89 );
			float4 temp_cast_1 = 0;
			float4 triplanar95 = TriplanarSampling95( _Rock_Mask_Triplanar, ase_worldPos, ase_worldNormal, 100.0, _Ground_Pebble_Scale, 1.0, 0 );
			float4 lerpResult169 = lerp( temp_cast_1 , triplanar95 , triplanar38.w);
			float3 lerpResult86 = lerp( lerpResult79 , tanTriplanarNormal89 , lerpResult169.xyz);
			float4 triplanar73 = TriplanarSampling73( _Noise, _Side_Triplanar, _Side_Triplanar, ase_worldPos, ase_worldNormal, 100.0, 1.0, float3( 1,1,1 ), float3(0,0,0) );
			float3 lerpResult185 = lerp( tex2DNode182 , BlendNormals( lerpResult86 , tex2DNode182 ) , triplanar73.xyz);
			o.Normal = lerpResult185;
			float2 uv_Main_Texture = i.uv_texcoord * _Main_Texture_ST.xy + _Main_Texture_ST.zw;
			float4 lerpResult151 = lerp( tex2D( _Main_Texture, uv_Main_Texture ) , _Rock_Colour , tex2DNode150.r);
			float4 lerpResult192 = lerp( lerpResult151 , _Road_Colour , tex2DNode150.b);
			float4 lerpResult191 = lerp( _Ground_Colour , _Road_Colour , tex2DNode150.b);
			float4 lerpResult100 = lerp( lerpResult191 , _Rock_Colour , triplanar95);
			float4 temp_cast_5 = 1;
			float4 lerpResult181 = lerp( triplanar73 , temp_cast_5 , tex2DNode150.g);
			float4 lerpResult26 = lerp( lerpResult192 , lerpResult100 , lerpResult181);
			o.Albedo = lerpResult26.rgb;
			float2 uv_Emissive_01 = i.uv_texcoord * _Emissive_01_ST.xy + _Emissive_01_ST.zw;
			o.Emission = tex2D( _Emissive_01, uv_Emissive_01 ).rgb;
			o.Metallic = _Spec_Metallic;
			o.Smoothness = _Spec_Smoothness;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows exclude_path:deferred 

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
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
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
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
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
546;172;2255;1259;-811.0299;452.5449;1.22155;True;True
Node;AmplifyShaderEditor.TexturePropertyNode;69;-646.037,282.2222;Float;True;Property;_Normal_Triplanar;Normal_Triplanar;8;0;Create;True;0;0;0;False;0;False;99e0b922816cbe64799f178686a4f5d5;99e0b922816cbe64799f178686a4f5d5;True;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;65;-866.809,54.28796;Inherit;False;Property;_Ground_Detail_Scale;Ground_Detail_Scale;17;0;Create;True;0;0;0;False;0;False;0.1;0.137;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;101;-758.2315,-1283.97;Float;True;Property;_White_Triplanar;White_Triplanar;3;1;[HideInInspector];Create;True;0;0;0;False;0;False;4cdae8663588a164f93c95e734dcb6f2;358612fdf9c718f4fa6f01d88164b407;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;171;-936.3992,-779.3782;Inherit;False;Property;_Ground_Falloff;Ground_Falloff;18;0;Create;True;0;0;0;False;0;False;5;65;5;500;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;62;-938.614,-520.6078;Float;True;Property;_Side_Triplanar;Side_Triplanar;2;1;[HideInInspector];Create;True;0;0;0;False;0;False;358612fdf9c718f4fa6f01d88164b407;358612fdf9c718f4fa6f01d88164b407;False;black;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.WorldPosInputsNode;49;-917.5267,-174.424;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;150;-2.064163,-1083.243;Inherit;True;Property;_GroundMask;GroundMask;5;1;[HideInInspector];Create;True;0;0;0;False;0;False;-1;1b35185671c1afb4cb44f0ff9e46e3d0;1b35185671c1afb4cb44f0ff9e46e3d0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TriplanarNode;38;-434.7,-1032.654;Inherit;True;Cylindrical;World;False;Top Texture 2;_TopTexture2;white;2;None;Mid Texture 2;_MidTexture2;white;1;None;Bot Texture 2;_BotTexture2;white;3;None;Triplanar Sampler;Tangent;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT3;1,1,1;False;3;FLOAT;1;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TriplanarNode;28;-244.6221,91.28374;Inherit;True;Spherical;World;True;Top Texture 1;_TopTexture1;white;2;None;Mid Texture 1;_MidTexture1;white;3;None;Bot Texture 1;_BotTexture1;white;6;None;Triplanar Sampler;Tangent;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;100;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;168;-287.8726,-734.6318;Inherit;False;Constant;_Int0;Int 0;20;0;Create;True;0;0;0;False;0;False;1;0;False;0;1;INT;0
Node;AmplifyShaderEditor.TexturePropertyNode;99;-1291.298,218.738;Float;True;Property;_Rock_Mask_Triplanar;Rock_Mask_Triplanar;4;0;Create;True;0;0;0;False;0;False;4c63ce07744d12e40bcac47b1c0f7c6d;2917dd9cb57f27a43a63a8e93800d545;False;black;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;180;-45.13549,290.9251;Inherit;False;Property;_Ground_Detail_Power;Ground_Detail_Power;19;0;Create;True;0;0;0;False;0;False;0.5;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;91;-245.5991,1305.544;Inherit;False;Property;_Ground_Pebble_Scale;Ground_Pebble_Scale;16;0;Create;True;0;0;0;False;0;False;0.05;0.0361;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;177;-298.6286,477.2089;Inherit;True;Property;_TextureSample0;Texture Sample 0;9;1;[HideInInspector];Create;True;0;0;0;False;0;False;-1;6c2c1457696dd074d955805ba1175e78;6c2c1457696dd074d955805ba1175e78;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;179;358.0404,244.3225;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TriplanarNode;95;516.6096,-113.1551;Inherit;True;Spherical;World;False;Top Texture 5;_TopTexture5;white;3;None;Mid Texture 5;_MidTexture5;white;1;None;Bot Texture 5;_BotTexture5;white;5;None;Triplanar Sampler;Tangent;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;100;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;84;-409.7986,1092.045;Float;True;Property;_Normal_Rocks;Normal_Rocks;7;0;Create;True;0;0;0;False;0;False;ed3b6c1d3e78c9f478c1753a9ab5f2a0;3ddce3d0e1b734843bc2b8cfc2c074c7;True;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.LerpOp;166;126.0995,-819.0908;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;170;628.5545,211.6254;Inherit;False;Constant;_Int1;Int 1;20;0;Create;True;0;0;0;False;0;False;0;0;False;0;1;INT;0
Node;AmplifyShaderEditor.ColorNode;102;460.161,-1000.95;Inherit;False;Property;_Ground_Colour;Ground_Colour;11;0;Create;True;0;0;0;False;0;False;0.4603157,0.4718575,0.4852941,0;0.8602941,0.8602941,0.8602941,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;79;709.69,472.2429;Inherit;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;21;137.9726,-1600.992;Inherit;True;Property;_Main_Texture;Main_Texture;0;0;Create;True;0;0;0;False;0;False;-1;None;d44fa91dd607b2243aac51d78b7922dd;True;0;False;gray;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;169;939.1041,292.2019;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TriplanarNode;89;245.5025,1101.244;Inherit;True;Spherical;World;True;Top Texture 11;_TopTexture11;white;2;None;Mid Texture 11;_MidTexture11;white;2;None;Bot Texture 11;_BotTexture11;white;7;None;Triplanar Sampler;Tangent;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;100;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;63;-1022.711,-1150.602;Float;True;Property;_Noise;Noise;6;0;Create;True;0;0;0;False;0;False;d4500c634f3a0684783559e1222d3ab5;85391693e75fc8d4391a37a03aeb9d66;False;black;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.ColorNode;153;117.9688,-1274.113;Inherit;False;Property;_Rock_Colour;Rock_Colour;12;0;Create;True;0;0;0;False;0;False;0.3161765,0.2836289,0.2836289,0;0.05622837,0.05482265,0.09558821,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;189;447.0558,-656.911;Inherit;False;Property;_Road_Colour;Road_Colour;13;0;Create;True;0;0;0;False;0;False;0.3161765,0.2836289,0.2836289,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;182;1105.324,1205.342;Inherit;True;Property;_Main_Normals;Main_Normals;1;0;Create;True;0;0;0;False;0;False;-1;2ee8591f903c8aa4382ef533fa1a51af;2ee8591f903c8aa4382ef533fa1a51af;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TriplanarNode;73;-370.0929,-423.9303;Inherit;True;Cylindrical;World;False;Top Texture 4;_TopTexture4;white;2;None;Mid Texture 4;_MidTexture4;white;1;None;Bot Texture 4;_BotTexture4;white;3;None;Triplanar Sampler;Tangent;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT3;1,1,1;False;3;FLOAT;1;False;4;FLOAT;100;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;151;932.5805,-1308.886;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;86;1027.242,636.7751;Inherit;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;191;827.3663,-631.585;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendNormalsNode;184;1745.861,864.6842;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;100;1193.501,-778.4813;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;181;504.8412,-428.9847;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LerpOp;192;1323.713,-1241.533;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;26;1825.635,-608.785;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;105;1412.033,-221.8609;Inherit;False;Property;_Spec_Smoothness;Spec_Smoothness;14;0;Create;True;0;0;0;False;0;False;0.5;0.068;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;107;1125.642,221.906;Inherit;False;Property;_Spec_Metallic;Spec_Metallic;15;0;Create;True;0;0;0;False;0;False;0.5;0.01;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;185;1818.244,1171.463;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;193;1718.03,358.565;Inherit;True;Property;_Emissive_01;Emissive_01;10;0;Create;True;0;0;0;False;0;False;-1;03db659f8a637b744943d87d1a6d7c5e;03db659f8a637b744943d87d1a6d7c5e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2373.013,-224.7957;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;SyntyStudios/EnvTriplanar;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;38;0;101;0
WireConnection;38;1;62;0
WireConnection;38;2;62;0
WireConnection;38;4;171;0
WireConnection;28;0;69;0
WireConnection;28;9;49;0
WireConnection;28;3;65;0
WireConnection;179;0;177;0
WireConnection;179;1;28;0
WireConnection;179;2;180;0
WireConnection;95;0;99;0
WireConnection;95;9;49;0
WireConnection;95;3;91;0
WireConnection;166;0;38;4
WireConnection;166;1;168;0
WireConnection;166;2;150;2
WireConnection;79;0;177;0
WireConnection;79;1;179;0
WireConnection;79;2;166;0
WireConnection;169;0;170;0
WireConnection;169;1;95;0
WireConnection;169;2;38;4
WireConnection;89;0;84;0
WireConnection;89;3;91;0
WireConnection;73;0;63;0
WireConnection;73;1;62;0
WireConnection;73;2;62;0
WireConnection;151;0;21;0
WireConnection;151;1;153;0
WireConnection;151;2;150;1
WireConnection;86;0;79;0
WireConnection;86;1;89;0
WireConnection;86;2;169;0
WireConnection;191;0;102;0
WireConnection;191;1;189;0
WireConnection;191;2;150;3
WireConnection;184;0;86;0
WireConnection;184;1;182;0
WireConnection;100;0;191;0
WireConnection;100;1;153;0
WireConnection;100;2;95;0
WireConnection;181;0;73;0
WireConnection;181;1;168;0
WireConnection;181;2;150;2
WireConnection;192;0;151;0
WireConnection;192;1;189;0
WireConnection;192;2;150;3
WireConnection;26;0;192;0
WireConnection;26;1;100;0
WireConnection;26;2;181;0
WireConnection;185;0;182;0
WireConnection;185;1;184;0
WireConnection;185;2;73;0
WireConnection;0;0;26;0
WireConnection;0;1;185;0
WireConnection;0;2;193;0
WireConnection;0;3;107;0
WireConnection;0;4;105;0
ASEEND*/
//CHKSM=376960D61A0F4FB9CCF0FA06936DCAEA142CE57D