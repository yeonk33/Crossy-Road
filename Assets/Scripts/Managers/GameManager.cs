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
		// ������ �ƴ�, ���ھ�0, �� �ʱ�ȭ, �÷��̾� ��ġ �ʱ�ȭ, ī�޶� �ʱ�ȭ(�Ͻ�����)
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
