using System.Collections.Generic;

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

	public static void Shuffle <T>(this List<T> list)
	{
		var n = list.Count;  
		while (n > 1) {  
			n--;
			var k = Random.Range(0, n);
			var value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}
}