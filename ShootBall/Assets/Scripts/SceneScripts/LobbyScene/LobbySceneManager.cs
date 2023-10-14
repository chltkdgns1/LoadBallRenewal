using DG.Tweening;
using System.Collections.Generic;
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

    public void RefreshStorePopupMenu(List<List<ProductItemData>> productItemDataList)
    {
        if (storePopup == null || storePopup.gameObject || storePopup.gameObject.activeSelf == false)
        {
            return;
        }

        storePopup.Refresh(productItemDataList);
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
