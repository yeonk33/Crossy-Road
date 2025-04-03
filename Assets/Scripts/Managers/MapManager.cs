using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapManager : MonoBehaviour
{
	[SerializeField] List<GameObject> mapPrefabs;
	public List<GameObject> obstaclePrefabs;
	[SerializeField] int mapLength = 15;
	private int z = 12;
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

			obstaclePrefabs = new List<GameObject>();
			obstaclePrefabs.Add(Resources.Load<GameObject>("Prefabs/Tree_4"));
			obstaclePrefabs.Add(Resources.Load<GameObject>("Prefabs/Rocks_2"));
			obstaclePrefabs.Add(Resources.Load<GameObject>("Prefabs/Tree_Cut"));

			// pool»ý¼º
			Managers.Pool.CreatePool(mapPrefabs[0], 10);
			Managers.Pool.CreatePool(mapPrefabs[1], 10);
			
			Managers.Pool.CreatePool(obstaclePrefabs[0], 10);
			Managers.Pool.CreatePool(obstaclePrefabs[1], 10);
			Managers.Pool.CreatePool(obstaclePrefabs[2], 10);
		}
		MapInit();
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

	public void MapInit()
	{
		z = 12;
		for (int i = 0; i < maps.Count; i++) {
			Managers.Pool.Push(maps.Dequeue());
		}

		for (int i = 0; i < 10; i++) {
			RepositionMap();
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
