using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager
{
	private const string coin = "Coin";	// ������ ������ const����
	private const string highScore = "highScore";
	public void SaveCoins(int amount)
	{
		PlayerPrefs.SetInt(coin, amount);
		PlayerPrefs.Save();
	}

	public int LoadCoins()
	{
		return PlayerPrefs.GetInt(coin, 0);
	}

	public void SaveHighScore(int score)
	{
		PlayerPrefs.SetInt(highScore, score);
	}

	public int LoadHighScore()
	{
		return PlayerPrefs.GetInt(highScore, 0);
	}
}
