using System;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class NoticePopup : PopupStack
{
    protected UIButton popupNoticeOkBtn;
    protected UIButton popupNoticeCancleBtn;
    protected LocalizeStringEvent localizeStringEvent;

    public bool IsExit
    {
        set
        {
            var image = GetComponent<Image>();
            if (image == null)
                return;

            if (value == true)
            {
                image.color = new Color(0, 0, 0, 1f);
            }
            else
            {
                image.color = new Color(0, 0, 0, 200f / 255f);
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        popupNoticeOkBtn = gameObject?.transform?.GetChild(0)?.GetChild(2)?.GetComponent<UIButton>();
        popupNoticeCancleBtn = gameObject?.transform?.GetChild(0)?.GetChild(1)?.GetComponent<UIButton>();
        localizeStringEvent = gameObject?.transform?.GetChild(0)?.GetChild(0)?.GetComponent<LocalizeStringEvent>();

        if (!popupNoticeOkBtn || !popupNoticeCancleBtn || !localizeStringEvent)
        {
            BackEndLogger.Log("Error", BackEndLogger.LogType.NOMAL, "NoticePopup !popupNoticeOkBtn || !popupNoticeCancleBtn || !localizeStringEvent");
            return;
        }
    }

    public void ResetOkAct()
    {
        if(popupNoticeOkBtn == null)
        {
            popupNoticeOkBtn = gameObject?.transform?.GetChild(0)?.GetChild(2)?.GetComponent<UIButton>();
        }

        popupNoticeOkBtn.act = null;
    }

    public void SetOkAct(Action act)
    {
        if (popupNoticeOkBtn == null)
        {
            popupNoticeOkBtn = gameObject?.transform?.GetChild(0)?.GetChild(2)?.GetComponent<UIButton>();
        }

        popupNoticeOkBtn.act = act;
    }

    public void ResetCancleAct()
    {
        if (popupNoticeCancleBtn == null)
        {
            popupNoticeCancleBtn = gameObject?.transform?.GetChild(0)?.GetChild(1)?.GetComponent<UIButton>();
        }

        popupNoticeCancleBtn.act = null;
    }

    public void SetCancleAct(Action act)
    {
        if (popupNoticeCancleBtn == null)
        {
            popupNoticeCancleBtn = gameObject?.transform?.GetChild(0)?.GetChild(1)?.GetComponent<UIButton>();
        }

        popupNoticeCancleBtn.act = act;
    }

    public void SetLocalizationString(string localization, string tableStr)
    {
        if (localizeStringEvent == null)
        {
            localizeStringEvent = transform.GetChild(0).GetChild(0).GetComponent<LocalizeStringEvent>();
        }

        if (popupNoticeOkBtn == null)
        {
            popupNoticeOkBtn = transform.GetChild(0).GetChild(2).GetComponent<UIButton>();
        }

        localizeStringEvent.StringReference.SetReference(localization, tableStr);
    }

    public virtual void OnErase()
    {
        gameObject.SetActive(false);
    }

    public void OnPrint()
    {
        gameObject.SetActive(true);
    }
}
