// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SyntyStudios/Hologram"
{
	Properties
	{
		_HoloLines("HoloLines", 2D) = "white" {}
		_Neon_Colour_01("Neon_Colour_01", Color) = (0.6965517,1,0,0)
		_Emission_Power("Emission_Power", Range( 0 , 10)) = 0
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Scroll_Speed("Scroll_Speed", Range( 0 , 10)) = 0.1
		_Opacity("Opacity", Range( 0 , 1)) = 0.8
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Pass
		{
			ColorMask 0
			ZWrite On
		}

		Tags{ "RenderType" = "Transparent"  "Queue" = "Overlay+0" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float4 _Neon_Colour_01;
		uniform sampler2D _HoloLines;
		uniform float _Scroll_Speed;
		uniform float _Emission_Power;
		uniform float _Opacity;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 tex2DNode44 = tex2D( _TextureSample0, uv_TextureSample0 );
			o.Albedo = tex2DNode44.rgb;
			float4 color32 = IsGammaSpace() ? float4(0,0,0,0) : float4(0,0,0,0);
			float4 lerpResult38 = lerp( _Neon_Colour_01 , color32 , 0.3);
			float2 temp_cast_1 = (_Scroll_Speed).xx;
			float3 ase_worldPos = i.worldPos;
			float2 temp_cast_2 = (ase_worldPos.y).xx;
			float2 panner3 = ( 1.0 * _Time.y * temp_cast_1 + temp_cast_2);
			float4 lerpResult20 = lerp( lerpResult38 , _Neon_Colour_01 , tex2D( _HoloLines, panner3 ));
			o.Emission = ( ( lerpResult20 * _Emission_Power ) * tex2DNode44 ).rgb;
			o.Alpha = _Opacity;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18909
-2894;152;2052;1359;2178.088;1130.052;2.239373;True;True
Node;AmplifyShaderEditor.WorldPosInputsNode;43;-2181.502,-243.313;Float;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;89;-1948.834,109.2431;Inherit;False;Property;_Scroll_Speed;Scroll_Speed;5;0;Create;True;0;0;0;False;0;False;0.1;0.59;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-710.4459,108.7375;Float;False;Constant;_GenericCalue;GenericCalue;8;0;Create;True;0;0;0;False;0;False;0.3;6.24;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;3;-1796.908,-150.8578;Inherit;False;3;0;FLOAT2;1,1;False;2;FLOAT2;0,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;32;-462.211,506.4921;Inherit;False;Constant;_Color0;Color 0;3;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.6965517,1,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;5;-596.5,-158.5;Inherit;False;Property;_Neon_Colour_01;Neon_Colour_01;2;0;Create;True;0;0;0;False;0;False;0.6965517,1,0,0;0.8455882,0.4733922,0.410359,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;38;-388.0394,75.85798;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;4;-1317.402,-17.06828;Inherit;True;Property;_HoloLines;HoloLines;1;0;Create;True;0;0;0;False;0;False;-1;54b3339ded3457f42945715232faa0bc;54b3339ded3457f42945715232faa0bc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;20;-216.1889,227.906;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;25;38.46143,304.058;Float;False;Property;_Emission_Power;Emission_Power;3;0;Create;True;0;0;0;False;0;False;0;1.68;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;44;426.0507,-293.8145;Inherit;True;Property;_TextureSample0;Texture Sample 0;4;0;Create;True;0;0;0;False;0;False;-1;None;d44fa91dd607b2243aac51d78b7922dd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;330.2929,143.6239;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;10;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;90;586.7337,331.762;Inherit;False;Property;_Opacity;Opacity;6;0;Create;True;0;0;0;False;0;False;0.8;0.73;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;786.538,192.9812;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;88;1159.513,26.59543;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;SyntyStudios/Hologram;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;True;0;Custom;0.53;True;False;0;True;Transparent;;Overlay;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;43;2
WireConnection;3;2;89;0
WireConnection;38;0;5;0
WireConnection;38;1;32;0
WireConnection;38;2;40;0
WireConnection;4;1;3;0
WireConnection;20;0;38;0
WireConnection;20;1;5;0
WireConnection;20;2;4;0
WireConnection;24;0;20;0
WireConnection;24;1;25;0
WireConnection;45;0;24;0
WireConnection;45;1;44;0
WireConnection;88;0;44;0
WireConnection;88;2;45;0
WireConnection;88;9;90;0
ASEEND*/
//CHKSM=942A3D61CE5404C0C7127EB05A1CB5BB58719CE7