using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UIManager
{
	private Text scoreText;
	private Text coinText;

	public UIManager Init()
	{
		scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
		coinText = GameObject.Find("CoinText").GetComponent<Text>();

		return this;
	}

	public void SetScore(int score)
	{
		scoreText.text = score.ToString();
	}

	public void SetCoin(int coin)
	{
		coinText.text = coin.ToString();
	}
}
