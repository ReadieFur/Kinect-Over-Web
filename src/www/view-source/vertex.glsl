//The vertex shader handles the positions of points in the image.
//An array that will store data to be processed, in my case the vertex positions.
//vec2 = 2D component.
attribute vec2 aVertexPosition;

void main()
{
    //gl_Position is the return value, in these shaders there is no "return" keyword.
    //To get this return value we will need to assign a value to gl_Position in the JS.
    //gl_Position expects a 4 point vector so that is why "vec4" is used here, for the remaining two values they have been hard-coded so thay are always the same (0.0, 1.0).
    //Value 1 = aVertexPosition.
    //Value 2 = aVertexPosition.
    //Value 3 (0.0) represents the layer depth of the vertex (z-index).
    //Value 4 (1.0) used for perspective (advanced value beyond this tutorial), for simple projects such as this one this value should remain as 1.
    gl_Position = vec4(aVertexPosition, 0.0, 1.0);
}