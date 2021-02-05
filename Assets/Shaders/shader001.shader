Shader "Custom/shader001"
{
	Properties
	{
		_MainTex("Diffuse", 2D) = "white" {}
		_MaskTex("Mask", 2D) = "white" {}
		_NormalMap("Normal Map", 2D) = "bump" {}

		_AspectRatio("AspectRatio",float) =1 //x:y
		_PixelPreUnit("PixelPreUnit",float) =1 //x:y
		_lineWidth("lineWidth",float) = 1
		
		_lineColor("lineColor",Color) = (1,1,1,1)

		// Legacy properties. They're here so that materials using this shader can gracefully fallback to the legacy sprite shader.
		[HideInInspector] _Color("Tint", Color) = (1,1,1,1)
		[HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
		[HideInInspector] _AlphaTex("External Alpha", 2D) = "white" {}
		[HideInInspector] _EnableExternalAlpha("Enable External Alpha", Float) = 0
	}

	HLSLINCLUDE
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
	ENDHLSL

	SubShader
	{
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		ZWrite Off

		Pass
		{
			Tags { "LightMode" = "Universal2D" }
			HLSLPROGRAM
			#pragma prefer_hlslcc gles
			#pragma vertex CombinedShapeLightVertex
			#pragma fragment CombinedShapeLightFragment
			#pragma multi_compile USE_SHAPE_LIGHT_TYPE_0 __
			#pragma multi_compile USE_SHAPE_LIGHT_TYPE_1 __
			#pragma multi_compile USE_SHAPE_LIGHT_TYPE_2 __
			#pragma multi_compile USE_SHAPE_LIGHT_TYPE_3 __

			struct Attributes
			{
				float3 positionOS   : POSITION;
				float4 color        : COLOR;
				float2  uv           : TEXCOORD0;
			};

			struct Varyings
			{
				float4  positionCS  : SV_POSITION;
				float4  color       : COLOR;
				float2	uv          : TEXCOORD0;
				float2	lightingUV  : TEXCOORD1;
			};

			#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"

			TEXTURE2D(_MainTex);
			SAMPLER(sampler_MainTex);
			TEXTURE2D(_MaskTex);
			SAMPLER(sampler_MaskTex);
			TEXTURE2D(_NormalMap);
			SAMPLER(sampler_NormalMap);
			half4 _MainTex_ST;
			half4 _NormalMap_ST;

			float _AspectRatio;
			float _PixelPreUnit;
			float _lineWidth;
			float4 _lineColor;

			#if USE_SHAPE_LIGHT_TYPE_0
			SHAPE_LIGHT(0)
			#endif

			#if USE_SHAPE_LIGHT_TYPE_1
			SHAPE_LIGHT(1)
			#endif

			#if USE_SHAPE_LIGHT_TYPE_2
			SHAPE_LIGHT(2)
			#endif

			#if USE_SHAPE_LIGHT_TYPE_3
			SHAPE_LIGHT(3)
			#endif

			Varyings CombinedShapeLightVertex(Attributes v)
			{
				Varyings o = (Varyings)0;

				o.positionCS = TransformObjectToHClip(v.positionOS);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				float4 clipVertex = o.positionCS / o.positionCS.w;
				o.lightingUV = ComputeScreenPos(clipVertex).xy;
				o.color = v.color;
				return o;
			}

			#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/CombinedShapeLightShared.hlsl"

			//half4 frag(Varyings i) : SV_Target
			//{
			//	half4 col = (half4)0;
			//	// 采样周围4个点
			//	float2 up_uv = i.uv + float2(0,1) * _lineWidth * _MainTex_TexelSize.xy;
			//	float2 down_uv = i.uv + float2(0,-1) * _lineWidth * _MainTex_TexelSize.xy;
			//	float2 left_uv = i.uv + float2(-1,0) * _lineWidth * _MainTex_TexelSize.xy;
			//	float2 right_uv = i.uv + float2(1,0) * _lineWidth * _MainTex_TexelSize.xy;
			//	// 如果有一个点透明度为0 说明是边缘
			//	float w = tex2D(_MainTex,up_uv).a * tex2D(_MainTex,down_uv).a * tex2D(_MainTex,left_uv).a * tex2D(_MainTex,right_uv).a;
	
			//	// if(w == 0){
			//		//    col.rgb = _lineColor;
			//	// }
			//	// 和原图做插值
			//	col.rgb = lerp(_lineColor,col.rgb,w);
			//	return col;
			//}
			float2 mygenf2_float2(in float pixelPerUnit,in int2 a)
			{
				return float2(a.x / pixelPerUnit *_lineWidth * _AspectRatio , a.y / pixelPerUnit *_lineWidth);
			};

			half4 CombinedShapeLightFragment(Varyings i) : SV_Target
			{
				half4 main;
				half4 mask;
				if (SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv).a == 0 )
				{	
					// float2 upleft_uv = i.uv + 	TRANSFORM_TEX(float2(-0.02*_lineWidth, 0.02 *_lineWidth)	,_MainTex);
					// float2 up_uv = i.uv + 		TRANSFORM_TEX(float2(0, 0.02				*_lineWidth)	,_MainTex);
					// float2 upright_uv = i.uv + 	TRANSFORM_TEX(float2(+0.02*_lineWidth, 0.02 *_lineWidth)	, _MainTex);
				
					// float2 downleft_uv = i.uv + TRANSFORM_TEX(float2(-0.02*_lineWidth, -0.02 *_lineWidth)	, _MainTex);
					// float2 down_uv = i.uv + 	TRANSFORM_TEX(float2(0, -0.02 				*_lineWidth)	, _MainTex);
					// float2 downright_uv = i.uv +TRANSFORM_TEX(float2(0.02*_lineWidth, -0.02 *_lineWidth)	, _MainTex);

					// float2 left_uv = i.uv + TRANSFORM_TEX(float2(0.02 *_lineWidth, 0), _MainTex);
					// float2 right_uv = i.uv + TRANSFORM_TEX(float2(-0.02*_lineWidth, 0), _MainTex);
				//
					float2 upleft_uv = i.uv + 	TRANSFORM_TEX(mygenf2_float2(_PixelPreUnit,int2(-1,1))	,_MainTex);
					float2 up_uv = i.uv + 		TRANSFORM_TEX(mygenf2_float2(_PixelPreUnit,int2(0,1))	,_MainTex);
					float2 upright_uv = i.uv + 	TRANSFORM_TEX(mygenf2_float2(_PixelPreUnit,int2(1,1))	, _MainTex);
				
					float2 downleft_uv = i.uv + TRANSFORM_TEX(mygenf2_float2(_PixelPreUnit,int2(-1,-1))	, _MainTex);
					float2 down_uv = i.uv + 	TRANSFORM_TEX(mygenf2_float2(_PixelPreUnit,int2(0,-1))	, _MainTex);
					float2 downright_uv = i.uv +TRANSFORM_TEX(mygenf2_float2(_PixelPreUnit,int2(1,-1))	, _MainTex);

					float2 left_uv = i.uv + TRANSFORM_TEX(mygenf2_float2(_PixelPreUnit,int2(-1,0)), _MainTex);
					float2 right_uv = i.uv + TRANSFORM_TEX(mygenf2_float2(_PixelPreUnit,int2(1,0)), _MainTex);

					float w = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, upleft_uv).a +
						SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, up_uv).a +
						SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, upright_uv).a +
						SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, downleft_uv).a +
						SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, down_uv).a +
						SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, downright_uv).a +
						SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, left_uv).a +
						SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, right_uv).a;
					if ((w>=1 && w<=8) )
					{
						main = _lineColor;
						mask = SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, i.uv);
						return CombinedShapeLightShared(main, mask, i.lightingUV);
					}
				}
				
				main = i.color * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
				mask = SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, i.uv);
				half4 rev = CombinedShapeLightShared(main, mask, i.lightingUV);
				return rev;

			}
			ENDHLSL
		}

		Pass
		{
			Tags { "LightMode" = "NormalsRendering"}
			HLSLPROGRAM
			#pragma prefer_hlslcc gles
			#pragma vertex NormalsRenderingVertex
			#pragma fragment NormalsRenderingFragment

			struct Attributes
			{
				float3 positionOS   : POSITION;
				float4 color		: COLOR;
				float2 uv			: TEXCOORD0;
				float4 tangent      : TANGENT;
			};

			struct Varyings
			{
				float4  positionCS		: SV_POSITION;
				float4  color			: COLOR;
				float2	uv				: TEXCOORD0;
				float3  normalWS		: TEXCOORD1;
				float3  tangentWS		: TEXCOORD2;
				float3  bitangentWS		: TEXCOORD3;
			};

			TEXTURE2D(_MainTex);
			SAMPLER(sampler_MainTex);
			TEXTURE2D(_NormalMap);
			SAMPLER(sampler_NormalMap);
			float4 _NormalMap_ST;  // Is this the right way to do this?

			Varyings NormalsRenderingVertex(Attributes attributes)
			{
				Varyings o = (Varyings)0;

				o.positionCS = TransformObjectToHClip(attributes.positionOS);
				o.uv = TRANSFORM_TEX(attributes.uv, _NormalMap);
				o.uv = attributes.uv;
				o.color = attributes.color;
				o.normalWS = TransformObjectToWorldDir(float3(0, 0, -1));
				o.tangentWS = TransformObjectToWorldDir(attributes.tangent.xyz);
				o.bitangentWS = cross(o.normalWS, o.tangentWS) * attributes.tangent.w;
				return o;
			}

			#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/NormalsRenderingShared.hlsl"

			float4 NormalsRenderingFragment(Varyings i) : SV_Target
			{
				float4 mainTex = i.color * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
				float3 normalTS = UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, i.uv));
				return NormalsRenderingShared(mainTex, normalTS, i.tangentWS.xyz, i.bitangentWS.xyz, i.normalWS.xyz);
				return 0;
			}
			ENDHLSL
		}
		Pass
		{
			Tags { "LightMode" = "UniversalForward" "Queue" = "Transparent" "RenderType" = "Transparent"}

			HLSLPROGRAM
			#pragma prefer_hlslcc gles
			#pragma vertex UnlitVertex
			#pragma fragment UnlitFragment

			struct Attributes
			{
				float3 positionOS   : POSITION;
				float4 color		: COLOR;
				float2 uv			: TEXCOORD0;
			};

			struct Varyings
			{
				float4  positionCS		: SV_POSITION;
				float4  color			: COLOR;
				float2	uv				: TEXCOORD0;
			};

			TEXTURE2D(_MainTex);
			SAMPLER(sampler_MainTex);
			float4 _MainTex_ST;

			Varyings UnlitVertex(Attributes attributes)
			{

				Varyings o = (Varyings)0;

				o.positionCS = TransformObjectToHClip(attributes.positionOS);
				o.uv = TRANSFORM_TEX(attributes.uv, _MainTex);
				o.uv = attributes.uv;
				o.color = attributes.color;
				return o;
			}

			float4 UnlitFragment(Varyings i) : SV_Target
			{
				float4 mainTex = i.color * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
				return mainTex;
			}
			ENDHLSL
		}
	}

	Fallback "Sprites/Default"
}
