//The fragment shader handles each individual pixel in the image.
//These first three lines are boilerplate coide, they must be there.
#ifdef GL_ES
    precision highp float;
#endif

//Define a colour variable.
//Uniform variables are constant.
//This value uses four points for the colour RGBA(0-1).
uniform vec4 uColor;

void main()
{
    gl_FragColor = uColor;
}