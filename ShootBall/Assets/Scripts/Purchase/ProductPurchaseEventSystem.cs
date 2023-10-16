using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class ProductPurchaseEvent
{
    public Product product;
    public Action<bool> complete;
    public Action<Product, Action<bool>> act;

    public ProductPurchaseEvent(Product product, Action<bool> complete, Action<Product, Action<bool>> act)
    {
        this.product = product;
        this.complete = complete;
        this.act = act;
    }
}

public class ProductPurchaseEventSystem : MonoBehaviour
{
    public static ProductPurchaseEventSystem instance = null;
    static Queue<ProductPurchaseEvent> eventQueue = new Queue<ProductPurchaseEvent>();

    [Serializable]
    class PendingProductInfo
    {
        public GameObject pendingPrefabs;
        public UIProduct uiPendingProduct { get; set; }

        public void Init(Transform parent)
        {
            var ob = Instantiate(pendingPrefabs, parent);
            uiPendingProduct = ob.GetComponent<UIProduct>();
        }
    }

    [SerializeField]
    PendingProductInfo pendingProductInfo;

    [SerializeField]
    Transform purchasePopupTrans;

    bool IsProcessState = false;

    public void Awake()
    {
        if (instance == null) 
            instance = this;
        else
        {
            enabled = false;
            return;
        }

        eventQueue.Clear();
        pendingProductInfo.Init(purchasePopupTrans);
    }

    public void OnEnable()
    {
        IsProcessState = false;
        purchasePopupTrans.gameObject.SetActive(false);
    }

    static public void AddPurchaseEvent(ProductPurchaseEvent evt)
    {
        eventQueue.Enqueue(evt);
    }

    private void Update()
    {
        if (eventQueue.Count == 0 || IsProcessState == true)
        {
            return;
        }

        IsProcessState = true;
        ExcutePurchaseEvent(eventQueue.Dequeue());
    }

    void ExcutePurchaseEvent(ProductPurchaseEvent evt)
    {
        purchasePopupTrans.gameObject.SetActive(true);

        var pId = evt.product.definition.id;
        var info = GlobalData.GetProductInfo(pId);

        if (info == null)
        {
            Debug.LogError("ExcutePurchaseEvent - info is null");
            return;
        }

        pendingProductInfo.uiPendingProduct.SetData(info);
        evt.complete += FinishProcessstate;
        StartCoroutine(WaitAction(evt));
    }

    IEnumerator WaitAction(ProductPurchaseEvent evt)
    {
        yield return new WaitForSeconds(3f);
        evt.act(evt.product, evt.complete);
    }

    public void SetProcessState(bool state)
    {
        IsProcessState = state;
    }

    public void FinishProcessstate(bool isSuccess) // 타입을 맞춰주기 위한 변수임..
    {
        Debug.LogWarning("미결제 상품 프로세스 종료");
        IsProcessState = false;
        purchasePopupTrans.gameObject.SetActive(false);
    }
}
