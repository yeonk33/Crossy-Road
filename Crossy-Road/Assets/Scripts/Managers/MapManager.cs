using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
	[SerializeField] GameObject[] mapPrefabs;
	[SerializeField] int mapLength = 10;
	private int z = 0;
	private int interval = 3;
	private Queue<Poolable> maps = new Queue<Poolable>();

	private void Start()
	{
		// ÃÊ±â ¸Ê ¹èÄ¡
		for (int i = 0; i < mapLength; i++) {
			RepositionMap();
		}

		
		StartCoroutine(InfiniteCoroutine());
		
	}

	private void RepositionMap()
	{
		int index = UnityEngine.Random.Range(0, mapPrefabs.Length);
		maps.Enqueue(Managers.Pool.Pop(mapPrefabs[index], z));
		z += interval;
		Debug.Log(z);
		if (maps.Count > mapLength) {
			Managers.Pool.Push(maps.Dequeue());
		}
	}

	IEnumerator InfiniteCoroutine()
	{
		while (true) {
			RepositionMap();
			yield return new WaitForSeconds(1.0f);
			
		}
	}
}
