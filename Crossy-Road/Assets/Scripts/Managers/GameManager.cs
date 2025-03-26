using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
	public bool gameStart = false;
	public int score = 0;

	public void GameStart()
	{
		gameStart = true;
		score = 0;
	}

	public void MoveForward()
	{
		score++;
	}
}
