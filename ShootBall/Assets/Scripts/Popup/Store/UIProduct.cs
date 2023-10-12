using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ProductInfo
{

}

public class UIProduct : MonoBehaviour
{
    string[] languageTablesNotice =
    {
            StringList.StoreDisconnectState, 
            StringList.StoreNotice
    };

    [SerializeField]
    protected ProductTypes productType;

    [SerializeField]
    int productIdIndex;

    public enum IAPState
    {
        READY_PRODUCTION,
        INGAME_GOLD,
        CASH
    }

    [SerializeField]
    protected IAPState iapState = IAPState.CASH;

    public void SetData(ProductInfo info)
    {

    }

    public virtual void OnClickProduct()
    {
        switch (iapState)
        {
            case IAPState.READY_PRODUCTION:
                SetClickProduct(StringList.StorePurchaseAlready);
                return;
            case IAPState.INGAME_GOLD:
                if (GlobalData.IsCanBuyProduct())
                {
                    GoogleIAP.PurchaseProduct(GetProductId());
                }
                break;
            case IAPState.CASH:
                if (GlobalData.IsCanBuyProduct())
                {
                    GoogleIAP.PurchaseProduct(GetProductId());
                }
                break;
            default:
                SetClickProduct(StringList.StoreDisconnectState);
                break;
        }
    }

    public virtual string GetProductId()
    {
        return GoogleIAPProductConverter.GetProductId(productIdIndex);
    }

    public void SetClickProduct(string message)
    {
        SetNoticeLocalizationString("Language", message);
        SetPurchaseOk();
        Invoke("SetNoticeActive", 0.1f);
    }

    public void SetPurchaseOk()
    {
        var noticePopup = PopupStack.PopupShow<NoticePopup>(PopupPath.PopupNotice);
        noticePopup.SetOkAct(()=>
        {
            noticePopup.gameObject.SetActive(false);
        });
    }

    public void SetNoticeLocalizationString(string localization, string tableStr)
    {
        var noticePopup = PopupStack.PopupShow<NoticePopup>(PopupPath.PopupNotice);
        noticePopup.SetLocalizationString(localization, tableStr);
    }
}
