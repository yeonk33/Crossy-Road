using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour
{
	public bool IsUsing;

	/// <summary>
	/// Poolable 오브젝트 활성화
	/// </summary>
	public void ActiveObject(int z = 0)
	{
		gameObject.SetActive(true);
		gameObject.transform.position += new Vector3(0, 0, z);
	}

	/// <summary>
	/// Poolable 오브젝트 비활성화
	/// </summary>
	public void InactiveObject()
	{
		gameObject.SetActive(false);
	}
}
