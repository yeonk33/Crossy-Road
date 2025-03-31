using Unity.VisualScripting;
using UnityEngine;

public static class Extensions
{
	/// <summary>
	/// ������Ʈ T�� ������ Get, ������ Add
	/// </summary>
	public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
	{
		return obj.GetComponent<T>() ?? obj.AddComponent<T>();
	}
}
