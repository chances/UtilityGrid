[[vk::binding(0)]]
cbuffer ViewProj : register(b0)
{
    float4x4 ViewProj;
}

[[vk::binding(1)]]
cbuffer Color : register(b1)
{
    float4 Color;
}

struct VertexIn
{
    float3 Position : POSITION0;
    float3 Normal : NORMAL0;
};

struct FragmentIn
{
    float4 Position : SV_Position;
    float3 Normal : NORMAL0;
};

FragmentIn VS(VertexIn input)
{
    FragmentIn output;
    float4 pos = float4(input.Position, 1);
    output.Position = mul(ViewProj, pos);
    output.Normal = input.Normal;
    return output;
}

float4 FS(FragmentIn input) : SV_Target0
{
    return Color;
}
