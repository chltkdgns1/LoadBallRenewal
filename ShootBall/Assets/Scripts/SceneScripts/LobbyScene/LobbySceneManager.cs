using DG.Tweening;
using UnityEngine;

public partial class LobbySceneManager : MonoBehaviour
{
    public static LobbySceneManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else enabled = false;

        PlayingGameManager.gameState = GameState.LOBBY;
        PlayingGameManager.sceneState = SceneState.LOBBY;
    }

    private void Start()
    {
        Init();
    }

    public void OnClickStartBtn()
    {
        SetPrintGameLevelPopup();
        //SetResetStartBtn();
    }

    public void OnClickSetting()
    {
        SetPrintSettingBack();
    }

    public void RefreshStorePopupMenu()
    {
        if(storePopup == null)
        {
            BackEndLogger.Log("Error", BackEndLogger.LogType.ERROR, "RefreshStorePopupMenu storePopup == null");
            return;
        }

        if (storePopup.gameObject == null)
        {
            BackEndLogger.Log("Error", BackEndLogger.LogType.ERROR, "RefreshStorePopupMenu storePopup.gameObject == null");
            return;
        }

        if (storePopup.gameObject.activeSelf == false)
        {
            return;
        }

        storePopup.RefreshMenuProducts();
    }

#if UNITY_EDITOR
    public void TestBack()
    {
        AppInput.instance.SetBack();
    }

    public void TestNet()
    {
        NetWorkManager.instance.Disconnect();
    }
#endif
}
