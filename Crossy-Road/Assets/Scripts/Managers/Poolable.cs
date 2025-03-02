using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour
{
	public bool IsUsing;

	/// <summary>
	/// Poolable ������Ʈ Ȱ��ȭ
	/// </summary>
	public void ActiveObject(int z = 0)
	{
		gameObject.SetActive(true);
		gameObject.transform.position += new Vector3(0, 0, z);
	}

	/// <summary>
	/// Poolable ������Ʈ ��Ȱ��ȭ
	/// </summary>
	public void InactiveObject()
	{
		gameObject.SetActive(false);
	}
}
