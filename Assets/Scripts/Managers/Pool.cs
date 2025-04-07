using System.Collections.Generic;
using UnityEngine;

public class Pool
{
	public GameObject PrefabObj { get; private set; }
	public Transform Root { get; private set; }
	private Stack<Poolable> _poolables;

	public void Init(GameObject obj, int count = 5)
	{
		PrefabObj = obj;
		Root = new GameObject($"{PrefabObj.name} Pool").transform;

		_poolables = new Stack<Poolable>();
		for (int i = 0; i < count; i++) {
			Push(Create());
		}
	}

	/// <summary>
	/// Poolable Stack에 Poolable 오브젝트 다시 넣기
	/// </summary>
	public void Push(Poolable poolable)
	{
		if (poolable == null) return;

		poolable.InactiveObject();
		poolable.IsUsing = false;
		poolable.transform.SetParent(Root);

		_poolables.Push(poolable);
	}

	private Poolable Create()
	{
		GameObject obj = Object.Instantiate(PrefabObj);
		obj.name = PrefabObj.name;
		return obj.GetOrAddComponent<Poolable>();
	}

	public Poolable Pop(int z = 0)
	{
		// stack에 없으면 새로 만들기
		Poolable poolable = _poolables.Count > 0 ? _poolables.Pop() : Create();
		poolable.ActiveObject(z);
		poolable.IsUsing = true;
		//poolable.transform.SetParent(parent);


		return poolable;
	}
}
