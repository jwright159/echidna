﻿using System.Drawing;
using Echidna.Rendering.Texture;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Echidna.Rendering;

public static class GLResourceUtil
{
	public static void Bind(this Mesh.Mesh mesh)
	{
		if (mesh.VertexArrayObject == 0) throw new ArgumentNullException(nameof(mesh));
		GL.BindVertexArray(mesh.VertexArrayObject);
	}
	public static void Bind(this Shader.Shader shader)
	{
		if (shader.Handle == 0) throw new ArgumentNullException(nameof(shader));
		GL.UseProgram(shader.Handle);
	}
	public static void Bind(this Texture.Texture texture, TextureUnit unit = TextureUnit.Texture0)
	{
		if (texture.Handle == 0) throw new ArgumentNullException(nameof(texture));
		GL.ActiveTexture(unit);
		GL.BindTexture(TextureTarget.Texture2D, texture.Handle);
	}
	public static void Bind(this CubeMap cubeMap, TextureUnit unit = TextureUnit.Texture0)
	{
		if (cubeMap.Handle == 0) throw new ArgumentNullException(nameof(cubeMap));
		GL.ActiveTexture(unit);
		GL.BindTexture(TextureTarget.TextureCubeMap, cubeMap.Handle);
	}
	public static void Bind(this Font.Font font, TextureUnit unit = TextureUnit.Texture0)
	{
		if (font.TextureHandle == 0 || font.VertexBufferObject == 0 || font.VertexArrayObject == 0) throw new ArgumentNullException(nameof(font));
		GL.ActiveTexture(unit);
		GL.BindTexture(TextureTarget.Texture2D, font.TextureHandle);
		GL.BindVertexArray(font.VertexArrayObject);
		GL.BindBuffer(BufferTarget.ArrayBuffer, font.VertexBufferObject);
	}
	
	public static void Draw(this Mesh.Mesh mesh)
	{
		GL.DrawElements(PrimitiveType.Triangles, mesh.Indices.Length, DrawElementsType.UnsignedInt, 0);
	}
	
	public static int GetAttribLocation(this Shader.Shader shader, string attribName) => GL.GetAttribLocation(shader.Handle, attribName);
	public static int GetUniformLocation(this Shader.Shader shader, string uniformName) => shader.Uniforms[uniformName];
	public static void SetInt(this Shader.Shader shader, string name, int data) => GL.Uniform1(shader.GetUniformLocation(name), data);
	public static void SetFloat(this Shader.Shader shader, string name, float data) => GL.Uniform1(shader.GetUniformLocation(name), data);
	public static void SetMatrix4(this Shader.Shader shader, string name, Matrix4 data) => GL.UniformMatrix4(shader.GetUniformLocation(name), true, ref data);
	public static void SetMatrix4(this Shader.Shader shader, int location, Matrix4 data) => GL.UniformMatrix4(location, true, ref data);
	public static void SetVector3(this Shader.Shader shader, string name, Vector3 data) => GL.Uniform3(shader.GetUniformLocation(name), data);
	public static void SetVector4(this Shader.Shader shader, string name, Vector4 data) => GL.Uniform4(shader.GetUniformLocation(name), data);
	public static void SetVector4(this Shader.Shader shader, string name, Color data) => GL.Uniform4(shader.GetUniformLocation(name), data);
}