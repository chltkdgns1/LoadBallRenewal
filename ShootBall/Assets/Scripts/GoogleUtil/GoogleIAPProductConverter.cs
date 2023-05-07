using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProductTypes
{
    DeleteAdsGoogle,
    UnLockStageThree,
    UnLockStageFour,
    UnLockStageFive,

    // 구글 상품이 아님
    NonGoolgeProduct_DeleteAds, // 해당 enum type 은 제일 하단에 위치함. 구글 상품이 아님
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
