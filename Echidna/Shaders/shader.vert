#version 330 core

in vec3 aPosition;
in vec3 aColor;

out vec3 vertexColor;
out vec3 worldPosition;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

const mat4 flip = mat4(
    1, 0, 0, 0,
    0, 0, 1, 0,
    0, -1, 0, 0,
    0, 0, 0, 1
);

void main()
{
    vec4 localPosition4 = vec4(aPosition, 1.0);
    vec4 worldPosition4 = localPosition4 * model;
    worldPosition = worldPosition4.xyz;
    gl_Position = worldPosition4 * view * flip * projection;
    vertexColor = aColor;
}