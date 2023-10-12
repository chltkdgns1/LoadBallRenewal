using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorePopup : PopupStack
{
    [SerializeField]
    UIMoneyAnimation moneyTxt;

    [SerializeField]
    UIFrame moneyFrame;

    [SerializeField]
    UIStoreMenu[] storeMenu;

    protected override void Awake()
    {
        base.Awake();
        SetCoin();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SetCoin();
        OnMenu(0);
    }

    void SetData()
    {
        for(int i = 0; i < storeMenu.Length; i++)
        {
            storeMenu[i].SetData(null);
        }
    }

    public void SetCoin()
    {
        moneyTxt.SetNomalCoinTxt(GlobalData.StringCoin);
        moneyFrame.SetSize(GlobalData.Coin.ToString());
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
    }

    public void OnMenu(int index)
    {
        if (index < 0 || storeMenu.Length >= index)
        {
            Debug.LogError("public void OnMenu(int index) out of index : " + index);
            return;
        }

        for (int i = 0; i < storeMenu.Length; i++)
        {
            if (i == index)
                storeMenu[i].OnMenu(true);
            else
                storeMenu[i].OnMenu(false);
        }
    }

    public void Refresh()
    {
        SetData();
    }
}

