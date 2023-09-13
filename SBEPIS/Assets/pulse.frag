#version 330 core

in vec3 vertexColor;

out vec4 FragColor;

uniform vec3 someColor;

void main()
{
    FragColor = vec4(vertexColor + someColor, 1.0f);
}