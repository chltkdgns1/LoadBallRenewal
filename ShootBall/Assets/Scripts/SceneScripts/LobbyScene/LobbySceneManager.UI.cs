using UnityEngine;

public partial class LobbySceneManager
{
    #region 게임 스테이지 팝업
    [SerializeField]
    GameObject gameLevelPopup;
    SlidePopup slidePopup;
    GameStage gameStage;
    #endregion

    #region 시작 버튼
    [SerializeField]
    GameObject startBtn;
    #endregion

    #region 세팅 팝업
    [SerializeField]
    GameObject settingBack;
    #endregion

    #region 스토어 팝업
    [SerializeField]
    StorePopup storePopup;
    #endregion

    public StorePopup StorePopupProperty
    {
        get { return storePopup; }
    }

    public void Init()
    {
        if(gameLevelPopup == null)
        {
            BackEndLogger.Log("Error", BackEndLogger.LogType.ERROR, "LobbySceneManager Init gameLevelPopup == null");
            return;
        }

        gameLevelPopup.SetActive(true);
        slidePopup = gameLevelPopup.GetComponent<SlidePopup>();
        gameStage = gameLevelPopup.GetComponent<GameStage>();

        if (slidePopup == null)
        {
            BackEndLogger.Log("Error", BackEndLogger.LogType.ERROR, "LobbySceneManager Init slidePopup == null");
            return;
        }

        if (gameStage == null)
        {
            BackEndLogger.Log("Error", BackEndLogger.LogType.ERROR, "LobbySceneManager Init gameStage == null");
            return;
        }
    }

    public void ResetScreen()
    {
        SetEraseGameLevelPopup();
        SetResetStartBtn();
    }
}
