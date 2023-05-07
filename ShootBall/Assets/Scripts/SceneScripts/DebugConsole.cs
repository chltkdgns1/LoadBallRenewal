using TMPro;
using UnityEngine;

public class DebugConsole : MonoBehaviour
{
    [SerializeField]
    GameObject debugConsoleOb;

    [SerializeField]
    GameObject debugBtn;

    [SerializeField]
    GameObject alphaBuildTxt;

    GameObject realAlphaBuildTxt;

    [SerializeField]
    GameObject debugTestBtn;

    [SerializeField]
    GameObject debugTestBunddleBack;

    private void Awake()
    {
#if REAL == false
        Application.logMessageReceived += ApplogMessageReceived;
        Application.logMessageReceivedThreaded += ApplogMessageReceivedThread;
#endif
    }

    private void OnDestroy()
    {
#if REAL == false
        Application.logMessageReceived -= ApplogMessageReceived;
        Application.logMessageReceivedThreaded -= ApplogMessageReceivedThread;
#endif
    }

    private void ApplogMessageReceived(string condition, string stackTrace, LogType type)
    {
        DebugScrollPopup.AddDebugMessage(new DebugStr(type, condition, stackTrace));
    }

    private void ApplogMessageReceivedThread(string condition, string stackTrace, LogType type)
    {
        if (type == LogType.Log) return;
        DebugScrollPopup.AddDebugMessage(new DebugStr(type, "In Thread - " + condition, stackTrace));
    }

    private void Start()
    {

#if REAL
        debugBtn.SetActive(false);

        if (debugTestBtn != null)
            debugTestBtn?.SetActive(false);
#else
        debugBtn.SetActive(true);
        CreateAlphaTxt();

        if (debugTestBtn != null)
            debugTestBtn?.SetActive(true);
#endif
    }

    void CreateAlphaTxt()
    {
        GameObject canvasObject = GameObject.Find("Canvas");
        if (canvasObject == null) return;

        realAlphaBuildTxt = Instantiate(alphaBuildTxt, canvasObject.transform);
        realAlphaBuildTxt.transform.SetAsLastSibling();
        realAlphaBuildTxt.SetActive(true);
        string str = realAlphaBuildTxt.GetComponent<TextMeshProUGUI>().text;
        realAlphaBuildTxt.GetComponent<TextMeshProUGUI>().text = str + " VESION " + Application.version;
    }

    public void OnPrint()
    {
        debugConsoleOb?.SetActive(true);
    }

    public void OnErase()
    {
        debugConsoleOb?.SetActive(false);
    }

    public void OnPrintBunddle()
    {
        if (debugTestBunddleBack != null)
            debugTestBunddleBack?.SetActive(true);
    }

    public void OnEraseBunddle()
    {
        Debug.Log("start OnEraseBunddle");
        if (debugTestBunddleBack != null)
            debugTestBunddleBack?.SetActive(false);
    }
}
