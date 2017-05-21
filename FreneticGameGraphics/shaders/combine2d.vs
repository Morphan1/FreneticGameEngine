#version 430 core

layout (location = 0) in vec3 position;
layout (location = 2) in vec2 texcoords;

layout (location = 1) uniform vec2 scaler = vec2(1.0);
layout (location = 2) uniform vec2 adder = vec2(0.0);

layout (location = 0) out vec2 f_texcoord;

void main()
{
	f_texcoord = texcoords;
	gl_Position = vec4(position, 1.0) * vec4(scaler, 1.0, 1.0) + vec4(adder, 0.0, 0.0);
}