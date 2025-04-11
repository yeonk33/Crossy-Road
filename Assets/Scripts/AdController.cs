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
		string adId = "ca-app-pub-3940256099942544/5224354917"; // 테스트 보상형 광고 ID

		AdRequest request = new AdRequest();

		RewardedAd.Load(adId, request, (RewardedAd ad, LoadAdError error) =>
		{
			if (error != null || ad == null) {
				Debug.LogError("보상형 광고 로드 실패: " + error);
				return;
			}

			rewardedAd = ad;

			// 이벤트 연결
			rewardedAd.OnAdFullScreenContentClosed += RewardedAd_OnAdFullScreenContentClosed;

			Debug.Log("보상형 광고 로드 성공");
		});
	}

	private void RewardedAd_OnAdFullScreenContentClosed()
	{
		Debug.Log("광고 닫힘");
		Init();	// 다시 로드
	}

	public void ShowRewardAd()
	{
		if (rewardedAd != null && rewardedAd.CanShowAd()) {
			rewardedAd.Show((Reward reward) =>
			{
				Debug.Log("광고 시청 완료");
				int coin = Managers.Data.LoadCoins() + 5;
				Managers.UI.SetCoin(coin);
				Managers.Data.SaveCoins(coin);
			});
		} else {
			Debug.Log("보상형 광고 준비 안 됨");
			Init();
			//ShowRewardAd();
		}
	}
}
