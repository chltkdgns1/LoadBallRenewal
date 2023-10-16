using BackEnd;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Purchasing;

public class GoogleIAP : MonoBehaviour, IStoreListener
{
    static GoogleIAP instance = null;
    private static IStoreController m_StoreController;
    private static IExtensionProvider m_ExtensionProvider;

    static public string purchaseProductId = null;

    public class ReciptVerifyResult
    {
        public bool result;
    }

    // id �� �̸��� ���ƾ�����...;;; ������ �ȳ���.. ���� ��;;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            enabled = false;
            return;
        }

        if (GlobalData.IsGoogleLogin == false)
        {
            enabled = false;
            return;
        }
    }

    void Start()
    {
        Init();
    }

    void Init()
    {
        purchaseProductId = null;
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        List<ProductData> prData = GoogleIAPProductConverter.productData;

        for (int i = 0; i < prData.Count; i++)
        {
            builder.AddProduct(prData[i].productId, ProductType.Consumable, new IDs
                {
                    {prData[i].productName,GooglePlay.Name }
                });
        }

        UnityPurchasing.Initialize(this, builder);
    }

    static public bool IsInitialized
    {
        get
        {
            return m_StoreController != null;
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions) //   UnityPurchasing.Initialize(this, builder); �� ����� �Ŀ� ȣ���
    {
        Debug.Log("���� ���� OnInitialized Start");

        m_StoreController = controller;
        m_ExtensionProvider = extensions;

        List<ProductData> prData = GoogleIAPProductConverter.productData;

        for (int i = 0; i < prData.Count; i++)
        {
            var product = m_StoreController.products.WithID(prData[i].productId);
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(prData[i].productId + " ���� ����");
            }
            else if (product == null)
            {
                Debug.Log(prData[i].productId + " product = null");
            }
            else
            {
                Debug.Log(prData[i].productId + " not availableToPurchase");
            }
        }
        Debug.Log("���� ���� OnInitialized finish");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log("�ʱ�ȭ ���� : " + message);
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("�ʱ�ȭ ���� : " + error);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        purchaseProductId = null;
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Debug.Log("���� ����");

        if (GlobalData.IsGoogleLogin == false)
        {
            Debug.Log("���� �α����� �Ǿ����� �ʽ��ϴ�. ������ ������ �� �����ϴ�.");
            return PurchaseProcessingResult.Pending;
        }

        if (purchaseProductId != null && purchaseProductId != args.purchasedProduct.definition.id)
        {
            Debug.Log("���� ���� ������ �����մϴ� : " + purchaseProductId);
            return PurchaseProcessingResult.Pending;
        }

#if UNITY_EDITOR
        GoogleIAPCallBackManager.GetComplete(args.purchasedProduct.definition.id)?.Invoke(true);
        return PurchaseProcessingResult.Complete;
#else
        if (GoogleIAPCallBackManager.IsRegisteredProduct(args.purchasedProduct.definition.id) == false)
        {
            return PurchaseProcessingResult.Pending;
        }

        if (purchaseProductId == null)
        {
            purchaseProductId = args.purchasedProduct.definition.id;
            ProductPurchaseEventSystem.AddPurchaseEvent(new ProductPurchaseEvent(args.purchasedProduct, GoogleIAPCallBackManager.GetComplete(args.purchasedProduct.definition.id), RequestVerifyReceipt));
            return PurchaseProcessingResult.Pending;
        }

        RequestVerifyReceipt(args.purchasedProduct, GoogleIAPCallBackManager.GetComplete(args.purchasedProduct.definition.id));
        return PurchaseProcessingResult.Pending;
#endif
    }

    void RequestVerifyReceipt(Product product, Action<bool> complete)
    {
        Debug.Log("������ ���� ��û : " + product.definition.id);

        if (Backend.Receipt == null)
        {
            BackEndLogger.Log("Error", BackEndLogger.LogType.ERROR, "RequestVerifyReceipt Backend.Receipt == null");
            complete?.Invoke(false);
            return;
        }

        Backend.Receipt.IsValidateGooglePurchase(product.receipt, "receiptDescription", false, (callback) =>
        {
            // ������ ������ ������ ���
            if (callback.IsSuccess())
            {
                Debug.LogWarning("������ ���� ����");
                m_StoreController.ConfirmPendingPurchase(product);
                complete?.Invoke(true);
            }
            else
            {
                // ������ ������ ������ ���
                Debug.LogWarning("������ ���� ���� : " + callback);
                Debug.LogWarning(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", product.definition.id));
                complete?.Invoke(false);
            }
        });
    }

    IEnumerator CoSendRequest(Product product, Action<bool> complete)
    {
        // https �� ������ �����͸� ����;;
        const string url = "https://iapverifyserver.web.app/googleiab/receipt/validation";
        //const string url = "localhost:5000/googleiab/receipt/validation";

        StringBuilder reciptStrBuilder = new StringBuilder();

        for (int i = 0; i < product.receipt.Length - 1; i++)
        {
            reciptStrBuilder.Append(product.receipt[i]);
        }

        string recipt = reciptStrBuilder.ToString() + ",\"APIKey\" : \"VicGameStudio\"}";

        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(recipt);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            Debug.Log("SendRequest, request to web server.");
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("error : " + www.error);
                complete?.Invoke(false);
            }
            else
            {
                Debug.Log("SendRequest, response from web server.");
                Debug.Log("[Body]\n" + www.downloadHandler.text);
                Debug.Log("ConfirmPendingPurchase");

                var verifyResult = JsonUtility.FromJson<ReciptVerifyResult>(www.downloadHandler.text);

                if (GlobalData.IsGoogleLogin == false)
                {
                    Debug.Log("failed purchase reason disconnect internet");
                    complete?.Invoke(false);
                }

                if (verifyResult.result == true)
                {
                    Debug.Log("success purchase");
                    complete?.Invoke(true);
                    m_StoreController.ConfirmPendingPurchase(product);
                }
                else
                {
                    Debug.Log("failed purchase");
                    complete?.Invoke(false);
                }

                Debug.Log("Purchase Findish");
            }
        }
    }

    public static void PurchaseProduct(string productId)
    {
        if (m_StoreController == null)
        {
            Debug.LogWarning("���� ���� : ���� ��� �ʱ�ȭ ���� productId : " + productId);
            return;
        }

        if (GoogleIAPCallBackManager.IsRegisteredProduct(productId) == false)
        {
            return;
        }

        if (purchaseProductId != null && (string.IsNullOrEmpty(purchaseProductId) == false || purchaseProductId.Length > 0))
        {
            Debug.Log("IsProcessPurchase : true , Process about purchase");
            return;
        }

        purchaseProductId = productId;
        //�������μ��� ����
        m_StoreController.InitiatePurchase(productId);
    }

    public string ConvertIAPReceipt(string receipt)
    {
        receipt.Replace("\\", "");
        return receipt;
    }
}