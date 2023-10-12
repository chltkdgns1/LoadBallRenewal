using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProductTypes
{
    DELETE_CASH_ADS,
    DELETE_GOLD_ADS
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
    static readonly string[] productId = { "addeleteproduct"};
    static readonly string[] productName = { "±¤°í Á¦°Å"};

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

    static public string GetProductId(int index)
    {
        return productId[index];
    }
}
