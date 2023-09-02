#version 430 core

layout (location = 0) in vec3 aPosition;

out vec3 texCoord;

layout (location = 1) uniform mat4 view;
layout (location = 2) uniform mat4 projection;

const mat4 flip = mat4(
    1, 0, 0, 0,
    0, 0, 1, 0,
    0, -1, 0, 0,
    0, 0, 0, 1
);

void main()
{
    vec4 position = vec4(aPosition, 1.0);
    vec4 projectedPosition = position * mat4(mat3(view)) * flip * projection;
    gl_Position = projectedPosition.xyww;
    texCoord = aPosition;
}