using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapManager : MonoBehaviour
{
	[SerializeField] List<GameObject> mapPrefabs;
	public List<GameObject> obstaclePrefabs;
	public List<GameObject> carPrefabs;
	private GameObject coinPrefab;
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

			carPrefabs = new List<GameObject>();
			carPrefabs.Add(Resources.Load<GameObject>("Prefabs/Car"));

			coinPrefab = Resources.Load<GameObject>("Prefabs/Coin");

			// pool»ý¼º
			Managers.Pool.CreatePool(mapPrefabs[0], 10);
			Managers.Pool.CreatePool(mapPrefabs[1], 10);
			
			Managers.Pool.CreatePool(obstaclePrefabs[0], 10);
			Managers.Pool.CreatePool(obstaclePrefabs[1], 10);
			Managers.Pool.CreatePool(obstaclePrefabs[2], 10);

			Managers.Pool.CreatePool(carPrefabs[0], 15);

			Managers.Pool.CreatePool(coinPrefab, 5);
		}
		MapInit();
		
		return this;	
	}

	public void RepositionMap()
	{
		int index = UnityEngine.Random.Range(0, mapPrefabs.Count);
		maps.Enqueue(Managers.Pool.Pop(mapPrefabs[index], z));

		z += interval;
		
		if (maps.Count > mapLength) {
			Managers.Pool.Push(maps.Dequeue());
			Roadway roadway = maps.Peek().GetComponent<Roadway>();
			if (roadway != null) { // Roadway
				roadway.Init();
				roadway.StartCarSpawn();
			}
		}

		CoinSpawn();
	}

	private void CoinSpawn()
	{
		float p = Random.Range(0f, 10f);
		if (p < 1f) {
			Poolable coin = Managers.Pool.Pop(coinPrefab);
			coin.gameObject.transform.position = new Vector3(Random.Range(0.57f, 8.5f), 1.5f, z - 1.5f);
		}
	}

	public void MapInit()
	{
		z = 12;

		while (maps.Count > 0) {
			Managers.Pool.Push(maps.Dequeue());
		}

		for (int i = 0; i < 10; i++) {
			RepositionMap();
		}

		foreach (var m in maps) {
			if (m != null) {
				Roadway roadway = m.GetComponent<Roadway>();
				if (roadway != null) { // Roadway
					roadway.Init();
					roadway.StartCarSpawn();
				}
			}
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
