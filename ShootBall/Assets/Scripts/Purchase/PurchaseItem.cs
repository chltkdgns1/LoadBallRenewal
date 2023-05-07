using System;
using UnityEngine;

public class PurchaseItem : MonoBehaviour
{
    [SerializeField]
    protected ProductTypes productType;

    public enum IAPState
    {
        OffLinePurchase,                    // 오프라인 상태에서 구매 클릭             ( 인게임 상태로 알아냄)
        ReadyProduction,                    // 상품 준비중                             ( 인스펙터 입력)
        AlReadyPurchase,                    // 이미 상품 구매함.                       ( 인게임 상태로 알아냄)
        InAppGameIAP_RemoveAdsGold,         // 게임 내 재화로 구매할 수 있는 상품      ( 인스펙터 입력)
        GoogleIAP,                          // 구글 IAP 로 결제 필요 상품              ( 인스펙터 입력)
        NONE                                // IAP 상품 클릭 안됨.
    }

    [SerializeField]
    protected IAPState iapState = IAPState.OffLinePurchase;

    protected Action<int> onClickProduct = null;

    public virtual void SetClickProduct(Action<int> act)
    {
        onClickProduct = act;
    }

    protected virtual void Awake() { }
    protected virtual void OnEnable() { }

    public virtual void OnClickProduct()
    {
        SetIAPStateCase();
        if (iapState != IAPState.GoogleIAP)
        {
            onClickProduct?.Invoke((int)iapState); // 출력될 팝업 세팅 및 출력
            return;
        }
        GoogleIAP.PurchaseProduct(GoogleIAPProductConverter.ConvertTypeToId(productType));
    }

    protected virtual void SetIAPStateCase()
    {
        if (GlobalData.IsGoogleLogin == false)
        {
            iapState = IAPState.OffLinePurchase;
        }
    }

    public virtual void SetPurchaseOk(Action<Action> act) { }
    public virtual void SetPurchasePopupResult(Action<int> act) { }
    public virtual void Refresh() { }
}
