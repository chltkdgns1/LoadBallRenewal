using UnityEngine;

public class NetWorkManager : MonoBehaviour
{
    public static NetWorkManager instance;

    [SerializeField]
    GameObject loadingPrefabs;

    float disconnectTime;
    float disconnectLimitTime = 3f;

    GameObject loadingReal;

    bool disconnectMessage;

    string[] forbiddenList = { };

    bool IsDiconnect = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else enabled = false;

        ResetState();
    }

    void Update()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            disconnectTime += Time.deltaTime;
            PrintLinkLessMesssage();

            if (disconnectLimitTime <= disconnectTime)
            {
                Disconnect();
                return;
            }

            GlobalData.IsConnectNetWork = false;
        }
        else
        {
            ResetState();
            GlobalData.IsConnectNetWork = true;
        }
    }

    public void Disconnect()
    {
        if (IsDiconnect == true)
            return;

        IsDiconnect = true;
        GlobalData.IsGoogleLogin = false;
        Popup<NoticePopup>.ShowPopup(PopupPath.PopupNotice, StringList.LanguageTable, StringList.OffLineNotice);
        Invoke("Quit", 3f);
    }

    void Quit()
    {
        UtilManager.Quit();
    }

    void ErasePopupForbiddenNetwork()
    {
        for (int i = 0; i < forbiddenList.Length; i++)
        {
            PopupStack.Remove(forbiddenList[i]);
        }
    }

    void ResetState()
    {
        EraseLoadBak();
        disconnectMessage = false;
        disconnectTime = 0f;
    }


    void PrintLinkLessMesssage()
    {
        if (disconnectMessage == false)
        {
            CreateLoadBack();
            disconnectMessage = true;

            Popup<NoticePopup>.ShowPopup(PopupPath.PopupNotice, StringList.LanguageTable, StringList.NetworkUnstable);
            //ToastMessageManager.instance.StartToastMessage("인터넷 연결이 불안정합니다.", disconnectLimitTime);
        }
    }

    void CreateLoadBack()
    {
        if (loadingReal != null)
        {
            loadingReal.SetActive(true);
            return;
        }

        GameObject canvasObject = GameObject.Find("Canvas");
        if (canvasObject == null) return;

        loadingReal = Instantiate(loadingPrefabs, canvasObject.transform);
        loadingReal.SetActive(true);
    }

    void EraseLoadBak()
    {
        loadingReal?.SetActive(false);
    }
}

