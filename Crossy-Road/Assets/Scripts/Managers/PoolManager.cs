using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
	private Transform _root;
	Dictionary<string, Pool> _pool = new Dictionary<string, Pool>(); // <"이름", 바구니>

	public PoolManager Init() // Pool들의 root object 생성
	{
		Debug.Log(System.Environment.StackTrace);
		if (_root == null) {
			_root = new GameObject("Pool Root").transform;
			Object.DontDestroyOnLoad(_root);
		}

		return this;
	}

	public void CreatePool(GameObject obj, int count = 5)
	{
		if (obj == null) return;
		if (_pool.ContainsKey(obj.name)) return; // 이미 있는 Pool이면 생성X

		Pool pool = new Pool();
		pool.Init(obj, count);
		pool.Root.SetParent(_root); // root 자식으로 붙이기

		_pool.Add(obj.name, pool);
	}

	public Poolable Pop(GameObject obj, int z = 0)
	{
		if (!_pool.ContainsKey(obj.name)) {
			CreatePool(obj);
		}

		return _pool[obj.name].Pop(z);
	}

	public void Push (Poolable obj) {
		string name = obj.gameObject.name;

		if (!_pool.ContainsKey(name)) {
			Object.Destroy(obj.gameObject);
			return;
		}

		_pool[name].Push(obj);
	}
}
