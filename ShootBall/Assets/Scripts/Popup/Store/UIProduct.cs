using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIProduct : MonoBehaviour
{
    ProductItemData productItemData;

    public enum IAPState
    {
        READY_PRODUCTION,
        INGAME_GOLD,
        CASH
    }

    [SerializeField]
    protected IAPState iapState = IAPState.CASH;

    [SerializeField] Text content1;
    [SerializeField] Text content2;
    [SerializeField] Text price;
    [SerializeField] Image itemImg;

    [SerializeField] GameObject gold;
    [SerializeField] GameObject cash;

    public void SetData(ProductItemData info)
    {
        productItemData = info;

        content1.text = info.content1;
        content2.text = info.content2.Replace("\\n", "\n");
        price.text = info.price.ToString() + " ¿ø";

        //content2.text = content2.text.Replace("\\n", "\n");

        itemImg.sprite = SpriteAtalsManager.instance.GetSprite("LobbyAtlas", info.imgPath);

        if (info.IsCash)
        {
            gold.SetActive(false);
            cash.SetActive(true);
            iapState = IAPState.CASH;
        }
        else
        {
            gold.SetActive(true);
            cash.SetActive(false);
            iapState = IAPState.INGAME_GOLD;
        }
    }

    public virtual void OnClickProduct()
    {
        var pId = GetProductId();
        switch (iapState)
        {
            case IAPState.READY_PRODUCTION:
                SetClickProduct(StringList.StoreReadyProduct);
                return;
            case IAPState.INGAME_GOLD:
                if (GlobalData.IsCanBuyProduct())
                {
                    if (IsCanBuyProduct(pId))
                        GoogleIAP.PurchaseProduct(pId);
                    else
                        SetClickProduct(StringList.StorePurchaseAlready);
                }
                break;
            case IAPState.CASH:
                if (GlobalData.IsCanBuyProduct())
                {
                    if (IsCanBuyProduct(pId))
                        GoogleIAP.PurchaseProduct(pId);
                    else
                        SetClickProduct(StringList.StorePurchaseAlready);
                }
                break;
            default:
                SetClickProduct(StringList.StoreDisconnectState);
                break;
        }
    }

    bool IsCanBuyProduct(string id)
    {
        var type = GoogleIAPProductConverter.GetProductType(GetProductId());
        switch (type)
        {
            case ProductTypes.DELETE_GOLD_ADS:
                if (GlobalData.IsDeleteAds)
                    return false;
                break;
            case ProductTypes.DELETE_CASH_ADS:
                if (GlobalData.IsDeleteAds)
                    return false;
                break;
            default:
                return false;
        }
        return true;
    }

    public virtual string GetProductId()
    {
        return productItemData.productId;
    }

    public void SetClickProduct(string message)
    {
        SetNoticeLocalizationString("Language", message);
        SetPurchaseOk();
        Invoke("SetNoticeActive", 0.1f);
    }

    void SetNoticeActive()
    {
        var noticePopup = PopupStack.PopupShow<NoticePopup>(PopupPath.PopupNotice);
        noticePopup.gameObject.SetActive(true);
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
