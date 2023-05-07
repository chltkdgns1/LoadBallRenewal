using System;
using System.Collections.Generic;
using UnityEngine;

public enum KeyNumber
{
    three = 3,
    four,
    five
}

public class GoogleIAPCallBackManager
{
    static Dictionary<string, Action<bool>> productCompleteDic = new Dictionary<string, Action<bool>>();

    static GoogleIAPCallBackManager intance = new GoogleIAPCallBackManager();

    GoogleIAPCallBackManager()
    {
        Init();
    }

    void Init()
    {
        productCompleteDic.Clear();
        List<ProductData> prData = GoogleIAPProductConverter.productData;
        AddPurchaseEvent(prData[0].productId, OnCompletePurchaseDeleteAds);
        AddPurchaseEvent(prData[1].productId, OnCompletePurchaseUnLockPageThree);
        AddPurchaseEvent(prData[2].productId, OnCompletePurchaseUnLockPageFour);
        AddPurchaseEvent(prData[3].productId, OnCompletePurchaseUnLockPageFive);
    }

    static public bool ContainProductId(string productId)
    {
        return productCompleteDic.ContainsKey(productId);
    }

    static public Action<bool> GetComplete(string productId)
    {
        if (productCompleteDic.ContainsKey(productId) == false)
        {
            return null;
        }
        return productCompleteDic[productId];
    }

    void AddPurchaseEvent(string productId, Action<bool> complete)
    {
        if (productCompleteDic.ContainsKey(productId))
        {
            Debug.LogError("Error : Same AddPurchaseEvent Id = " + productId);
            return;
        }

        productCompleteDic.Add(productId, complete);
    }

    static public bool IsRegisteredProduct(string productId)
    {
        if (productCompleteDic.ContainsKey(productId) == false)
        {
            Debug.LogWarning("���� ���� : ��ǳʸ� �������� �ʴ� productId : " + productId);
            return false;
        }

        List<ProductData> prData = GoogleIAPProductConverter.productData;

        for (int i = 0; i < prData.Count; i++)
        {
            if (prData[i].productId == productId)
            {
                return true;
            }
        }

        Debug.LogWarning("���� ���� : ��� ���δ�Ʈ�� �������� �ʴ� productId : " + productId);
        return false;
    }

    void OnCompletePurchaseItem(bool isSuccess)
    {
        if (isSuccess)
        {
            Debug.Log("���� ����");
        }
        else
        {
            Debug.Log("���� ����");
        }

        GoogleIAP.purchaseProductId = null;
    }

    static public void FinishPurchase(bool result = true)
    {
        GoogleIAP.purchaseProductId = null;
        LobbySceneManager.instance?.RefreshStorePopupMenu();

        if (result == false)
        {
            Popup<NoticePopup>.ShowPopup(PopupPath.PopupNotice, StringList.LanguageTable, StringList.PurchaseFailed);
        }
    }

    void OnCompletePurchaseDeleteAds(bool isSuccess)
    {
        if (isSuccess == false)
        {
            Debug.Log("���� ����");
            FinishPurchase(false);
        }
        else
        {
            PurchaseResultManager.ResultPurchaseProduct(ProductTypes.DeleteAdsGoogle, 1);
            Debug.Log("Success Purchase 'Delete Ads'");
        }
    }

    void OnCompletePurchaseUnLockPageThree(bool isSuccess)
    {
        if (isSuccess == false)
        {
            Debug.Log("���� ���� " + KeyNumber.three);
            FinishPurchase(false);
        }
        else
        {
            PurchaseResultManager.ResultPurchaseProduct(ProductTypes.UnLockStageThree, 1);
            Debug.Log("Success Purchase 'UnLockPage'");
        }
    }

    void OnCompletePurchaseUnLockPageFour(bool isSuccess)
    {
        if (isSuccess == false)
        {
            Debug.Log("���� ���� " + KeyNumber.four);
            FinishPurchase(false);
        }
        else
        {
            PurchaseResultManager.ResultPurchaseProduct(ProductTypes.UnLockStageFour, 1);
            Debug.Log("Success Purchase 'UnLockPage'");
        }
    }

    void OnCompletePurchaseUnLockPageFive(bool isSuccess)
    {
        if (isSuccess == false)
        {
            Debug.Log("���� ���� " + KeyNumber.five);
            FinishPurchase(false);
        }
        else
        {
            PurchaseResultManager.ResultPurchaseProduct(ProductTypes.UnLockStageFive, 1);
            Debug.Log("Success Purchase 'UnLockPage'");
        }
    }
}
