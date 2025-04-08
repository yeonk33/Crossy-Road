using UnityEngine;

public class Managers : MonoBehaviour
{
	private static Managers s_instance;
	public static Managers Instance
	{
		get
		{
			s_instance = FindFirstObjectByType<Managers>(); // Managers 컴포넌트가 있는지 찾기
			
			// 없으면 컴포넌트 붙여서 만들기
			if (s_instance == null) {
				GameObject gameObject = new GameObject("Managers");
				s_instance = gameObject.AddComponent<Managers>();
				DontDestroyOnLoad(gameObject);
			}
			return s_instance;
		}
	}

	private PoolManager _pool;
	private MapManager _map;
	private GameManager _game;
	private PlayerDataManager _data;
	private UIManager _ui;
	public static PoolManager Pool => Instance._pool ??= new PoolManager().Init();
	public static MapManager Map => Instance._map ??= new MapManager();
	public static GameManager Game => Instance._game ??= new GameManager();
	public static PlayerDataManager Data => Instance._data ??= new PlayerDataManager();
	public static UIManager UI => Instance._ui ??= new UIManager().Init();
}
