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

    static public void FinishPurchase(bool result = true)
    {
        GoogleIAP.purchaseProductId = null;
        LobbySceneManager.instance?.RefreshStorePopupMenu();
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
            ProductResultManager.ResultPurchaseProduct(ProductTypes.DELETE_CASH_ADS);
            Debug.Log("Success Purchase 'Delete Ads'");
        }
    }
}
