public class PausePopup : PopupStack
{  
    public void CanclePausePopup()
    {
        InGameManager.instance?.CanclePausePopup();
    }

    public void LeaveGame()
    {
        InGameManager.instance?.LeaveGame();
    }

    public void RestartGame()
    {
        InGameManager.instance?.ReStartGame();
    }
}
