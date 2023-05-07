public class PurchaseDeleteAds : PurchaseItem
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void OnClickProduct()
    {
        base.OnClickProduct();
    }

    protected override void SetIAPStateCase()
    {
        if (GlobalData.IsDeleteAds == true)
        {
            iapState = IAPState.AlReadyPurchase;
        }
        base.SetIAPStateCase();
    }
}

