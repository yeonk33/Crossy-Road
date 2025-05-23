using System.Collections.Generic;
using UnityEngine;

public class Sidewalk : MonoBehaviour
{
	private List<Poolable> objects;
	private void Start()
	{
		Init();
	}

	private void Init()
	{
		objects = new List<Poolable>();
		// Camera left limit(-0.14, 0.57, 7.52)
		// Camera right limit(8.5, 0.64, 7.37)
		float y = 0.65f;
		float z = this.transform.position.z - 1.5f;
		float spawnNum = Random.Range(0, 2);
		for (int i = 0; i < spawnNum; i++) {
			float x = Random.Range(0.57f, 8.5f);
			GameObject obs = Managers.Pool.Pop(Managers.Map.obstaclePrefabs[Random.Range(0, Managers.Map.obstaclePrefabs.Count)]).gameObject;
			obs.transform.position = new Vector3(x, y, z);
			objects.Add(obs.GetOrAddComponent<Poolable>());
		}
	}

	private void OnDisable()
	{
		if (objects == null) return;
		//Debug.Log($"Sidewalk Disabled! {gameObjects.Count}�� Push");
		foreach (var item in objects) {
			if (item != null && item.gameObject != null) 
				Managers.Pool.Push(item);
		}
	}
}
