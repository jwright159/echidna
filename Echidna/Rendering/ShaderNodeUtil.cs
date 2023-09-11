using OpenTK.Mathematics;

namespace Echidna.Rendering;

public static class ShaderNodeUtil
{
	public static ShaderProgram MainShader = new()
	{
		Position = new PositionOutput(
			new Vector4TimesMatrix4(
				new Vector4TimesMatrix4(
					new Vector4TimesMatrix4(
						new Vector3ToVector4(
							new PositionInput().Output,
							new FloatValue(1.0f).Output
							).Output,
						new TransformInput().Output
						).Output,
					new ViewInput().Output
					).Output,
				new ProjectionInput().Output
				).Output),
		Bindings = new OutInBinding[]
		{
			new OutInBinding<Vector3>("worldPosition", 
				new Vector4XYZ(
					new Vector4TimesMatrix4(
						new Vector3ToVector4(
							new PositionInput().Output,
							new FloatValue(1.0f).Output
							).Output,
						new TransformInput().Output
						).Output
					).Output
				),
			new OutInBinding<Vector2>("texCoord", new TexCoordInput().Output),
			new OutInBinding<Vector3>("vertexColor", new VertexColorInput().Output),
		},
	};
	
	public static ShaderProgram Skybox = new()
	{
		Position = new PositionOutput(
			new Vector4XYWW(
				new Vector4TimesMatrix4(
					new Vector4TimesMatrix4(
						new Vector3ToVector4(
							new PositionInput().Output,
							new FloatValue(1.0f).Output
							).Output,
						new Matrix3ToMatrix4(
							new Matrix4ToMatrix3(
								new ViewInput().Output
								).Output
							).Output
						).Output,
					new ProjectionInput().Output
					).Output
				).Output),
		Bindings = new OutInBinding[]
		{
			new OutInBinding<Vector3>("texCoord", new PositionInput().Output),
		},
	};
    
	public static string GetShaderTypeName(Type type)
	{
		return type switch
		{
			not null when type == typeof(Vector2) => "vec2",
			not null when type == typeof(Vector3) => "vec3",
			not null when type == typeof(Vector4) => "vec4",
			_ => throw new ArgumentException($"Invalid type {type}", nameof(type))
		};
	}
}

public class ShaderProgram
{
	public PositionOutput? Position { get; init; }
	public OutInBinding[] Bindings { get; init; } = Array.Empty<OutInBinding>();
    
	public string VertexSource => $@"
#version 430 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aTexCoord;
layout (location = 2) in vec3 aColor;

{string.Join("\n", Bindings.Select(binding => $"out {binding.Type} {binding.Name};"))}

layout (location = 0) uniform mat4 transform;
layout (location = 1) uniform mat4 view;
layout (location = 2) uniform mat4 projection;

const mat4 flip = mat4(
    1, 0, 0, 0,
    0, 0, 1, 0,
    0, -1, 0, 0,
    0, 0, 0, 1
);

void main()
{{
    {Position?.ToString() ?? ""}
    {string.Join("\n    ", Bindings.Select(binding => binding.ToString()))}
}}
";
}

public interface ShaderNode
{
}

// ReSharper disable once UnusedTypeParameter
public class ShaderNodeSlot<T>
{
	private Func<string> selector;
	
	public ShaderNodeSlot(Func<string> selector)
	{
		this.selector = selector;
	}
	
	public override string ToString() => selector();
}

public class TransformInput : ShaderNode
{
	public readonly ShaderNodeSlot<Matrix4> Output;
	
	public TransformInput()
	{
		Output = new ShaderNodeSlot<Matrix4>(ToString);
	}
	
	public override string ToString() => "transform";
}

public class ViewInput : ShaderNode
{
	public readonly ShaderNodeSlot<Matrix4> Output;
	
	public ViewInput()
	{
		Output = new ShaderNodeSlot<Matrix4>(ToString);
	}
	
	public override string ToString() => "view * flip";
}

public class ProjectionInput : ShaderNode
{
	public readonly ShaderNodeSlot<Matrix4> Output;
	
	public ProjectionInput()
	{
		Output = new ShaderNodeSlot<Matrix4>(ToString);
	}
	
	public override string ToString() => "projection";
}

public class PositionInput : ShaderNode
{
	public readonly ShaderNodeSlot<Vector3> Output;
	
	public PositionInput()
	{
		Output = new ShaderNodeSlot<Vector3>(ToString);
	}
	
	public override string ToString() => "aPosition";
}

public class TexCoordInput : ShaderNode
{
	public readonly ShaderNodeSlot<Vector2> Output;
	
