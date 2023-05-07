using UnityEngine;

public class PurchaseResultManager
{
    public static void ResultPurchaseProduct(ProductTypes type, int purchaseCnt, long price = 0)
    {
        ResultCase(type, purchaseCnt, price);
    }

    static void ResultCase(ProductTypes type, int purchaseCnt, long price)
    {
        switch (type)
        {
            case ProductTypes.NonGoolgeProduct_DeleteAds:
                SetDeleteAdsGoldResult(purchaseCnt, price);
                break;
            case ProductTypes.DeleteAdsGoogle:
                SetDeleteAdsGoogleResult(purchaseCnt);
                break;
            case ProductTypes.UnLockStageThree:
                SetUnLockThreeResult(purchaseCnt);
                break;
            case ProductTypes.UnLockStageFour:
                SetUnLockFourResult(purchaseCnt);
                break;
            case ProductTypes.UnLockStageFive:
                SetUnLockFiveResult(purchaseCnt);
                break;
        }
    }

    static async void SetDeleteAdsGoldResult(int purchaseCnt, long price)
    {
        int per = Random.Range(0, 100);
        Debug.Log("±¤°í Á¦°Å È®·ü : " + per);

        if (per >= 99)
        {
            // ¼º°ø
            await GoogleFirebaseManager.WriteDeleteAds(GlobalData.Uid);
            GlobalData.IsDeleteAds = true;

            PurchaseAnimManager.instance.StartMoneyAnim(price).OnComplete((ob) =>
            {
                PurchaseAnimManager.instance.StartAnimation(ProductTypes.NonGoolgeProduct_DeleteAds).OnComplete((ob) =>
                {
                    Popup<NoticePopup>.ShowPopup(PopupPath.PopupNotice, StringList.LanguageTable, StringList.PurchaseDeleteAdsGoldSuccess);
                }, null);
            }, null);
        }
        else
        {
            PurchaseAnimManager.instance.StartMoneyAnim(price).OnComplete((ob) =>
            {
                PurchaseAnimManager.instance.StartAnimation(ProductTypes.NonGoolgeProduct_DeleteAds).OnComplete((ob) =>
                {
                    Popup<NoticePopup>.ShowPopup(PopupPath.PopupNotice, StringList.LanguageTable, StringList.PurchaseDeleteAdsGoldFailed);
                }, null);
            }, null);
        }
        GoogleIAPCallBackManager.FinishPurchase();
    }

    static async void SetDeleteAdsGoogleResult(int purchaseCnt)
    {
        await GoogleFirebaseManager.WriteDeleteAds(GlobalData.Uid);
        GlobalData.IsDeleteAds = true;
        GoogleIAPCallBackManager.FinishPurchase();
        SuccessPurchase(ProductTypes.DeleteAdsGoogle);
    }

    static async void SetUnLockThreeResult(int purchaseCnt)
    {
        await GoogleFirebaseManager.WriteUnLockStageData(GlobalData.Uid, (int)KeyNumber.three);
        GlobalData.UnLockPage = (int)KeyNumber.three;
        GoogleIAPCallBackManager.FinishPurchase();
        SuccessPurchase(ProductTypes.UnLockStageThree);
    }

    static async void SetUnLockFourResult(int purchaseCnt)
    {
        await GoogleFirebaseManager.WriteUnLockStageData(GlobalData.Uid, (int)KeyNumber.four);
        GlobalData.UnLockPage = (int)KeyNumber.four;
        GoogleIAPCallBackManager.FinishPurchase();
        SuccessPurchase(ProductTypes.UnLockStageFour);
    }

    static async void SetUnLockFiveResult(int purchaseCnt)
    {
        await GoogleFirebaseManager.WriteUnLockStageData(GlobalData.Uid, (int)KeyNumber.five);
        GlobalData.UnLockPage = (int)KeyNumber.five;
        GoogleIAPCallBackManager.FinishPurchase();
        SuccessPurchase(ProductTypes.UnLockStageFive);
    }

    static void SuccessPurchase(ProductTypes type)
    {
        PurchaseAnimManager.instance.StartAnimation(type).OnComplete((ob) =>
        {
            Popup<NoticePopup>.ShowPopup(PopupPath.PopupNotice, StringList.LanguageTable, StringList.PurchaseSuccess);
        }, null);
    }
}

