using GoogleMobileAds.Api;
using UnityEngine;

public class AdController : MonoBehaviour
{
	private RewardedAd rewardedAd;
	private void Start()
	{
		MobileAds.Initialize(initStatus => { });
		Init();
	}

	private void Init()
	{
		string adId = "ca-app-pub-3940256099942544/5224354917"; // �׽�Ʈ ������ ���� ID

		AdRequest request = new AdRequest();

		RewardedAd.Load(adId, request, (RewardedAd ad, LoadAdError error) =>
		{
			if (error != null || ad == null) {
				Debug.LogError("������ ���� �ε� ����: " + error);
				return;
			}

			rewardedAd = ad;

			// �̺�Ʈ ����
			rewardedAd.OnAdFullScreenContentClosed += RewardedAd_OnAdFullScreenContentClosed;

			Debug.Log("������ ���� �ε� ����");
		});
	}

	private void RewardedAd_OnAdFullScreenContentClosed()
	{
		Debug.Log("���� ����");
		Init();	// �ٽ� �ε�
	}

	public void ShowRewardAd()
	{
		if (rewardedAd != null && rewardedAd.CanShowAd()) {
			rewardedAd.Show((Reward reward) =>
			{
				Debug.Log("���� ��û �Ϸ�");
				int coin = Managers.Data.LoadCoins() + 5;
				Managers.UI.SetCoin(coin);
				Managers.Data.SaveCoins(coin);
			});
		} else {
			Debug.Log("������ ���� �غ� �� ��");
			Init();
			//ShowRewardAd();
		}
	}
}
