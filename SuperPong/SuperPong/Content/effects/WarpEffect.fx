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

float2 SineWave(float2 p, float time, float speed) {
	float pi = 3.14159;
	float A = 0.05;
	float w = 10 * pi;
	float t = time * speed * pi / 180;
	float y = sin(w * p.x + t) * A;
	return float2(p.x, p.y + y);
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float2 p = input.TextureCoordinates;
	float2 uv = SineWave(p, -time, speed);
	return tex2D(SpriteTextureSampler, uv) * input.Color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};