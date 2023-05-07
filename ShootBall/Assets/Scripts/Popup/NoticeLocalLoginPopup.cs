public class NoticeLocalLoginPopup : NoticePopup
{
    public override void OnErase()
    {
        Invoke("OnQuit", 0.5f);
    }

    public void OnOk()
    {
        UtilManager.Quit();
        // LoadSceneManager.instance.LoadScene(StringList.LobbyScene);
    }

    public void OnQuit()
    {
        UtilManager.Quit();
    }
}

