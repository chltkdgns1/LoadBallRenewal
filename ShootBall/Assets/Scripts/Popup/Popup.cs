using System;

public class Popup<T> where T : NoticePopup
{
    static public T ShowPopup(string popupPath, string table, string tableKey)
    {
        var noticePopup = PopupStack.PopupShow<T>(popupPath);
        noticePopup.SetLocalizationString(table, tableKey);
        noticePopup.ResetOkAct();
        noticePopup.ResetCancleAct();
        noticePopup.gameObject.SetActive(true);
        return noticePopup;
    }
}

