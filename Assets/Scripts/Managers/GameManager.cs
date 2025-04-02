using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager
{
	public bool gameStart = false;
	public int score = 0;
	private Text scoreText;
	private GameObject player;

	public void Init()
	{
		score = 0;
		scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
		scoreText.text = score.ToString();
		player = GameObject.Find("Player");
	}

	public void GameStart()
	{
		gameStart = true;
	}

	public void MoveForward()
	{
		score++;
		scoreText.text = score.ToString();
	}

	public void GameOver()
	{
		// ������ �ƴ�, ���ھ�0, �� �ʱ�ȭ, �÷��̾� ��ġ �ʱ�ȭ, ī�޶� �ʱ�ȭ(�Ͻ�����)
		gameStart = false;
		score = 0;
		scoreText.text = score.ToString();
		Managers.Map.MapInit();
		player.transform.position = new Vector3(4.5f, 2, 4.5f);
		Camera.main.gameObject.transform.position =  Camera.main.gameObject.GetComponent<CameraController>().positionDelta;
	}
}
