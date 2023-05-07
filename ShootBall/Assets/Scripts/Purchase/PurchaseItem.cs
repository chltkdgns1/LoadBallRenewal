using System;
using UnityEngine;

public class PurchaseItem : MonoBehaviour
{
    [SerializeField]
    protected ProductTypes productType;

    public enum IAPState
    {
        OffLinePurchase,                    // �������� ���¿��� ���� Ŭ��             ( �ΰ��� ���·� �˾Ƴ�)
        ReadyProduction,                    // ��ǰ �غ���                             ( �ν����� �Է�)
        AlReadyPurchase,                    // �̹� ��ǰ ������.                       ( �ΰ��� ���·� �˾Ƴ�)
        InAppGameIAP_RemoveAdsGold,         // ���� �� ��ȭ�� ������ �� �ִ� ��ǰ      ( �ν����� �Է�)
        GoogleIAP,                          // ���� IAP �� ���� �ʿ� ��ǰ              ( �ν����� �Է�)
        NONE                                // IAP ��ǰ Ŭ�� �ȵ�.
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
            onClickProduct?.Invoke((int)iapState); // ��µ� �˾� ���� �� ���
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
