#if OPENGL
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif
sampler TextureSampler : register(s0);

float BloomThreshold;

struct VSOutput
{
	float4 position		: SV_Position;
	float4 color		: COLOR0;
	float2 texCoord		: TEXCOORD0;
};

float4 MainPS(VSOutput input) : COLOR0
{
	float4 color = tex2D(TextureSampler, input.texCoord);  // Look up the original image color.    

	// Adjust it to keep only values brighter than the specified threshold.
	return saturate((color - BloomThreshold) / (1 - BloomThreshold));
}

technique BloomExtract //Main Function
{
	pass Pass1
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
}