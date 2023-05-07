using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetryPopup : PopupStack
{
    public void LeaveGame()
    {
        InGameManager.instance?.LeaveGame();
    }

    public void RestartGame()
    {
        InGameManager.instance?.ReStartGame();
    }
}
