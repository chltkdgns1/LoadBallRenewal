using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugStr
{
    public LogType logType;
    public string message;
    public string stackTrace;

    public DebugStr(LogType logType, string message, string stackTrace)
    {
        this.logType = logType;
        this.message = message;
        this.stackTrace = stackTrace;
    }
}

public class DebugScrollPopup : PopupStack
{
    [SerializeField]
    GameObject debugTxtPrefab;

    [SerializeField]
    Transform scrollContent;

    [SerializeField]
    Text DetailTxt;

    [SerializeField]
    DebugLogCntTxt[] debugLogCntTxt;

    int printSize;

    DebugItem clickDebugItem;

    static public List<DebugStr> DebugStrList { get; set; } = new List<DebugStr>();
    static public int[] logCnt = new int[3];

    List<GameObject> objPool = new List<GameObject>();

    protected override void Awake()
    {
        base.Awake();

        DetailTxt?.transform?.parent?.parent?.gameObject?.SetActive(false);
        printSize = 0;
        for (int i = 0; i < debugLogCntTxt?.Length; i++)
        {
            debugLogCntTxt[i].act = PrintDebugLog;
            debugLogCntTxt[i].IsClicked = false;
        }
    }

    static public void AddDebugMessage(DebugStr value)
    {
        if (value.logType == LogType.Log) logCnt[0]++;
        else if (value.logType == LogType.Warning) logCnt[1]++;
        else if (value.logType == LogType.Error) logCnt[2]++;

        DebugStrList.Add(value);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        PrintDebugLog();
    }

    void PrintDebugLog()
    {
        if(debugLogCntTxt.Length != logCnt.Length)
        {
            Debug.Log("PrintDebugLog debugLogCntTxt.Length != logCnt.Length");
            return;
        }

        for (int i = 0; i < logCnt.Length; i++)
        {
            debugLogCntTxt[i].txtCnt.text = logCnt[i].ToString();
        }

        clickDebugItem = null;
        foreach (var obj in objPool)
            obj.SetActive(false);

        int sz = DebugStrList.Count;
        for (int i = 0; i < sz; i++)
        {
            GameObject temp = null;

            int index = 0;
            if (DebugStrList[i].logType == LogType.Log) index = 0;
            else if (DebugStrList[i].logType == LogType.Warning) index = 1;
            else if (DebugStrList[i].logType == LogType.Error) index = 2;

            if (debugLogCntTxt[index].IsClicked == true) continue;

            if (objPool.Count > i)
            {
                temp = objPool[i];
                temp.SetActive(true);
            }
            else
            {
                temp = Instantiate(debugTxtPrefab, scrollContent);
                objPool.Add(temp);
            }

            temp.GetComponent<DebugItem>().debugStr = DebugStrList[i];
            temp.GetComponent<DebugItem>().act = (debugItem) =>
            {
                DetailTxt.transform.parent.parent.gameObject.SetActive(true);
                DetailTxt.text = debugItem.txt.text + "\n" + debugItem.debugStr.stackTrace;
                clickDebugItem?.SetNormalColor();
                clickDebugItem = debugItem;
            };
        }
    }

    public void OnErase()
    {
        gameObject.SetActive(false);
    }
}

