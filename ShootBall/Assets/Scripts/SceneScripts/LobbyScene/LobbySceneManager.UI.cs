using UnityEngine;

public partial class LobbySceneManager
{
    #region ���� �������� �˾�
    [SerializeField]
    GameObject gameLevelPopup;
    SlidePopup slidePopup;
    GameStage gameStage;
    #endregion

    #region ���� ��ư
    [SerializeField]
    GameObject startBtn;
    #endregion

    #region ���� �˾�
    [SerializeField]
    GameObject settingBack;
    #endregion

    #region ����� �˾�
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
