#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

uniform extern texture ScreenTexture;

sampler screen = sampler_state
{
	Texture = <ScreenTexture>;
};

float4 PixelShaderFunction(float2 inCoord: TEXCOORD0) : COLOR
{
	float4 color = tex2D(screen, inCoord);

	color.rgb = (color.r + color.g + color.b) / 3.0f;

	return color;
}

technique
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
	}
};
