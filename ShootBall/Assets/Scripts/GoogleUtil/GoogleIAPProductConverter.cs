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

    public ProductData(string id)
    {
        productId = id;
    }
}

public class GoogleIAPProductConverter
{
    static readonly string[] productId = { "addeleteproduct"};

    static public List<ProductData> productData = new List<ProductData>();
    static GoogleIAPProductConverter instance = new GoogleIAPProductConverter();

    GoogleIAPProductConverter()
    {
        productData.Clear();
        for (int i = 0; i < productId.Length; i++)
        {
            productData.Add(new ProductData(productId[i]));
        }
    }

    public static ProductTypes GetProductType(string id)
    {
        if (id.Equals("addeleteproduct"))
        {
            return ProductTypes.DELETE_CASH_ADS;
        }

        return ProductTypes.DELETE_GOLD_ADS;
    }
}
