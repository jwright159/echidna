using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Echidna.Rendering;

public static class GLResourceUtil
{
	public static void Bind(this Mesh mesh) => GL.BindVertexArray(mesh.VertexArrayObject);
	public static void Bind(this Shader shader) => GL.UseProgram(shader.Handle);
	public static void Bind(this Texture texture, TextureUnit unit = TextureUnit.Texture0)
	{
		GL.ActiveTexture(unit);
		GL.BindTexture(TextureTarget.Texture2D, texture.Handle);
	}
	public static void Bind(this CubeMap cubeMap, TextureUnit unit = TextureUnit.Texture0)
	{
		GL.ActiveTexture(unit);
		GL.BindTexture(TextureTarget.TextureCubeMap, cubeMap.Handle);
	}
	
	public static void Draw(this Mesh mesh) => GL.DrawElements(PrimitiveType.Triangles, mesh.Indices.Length, DrawElementsType.UnsignedInt, 0);
	
	public static int GetAttribLocation(this Shader shader, string attribName) => GL.GetAttribLocation(shader.Handle, attribName);
	public static int GetUniformLocation(this Shader shader, string uniformName) => shader.Uniforms[uniformName];
	public static void SetInt(this Shader shader, string name, int data) => GL.Uniform1(shader.GetUniformLocation(name), data);
	public static void SetFloat(this Shader shader, string name, float data) => GL.Uniform1(shader.GetUniformLocation(name), data);
	public static void SetMatrix4(this Shader shader, string name, Matrix4 data) => GL.UniformMatrix4(shader.GetUniformLocation(name), true, ref data);
	public static void SetMatrix4(this Shader shader, int location, Matrix4 data) => GL.UniformMatrix4(location, true, ref data);
	public static void SetVector3(this Shader shader, string name, Vector3 data) => GL.Uniform3(shader.GetUniformLocation(name), data);
}