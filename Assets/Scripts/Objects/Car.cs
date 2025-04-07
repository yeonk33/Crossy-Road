using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Car : MonoBehaviour
{    
	[SerializeField] float speed = 5.0f;
	private bool start = false;
	private Vector3 dir;
	public Vector3 end;

	private void Update()
	{
		// 맞은편 spawnpoint에 도달하면 pool에 넣기
		if (Vector3.Distance(transform.position, end) < 0.5f) {
			Managers.Pool.Push(this.GetComponent<Poolable>());
			start = false;
		}

		if (start)
			this.transform.position += dir * Time.deltaTime * speed;
	}

	public void Init(Quaternion rot, Vector3 dir, Vector3 endPos, float speed = 5.0f)
	{
		this.transform.rotation = rot;
		this.dir = dir;
		this.end = endPos;
		this.speed = speed;
		start = true;
	}
	private void OnDisable()
	{
		start = false;
	}
}
