using UnityEngine;

public class GameManager
{
	public bool gameStart = false;
	public int score = 0;
	private int coin = 0;
	private int totalCoin;
	private GameObject player;

	public void Init()
	{
		score = 0;
		Managers.UI.SetScore(score);
		totalCoin = Managers.Data.RoadCoins();
		Managers.UI.SetCoin(coin);
		player = GameObject.Find("Player");
	}

	public void GameStart()
	{
		gameStart = true;
	}

	public void MoveForward()
	{
		score++;
		Managers.UI.SetScore(score);
	}

	public void GameOver()
	{
		// 게임중 아님, 스코어0, 맵 초기화, 플레이어 위치 초기화, 카메라 초기화(일시정지)
		gameStart = false;
		score = 0;
		Managers.UI.SetScore(score);
		Managers.Map.MapInit();
		player.transform.position = new Vector3(4.5f, 2, 4.5f);
		Camera.main.gameObject.transform.position =  Camera.main.gameObject.GetComponent<CameraController>().positionDelta;
		player.GetComponent<PlayerController>().PlayerReset();
		totalCoin += coin;
		Managers.Data.SaveCoins(totalCoin);
	}

	public void GetCoin()
	{
		coin++;
		Managers.UI.SetCoin(coin);
	}
}
