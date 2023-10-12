
using UnityEngine;

public class AppInput : MonoBehaviour
{
    static public AppInput instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else enabled = false;
    }

    private void Start()
    {

    }

    void Update()
    {
        CheckKeyDown();
    }

    void CheckKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetBack();
        }
    }

    public void SetBack()
    {
        bool flag = PopupStack.RemoveBack();
        if (flag == false)
        {
            if (PlayingGameManager.IsInGame())
            {
                // 인게임일 경우 팝업창을 출력한다.
                InGameManager.instance?.SetPrintGamePausePopup();
            }
            else
            {
                var popup = Popup<NoticePopup>.ShowPopup(PopupPath.PopupNotice, StringList.LanguageTable, StringList.ExitGame);
                popup.SetOkAct(() =>
                {
                    UtilManager.Quit();
                });
            }
        }
    }
}

