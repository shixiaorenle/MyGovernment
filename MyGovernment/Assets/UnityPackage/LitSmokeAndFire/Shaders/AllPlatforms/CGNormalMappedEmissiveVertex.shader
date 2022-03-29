// Upgrade NOTE: replaced 'defined SOFTPARTICLES_ON' with 'defined (SOFTPARTICLES_ON)'

// Upgrade NOTE: replaced 'defined SPOTLIGHTS_ON' with 'defined (SPOTLIGHTS_ON)'

Shader "Encap/LitSmokeAndFire/CGNormalMappedEmisiveVertex"
{
	Properties{
		_MainTex("Normal(RGB), Alpha (A)", 2D) = "white" {}


	//[Toggle()] _UsingNonSquareUVs("Using non square UVs", Float) = 0
	//_UVColumns("UV Columns", Float) = 1
	//_UVRows("UV Columns", Float) = 1
	//_NonSquareUVFix("Non Square UV Fix" , Vector) = (1,1,1,1)
	_ParticleTint("Color Tint",Color) = (1,1,1,1)
	_ParticleMaxBrightnesCap("Max Brightness Colour",Color) = (1,1,1,1)
		_ParticleMinBrightnesCap("Min Brightness Colour",Color) = (0,0,0,0)

		_LightScatterSharpness("Light Scatter Sharpness", float) = 1
		_AlphaScatterEffect("Transparency effect on light Scatter ", float) = 1
		_AlphaScatterSharpness("Transparency effect on light Sharpness ", float) = 1
		_ScatterLightMultiplyer("Light Scatter Multiplyer", Range(0, 10)) = 1

		_LightWrapAround("Light wrap around", float) = 1
		_ShadowEffectMultiplyer("Shadow effect multiplyer", float) = 1
		_LightPower("Light Sharpness" , float) = 2

		//light calculation options 
		[Toggle()] _VertexLightOptionsToggle("Use Different Options for vertex", float) = 0

		_VertexLightWrapAround("Vertex Light wrap around", float) = 1
		_VertexShadowEffectMultiplyer("Vertex Shadow Effect Multiplyer", float) = 1
		_VertexLightPower("Vertex Light Sharpness" , float) = 2
		_VertexLightBoost("Vertex Light Brightness boost", float) = 1

		_InvFade("Soft Particle Thickness", float) = 1

		_DistanceFadeStart("Distance Fade Start", float) = 1
		_DistanceFadeEnd("Distance Fade End", float) = 1
		_DistanceFadeRate("Distance Fade Rate", float) = 1

		[Toggle()] _AdvancedBlendMode("SrcMode", Float) = 0
		_BasicMode("Basic Blend Mode", Float) = 0
		_BlendOP("blend operation" , Float) = 0
		_SrcMode("SrcMode", Float) = 0
		_DstMode("DstMode", Float) = 0
		
		_EmissiveMultiplyer("Emissive Multiplyer",  Range(0,20)) = 1

		_LightCount("_LightCount", int) = 0
		[HideInInspector] _LightCountFrag("_LightCountFrag", int) = 1
		_VertexLightCount("_VertexLightCount", int) = 0.0



		//light scattering options 
		[Toggle(LIGHT_SCATTERING_ON)] _LightScatteringToggle("Light Scattering", float) = 0

		//Spotlight Options 
		[Toggle(SPOTLIGHTS_ON)] _SpotlightToggle("Spotlight Support", float) = 0

		//Orthographic Options 
		//[Toggle(ORTHOGRAPHIC)] _OrthographicToggle("Orthographic Perspective", float) = 0

		//Orthographic Options 
		[Toggle(FRAME_BLENDING)] _FrameBlendingToggle("Sprite Sheet Frame Blending", float) = 0
	}

		SubShader
	{

		Tags{ "LightMode" = "Vertex"  "Queue" = "Transparent" }
		Tags{ "Queue" = "Transparent"   "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		ZWrite Off

		Pass
	{

		BlendOp[_BlendOP]
		Blend[_SrcMode][_DstMode]

		CGPROGRAM

#pragma multi_compile_fog
#pragma multi_compile_particles
#pragma multi_compile __ PIXEL_LIGHT_OPTION_BIT_1
#pragma multi_compile __ PIXEL_LIGHT_OPTION_BIT_2
#pragma multi_compile __ LIGHT_SCATTERING_ON
#pragma multi_compile __ SPOTLIGHTS_ON
//pragma multi_compile __ ORTHOGRAPHIC
#pragma multi_compile __ FRAME_BLENDING

#pragma vertex vert
#pragma fragment frag
		// make fog work
#pragma multi_compile_fog

#include "UnityCG.cginc"


		//--------------------------------- Definition Sorting ----------------------------

#if !defined(SOFTPARTICLES_ON) &&  !defined(LIGHT_SCATTERING_ON)
#define NOT_USING_VERT_VIEW_POS
#else
#define USING_VERT_VIEW_POS
#endif

		//pixel light options
#if !defined(PIXEL_LIGHT_OPTION_BIT_1) && !defined(PIXEL_LIGHT_OPTION_BIT_2)		
#define PIXEL_LIGHT_1
#define PIXEL_LIGHT_COUNT 1
#endif

#if defined(PIXEL_LIGHT_OPTION_BIT_1) && !defined(PIXEL_LIGHT_OPTION_BIT_2)		
#define PIXEL_LIGHT_1
#define PIXEL_LIGHT_2
#define PIXEL_LIGHT_COUNT 2
#endif

#if !defined(PIXEL_LIGHT_OPTION_BIT_1) && defined(PIXEL_LIGHT_OPTION_BIT_2)		
#define PIXEL_LIGHT_1
#define PIXEL_LIGHT_2
#define PIXEL_LIGHT_3
#define PIXEL_LIGHT_COUNT 3
#endif

#if defined(PIXEL_LIGHT_OPTION_BIT_1) && defined(PIXEL_LIGHT_OPTION_BIT_2)		
#define PIXEL_LIGHT_1
#define PIXEL_LIGHT_2
#define PIXEL_LIGHT_3



//define PIXEL_LIGHT_4
#define PIXEL_LIGHT_COUNT 3


#endif

		//--------------------------------- Defines ----------------------------------------
#if defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2)
#define CALCULATE_PARTICLE_FOG_FACTOR(Distance,Destination)  UNITY_CALC_FOG_FACTOR(Distance); Destination = unityFogFactor
#else
		//define null value to avoid calculations when not needed
#define CALCULATE_PARTICLE_FOG_FACTOR(Distance,Destination)
#endif

#if defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2)
#define APPLY_PARTICLE_FOG(FogFactor, Destination)   UNITY_FOG_LERP_COLOR(Destination, unity_FogColor, FogFactor)
#else
		//define null value to avoid calculations when not needed
#define APPLY_PARTICLE_FOG(FogFactor,Destination)
#endif

#if defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2)
#define CALC_AND_APPLY_PARTICLE_FOG(Distance, Destination) UNITY_CALC_FOG_FACTOR(Distance); UNITY_FOG_LERP_COLOR(Destination, unity_FogColor, unityFogFactor)
#else
		//define null value to avoid calculations when not needed
#define CALC_AND_APPLY_PARTICLE_FOG(Distance,Destination)
#endif

		//---------------------------------------------- Functions ----------------------------------------------
		//calculate the normal lighting 
		float NormalLightEffect(float3 normal, float3 lightdirection, float LightWrapAround, float ShadowEffectMultiplyer, float LightSharpness)
	{
		//dot the light to the light source
		float fLightDot = dot(lightdirection, normal);

		return pow((clamp((fLightDot * LightWrapAround) + 1.0 - LightWrapAround, 0.0, 1.0) * ShadowEffectMultiplyer) + (1.0 - ShadowEffectMultiplyer), LightSharpness);
		//float fLight  = (clamp((dot(lightdirection, normal) * LightWrapAround) + 1.0 - LightWrapAround, 0.0, 1.0) * ShadowEffectMultiplyer) + (1.0 - ShadowEffectMultiplyer);

		//return fLight * fLight;
	}

	float ScatterLightEffect(float LightZ,float Alpha, float LightScatterSharpness, float AlphaScatterSharpness, float AlphaScatterEffect)
	{
	
		return clamp(pow(saturate(-LightZ), LightScatterSharpness) * (1.0 - clamp((pow(Alpha, AlphaScatterSharpness) * AlphaScatterEffect), 0.0, 1.0)), 0.0, 1.0);

	}

	void CalcDistanceAndDirectionToLight(float3 LightPos, float LightType, float3 VertPos,out float3 lightDirecton, out float LengthSquared)
	{
		lightDirecton = LightPos - VertPos * LightType;
		LengthSquared = dot(lightDirecton, lightDirecton);
		lightDirecton *= rsqrt(LengthSquared);

		LengthSquared * LightType;
	}

	float CalculateDistanceAttenuationVertex(float DistanceSquared, float4 LightDistanceAttenuation)
	{
		return clamp(1.0 / (1.0 + (DistanceSquared * LightDistanceAttenuation.z)) *  (1.0 - (DistanceSquared / LightDistanceAttenuation.w)), 0.0, 1.0);
	}

	float CalculateDistanceAttenuation(sampler2D AtteuationLookupTexture, float DistanceSquared, float4 LightDistanceAttenuation)
	{
		return tex2D(AtteuationLookupTexture, float2(DistanceSquared / LightDistanceAttenuation.w,0)).w;
	}

	float CalculateSpotlightAttenuation(float3 LightDirection, float3 SpotlightDirection, float2 SpotlightAngleValues)
	{
		return clamp((clamp(dot(LightDirection, SpotlightDirection), 0.0, 1.0) - SpotlightAngleValues.x)  * SpotlightAngleValues.y, 0.0, 1.0);
	}


	void RotateNormal(inout float3 Normal, in float fRotation)
	{
		//get the rotation sin and cos
		float2 DeltaUV = float2(cos(fRotation), sin(fRotation));

		Normal = float3((Normal.x * DeltaUV.x) + (Normal.y * DeltaUV.y), (Normal.x * -DeltaUV.y) + (Normal.y *  DeltaUV.x), Normal.z);
	}

	void RotateNormal(inout float3 Normal,  float2 Rotation)
	{

		Normal = float3((Normal.x * Rotation.x) + (Normal.y * Rotation.y), (Normal.x * -Rotation.y) + (Normal.y *  Rotation.x), Normal.z);
	}

	void RotateNormal(inout float3 Normal, float3 EyeX, float3 EyeY, float3 EyeZ)
	{

		Normal = float3(dot(Normal, EyeX), dot(Normal, EyeY), dot(Normal, EyeZ));
	}

	void CalculateEyeSpaceDirections(inout float3 ViewX, inout float3 ViewY, float3 ViewZ, float fRotation)
	{
		//rotate space up
		RotateNormal(ViewY, fRotation);

		//calculate x
		ViewX = cross(ViewZ, ViewY);

		//recalculate y 
		ViewY = cross(ViewZ, ViewX);
	}

	float3 ReconstructNormal(float3 NormalSource)
	{
		return (NormalSource - float3(0.5, 0.5, 0.5)) * float3(2,2,2);
	}

	float3 ReconstructNormal(float2 NormalSource)
	{
		float3 Normal = float3((NormalSource - float2(0.5,0.5)) * float2(2.0,2.0), 1.0);

		Normal.z = sqrt(1.0 - clamp(dot(Normal.xy, Normal.xy), 0.0, 1.0));

		return Normal;
	}



	//setup values for particle soft factor
	void VertParticleSoftFactor(float4 VertViewPos, out float2 ScreenSpaceCords)
	{
		//convert projected cords into screenspace cords
		ScreenSpaceCords.xy = VertViewPos.xy;
		ScreenSpaceCords.xy /= VertViewPos.w;
#if defined(SHADER_API_OPENGL) || defined(SHADER_API_GLCORE) || defined(SHADER_API_GLES)|| defined(SHADER_API_GLES3)|| defined(SHADER_API_METAL)
		ScreenSpaceCords.xy = (ScreenSpaceCords.xy * 0.5) + 0.5;
#else
		ScreenSpaceCords.xy = (ScreenSpaceCords.xy * float2(0.5, -0.5)) + 0.5;
#endif
	}

	// Z buffer to linear depth
	float LinearEyeDepth(float z, float4 _ZBufferParams)
	{
		return 1.0 / (_ZBufferParams.z * z + _ZBufferParams.w);
	}

	//calculate soft particel factor
	float FragmentParticleSoftFactor(sampler2D ScreenDepth, float2 ScreenSpaceCord, float ParticleDepth, float _InvFade, float4 _ZBufferParams)
	{
		//	//compare current depth of pixel to new pixel depth

		return clamp(abs(_InvFade * (LinearEyeDepth(tex2D(ScreenDepth, ScreenSpaceCord).r, _ZBufferParams) - ParticleDepth)), 0.0, 1.0);
	}

	void CalculateFogFactor(out float FogFactor, float Distance, float4 unity_FogParams)
	{
#if defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2)

#if defined(FOG_LINEAR)
		// factor = (end-z)/(end-start) = z * (-1/(end-start)) + (end/(end-start))
		FogFactor = -Distance * unity_FogParams.z + unity_FogParams.w;
#elif defined(FOG_EXP)
		// factor = exp(-density*z)
		FogFactor = unity_FogParams.y * -Distance;
		FogFactor = exp2(-FogFactor);
#elif defined(FOG_EXP2)
		// factor = exp(-(density*z)^2)
		FogFactor = unity_FogParams.x * -Distance;
		FogFactor = exp2(-FogFactor*FogFactor);
#else
		FogFactor = 0.0;
#endif

#else
		FogFactor = 0.0;
		//define null value to avoid calculations when not needed
#endif
	}

	void ApplyFogFactor(inout float3 TargetColour, float3 FogColour, float FogFactor)
	{
#if defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2)
		TargetColour = lerp(FogColour, TargetColour, clamp(FogFactor, 0.0, 1.0));
#else
		//define null value to avoid calculations when not needed
#endif
	}

	void ApplyDistanceFade(inout float fAlpha, inout float4 VertPos, float Distance, float DistanceFadeStart, float DistanceFadeRate)
	{
		fAlpha *= clamp((-Distance - DistanceFadeStart) * DistanceFadeRate, 0.0, 1.0);

		//hide particle when range limit reached
		VertPos = lerp(float4(9999.0, 9999.0, 9999.0, 9999.0), VertPos, clamp(fAlpha * 10000000.0, 0.0, 1.0));
	}

	//uniform float4 unity_FogParams;

	//uniform float4 unity_LightColor[8];
	//uniform float4 unity_LightPosition[8];
	//uniform float4 unity_LightAtten[8];
	//uniform float4 unity_SpotDirection[8];
	//uniform float4 _ZBufferParams;
	//uniform float4 unity_FogColor;

	//pixel lighting
	//uniform int _LightCount;
	uniform float _LightWrapAround;
	uniform float _ShadowEffectMultiplyer;
	uniform float _LightPower;
	uniform float _InvFade;

	//vertex lighiting 
	uniform int _VertexLightCount;
	uniform float _VertexLightWrapAround;
	uniform float _VertexShadowEffectMultiplyer;
	uniform float _VertexLightBoost;
	uniform float _VertexLightPower;

	uniform float _DistanceFadeStart;
	uniform float _DistanceFadeRate;

	uniform sampler2D _CameraDepthTexture;
	uniform sampler2D _MainTex;
	uniform sampler2D _LightTextureB0;

	uniform float4 _ParticleMaxBrightnesCap;
	uniform float4 _ParticleMinBrightnesCap;

	uniform float _LightScatterSharpness;
	uniform float _AlphaScatterEffect;
	uniform float _AlphaScatterSharpness;
	uniform float _ScatterLightMultiplyer;

	//particle colour tint
	uniform float4 _ParticleTint;

	uniform float _EmissiveMultiplyer;


	struct appdata
	{
		float4 vertex : POSITION;
		float4 color : COLOR;
		float3 normal : NORMAL;
		float2 uv : TEXCOORD0;
		float3 uv2blendandframe : TEXCOORD1;
		float3 rotation : TEXCOORD2;

	};

	struct v2f
	{
		float4 vertex : SV_POSITION;
		float4 TextureCoordinate : TEXCOORD0;
		float4 UV2BlendAndDepth : TEXCOORD1;
		float4 ParticleColour : TEXCOORD2;
		float4 ScreenSpaceCordAndFog : TEXCOORD3;

#if defined(PIXEL_LIGHT_1)
		float4 _LightDirectionAndBrightness1 : TEXCOORD4;
#endif

#if defined(PIXEL_LIGHT_2)
		float4 _LightDirectionAndBrightness2 : TEXCOORD5;
#endif

#if defined(PIXEL_LIGHT_3)
		float4 _LightDirectionAndBrightness3 : TEXCOORD6;
#endif

#if defined(PIXEL_LIGHT_4)
		float4 _LightDirectionAndBrightness4 : TEXCOORD7;
#endif
	};

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.TextureCoordinate.xy = v.uv.xy;
		o.UV2BlendAndDepth.xyz = v.uv2blendandframe.xyz;


		//vert projection pos
		o.vertex = UnityObjectToClipPos(v.vertex);

		//vert view pos
		float3 VertViewPos = mul(UNITY_MATRIX_MV,v.vertex).xyz;

		//emisive colour + alpha
		o.ParticleColour = v.color * float4(_EmissiveMultiplyer, _EmissiveMultiplyer, _EmissiveMultiplyer, _ParticleTint.a);

#if defined(PIXEL_LIGHT_1)
		CalcDistanceAndDirectionToLight(unity_LightPosition[0].xyz, unity_LightPosition[0].w, VertViewPos, o._LightDirectionAndBrightness1.xyz, o._LightDirectionAndBrightness1.a);
		o._LightDirectionAndBrightness1.a = CalculateDistanceAttenuationVertex(o._LightDirectionAndBrightness1.a, unity_LightAtten[0]);
#if defined (SPOTLIGHTS_ON)
		o._LightDirectionAndBrightness1.a *= CalculateSpotlightAttenuation(o._LightDirectionAndBrightness1.xyz, unity_SpotDirection[0].xyz, unity_LightAtten[0].xy);
#endif
#endif

#if defined(PIXEL_LIGHT_2)
		CalcDistanceAndDirectionToLight(unity_LightPosition[1].xyz, unity_LightPosition[1].w, VertViewPos, o._LightDirectionAndBrightness2.xyz, o._LightDirectionAndBrightness2.a);
		o._LightDirectionAndBrightness2.a = CalculateDistanceAttenuationVertex(o._LightDirectionAndBrightness2.a, unity_LightAtten[1]);
#if defined (SPOTLIGHTS_ON)
		o._LightDirectionAndBrightness2.a *= CalculateSpotlightAttenuation(o._LightDirectionAndBrightness2.xyz, unity_SpotDirection[1].xyz, unity_LightAtten[1].xy);
#endif
#endif

#if defined(PIXEL_LIGHT_3)
		CalcDistanceAndDirectionToLight(unity_LightPosition[2].xyz, unity_LightPosition[2].w, VertViewPos, o._LightDirectionAndBrightness3.xyz,o._LightDirectionAndBrightness3.a);
		o._LightDirectionAndBrightness3.a = CalculateDistanceAttenuationVertex(o._LightDirectionAndBrightness3.a, unity_LightAtten[2]);
#if defined (SPOTLIGHTS_ON)
		o._LightDirectionAndBrightness3.a *= CalculateSpotlightAttenuation(o._LightDirectionAndBrightness3.xyz, unity_SpotDirection[2].xyz, unity_LightAtten[2].xy);
#endif
#endif

#if defined(PIXEL_LIGHT_4)
		CalcDistanceAndDirectionToLight(unity_LightPosition[3].xyz, unity_LightPosition[3].w, VertViewPos, o._LightDirectionAndBrightness4.xyz, o._LightDirectionAndBrightness4.a);
		o._LightDirectionAndBrightness4.a = CalculateDistanceAttenuationVertex(o._LightDirectionAndBrightness4.a, unity_LightAtten[3]);
#if defined (SPOTLIGHTS_ON)
		o._LightDirectionAndBrightness4.a *= CalculateSpotlightAttenuation(o._LightDirectionAndBrightness4.xyz, unity_SpotDirection[3].xyz, unity_LightAtten[3].xy);
#endif 
#endif

		//setup soft particle value
		VertParticleSoftFactor(o.vertex, o.ScreenSpaceCordAndFog.xy);

		//save depth
		o.UV2BlendAndDepth.w = -VertViewPos.z;

		//save uv2 and blend factor
		o.UV2BlendAndDepth.xyz = v.uv2blendandframe.xyz;

		//setup fog 
		CalculateFogFactor(o.ScreenSpaceCordAndFog.z, VertViewPos.z, unity_FogParams);

		//calculate distance fade 
		ApplyDistanceFade(o.ParticleColour.a, o.vertex, VertViewPos.z, _DistanceFadeStart, _DistanceFadeRate);

		//calculate vertex lights
		//calculate vertex lighting 
		float DistanceSquared;
		float3 LightDirection;
		float LightCombine = 0.0;
		float3 VertexLight = float3(0.0,0.0,0.0);

		//normal direction
		float3 Normal = mul(UNITY_MATRIX_MV , float4(v.normal, 0.0)).xyz;

		for (int i = PIXEL_LIGHT_COUNT; i < 8; i++)
		{

			if (!(i <_VertexLightCount))
			{
				break;
			}

			//calc light
			CalcDistanceAndDirectionToLight(unity_LightPosition[i].xyz, unity_LightPosition[i].w, VertViewPos, LightDirection, DistanceSquared);

			//calc normal lighting 
			LightCombine = NormalLightEffect(LightDirection, Normal, _VertexLightWrapAround, _VertexShadowEffectMultiplyer, _VertexLightPower);

			//apply light falloff
			LightCombine *= CalculateDistanceAttenuationVertex(DistanceSquared, unity_LightAtten[i]);

#if defined (SPOTLIGHTS_ON)
			//apply spotlight falloff
			LightCombine *= CalculateSpotlightAttenuation(LightDirection, unity_SpotDirection[i].xyz, unity_LightAtten[i].xy);
#endif

			//boost light brightness for vertex lighting 
			LightCombine *= _VertexLightBoost;
			//calculate lighting contribution
			VertexLight += (unity_LightColor[i].rgb * LightCombine);
		}

		//store vertex light plus UV
		o.TextureCoordinate = float4(v.uv.xy, VertexLight.rg);
		o.ScreenSpaceCordAndFog.a = VertexLight.b;

		//normalise view pos
		//rotate all the light vectors
//if defined(ORTHOGRAPHIC)
//
//	float2 Rotation = float2(cos(-(v.tangent.a)), sin(-(v.tangent.a)));
//
//if defined(PIXEL_LIGHT_1)
//	RotateNormal(o._LightDirectionAndBrightness1.xyz, Rotation);
//endif
//
//if defined(PIXEL_LIGHT_2)
//	RotateNormal(o._LightDirectionAndBrightness2.xyz, Rotation);
//endif
//if defined(PIXEL_LIGHT_3)
//	RotateNormal(o._LightDirectionAndBrightness3.xyz, Rotation);
//endif
//if defined(PIXEL_LIGHT_4)
//	RotateNormal(o._LightDirectionAndBrightness4.xyz, Rotation);
//endif
//
//else
		float3 EyeX = float3(1.0, 0.0, 0.0);
		float3 EyeY = float3(0.0, -1.0, 0.0);
		float3 EyeZ = -normalize(VertViewPos);

		CalculateEyeSpaceDirections(EyeX, EyeY, EyeZ, v.rotation.x);

#if defined(PIXEL_LIGHT_1)
		RotateNormal(o._LightDirectionAndBrightness1.xyz, EyeX, EyeY, EyeZ);
#endif

#if defined(PIXEL_LIGHT_2)
		RotateNormal(o._LightDirectionAndBrightness2.xyz, EyeX, EyeY, EyeZ);
#endif
#if defined(PIXEL_LIGHT_3)
		RotateNormal(o._LightDirectionAndBrightness3.xyz, EyeX, EyeY, EyeZ);
#endif
#if defined(PIXEL_LIGHT_4)
		RotateNormal(o._LightDirectionAndBrightness4.xyz, EyeX, EyeY, EyeZ);
#endif


//#endif

		return o;

	}

	fixed4 frag(v2f i) : SV_Target
	{
		float4 NormalAlpha = tex2D(_MainTex, i.TextureCoordinate.xy);

#if defined (FRAME_BLENDING)
		NormalAlpha = lerp(NormalAlpha, tex2D(_MainTex, i.UV2BlendAndDepth.xy), i.UV2BlendAndDepth.z);
#endif

#if defined (SOFTPARTICLES_ON)
		//calculate soft particle factor
		NormalAlpha.a *= FragmentParticleSoftFactor(_CameraDepthTexture,i.ScreenSpaceCordAndFog.xy, i.UV2BlendAndDepth.w, _InvFade, _ZBufferParams);
#endif

		float alpha = NormalAlpha.a - (1.0 - i.ParticleColour.a);

		//calculate particle emissive 
		float3 Emissive = i.ParticleColour.rgb * NormalAlpha.b;

		//get the normal
		float3 Normal = ReconstructNormal(NormalAlpha.xy);

		//lighting variables
		float Luminance = 0.0;
		float3 Lighting = float3(0.0,0.0,0.0);

		//----------- First light ---------------
#if defined(PIXEL_LIGHT_1)
		//calc normal lighting 
		Luminance = NormalLightEffect(i._LightDirectionAndBrightness1.xyz, Normal, _LightWrapAround, _ShadowEffectMultiplyer, _LightPower);

#if defined(LIGHT_SCATTERING_ON)
		//calc back light
		Luminance += ScatterLightEffect(i._LightDirectionAndBrightness1.z, alpha, _LightScatterSharpness, _AlphaScatterSharpness, _AlphaScatterEffect) * _ScatterLightMultiplyer;
#endif

		//combine lighting contribution
		Lighting += unity_LightColor[0].rgb *(Luminance * i._LightDirectionAndBrightness1.a);
#endif
		//----------- Second light ---------------

#if defined(PIXEL_LIGHT_2)
		//calc normal lighting 
		Luminance = NormalLightEffect(i._LightDirectionAndBrightness2.xyz, Normal, _LightWrapAround, _ShadowEffectMultiplyer, _LightPower);

#if defined(LIGHT_SCATTERING_ON)
		//calc back light
		Luminance += ScatterLightEffect(i._LightDirectionAndBrightness2.z, alpha, _LightScatterSharpness, _AlphaScatterSharpness, _AlphaScatterEffect) * _ScatterLightMultiplyer;
#endif

		//combine lighting contribution
		Lighting += unity_LightColor[1].rgb *(Luminance * i._LightDirectionAndBrightness2.a);
#endif
		//----------- Third light ---------------
#if defined(PIXEL_LIGHT_3)
		//calc normal lighting 
		Luminance = NormalLightEffect(i._LightDirectionAndBrightness3.xyz, Normal, _LightWrapAround, _ShadowEffectMultiplyer, _LightPower);

#if defined(LIGHT_SCATTERING_ON)
		//calc back light
		Luminance += ScatterLightEffect(i._LightDirectionAndBrightness3.z,  alpha, _LightScatterSharpness, _AlphaScatterSharpness, _AlphaScatterEffect) * _ScatterLightMultiplyer;
#endif

		//combine lighting contribution
		Lighting += unity_LightColor[2].rgb *(Luminance * i._LightDirectionAndBrightness3.a);
#endif
		//----------- Forth light ---------------
#if defined(PIXEL_LIGHT_4)
		//calc normal lighting 
		Luminance = NormalLightEffect(i._LightDirectionAndBrightness4.xyz, Normal, _LightWrapAround, _ShadowEffectMultiplyer, _LightPower);

#if defined(LIGHT_SCATTERING_ON)
		//calc back light
		Luminance += ScatterLightEffect(i._LightDirectionAndBrightness4.z, alpha, _LightScatterSharpness, _AlphaScatterSharpness, _AlphaScatterEffect) * _ScatterLightMultiplyer;
#endif

		//combine lighting contribution
		Lighting += unity_LightColor[3].rgb *(Luminance * i._LightDirectionAndBrightness4.a);
#endif

		//add vertex lighting 
		Lighting += float3(i.TextureCoordinate.zw, i.ScreenSpaceCordAndFog.a);

		//final colour
		float3 FinalColour = (Lighting * _ParticleTint.rgb) + Emissive;

		//apply the fog effect
		ApplyFogFactor(FinalColour, unity_FogColor.rgb, i.ScreenSpaceCordAndFog.z);

		//gl_FragColor = vec4(TextureCoordinate.zw, ScreenSpaceCordAndFog.a, alpha);
		//gl_FragColor = vec4(ScreenSpaceCordAndFog.zzz, alpha);
		//gl_FragColor = vec4(ParticleColour, alpha) ;
		return clamp(float4(FinalColour, alpha), _ParticleMinBrightnesCap, _ParticleMaxBrightnesCap);
		//gl_FragColor = clamp(vec4(ParticleColour * alpha, alpha), _ParticleMinBrightnesCap, _ParticleMaxBrightnesCap);

	}
		ENDCG
	}
	}

		CustomEditor "CGLSAFShaderEditor"
}
