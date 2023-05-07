using UnityEngine;

public partial class InGameManager
{
    [SerializeField]
    RetryPopup retryPopup;

    [SerializeField]
    PausePopup pausePopup;

    [SerializeField]
    CompletePopup completePopup;

    void InitUI()
    {
        AllEraseBack();
    }

    public void SetPrintGameOver()
    {
        SetWaitState();
        retryPopup.gameObject.SetActive(true);
    }

    public void SetEraseGameOver()
    {
        retryPopup.gameObject.SetActive(false);
        SetGameState();
    }

    public void SetPrintGameClear(float remainTime, EndResultStar startCnt)
    {
        if (IsPossibleNextStage()) completePopup.IsUseNextBtn = true;
        else completePopup.IsUseNextBtn = false;

        SetWaitState();
        completePopup.gameObject.SetActive(true);
        completePopup.SetCompleteState(remainTime, GetCoin(remainTime), (int)startCnt);
    }

    public void SetEraseGameClear()
    {
        completePopup.gameObject.SetActive(false);
        SetGameState();
    }

    public void SetPrintGamePausePopup()
    {
        inGameTimer.Stop();
        SetWaitState();
        pausePopup.gameObject.SetActive(true);
    }

    public void SetEraseGamePausePopup()
    {
        pausePopup.gameObject.SetActive(false);
        SetGameState();
    }

    public void SetPrintLoadingComplete(float remainTime, EndGameState endGameState, EndResultStar endResultStar)
    {
        loadingComplete.endGameState = endGameState;
        loadingComplete.endResultStarState = endResultStar;
        loadingComplete.duration = 2f;
        loadingComplete.act = () =>
        {
            if (endGameState == EndGameState.FAILED) SetPrintGameOver();
            else SetPrintGameClear(remainTime, endResultStar);
        };
        loadingComplete.StartLoading();
    }

    public void SetEraseLoadingComplete()
    {
        loadingComplete.gameObject.SetActive(false);
    }

    public void AllEraseBack()
    {
        SetEraseGameClear();
        SetEraseGameOver();
        SetEraseGamePausePopup();
        SetEraseLoadingComplete();
    }

    void SetWaitState()
    {
        PlayingGameManager.gameState = GameState.IN_GAME_WAIT;
    }

    void SetGameState()
    {
        bool flag = PopupStack.IsEmpty();
        if (flag) PlayingGameManager.gameState = GameState.IN_GAME_PLAY;
    }
}

