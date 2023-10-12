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

    public void SetData(List<ProductInfo> infoList)
    {
        for(int i = 0; i < infoList.Count; i++)
        {
            var uiProduct = Instantiate(productPrefab, transform).GetComponent<UIProduct>();
            uiProduct.SetData(infoList[i]);
            productList.Add(uiProduct);
        }
    }

    public void OnClick()
    {
        act?.Invoke(menuIndex);
    }

    public void OnMenu(bool isOn)
    {
        var color = pressColor;

        if (!isOn)
            color = normalColor;

        imgLine.color = color;
        txt.color = color;

        for(int i = 0; i < productList.Count; i++)
        {
            productList[i].gameObject.SetActive(isOn);
        }
    }
}

