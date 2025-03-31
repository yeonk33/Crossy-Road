using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager
{
	public bool gameStart = false;
	public int score = 0;
	Text scoreText;

	public void GameStart()
	{
		gameStart = true;
		score = 0;
		scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
		scoreText.text = score.ToString();
	}

	public void MoveForward()
	{
		score++;
		scoreText.text = score.ToString();
	}
}
