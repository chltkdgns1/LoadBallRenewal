using UnityEngine;

public class ProductResultManager
{
    public static void ResultPurchaseProduct(ProductTypes type)
    {
        ResultCase(type);
    }

    static void ResultCase(ProductTypes type)
    {
        switch (type)
        {
            case ProductTypes.DELETE_GOLD_ADS:
                SetDeleteAdsGoldResult();
                break;
            case ProductTypes.DELETE_CASH_ADS:
                SetDeleteAdsCashResult();
                break;         
        }
    }

    static void SetDeleteAdsGoldResult()
    {
        GoogleIAPCallBackManager.FinishPurchase();
        Popup<NoticePopup>.ShowPopup(PopupPath.PopupNotice, StringList.LanguageTable, StringList.PurchaseDeleteAdsGoldSuccess);
    }

    static async void SetDeleteAdsCashResult()
    {
        await GoogleFirebaseManager.WriteDeleteAds(GlobalData.Uid);
        GlobalData.IsDeleteAds = true;
        GoogleIAPCallBackManager.FinishPurchase();
        Popup<NoticePopup>.ShowPopup(PopupPath.PopupNotice, StringList.LanguageTable, StringList.PurchaseDeleteAdsGoldSuccess);
    }
}

