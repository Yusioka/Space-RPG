// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SyntyStudios/ColourChange"
{
	Properties
	{
		_Colour_Hair("Colour_Hair", Color) = (0.3098039,0.254902,0.1803922,0)
		_Colour_Primary("Colour_Primary", Color) = (0.3161765,0.2836289,0.2836289,0)
		_Colour_Secondary("Colour_Secondary", Color) = (0.3161765,0.2836289,0.2836289,0)
		_Colour_Emissive("Colour_Emissive", Color) = (0.3161765,0.2836289,0.2836289,0)
		_Colour_Skin("Colour_Skin", Color) = (0.3161765,0.2836289,0.2836289,0)
		_Spec_Smoothness("Spec_Smoothness", Range( 0 , 1)) = 0.5
		_Spec_Metallic("Spec_Metallic", Range( 0 , 1)) = 0.5
		_PolygonSciFiPlanets_Texture_01_A("PolygonSciFiPlanets_Texture_01_A", 2D) = "white" {}
		_Hair_Mask("Hair_Mask", 2D) = "white" {}
		_Colour_Mask_01("Colour_Mask_01", 2D) = "white" {}
		_Colour_Mask_02("Colour_Mask_02", 2D) = "white" {}
		_skin_Mask("skin_Mask", 2D) = "white" {}
		_Emissive_01("Emissive_01", 2D) = "white" {}
		_Colour_Multiply("Colour_Multiply", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;
		uniform sampler2D _Colour_Multiply;
		uniform float4 _Colour_Multiply_ST;
		uniform sampler2D _PolygonSciFiPlanets_Texture_01_A;
		uniform float4 _PolygonSciFiPlanets_Texture_01_A_ST;
		uniform float4 _Colour_Primary;
		uniform sampler2D _Colour_Mask_01;
		uniform float4 _Colour_Mask_01_ST;
		uniform float4 _Colour_Secondary;
		uniform sampler2D _Colour_Mask_02;
		uniform float4 _Colour_Mask_02_ST;
		uniform float4 _Colour_Skin;
		uniform sampler2D _skin_Mask;
		uniform float4 _skin_Mask_ST;
		uniform float4 _Colour_Hair;
		uniform sampler2D _Hair_Mask;
		uniform float4 _Hair_Mask_ST;
		uniform float4 _Colour_Emissive;
		uniform sampler2D _Emissive_01;
		uniform float4 _Emissive_01_ST;
		uniform float _Spec_Metallic;
		uniform float _Spec_Smoothness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			float2 uv_Colour_Multiply = i.uv_texcoord * _Colour_Multiply_ST.xy + _Colour_Multiply_ST.zw;
			float2 uv_PolygonSciFiPlanets_Texture_01_A = i.uv_texcoord * _PolygonSciFiPlanets_Texture_01_A_ST.xy + _PolygonSciFiPlanets_Texture_01_A_ST.zw;
			float2 uv_Colour_Mask_01 = i.uv_texcoord * _Colour_Mask_01_ST.xy + _Colour_Mask_01_ST.zw;
			float4 lerpResult8 = lerp( tex2D( _PolygonSciFiPlanets_Texture_01_A, uv_PolygonSciFiPlanets_Texture_01_A ) , _Colour_Primary , tex2D( _Colour_Mask_01, uv_Colour_Mask_01 ));
			float2 uv_Colour_Mask_02 = i.uv_texcoord * _Colour_Mask_02_ST.xy + _Colour_Mask_02_ST.zw;
			float4 lerpResult10 = lerp( lerpResult8 , _Colour_Secondary , tex2D( _Colour_Mask_02, uv_Colour_Mask_02 ));
			float2 uv_skin_Mask = i.uv_texcoord * _skin_Mask_ST.xy + _skin_Mask_ST.zw;
			float4 lerpResult11 = lerp( lerpResult10 , _Colour_Skin , tex2D( _skin_Mask, uv_skin_Mask ));
			float2 uv_Hair_Mask = i.uv_texcoord * _Hair_Mask_ST.xy + _Hair_Mask_ST.zw;
			float4 lerpResult37 = lerp( lerpResult11 , _Colour_Hair , tex2D( _Hair_Mask, uv_Hair_Mask ));
			float4 blendOpSrc27 = tex2D( _Colour_Multiply, uv_Colour_Multiply );
			float4 blendOpDest27 = lerpResult37;
			float4 blendOpSrc36 = tex2D( _TextureSample1, uv_TextureSample1 );
			float4 blendOpDest36 = ( saturate( ( blendOpSrc27 * blendOpDest27 ) ));
			float4 lerpBlendMode36 = lerp(blendOpDest36,	max( blendOpSrc36, blendOpDest36 ),0.2);
			o.Albedo = lerpBlendMode36.rgb;
			float4 temp_cast_1 = 0;
			float2 uv_Emissive_01 = i.uv_texcoord * _Emissive_01_ST.xy + _Emissive_01_ST.zw;
			float4 lerpResult19 = lerp( temp_cast_1 , _Colour_Emissive , tex2D( _Emissive_01, uv_Emissive_01 ));
			o.Emission = lerpResult19.rgb;
			o.Metallic = _Spec_Metallic;
			o.Smoothness = _Spec_Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18909
3237;394;2255;1259;1028.925;1594.054;1.865183;True;True
Node;AmplifyShaderEditor.SamplerNode;2;-211.5855,-141.0428;Inherit;True;Property;_Colour_Mask_01;Colour_Mask_01;9;0;Create;True;0;0;0;False;0;False;-1;21e1e09c0ad6bfc488ceb5122f262dea;21e1e09c0ad6bfc488ceb5122f262dea;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-402.5145,-723.0428;Inherit;True;Property;_PolygonSciFiPlanets_Texture_01_A;PolygonSciFiPlanets_Texture_01_A;7;0;Create;True;0;0;0;False;0;False;-1;d44fa91dd607b2243aac51d78b7922dd;d44fa91dd607b2243aac51d78b7922dd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;7;-538.8201,-272.6557;Inherit;False;Property;_Colour_Primary;Colour_Primary;1;0;Create;True;0;0;0;False;0;False;0.3161765,0.2836289,0.2836289,0;0.08953286,0.116427,0.1691176,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;530.7648,-14;Inherit;True;Property;_Colour_Mask_02;Colour_Mask_02;10;0;Create;True;0;0;0;False;0;False;-1;1e4e9659725834246a55cb6ffa1cc7de;1e4e9659725834246a55cb6ffa1cc7de;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;9;389.1942,-265.6135;Inherit;False;Property;_Colour_Secondary;Colour_Secondary;2;0;Create;True;0;0;0;False;0;False;0.3161765,0.2836289,0.2836289,0;0.08953286,0.116427,0.1691176,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;8;150.7913,-418.4291;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;10;853.8059,-549.3865;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;12;952.7765,-177.5468;Inherit;False;Property;_Colour_Skin;Colour_Skin;4;0;Create;True;0;0;0;False;0;False;0.3161765,0.2836289,0.2836289,0;0.08953286,0.116427,0.1691176,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;5;1004,97.49997;Inherit;True;Property;_skin_Mask;skin_Mask;11;0;Create;True;0;0;0;False;0;False;-1;485dd52fb681701489fc06c092d4ff7e;485dd52fb681701489fc06c092d4ff7e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;39;518.0392,-945.9948;Inherit;False;Property;_Colour_Hair;Colour_Hair;0;0;Create;True;0;0;0;False;0;False;0.3098039,0.254902,0.1803922,0;0.08953286,0.116427,0.1691176,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;38;484.3363,-763.9974;Inherit;True;Property;_Hair_Mask;Hair_Mask;8;0;Create;True;0;0;0;False;0;False;-1;2be28d1396e389247a1f73fc6f3bbb8e;21e1e09c0ad6bfc488ceb5122f262dea;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;11;1143.507,-538.1257;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;37;1301.4,-814.0385;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;34;1640.002,-316.081;Inherit;True;Property;_Colour_Multiply;Colour_Multiply;13;0;Create;True;0;0;0;False;0;False;-1;9067817c10abdf447a27ef2eb7177267;9067817c10abdf447a27ef2eb7177267;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;17;1958.978,511.4535;Inherit;True;Property;_Emissive_01;Emissive_01;12;0;Create;True;0;0;0;False;0;False;-1;03db659f8a637b744943d87d1a6d7c5e;03db659f8a637b744943d87d1a6d7c5e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;20;2324.278,209.853;Inherit;False;Constant;_Int0;Int 0;13;0;Create;True;0;0;0;False;0;False;0;0;False;0;1;INT;0
Node;AmplifyShaderEditor.BlendOpsNode;27;1680.858,-560.6263;Inherit;False;Multiply;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;18;1778.678,-29.14692;Inherit;False;Property;_Colour_Emissive;Colour_Emissive;3;0;Create;True;0;0;0;False;0;False;0.3161765,0.2836289,0.2836289,0;0.08953286,0.116427,0.1691176,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;35;1735.353,-861.564;Inherit;True;Property;_TextureSample1;Texture Sample 1;14;0;Create;True;0;0;0;False;0;False;-1;965f16c76090fef4b9826d9a5dfb91e3;965f16c76090fef4b9826d9a5dfb91e3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;19;2183.878,-87.84705;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;36;2177.071,-618.9709;Inherit;False;Lighten;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.2;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;16;2503.344,529.1121;Inherit;False;Property;_Spec_Metallic;Spec_Metallic;6;0;Create;True;0;0;0;False;0;False;0.5;0.254;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;2517.144,317.6406;Inherit;False;Property;_Spec_Smoothness;Spec_Smoothness;5;0;Create;True;0;0;0;False;0;False;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2988.464,4.028276;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;SyntyStudios/ColourChange;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;8;0;1;0
WireConnection;8;1;7;0
WireConnection;8;2;2;0
WireConnection;10;0;8;0
WireConnection;10;1;9;0
WireConnection;10;2;3;0
WireConnection;11;0;10;0
WireConnection;11;1;12;0
WireConnection;11;2;5;0
WireConnection;37;0;11;0
WireConnection;37;1;39;0
WireConnection;37;2;38;0
WireConnection;27;0;34;0
WireConnection;27;1;37;0
WireConnection;19;0;20;0
WireConnection;19;1;18;0
WireConnection;19;2;17;0
WireConnection;36;0;35;0
WireConnection;36;1;27;0
WireConnection;0;0;36;0
WireConnection;0;2;19;0
WireConnection;0;3;16;0
WireConnection;0;4;15;0
ASEEND*/
//CHKSM=CF07978A6EF6AEF82557FF60BC4CC6B4CD6CB923