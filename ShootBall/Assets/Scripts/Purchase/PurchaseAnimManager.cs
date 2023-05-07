using System;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseAnimManager : MonoBehaviour
{
    static public PurchaseAnimManager instance;

    [SerializeField]
    PurchaseAnimation purchaseAnimation;

    [SerializeField]
    PurchaseMoneyAnimation purchaseMoneyAnimation;

    [Serializable]
    class PurchaseAnimData
    {
        public ProductTypes productType;
        public string[] spritePath;
    }

    [SerializeField]
    PurchaseAnimData[] purchaseAnimData;

    Dictionary<ProductTypes, PurchaseAnimData> purchaseDic = new Dictionary<ProductTypes, PurchaseAnimData>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            enabled = false;
            return;
        }

        Init();
    }

    void Init()
    {
        purchaseDic.Clear();

        if (purchaseAnimData == null)
        {
            BackEndLogger.Log("Error", BackEndLogger.LogType.ERROR, "PurchaseAnimManager Init purchaseAnimData == null");
            return;
        }

        for (int i = 0; i < purchaseAnimData.Length; i++)
        {
            ProductTypes type = purchaseAnimData[i].productType;
            purchaseDic.Add(type, purchaseAnimData[i]);
        }

        BackEndLogger.Log("PurchaseAnimManager", BackEndLogger.LogType.NOMAL, "Init ³¡");
    }

    public IComplete StartAnimation(ProductTypes type)
    {
        if (purchaseDic.ContainsKey(type) == false)
        {
            Debug.LogError("StartAnimation(type) hasnt : " + type);
            return null;
        }

        LobbySceneManager.instance?.StorePopupProperty?.gameObject.SetActive(true);
        LobbySceneManager.instance?.StorePopupProperty?.SetMenuProducts(type);

        List<string> spritePath = UtilManager.ToList<string>(purchaseDic[type].spritePath);
        purchaseAnimation.SetSpritePath(spritePath);
        //purchaseAnimation.transform.SetAsLastSibling();
        purchaseAnimation.gameObject.SetActive(true);
        return purchaseAnimation;
    }

    public IComplete StartMoneyAnim(long price)
    {
        return purchaseMoneyAnimation.StartCoinAnim(price);
    }
}

