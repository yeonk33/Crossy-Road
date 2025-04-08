using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager
{
	private string coin = "Coin";
	private string highScore = "highScore";
	public void SaveCoins(int amount)
	{
		PlayerPrefs.SetInt(coin, amount);
		PlayerPrefs.Save();
	}

	public int RoadCoins()
	{
		return PlayerPrefs.GetInt(coin, 0);
	}

	public void SaveHighScore(int score)
	{
		PlayerPrefs.SetInt(highScore, score);
	}

	public int RoadHighScore()
	{
		return PlayerPrefs.GetInt(highScore, 0);
	}
}
