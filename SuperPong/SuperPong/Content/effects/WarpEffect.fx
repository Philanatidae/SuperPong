#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float time;
float speed;
float amplitude;
float period;

float2 SineWave(float2 p, float period, float time, float speed, float amplitude) {
	float pi = 3.14159;
	float w = period * 10 * pi;
	float t = time * speed * pi / 180;
	float y = sin(w * p.x + t) * amplitude;
	return float2(p.x, p.y + y);
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float2 p = input.TextureCoordinates;
	float2 uv = SineWave(p, period, time, speed, amplitude);
	return tex2D(SpriteTextureSampler, uv) * input.Color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};