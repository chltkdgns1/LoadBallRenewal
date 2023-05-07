using System;
using UnityEngine;
using UnityEngine.UI;

public class DebugLogCntTxt : MonoBehaviour
{
    public Image img;
    public Text txtCnt;
    public Color normalColor;
    public Color clickedColor;

    public Action act;

    public bool IsClicked { get; set; }

    public void OnEnable()
    {
        IsClicked = false;
        img.color = normalColor;
    }

    public void OnClick()
    {
        IsClicked = !IsClicked;
        if (IsClicked == true) img.color = clickedColor;
        else img.color = normalColor;
        act?.Invoke();
    }
}

