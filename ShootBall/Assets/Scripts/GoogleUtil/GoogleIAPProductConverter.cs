using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProductTypes
{
    DeleteAdsGoogle,
    UnLockStageThree,
    UnLockStageFour,
    UnLockStageFive,

    // ���� ��ǰ�� �ƴ�
    NonGoolgeProduct_DeleteAds, // �ش� enum type �� ���� �ϴܿ� ��ġ��. ���� ��ǰ�� �ƴ�
}

public class ProductData
{
    public string productId;
    public string productName;

    public ProductData(string id, string name)
    {
        productId = id;
        productName = name;
    }
}

public class GoogleIAPProductConverter
{
    static readonly string[] productId = { "delads", "keythree", "keyfour", "keyfive" };
    static readonly string[] productName = { "delads", "keythree", "keyfour", "keyfive" };

    static public List<ProductData> productData = new List<ProductData>();
    static GoogleIAPProductConverter instance = new GoogleIAPProductConverter();

    GoogleIAPProductConverter()
    {
        productData.Clear();
        for (int i = 0; i < productId.Length; i++)
        {
            productData.Add(new ProductData(productId[i], productName[i]));
        }
    }

    static public string ConvertTypeToId(ProductTypes type)
    {
        int typeIndex = (int)type;
        if (productId.Length <= typeIndex)
        {
            return GetNonGoogleId(type);
        }
        return productId[typeIndex];
    }

    static string GetNonGoogleId(ProductTypes type)
    {
        switch (type)
        {
            case ProductTypes.NonGoolgeProduct_DeleteAds:
                return "NonGoolgeProduct_DeleteAds";
        }
        return null;
    }
}
