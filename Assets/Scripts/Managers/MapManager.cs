using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MapManager : MonoBehaviour
{
	[SerializeField] List<GameObject> mapPrefabs;
	[SerializeField] int mapLength = 10;
	private int z = 0;
	private int interval = 3;
	private Queue<Poolable> maps = new Queue<Poolable>();
	Transform _root;
	public MapManager Init()
	{
		if (_root == null) {
			_root = new GameObject("Maps").transform;
			_root.AddComponent<MapManager>();
			Object.DontDestroyOnLoad(_root);

			mapPrefabs = new List<GameObject>();
			mapPrefabs.Add(Resources.Load<GameObject>("Prefabs/Sidewalk"));
			mapPrefabs.Add(Resources.Load<GameObject>("Prefabs/Roadway"));

			// pool생성
			Managers.Pool.CreatePool(mapPrefabs[0], 10);
			Managers.Pool.CreatePool(mapPrefabs[1], 10);
			
		}

		// 초기 맵 배치
		for (int i = 0; i < mapLength; i++) {
			RepositionMap();
		}

		//StartCoroutine(InfiniteCoroutine());	
		return this;	
	}

	public void RepositionMap()
	{
		int index = UnityEngine.Random.Range(0, mapPrefabs.Count);
		maps.Enqueue(Managers.Pool.Pop(mapPrefabs[index], z));
		z += interval;
		
		if (maps.Count > mapLength) {
			Managers.Pool.Push(maps.Dequeue());
		}
	}

	IEnumerator InfiniteCoroutine()
	{
		while (true) {
			RepositionMap();
			yield return new WaitForSeconds(5.0f);
			
		}
	}
}
