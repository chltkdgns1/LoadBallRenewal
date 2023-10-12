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
    class ProductData
    {
        public GameObject productPrefabs;
        public UIPendingProduct uiPendingProduct { get; set; }

        public void Init(Transform parent)
        {
            var ob = Instantiate(productPrefabs, parent);
            uiPendingProduct = ob.GetComponent<UIPendingProduct>();
        }

        public void SetActive(bool isFlag)
        {
            uiPendingProduct.gameObject.SetActive(isFlag);
        }
    }

    [SerializeField]
    ProductData[] productData;

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

        if (productData == null)
        {
            BackEndLogger.Log("Error", BackEndLogger.LogType.ERROR, "PurchaseEventSystem Awake productData == null");
            return;
        }

        for (int i = 0; i < productData.Length; i++)
        {
            productData[i].Init(purchasePopupTrans);
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

        for (int i = 0; i < productData.Length; i++)
        {
            productData[i].SetActive(false);
        }

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

    void PrintIndex(int index)
    {
        if (index < 0 || index >= productData.Length)
        {
            Debug.LogError("PrintIndex �ε��� ���� " + productData.Length + " : " + index);
            return;
        }

        for (int i = 0; i < productData.Length; i++)
        {
            productData[i].SetActive(false);
        }

        productData[index].SetActive(true);
    }

    void ExcutePurchaseEvent(ProductPurchaseEvent evt)
    {
        purchasePopupTrans.gameObject.SetActive(true);
        for (int i = 0; i < productData.Length; i++)
        {
            if (productData[i].uiPendingProduct.GetProductId() == evt.product.definition.id)
            {
                Debug.LogWarning("�̰��� ��ǰ ���� ����");
                PrintIndex(i);
                evt.complete += FinishProcessstate;
                //evt.act(evt.product, evt.complete);
                StartCoroutine(WaitAction(evt));
                return;
            }
        }

        Debug.LogError(evt.product.definition.id + " ��ǰ ��ϵ��� ���� ���̵�");
        // �̰����� �ȳ� ��ǰ�� �����..
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

    public void FinishProcessstate(bool isSuccess) // Ÿ���� �����ֱ� ���� ������..
    {
        Debug.LogWarning("�̰��� ��ǰ ���μ��� ����");
        IsProcessState = false;
        purchasePopupTrans.gameObject.SetActive(false);
    }
}
