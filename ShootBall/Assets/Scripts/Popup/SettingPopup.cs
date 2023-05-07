using UnityEngine;

public class SettingPopup : PopupStack
{
    [SerializeField]
    UIButton logoutBtn;

    [SerializeField]
    Toggle inGameSoundToggle;

    [SerializeField]
    Toggle BackGroundSoundToggle;

    protected override void Awake()
    {
        base.Awake();


        if (!logoutBtn || !inGameSoundToggle || !BackGroundSoundToggle)
        {
            BackEndLogger.Log("Error", BackEndLogger.LogType.NOMAL, "SettingPopup !logoutBtn || !inGameSoundToggle || !BackGroundSoundToggle");
            return;
        }

        logoutBtn.EnableClick = GlobalData.IsGoogleLogin;
        inGameSoundToggle.On = GlobalData.soundSettings.inGameSound;
        BackGroundSoundToggle.On = GlobalData.soundSettings.backgroundSound;
    }

    public void OnInGameSoundClick()
    {
        GlobalData.soundSettings.inGameSound = !GlobalData.soundSettings.inGameSound;
        inGameSoundToggle.On = GlobalData.soundSettings.inGameSound;
    }

    public void OnBackGroundSoundClick()
    {
        GlobalData.soundSettings.backgroundSound = !GlobalData.soundSettings.backgroundSound;
        BackGroundSoundToggle.On = GlobalData.soundSettings.backgroundSound;
    }

    public void OnLogout()
    {
        if (GlobalData.IsGoogleLogin == false)
        {
            ToastMessageManager.instance.StartToastMessage("게스트 로그인 계정은 로그아웃 할 수 없습니다.", 2f);
            return;
        }

        GoogleLogin.OnLogout();
        LoadSceneManager.instance.LoadScene(StringList.IntroScene);
    }

    public void OnErase()
    {
        gameObject.SetActive(false);
    }
}
