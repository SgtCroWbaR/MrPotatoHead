#version 330 core

in vec2 ourTexCord;
out vec4 FragColor;

uniform sampler2D t0;

void main(){
    FragColor = texture(t0, ourTexCord);
}