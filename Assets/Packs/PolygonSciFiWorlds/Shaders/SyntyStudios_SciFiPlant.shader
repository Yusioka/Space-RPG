// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SyntyStudios/SciFiPlant"
{
	Properties
	{
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Plant_Colour("Plant_Colour", Color) = (0.3235835,0.5864827,0.7720588,0)
		_Emissive_Colour("Emissive_Colour", Color) = (0,0,0,0)
		_Tree_NoiseTexture1("Tree_NoiseTexture", 2D) = "white" {}
		_Big_WindAmount("Big_WindAmount", Float) = 1
		_Small_Wave("Small_Wave", Range( 0 , 10)) = 0
		_Small_WindSpeed("Small_WindSpeed", Float) = 0
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_Plant_Mask("Plant_Mask", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
		[Header(Forward Rendering Options)]
		[ToggleOff] _GlossyReflections("Reflections", Float) = 1.0
	}

	SubShader
	{
		Tags{ "RenderType" = "TreeOpaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma shader_feature _GLOSSYREFLECTIONS_OFF
		#pragma surface surf StandardSpecular keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _Big_WindAmount;
		uniform sampler2D _Tree_NoiseTexture1;
		uniform float _Small_WindSpeed;
		uniform float _Small_Wave;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float4 _Plant_Colour;
		uniform sampler2D _Plant_Mask;
		uniform float4 _Plant_Mask_ST;
		uniform float4 _Emissive_Colour;
		uniform sampler2D _TextureSample2;
		uniform float4 _TextureSample2_ST;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float2 temp_cast_0 = (( ( ase_vertex3Pos.x + ( _Time.y * _Small_WindSpeed ) ) / ( 1.0 - _Small_Wave ) )).xx;
			float lerpResult18 = lerp( 0.0 , ( _Big_WindAmount * tex2Dlod( _Tree_NoiseTexture1, float4( temp_cast_0, 0, 0.0) ).r ) , v.color.r);
			float3 appendResult19 = (float3(lerpResult18 , 0.0 , 0.0));
			v.vertex.xyz += float3( (appendResult19).xz ,  0.0 );
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float2 uv_Plant_Mask = i.uv_texcoord * _Plant_Mask_ST.xy + _Plant_Mask_ST.zw;
			float4 lerpResult39 = lerp( tex2D( _TextureSample0, uv_TextureSample0 ) , _Plant_Colour , tex2D( _Plant_Mask, uv_Plant_Mask ));
			o.Albedo = lerpResult39.rgb;
			float2 uv_TextureSample2 = i.uv_texcoord * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
			float4 lerpResult4 = lerp( float4( 0,0,0,0 ) , _Emissive_Colour , tex2D( _TextureSample2, uv_TextureSample2 ));
			o.Emission = lerpResult4.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18909
-2649;122;2390;1557;2326.341;504.0316;1.373608;True;True
Node;AmplifyShaderEditor.CommentaryNode;7;-1737.869,577.579;Inherit;False;1333.21;549.45;Blue Vertex;13;20;19;18;17;16;15;14;29;28;32;30;33;35;Tree Vertex Animation;0,0.3379312,1,1;0;0
Node;AmplifyShaderEditor.SimpleTimeNode;28;-1610.936,843.9158;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-1609.685,982.2954;Float;False;Property;_Small_WindSpeed;Small_WindSpeed;7;0;Create;True;0;0;0;False;0;False;0;0.79;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-1626.809,1110.695;Float;False;Property;_Small_Wave;Small_Wave;6;0;Create;True;0;0;0;False;0;False;0;8.43;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;32;-1730.477,586.4956;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-1385.113,842.1677;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;34;-1308.978,1091.04;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;33;-1406.785,653.681;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;35;-1132.505,877.2847;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;14;-1010.89,921.665;Inherit;True;Property;_Tree_NoiseTexture1;Tree_NoiseTexture;3;0;Create;False;0;0;0;False;0;False;-1;661313bdb77fa944882bb60df3b5cfb8;661313bdb77fa944882bb60df3b5cfb8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;15;-1049.15,759.4714;Float;False;Property;_Big_WindAmount;Big_WindAmount;5;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-776.3145,769.4919;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;17;-1246.036,628.9259;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;18;-601.302,614.4567;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-696.1995,-307.2;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;19;-589.7708,826.2839;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;43;-744.4391,-55.69666;Inherit;True;Property;_Plant_Mask;Plant_Mask;9;0;Create;True;0;0;0;False;0;False;-1;1fcd00d11947d51428515be1a2f64679;1fcd00d11947d51428515be1a2f64679;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;37;-988.34,280.8035;Inherit;True;Property;_TextureSample2;Texture Sample 2;8;0;Create;True;0;0;0;False;0;False;-1;a6cf282deec624741b185ff6bfae5cc6;a6cf282deec624741b185ff6bfae5cc6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;38;-432.1393,-90.49666;Inherit;False;Property;_Plant_Colour;Plant_Colour;1;0;Create;True;0;0;0;False;0;False;0.3235835,0.5864827,0.7720588,0;0.3970588,0.1401384,0.1401384,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;3;-1004.799,100.7004;Inherit;False;Property;_Emissive_Colour;Emissive_Colour;2;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.3970588,0.1401384,0.1401384,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;20;-626.9183,1035.763;Inherit;False;True;False;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;4;-131.5,210.5;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;39;3.360456,-42.39668;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;288,-16;Float;False;True;-1;2;ASEMaterialInspector;0;0;StandardSpecular;SyntyStudios/SciFiPlant;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;TreeOpaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;4;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;30;0;28;0
WireConnection;30;1;29;0
WireConnection;34;0;31;0
WireConnection;33;0;32;1
WireConnection;33;1;30;0
WireConnection;35;0;33;0
WireConnection;35;1;34;0
WireConnection;14;1;35;0
WireConnection;16;0;15;0
WireConnection;16;1;14;1
WireConnection;18;1;16;0
WireConnection;18;2;17;1
WireConnection;19;0;18;0
WireConnection;20;0;19;0
WireConnection;4;1;3;0
WireConnection;4;2;37;0
WireConnection;39;0;1;0
WireConnection;39;1;38;0
WireConnection;39;2;43;0
WireConnection;0;0;39;0
WireConnection;0;2;4;0
WireConnection;0;11;20;0
ASEEND*/
//CHKSM=E53421C5A2EE86F3C3D3080941883D398944A8EE