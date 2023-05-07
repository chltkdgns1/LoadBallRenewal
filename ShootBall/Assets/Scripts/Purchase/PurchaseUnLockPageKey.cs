using UnityEngine;

public class PurchaseUnLockPageKey : PurchaseItem
{
    [SerializeField]
    protected int keyNumber; // 1 부터 시작함. 

    Transform lockObj;

    protected override void Awake()
    {
        base.Awake();
        lockObj = transform.Find("LockBack");

        if (lockObj == null)
        {
            lockObj = transform.GetChild(0).GetChild(1);
            Debug.Log("lockObj child Find");
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (GlobalData.UnLockPage + 1 < keyNumber)
        {
            lockObj.gameObject.SetActive(true);
        }
        else
        {
            lockObj.gameObject.SetActive(false);
        }
    }

    public override void Refresh()
    {
        if (GlobalData.UnLockPage + 1 < keyNumber)
        {
            lockObj.gameObject.SetActive(true);
        }
        else
        {
            lockObj.gameObject.SetActive(false);
        }
    }

    public override void OnClickProduct()
    {
        SetIAPStateCase();
        if (iapState == IAPState.NONE)
            return;

        base.OnClickProduct();
    }

    protected override void SetIAPStateCase()
    {
        if (GlobalData.UnLockPage >= keyNumber)
        {
            iapState = IAPState.AlReadyPurchase;
        }
        else if (GlobalData.UnLockPage + 1 != keyNumber)
        {
            iapState = IAPState.NONE;
        }

        base.SetIAPStateCase();
    }
}

