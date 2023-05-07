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
        Debug.Log("�������");
#else
        frontAdsKey = testFrontAdsKeyStr;
        Debug.Log("���ĺ���");
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
        Debug.LogWarning("���� ��� �غ�");
        if (GlobalData.IsDeleteAds)
        {
            Debug.LogWarning("���� ���ŷ� ���� �����");
            return;
        }

        if (frontAds == null)
        {
            Debug.LogWarning("���� ����� �� �����ϴ�.");
            return;
        }

        Debug.LogWarning("���� ��� ����");
        if (frontAds.IsLoaded())
        {
            Debug.LogWarning("���� �ε�� �� ����");
            frontAds.Show();
        }
        Debug.LogWarning("���� ��� �Ǵ� ���� �� �ٽ� ��ε�");
        InitFrontAds();
    }
}


