using System;
using UnityEngine;
using UnityEngine.UI;

public class DebugItem : MonoBehaviour
{
    public DebugStr debugStr;
    public Text txt;

    Image ItemImg;

    public Color normalColor;
    public Color clickColor;

    public Action<DebugItem> act;

    private void Awake()
    {
        ItemImg = GetComponent<Image>();
        ItemImg.color = normalColor;
    }

    private void Start()
    {
        string colorStr = "white";
        if (debugStr.logType == LogType.Error) colorStr = "red";
        else if (debugStr.logType == LogType.Warning) colorStr = "yellow";
        txt.text = "<color=" + colorStr + "> " + debugStr.logType + "\nLog : " + debugStr.message + "\n</color>";
    }

    private void OnEnable()
    {
        SetNormalColor();
    }

    public void OnClick()
    {
        ItemImg.color = clickColor;
        act?.Invoke(this);
    }

    public void SetNormalColor()
    {
        ItemImg.color = normalColor;
    }
}

