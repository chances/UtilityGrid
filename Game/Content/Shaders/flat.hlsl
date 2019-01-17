cbuffer ViewProj : register(b0)
{
    float4x4 ViewProj;
}

struct VertexIn
{
    float3 Position : POSITION0;
    float3 Normal : NORMAL0;
    float4 Color : COLOR0;
};

struct FragmentIn
{
    float4 Position : SV_Position;
    float3 Normal : NORMAL0;
    float4 Color : COLOR0;
};

FragmentIn VS(VertexIn input)
{
    FragmentIn output;
    float4 pos = float4(input.Position, 1)
    output.Position = mul(ViewProj, pos);
    output.Normal = input.Normal;
    output.Color = input.Color;
    return output;
}

float4 FS(FragmentIn input) : SV_Target0
{
    return input.Color;
}
