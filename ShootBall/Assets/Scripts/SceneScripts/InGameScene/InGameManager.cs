using UnityEngine;
using UnityEngine.UI;

public enum Direction
{
    RIGHT = 0,
    TOP,
    LEFT,
    BOTTOM
}

public partial class InGameManager : MonoBehaviour
{
    public static InGameManager instance;

    [SerializeField]
    GameObject gameLevelGroup;

    [SerializeField]
    InGameTimer inGameTimer;

    [SerializeField]
    LoadingComplete loadingComplete;

    [SerializeField]
    GameObject[] prefabsStageArr;

    [SerializeField]
    Text stageNumberTxt;

    GameObject nowStage;
    GameObject player;
    PlayerState playerState;

    public Vector3 PlayerPosition { get; set; } = new Vector3(0, 0, 0);

    private void Awake()
    {
        if (instance == null) instance = this;
        else enabled = false;

        PlayingGameManager.gameState = GameState.IN_GAME_PLAY;
        PlayingGameManager.sceneState = SceneState.INGAME;
    }

    public void StartGame()
    {
        inGameTimer.StartInGameTimer();
    }

    public void NextStage()
    {
        PlayingGameManager.gameState = GameState.LOADING;

        PlayingGameManager.gameLevel++;

        SetEraseNowStage();
        FindNowStage();
        FindPlayer();
        SetPrintNowLevel();

        WaitLoadingManager.instance.StartWaitLoading(0.8f, () =>
        {
            AllEraseBack();
            PlayingGameManager.gameState = GameState.IN_GAME_PLAY;
            StartGame();
        });

        SetStageNumber();
    }

    void SetStageNumber()
    {
        stageNumberTxt.text = PlayingGameManager.gameLevel + " " + LanguageString.GetStageString();
    }

    public void FindPlayer()
    {
        if (PlayingGameManager.gameLevel == null)
        {
            Debug.LogError("gameLevel error");
            return;
        }

        int index = PlayingGameManager.gameLevel.Value - 1;
        player = nowStage.transform.Find("BallPlayer").gameObject;
        PlayerPosition = player.transform.position;
        InitPlayer();
    }

    void SetPrintNowLevel()
    {
        if (nowStage == null) return;
        nowStage.SetActive(true);
    }

    void SetEraseNowStage()
    {
        nowStage.SetActive(false);

        nowStage = null;
        player = null;
        playerState = null;
    }

    void FindNowStage()
    {
        int index = PlayingGameManager.gameLevel.Value - 1;
        nowStage = Instantiate(prefabsStageArr[index], gameLevelGroup.transform);
    }

    void Start()
    {
        FindNowStage();
        FindPlayer();
        SetPrintNowLevel();
        InitUI();
        StartGame();
        SetStageNumber();
    }

    void InitPlayer()
    {
        playerState = player.GetComponent<PlayerState>();
    }

    public void MovePlayer(Direction dir)
    {
        playerState.MovePlayer(dir);
    }

    public void GameClear()
    {
        inGameTimer.FinishTimer();
        SetPrintLoadingComplete(inGameTimer.RemainTime, EndGameState.CLEAR, (EndResultStar)inGameTimer.GetStar());

        StageGroup stageGroup = GlobalData.StageGroupPro.GetStageGroup();
        stageGroup.SetStageClear(PlayingGameManager.gameLevel.Value, inGameTimer.GetStar(), GetCoin(inGameTimer.RemainTime));
        GlobalData.StageGroupPro = stageGroup.GetStageGroupEncry();

        GoogleAds.AdsShow();
        //popupComplete.SetCompleteState(inGameTimer.RemainTime, GetCoin(inGameTimer.RemainTime), inGameTimer.GetStar());
    }


    int GetCoin(float remainTime)
    {
        return (int)(remainTime * PlayingGameManager.gameLevel * 3.33f);
    }

    public void GameOver()
    {
        inGameTimer.FinishTimer();
        SetPrintLoadingComplete(0, EndGameState.FAILED, (EndResultStar)inGameTimer.GetStar());
        GoogleAds.AdsShow();
    }

    public void CanclePausePopup()
    {
        SetEraseGamePausePopup();
        inGameTimer.Restart();
    }

    public void ReStartGame()
    {
        AllEraseBack();
        player.SetActive(true);
        playerState.ResetState();
        StartGame();
    }

    public void LeaveGame()
    {
        Debug.Log(" public void LeaveGame()");
        PlayingGameManager.gameState = GameState.LOADING;

        LoadSceneManager.instance.LoadScene(StringList.LobbyScene);
        //WaitLoadingManager.instance.StartWaitLoading(2f, () =>
        //{
        //     SceneManager.LoadScene(StringList.LobbyScene);
        //});
    }

    public bool IsPossibleNextStage()
    {
        return GlobalData.StageSize >= PlayingGameManager.gameLevel.Value;
    }

#if UNITY_EDITOR
    public void TestBackButton()
    {
        AppInput.instance.SetBack();
    }
#endif

}
