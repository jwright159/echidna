﻿using Echidna.Core;

namespace Echidna.Rendering.Shader;

public class CameraShaders3d : Component
{
	internal readonly Shader[] Shaders;
	
	public CameraShaders3d(params Shader[] shaders)
	{
		Shaders = shaders;
	}
}