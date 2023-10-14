using UnityEngine;
using UnityEngine.UI;

public partial class LobbySceneManager
{
    public void SetPrintGameLevelPopup()
    {
        if (gameStage == null)
        {
            BackEndLogger.Log("Error", BackEndLogger.LogType.ERROR, "SetPrintGameLevelPopup gameStage == null");
            return;
        }

        if (slidePopup == null)
        {
            BackEndLogger.Log("Error", BackEndLogger.LogType.ERROR, "SetPrintGameLevelPopup slidePopup == null");
            return;
        }

        gameStage.gameObject.SetActive(true);
        gameStage.SetStage();
        gameStage.SetStageTxt();
        gameStage.EnableCenterOb();
        slidePopup.MoveInSide(gameStage.EnableOb);
    }

    public void SetEraseGameLevelPopup()
    {
        if (slidePopup == null)
        {
            BackEndLogger.Log("Error", BackEndLogger.LogType.ERROR, "SetEraseGameLevelPopup slidePopup == null");
            return;
        }

        slidePopup.MoveOutSide();
        gameStage.DisableOb();
    }

    public void SetResetStartBtn()
    {
        if (startBtn == null)
        {
            BackEndLogger.Log("Error", BackEndLogger.LogType.ERROR, "SetResetStartBtn startBtn == null");
            return;
        }

        if (startBtn.gameObject == null)
        {
            BackEndLogger.Log("Error", BackEndLogger.LogType.ERROR, "SetResetStartBtn startBtn.gameObject == null");
            return;
        }

        startBtn.gameObject.transform.localScale = new Vector3(1, 1);
        startBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }

    public void SetPrintStartBtn()
    {
        if (startBtn == null)
        {
            BackEndLogger.Log("Error", BackEndLogger.LogType.ERROR, "SetPrintStartBtn startBtn == null");
            return;
        }

        startBtn.SetActive(true);
    }

    public void SetEraseStartBtn()
    {
        if (startBtn == null)
        {
            BackEndLogger.Log("Error", BackEndLogger.LogType.ERROR, "SetEraseStartBtn startBtn == null");
            return;
        }

        startBtn.SetActive(false);
    }

    public void SetPrintSettingBack()
    {
        if (settingBack == null)
        {
            BackEndLogger.Log("Error", BackEndLogger.LogType.ERROR, "SetPrintSettingBack startBtn == null");
            return;
        }

        settingBack.SetActive(true);
    }

    public void SetEraseSettingBack()
    {
        if (settingBack == null)
        {
            BackEndLogger.Log("Error", BackEndLogger.LogType.ERROR, "SetEraseSettingBack settingBack == null");
            return;
        }

        settingBack.SetActive(false);
    }

    public void SetPrintStorePopup()
    {
        if (storePopup == null)
        {
            BackEndLogger.Log("Error", BackEndLogger.LogType.ERROR, "SetPrintStorePopup storePopup == null");
            return;
        }

        storePopup.SetData(GlobalData.productItemDataList);
        storePopup.gameObject.SetActive(true);
    }

    public void SetEraseStorePopup()
    {
        if (storePopup == null)
        {
            BackEndLogger.Log("Error", BackEndLogger.LogType.ERROR, "SetEraseStorePopup storePopup == null");
            return;
        }

        if (storePopup.gameObject == null)
        {
            BackEndLogger.Log("Error", BackEndLogger.LogType.ERROR, "SetEraseStorePopup storePopup.gameObject == null");
            return;
        }

        storePopup.gameObject.SetActive(false);
    }

    public void SetPrintMatchPopupReady()
    {
        if (GlobalData.IsOpenRankingChallenge)
        {
            Popup<NoticePopup>.ShowPopup(PopupPath.PopupRanking, StringList.LanguageTable, StringList.MatchingPopupReadyEnd);
        }
        else
        {
            Popup<NoticePopup>.ShowPopup(PopupPath.PopupNotice, StringList.LanguageTable, StringList.MatchingPopupReady);
        }
    }
}