	public TexCoordInput()
	{
		Output = new ShaderNodeSlot<Vector2>(ToString);
	}
	
	public override string ToString() => "aTexCoord";
}

public class VertexColorInput : ShaderNode
{
	public readonly ShaderNodeSlot<Vector3> Output;
	
	public VertexColorInput()
	{
		Output = new ShaderNodeSlot<Vector3>(ToString);
	}
	
	public override string ToString() => "aColor";
}

public class PositionOutput : ShaderNode
{
	private readonly ShaderNodeSlot<Vector4> position;
	
	public PositionOutput(ShaderNodeSlot<Vector4> position)
	{
		this.position = position;
	}
    
	public override string ToString() => $"gl_Position = {position};";
}

public class Vector4TimesMatrix4 : ShaderNode
{
	private readonly ShaderNodeSlot<Vector4> left;
	private readonly ShaderNodeSlot<Matrix4> right;
	
	public readonly ShaderNodeSlot<Vector4> Output;
	
	public Vector4TimesMatrix4(ShaderNodeSlot<Vector4> left, ShaderNodeSlot<Matrix4> right)
	{
		this.left = left;
		this.right = right;
		Output = new ShaderNodeSlot<Vector4>(ToString);
	}
	
	public override string ToString() => $"({left} * {right})";
}

public class Vector3ToVector4 : ShaderNode
{
	private readonly ShaderNodeSlot<Vector3> xyz;
	private readonly ShaderNodeSlot<float> w;
	
	public readonly ShaderNodeSlot<Vector4> Output;
	
	public Vector3ToVector4(ShaderNodeSlot<Vector3> xyz, ShaderNodeSlot<float> w)
	{
		this.xyz = xyz;
		this.w = w;
		Output = new ShaderNodeSlot<Vector4>(ToString);
	}
	
	public override string ToString() => $"vec4({xyz}, {w})";
}

public class Matrix3ToMatrix4 : ShaderNode
{
	private readonly ShaderNodeSlot<Matrix3> mat3;
	
	public readonly ShaderNodeSlot<Matrix4> Output;
	
	public Matrix3ToMatrix4(ShaderNodeSlot<Matrix3> mat3)
	{
		this.mat3 = mat3;
		Output = new ShaderNodeSlot<Matrix4>(ToString);
	}
	
	public override string ToString() => $"mat4({mat3})";
}

public class Matrix4ToMatrix3 : ShaderNode
{
	private readonly ShaderNodeSlot<Matrix4> mat4;
	
	public readonly ShaderNodeSlot<Matrix3> Output;
	
	public Matrix4ToMatrix3(ShaderNodeSlot<Matrix4> mat4)
	{
		this.mat4 = mat4;
		Output = new ShaderNodeSlot<Matrix3>(ToString);
	}
	
	public override string ToString() => $"mat3({mat4})";
}

public class Vector4XYZ : ShaderNode
{
	private readonly ShaderNodeSlot<Vector4> xyzw;
	
	public readonly ShaderNodeSlot<Vector3> Output;
	
	public Vector4XYZ(ShaderNodeSlot<Vector4> xyzw)
	{
		this.xyzw = xyzw;
		Output = new ShaderNodeSlot<Vector3>(ToString);
	}
	
	public override string ToString() => $"{xyzw}.xyz";
}

public class Vector4XYWW : ShaderNode
{
	private readonly ShaderNodeSlot<Vector4> xyzw;
	
	public readonly ShaderNodeSlot<Vector4> Output;
	
	public Vector4XYWW(ShaderNodeSlot<Vector4> xyzw)
	{
		this.xyzw = xyzw;
		Output = new ShaderNodeSlot<Vector4>(ToString);
	}
	
	public override string ToString() => $"{xyzw}.xyww";
}

public class FloatValue : ShaderNode
{
	private float value;
	
	public readonly ShaderNodeSlot<float> Output;
	
	public FloatValue(float value)
	{
		this.value = value;
		Output = new ShaderNodeSlot<float>(ToString);
	}
	
	public override string ToString() => $"{value}";
}

public abstract class OutInBinding
{
	public readonly string Name;
	public abstract string Type { get; }
    
	protected OutInBinding(string name)
	{
		Name = name;
	}
}

public class OutInBinding<T> : OutInBinding
{
	public override string Type => ShaderNodeUtil.GetShaderTypeName(typeof(T));
	
	public readonly ShaderNodeSlot<T> Output;
    
	public OutInBinding(string name, ShaderNodeSlot<T> output) : base(name)
	{
		Output = output;
	}
    
	public override string ToString() => $"{Name} = {Output};";
}