using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorePopup : PopupStack
{
    public Vector3 startScale = new Vector3(0.8f, 0.8f);
    public float endScale = 1f;
    public float duration = 0.5f;
    public float endAlpha = 1f;
    public float delay = 0.1f;

    string[] languageTablesNotice =
    {
            StringList.StoreDisconnectState, StringList.StoreNotice,
            StringList.StorePurchaseAlready, StringList.StoreRemoveAdsGoldKey
        };

    string[] langugeTableNoticeOkResult =
    {
            StringList.StoreCanNotProcess1,StringList.StoreCanNotProcess2,StringList.StoreCanNotProcess3,StringList.StoreCanNotProcess4,
            StringList.StoreLackMoney
        };

    [SerializeField]
    PurchaseMoneyAnimation moneyTxt;

    [SerializeField]
    UIFrame moneyFrame;

    [Serializable]
    class MenuManage
    {
        public GameObject contentsPar;

        public Color normalColor;
        public Color pressColor;

        [Serializable]
        public class MenuData
        {
            public List<GameObject> menuList = new List<GameObject>();
            public Image imgLine;
            public Text txt;
        }

        public List<MenuData> StoreItemList { get; set; } = new List<MenuData>();
        public List<MenuData> storePrefabsList = new List<MenuData>();

        public int Index { get; set; }

        public void Init(Action<int> onClickProduct, Action<Action> onPurchaseOk, Action<int> onPurchasePopupResult)
        {
            StoreItemList.Clear();
            int cnt = storePrefabsList.Count;
            for (int i = 0; i < cnt; i++)
            {
                MenuData temp = new MenuData();
                temp.imgLine = storePrefabsList[i].imgLine;
                temp.txt = storePrefabsList[i].txt;

                foreach (var prefabs in storePrefabsList[i].menuList)
                {
                    GameObject gTemp = Instantiates(prefabs, contentsPar.transform);

                    var pItem = gTemp.GetComponent<PurchaseItem>();

                    pItem.SetPurchasePopupResult(onPurchasePopupResult);
                    pItem.SetClickProduct(onClickProduct);
                    pItem.SetPurchaseOk(onPurchaseOk);

                    gTemp.SetActive(false);
                    temp.menuList.Add(gTemp);
                }
                StoreItemList.Add(temp);
            }
        }

        public void OnMenu(int index)
        {
            if (Index != index)
                OffMenu(Index);

            Index = index;

            StoreItemList[Index].imgLine.color = pressColor;
            StoreItemList[Index].txt.color = pressColor;

            var obArr = StoreItemList[Index].menuList;
            int cnt = obArr.Count;

            for (int i = 0; i < cnt; i++)
            {
                obArr[i].SetActive(true);
            }
        }

        public void OffMenu(int index)
        {
            var obArr = StoreItemList[Index].menuList;
            int cnt = obArr.Count;

            StoreItemList[Index].imgLine.color = normalColor;
            StoreItemList[Index].txt.color = normalColor;

            for (int i = 0; i < cnt; i++)
            {
                obArr[i].SetActive(false);
            }
        }

        public void Clear()
        {
            int cnt = StoreItemList.Count;
            for (int i = 0; i < cnt; i++)
            {
                StoreItemList[i].imgLine.color = normalColor;
                StoreItemList[i].txt.color = normalColor;
                foreach (var ob in StoreItemList[i].menuList)
                {
                    ob.SetActive(false);
                }
            }
        }

        public void Refresh()
        {
            var obArr = StoreItemList[Index].menuList;
            int cnt = obArr.Count;

            for (int i = 0; i < cnt; i++)
            {
                obArr[i].SetActive(true);
                obArr[i].GetComponent<PurchaseItem>().Refresh();
            }
        }
    }

    [SerializeField]
    MenuManage tabManager;

    static public int CacheIndex { get; set; }

    static public GameObject Instantiates(GameObject prefabs, Transform trans)
    {
        return Instantiate(prefabs, trans);
    }

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        CacheIndex = 0;
        tabManager.Clear();
        tabManager.OnMenu(CacheIndex);
        ResetAll();
        UIEffectManager.PrintBouncePopup(gameObject, startScale, duration, endScale, endAlpha, delay);
    }

    protected override void Start()
    {
        ResetAll();
    }

    void Init()
    {
        tabManager.Init(SetClickProduct, SetPurchaseOk, SetClickProductOk);
    }

    public void ResetAll()
    {
        SetCoin();
    }

    public void SetCoin()
    {
        moneyTxt.SetNomalCoinTxt(GlobalData.StringCoin);
        moneyFrame.SetSize(GlobalData.Coin.ToString());
    }

    void Reset() // 리셋이란 인스펙터의 데이터를 클리어해주는 메서드
    {

    }

    public void OnMenuClick()
    {
        tabManager.OnMenu(CacheIndex);
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
    }

    public void SetClickProduct(int index)
    {
        if (index < 0 || languageTablesNotice.Length <= index)
        {
            Debug.LogError("out of index on SetNoticeTxt " + index);
            return;
        }

        SetNoticeLocalizationString("Language", languageTablesNotice[index]);
        ResetPopupNoticeOkAct();
        Invoke("SetNoticeActive", 0.1f);
    }

    public void SetClickProductOk(int index)
    {
        if (index < 0 || langugeTableNoticeOkResult.Length <= index)
        {
            Debug.LogError("out of index on SetNoticeTxt " + index);
            return;
        }

        SetNoticeLocalizationString("Language", langugeTableNoticeOkResult[index]);
        ResetPopupNoticeOkAct();
        Invoke("SetNoticeActive", 0.1f);
    }

    public void SetNoticeActive()
    {
        var noticePopup = PopupStack.PopupShow<NoticePopup>(PopupPath.PopupNotice);
        noticePopup.gameObject.SetActive(true);
    }

    public void SetPurchaseOk(Action act)
    {
        var noticePopup = PopupStack.PopupShow<NoticePopup>(PopupPath.PopupNotice);
        noticePopup.SetOkAct(act);
    }

    public void ResetPopupNoticeOkAct()
    {
        var noticePopup = PopupStack.PopupShow<NoticePopup>(PopupPath.PopupNotice);
        noticePopup.ResetOkAct();
    }

    public void SetNoticeLocalizationString(string localization, string tableStr)
    {
        var noticePopup = PopupStack.PopupShow<NoticePopup>(PopupPath.PopupNotice);
        noticePopup.SetLocalizationString(localization, tableStr);
    }

    public void RefreshMenuProducts()
    {
        tabManager.Refresh();
    }

    int GetMenuIndex(ProductTypes type)
    {
        switch (type)
        {
            case ProductTypes.DeleteAdsGoogle:
            case ProductTypes.NonGoolgeProduct_DeleteAds:
                return 0;
            case ProductTypes.UnLockStageThree:
            case ProductTypes.UnLockStageFour:
            case ProductTypes.UnLockStageFive:
                return 1;
        }
        return 0;
    }

    public void SetMenuProducts(ProductTypes type)
    {
        CacheIndex = GetMenuIndex(type);
        tabManager.OnMenu(CacheIndex);
    }
}

