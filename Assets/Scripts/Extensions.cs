using UnityEngine;

public static class Extensions
{
	public static Vector3 ClearZ(this Vector3 toClear)
	{
		return new Vector3(toClear.x, toClear.y);
	}

	public static Vector3 OnlyZ(this Vector3 obj)
	{
		return new Vector3(0, 0, obj.z);
	}
}