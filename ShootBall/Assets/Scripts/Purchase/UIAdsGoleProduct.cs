using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UIAdsGoleProduct : UIProduct
{
    //[SerializeField]
    //string purchaseSuccessData;

    //public enum PurchaseResult
    //{
    //    INVALID_PURCHASE_DATA,
    //    DON_TAKE_MONEY,
    //    INCONSISTANT_PRICE_DATA,
    //    DON_TAKE_PRICE_DATA,
    //    LACK_MONEY,
    //    SUCCESS,
    //    NONE
    //}

    //QueryAns coinResult;
    //PurchaseResult purchaseResult;

    //protected Action<Action> onPurchaseOk = null;
    //protected Action<int> onPurchasePopupResult = null;

    //protected override void Awake()
    //{

    //}

    //public override void SetPurchaseOk(Action<Action> act)
    //{
    //    onPurchaseOk = act;
    //}

    //public override void SetPurchasePopupResult(Action<int> act)
    //{
    //    onPurchasePopupResult = act;
    //}

    //protected override void OnEnable()
    //{
    //    base.OnEnable();
    //}

    //public override void OnClickProduct()
    //{
    //    SetIAPStateCase();
    //    onClickProduct?.Invoke((int)iapState);
    //    if (iapState != IAPState.OffLinePurchase && iapState != IAPState.AlReadyPurchase)
    //    {
    //        onPurchaseOk?.Invoke(OnPurchaseSuccess);
    //    }
    //}

    //protected override void SetIAPStateCase()
    //{
    //    iapState = IAPState.InAppGameIAP_RemoveAdsGold;

    //    if (GlobalData.IsDeleteAds == true)
    //    {
    //        iapState = IAPState.AlReadyPurchase;
    //    }
    //    base.SetIAPStateCase();
    //}

    //public void OnPurchaseSuccess()
    //{
    //    OnCompletePurchase(true);
    //}

    //async public void OnCompletePurchase(bool isSuccess)
    //{
    //    List<string> purchaseData = new List<string>();
    //    UtilManager.Split(purchaseData, purchaseSuccessData, ',');

    //    // 사는 물건 명, 수량, 구매하는 재화, 가격
    //    await ProcessPurchase(purchaseData);
    //    PrintPurchaseResult(ref purchaseData);
    //}

    //async Task ProcessPurchase(List<string> purchaseData)
    //{
    //    purchaseResult = PurchaseResult.NONE;
    //    Debug.Log("Start ProcessPurchase");
    //    if (purchaseData.Count != 4)
    //    {
    //        Debug.LogWarning("Error : purchaseData size is invalid");
    //        purchaseResult = PurchaseResult.INVALID_PURCHASE_DATA;
    //        return;
    //    }

    //    string item = purchaseData[0];
    //    int cnt = int.Parse(purchaseData[1]);
    //    string moneyType = purchaseData[2];
    //    long price = long.Parse(purchaseData[3]);
    //    long totalPrice = price * cnt;

    //    if (IsPossiblePurchaseItem(moneyType, totalPrice) == false)
    //    {
    //        Debug.LogWarning("LACK_MONEY");
    //        purchaseResult = PurchaseResult.LACK_MONEY;
    //        return;
    //    }

    //    coinResult = QueryAns.NONE;

    //    await GoogleFirebaseManager.ReadCoinData(GlobalData.Uid, (result) =>
    //    {
    //        coinResult = result;
    //    });

    //    if (coinResult != QueryAns.SUCCESS)
    //    {
    //        Debug.LogWarning("Error : DON_TAKE_MONEY");
    //        purchaseResult = PurchaseResult.DON_TAKE_MONEY;
    //        return;
    //    }

    //    switch (item)
    //    {
    //        case StringList.PurchaseDeleteAdsKey:
    //            await GoogleFirebaseManager.ReadPurchaseData(StringList.FirebaseStoreDeleteAdsPath, (result, resPrice) =>
    //            {
    //                if (result == QueryAns.SUCCESS)
    //                {
    //                    if (price != resPrice)
    //                    {
    //                        Debug.LogWarning("Error : purchase price is not same");
    //                        purchaseResult = PurchaseResult.INCONSISTANT_PRICE_DATA;
    //                        return;
    //                    }
    //                }
    //                else
    //                {
    //                    Debug.LogWarning("Error : DON_TAKE_PRICE_DATA result : " + result);
    //                    purchaseResult = PurchaseResult.DON_TAKE_PRICE_DATA;
    //                }
    //            });
    //            break;
    //        default:
    //            return;
    //    }

    //    if (purchaseResult == PurchaseResult.NONE)
    //    {
    //        purchaseResult = PurchaseResult.SUCCESS;

    //        GlobalData.Coin -= price;
    //        if (GlobalData.Coin < 0)
    //        {
    //            Debug.LogError("해킹이 의심됨.");
    //            GlobalData.Coin = 0;
    //        }

    //        await GoogleFirebaseManager.WriteCoinData(GlobalData.Uid, GlobalData.Coin);
    //        Debug.Log("purchase is success");
    //    }
    //    return;
    //}

    //void PrintPurchaseResult(ref List<string> purchaseData)
    //{
    //    if (purchaseResult >= PurchaseResult.NONE || purchaseResult < 0)
    //    {
    //        Debug.LogError("Error : PrintPurchaseResult result = NONE or Under Zero");
    //        return;
    //    }

    //    if (purchaseResult == PurchaseResult.SUCCESS)
    //    {
    //        PurchaseResultManager.ResultPurchaseProduct(productType, int.Parse(purchaseData[1]), long.Parse(purchaseData[3]));
    //    }
    //    else
    //    {
    //        onPurchasePopupResult?.Invoke((int)purchaseResult);
    //    }
    //}

    //bool IsPossiblePurchaseItem(string moneyType, long price)
    //{
    //    switch (moneyType)
    //    {
    //        case StringList.PurchaseMoneyTypeCash:
    //            return false;
    //        case StringList.PurchaseMoneyTypeGold:
    //            if (GlobalData.Coin < price)
    //            {
    //                return false;
    //            }
    //            break;
    //    }
    //    return true;
    //}
}

