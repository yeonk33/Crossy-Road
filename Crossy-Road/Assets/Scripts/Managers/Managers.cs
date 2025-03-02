using UnityEngine;

public class Managers : MonoBehaviour
{
	private static Managers s_instance;
	public static Managers Instance
	{
		get
		{
			s_instance = FindFirstObjectByType<Managers>(); // Managers ������Ʈ�� �ִ��� ã��
			
			// ������ ������Ʈ �ٿ��� �����
			if (s_instance == null) {
				GameObject gameObject = new GameObject("Managers");
				s_instance = gameObject.AddComponent<Managers>();
				DontDestroyOnLoad(gameObject);
			}
			return s_instance;
		}
	}

	private PoolManager _pool;
	public static PoolManager Pool => Instance._pool ??= new PoolManager().Init();
}
