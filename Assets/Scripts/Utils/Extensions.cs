using Unity.VisualScripting;
using UnityEngine;

public static class Extensions
{
	/// <summary>
	/// 컴포넌트 T가 있으면 Get, 없으면 Add
	/// </summary>
	public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
	{
		return obj.GetComponent<T>() ?? obj.AddComponent<T>();
	}
}
