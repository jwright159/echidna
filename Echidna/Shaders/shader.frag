#version 330 core

in vec3 vertexColor;

out vec4 FragColor;

uniform vec3 someColor;

void main()
{
    vec3 finalColor = vertexColor + someColor;
    FragColor = vec4(finalColor, 1.0f);
}