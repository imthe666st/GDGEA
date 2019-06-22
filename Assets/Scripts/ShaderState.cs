using System;

public enum ShaderState
{
	None,
	Half,
	Complete,
}

public static class ShaderStateExt {
	public static float toFloat(this ShaderState state)
	{
		switch (state)
		{
			case ShaderState.None:
				return 0.2f;
			case ShaderState.Half:
				return 0.6f;
			case ShaderState.Complete:
				return 1f;
			default:
				throw new ArgumentOutOfRangeException(nameof(state), state, null);
		}
	}
}
