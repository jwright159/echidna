#version 330 core

in vec3 texCoord;

out vec4 FragColor;

uniform samplerCube cubeMap;

void main()
{
    FragColor = texture(cubeMap, texCoord);
}