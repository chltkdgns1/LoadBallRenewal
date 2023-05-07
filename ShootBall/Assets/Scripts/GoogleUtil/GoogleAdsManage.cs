using UnityEngine;
using GoogleMobileAds.Api;

public class GoogleAds
{
    static private InterstitialAd frontAds;

    const string testFrontAdsKeyStr = "ca-app-pub-3940256099942544/1033173712";
    const string realFrontAdsKeyStr = "ca-app-pub-4598746676104401/1300804578";

    static string frontAdsKey = "";

    static public bool IsCheckBuildAppBundle;

    static private GoogleAds instance = new GoogleAds();


    private GoogleAds()
    {
        InitAdsKey();
        InitFrontAds();
    }

    static public void OnStatic()
    {

    }

    void InitAdsKey()
    {
#if REAL
        frontAdsKey = realFrontAdsKeyStr;
        Debug.Log("리얼빌드");
#else
        frontAdsKey = testFrontAdsKeyStr;
        Debug.Log("알파빌드");
#endif
    }

    static private void InitFrontAds()
    {
        frontAds = new InterstitialAd(frontAdsKey);
        AdRequest request = new AdRequest.Builder().Build();

        frontAds.OnAdClosed += (sender, e) =>
        {
            Debug.LogWarning("OnAdClosed");
        };

        frontAds.OnAdDidRecordImpression += (sender, e) =>
        {
            Debug.LogWarning("OnAdDidRecordImpression");
        };

        frontAds.OnAdOpening += (sender, e) =>
        {
            Debug.LogWarning("OnAdOpening");
        };

        frontAds.OnPaidEvent += (sender, e) =>
        {
            Debug.LogWarning("OnPaidEvent");
        };

        frontAds.OnAdFailedToShow += (sender, e) =>
        {
            Debug.LogWarning("OnAdFailedToShow");
        };

        frontAds.OnAdFailedToLoad += (sender, e) =>
        {
            Debug.LogWarning("OnAdFailedToLoad");
        };
        frontAds.OnAdLoaded += (sender, e) =>
        {
            Debug.LogWarning("OnAdLoaded");
        };

        frontAds.LoadAd(request);
    }

    static public void AdsShow()
    {
        Debug.LogWarning("광고 출력 준비");
        if (GlobalData.IsDeleteAds)
        {
            Debug.LogWarning("광고 제거로 광고 미출력");
            return;
        }

        if (frontAds == null)
        {
            Debug.LogWarning("광고 출력할 수 없습니다.");
            return;
        }

        Debug.LogWarning("광고 출력 시작");
        if (frontAds.IsLoaded())
        {
            Debug.LogWarning("광고 로드됨 곧 실행");
            frontAds.Show();
        }
        Debug.LogWarning("광고 출력 또는 실패 후 다시 재로드");
        InitFrontAds();
    }
}


