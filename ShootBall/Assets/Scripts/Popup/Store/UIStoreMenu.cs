using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStoreMenu : MonoBehaviour
{
    [SerializeField]
    int menuIndex;

    Action<int> act = null;

    public Color normalColor;
    public Color pressColor;

    public Image imgLine;
    public Text txt;

    List<UIProduct> productList = new List<UIProduct>();

    [SerializeField]
    GameObject productPrefab;

    [SerializeField]
    Transform scrollView;

    int itemSize = 0;

    public void SetData(List<ProductItemData> productItemDataList, Action<int> act)
    {
        this.act = act;

        if (productPrefab == null)
            return;

        if(productList.Count < productItemDataList.Count)
        {
            for (int i = 0; i < productItemDataList.Count - productList.Count; i++)
            {
                var uiProduct = Instantiate(productPrefab, scrollView).GetComponent<UIProduct>();
                productList.Add(uiProduct);
            }
        }
        else
        {
            for (int i = productItemDataList.Count; i < productList.Count; i++)
            {
                productList[i].gameObject.SetActive(false);
            }
        }

        itemSize = productItemDataList.Count;

        for (int i = 0; i < productItemDataList.Count; i++)
        {
            productList[i].SetData(productItemDataList[i]);
        }
    }

    public void OnClick()
    {
        if(productPrefab == null)
        {
            Popup<NoticePopup>.ShowPopup(PopupPath.PopupNotice, StringList.LanguageTable, StringList.StoreReadyProduct);
            act?.Invoke(0);
            return;
        }

        act?.Invoke(menuIndex);
    }

    public void OnMenu(bool isOn)
    {
        var color = pressColor;

        if (!isOn)
            color = normalColor;

        imgLine.color = color;
        txt.color = color;

        for(int i = 0; i < itemSize; i++)
        {
            productList[i].gameObject.SetActive(isOn);
        }
    }
}

