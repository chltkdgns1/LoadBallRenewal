using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class PurchaseEvent
{
    public Product product;
    public Action<bool> complete;
    public Action<Product, Action<bool>> act;

    public PurchaseEvent(Product product, Action<bool> complete, Action<Product, Action<bool>> act)
    {
        this.product = product;
        this.complete = complete;
        this.act = act;
    }
}

public class PurchaseEventSystem : MonoBehaviour
{
    public static PurchaseEventSystem instance = null;
    static Queue<PurchaseEvent> eventQueue = new Queue<PurchaseEvent>();

    [Serializable]
    class ProductData
    {
        public GameObject productPrefabs;
        public ProductTypes productType;
        public GameObject productOb { get; set; }
        public string productId { get; set; }
    }

    [SerializeField]
    ProductData[] productData;

    [SerializeField]
    Transform purchasePopupTrans;

    bool IsProcessState = false;

    public void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            enabled = false;
            return;
        }

        eventQueue.Clear();

        if (productData == null)
        {
            BackEndLogger.Log("Error", BackEndLogger.LogType.ERROR, "PurchaseEventSystem Awake productData == null");
            return;
        }

        for (int i = 0; i < productData.Length; i++)
        {
            productData[i].productOb = Instantiate(productData[i].productPrefabs, purchasePopupTrans);
            productData[i].productOb.SetActive(false);
            productData[i].productId = GoogleIAPProductConverter.ConvertTypeToId(productData[i].productType);
        }
    }

    public void OnEnable()
    {
        IsProcessState = false;

        if (purchasePopupTrans == null)
        {
            BackEndLogger.Log("Error", BackEndLogger.LogType.ERROR, "PurchaseEventSystem OnEnable purchasePopupTrans == null");
            return;
        }

        if (purchasePopupTrans.gameObject == null)
        {
            BackEndLogger.Log("Error", BackEndLogger.LogType.ERROR, "PurchaseEventSystem OnEnable purchasePopupTrans == null");
            return;
        }

        purchasePopupTrans.gameObject.SetActive(false);
        for (int i = 0; i < productData.Length; i++)
        {
            productData[i].productOb.SetActive(false);
        }
    }

    static public void AddPurchaseEvent(PurchaseEvent evt)
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

    void PrintIndex(int index)
    {
        if (index < 0 || index >= productData.Length)
        {
            Debug.LogError("PrintIndex 인덱스 에러 " + productData.Length + " : " + index);
            return;
        }

        for (int i = 0; i < productData.Length; i++)
        {
            productData[i].productOb.SetActive(false);
        }

        productData[index].productOb.SetActive(true);
    }

    void ExcutePurchaseEvent(PurchaseEvent evt)
    {
        purchasePopupTrans.gameObject.SetActive(true);
        for (int i = 0; i < productData.Length; i++)
        {
            if (productData[i].productId == evt.product.definition.id)
            {
                Debug.LogWarning("미결제 상품 결제 시작");
                PrintIndex(i);
                evt.complete += FinishProcessstate;
                //evt.act(evt.product, evt.complete);
                StartCoroutine(WaitAction(evt));
                return;
            }
        }

        Debug.LogError(evt.product.definition.id + " 제품 등록되지 않은 아이디");
        // 미결제된 안내 상품을 출력함..
    }

    IEnumerator WaitAction(PurchaseEvent evt)
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
        // 미결제 상품 프로세스 종료

        Debug.LogWarning("미결제 상품 프로세스 종료");

        if (isSuccess == false)
        {
            ThreadEvent.AddThreadEvent(() =>
            {
                IsProcessState = false;
                purchasePopupTrans.gameObject.SetActive(false);
            });
        }
        else
        {
            ThreadEvent.AddThreadEvent(() =>
            {
                IsProcessState = false;
                purchasePopupTrans.gameObject.SetActive(false);
            });
        }
    }
}
