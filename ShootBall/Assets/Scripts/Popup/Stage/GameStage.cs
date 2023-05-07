using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static StageData;

public class GameStage : MonoBehaviour
{
    [SerializeField]
    GameObject[] _levelArr;

    [SerializeField]
    Sprite[] startSprite;

    [SerializeField]
    Text startTxt;

    const int _inGroupCnt = 18;
    const int _lineCnt = 6;

    [SerializeField]
    ScrollGroupSlice scrollGroup;
    GameStageElements gameStageElements;

    [SerializeField]
    RadioGroup radioGroup;

    struct LevelStruct
    {
        public GameObject lockOb;
        public TextMeshProUGUI txt;
        public Image startImg;
        public GameObject rightLineOb;
        public GameObject rightLineParOb;
        public GameObject bottomLineParLongOb;
        public GameObject bottomLineLongOb;
        public GameObject leftLineParLongOb;
        public GameObject leftLineLongOb;

        public void AllActive()
        {
            lockOb.SetActive(true);
            txt.gameObject.SetActive(true);
            startImg.gameObject.SetActive(true);
            rightLineOb.SetActive(true);
            rightLineParOb.SetActive(true);
            bottomLineParLongOb.SetActive(true);
            bottomLineLongOb.SetActive(true);

            if (leftLineParLongOb != null)
            {
                leftLineParLongOb.SetActive(true);
                leftLineLongOb.SetActive(true);
            }
        }
    }

    List<LevelStruct> levelList = new List<LevelStruct>();

    private void Awake()
    {
        Init();
        InitLevelList();
        AllActiveLevelList();
        SetStage();
        InitButton();
    }

    void Init()
    {
        gameStageElements = scrollGroup.GetComponent<GameStageElements>();
        scrollGroup.DisableOb();

        gameStageElements.SetActiveLock(false);
    }

    private void Start()
    {

    }

    void InitButton()
    {
        for (int i = 0; i < _levelArr.Length; i++)
        {
            _levelArr[i].AddComponent<GameStageBtn>();
            GameStageBtn tempStageBtn = _levelArr[i].GetComponent<GameStageBtn>();
            tempStageBtn.size = 0.8f;
            tempStageBtn.index = i;
            tempStageBtn.duration = 0.5f;
            tempStageBtn.actInt = (int index) =>
            {
                if (GlobalData.StageGroupPro.stageDataList[index].state.value == (long)StageState.NOT_CLEAR)
                    return;

                int pageStageEnd = GlobalData.UnLockPage * GlobalData.PageStageSize;

                int clickStageNum = index + 1;
                if (pageStageEnd < clickStageNum)
                    return;

                PlayingGameManager.gameLevel = index + 1;
                LoadSceneManager.instance.LoadScene(StringList.InGameScene);

                //WaitLoadingManager.instance.StartWaitLoading(2f, () =>
                //{
                //    PlayingGameManager.gameLevel = index + 1;
                //    SceneManager.LoadScene(StringList.InGameScene);
                //});
            };
        }
    }

    void InitLevelList()
    {
        levelList.Clear();

        for (int i = 0; i < _levelArr.Length; i++)
        {
            LevelStruct tempStrcut = new LevelStruct();
            tempStrcut.lockOb = _levelArr[i].transform.GetChild(0).gameObject;
            tempStrcut.bottomLineParLongOb = _levelArr[i].transform.GetChild(1).gameObject;
            tempStrcut.bottomLineLongOb = _levelArr[i].transform.GetChild(1).GetChild(0).gameObject;
            tempStrcut.txt = _levelArr[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            tempStrcut.startImg = _levelArr[i].transform.GetChild(2).GetChild(0).GetComponent<Image>();
            tempStrcut.rightLineParOb = _levelArr[i].transform.GetChild(3).gameObject;
            tempStrcut.rightLineOb = _levelArr[i].transform.GetChild(3).GetChild(0).gameObject;

            if (_levelArr[i].transform.childCount == 5)
            {
                tempStrcut.leftLineParLongOb = _levelArr[i].transform.GetChild(4)?.gameObject;
                tempStrcut.leftLineLongOb = _levelArr[i].transform.GetChild(4)?.GetChild(0)?.gameObject;
            }

            levelList.Add(tempStrcut);
        }
    }

    void AllActiveLevelList()
    {
        for (int i = 0; i < levelList.Count; i++)
        {
            levelList[i].AllActive();
        }
    }

    public void SetStageTxt()
    {
        startTxt.text = "<size=50>" + GlobalData.StageGroupPro.sumStart.value.ToString() + "</size><color=#FFFFFF44><size=35> / 270</size></color>";
    }

    public void SetStage()
    {
        StageGroup tempList = GlobalData.StageGroupPro.GetStageGroup();

        if (_levelArr.Length != tempList.stageDataList.Count)
        {
            Debug.LogError("_levelArr.Length != tempList.stageDataList.Count Not Same Error");
            return;
        }

        AllActiveLevelList();

        int groupCnt = _levelArr.Length / _inGroupCnt;

        int topCnt = 1;
        for (int i = 0; i < groupCnt; i++)
        {
            if (i % 2 == 0) TopToBottomStage(tempList, i * _inGroupCnt, topCnt);
            else BottomToTopStage(tempList, i * _inGroupCnt, topCnt);
            topCnt += 3;
        }
    }

    void TopToBottomStage(StageGroup tempList, int startIndex, int top)
    {
        if (startIndex == 0)
            levelList[startIndex].leftLineParLongOb.SetActive(false);

        int endIndex = startIndex + _inGroupCnt;

        for (int i = startIndex; i < endIndex; i++)
        {
            levelList[i].bottomLineParLongOb.SetActive(false);

            int index = i + 1;
            if (index % _lineCnt == 0 && index / _lineCnt == top)
            {
                levelList[i].rightLineParOb.SetActive(false);
                levelList[i + 1].rightLineParOb.SetActive(false);
            }

            if (index % _lineCnt == 0)
                levelList[i].bottomLineParLongOb.SetActive(true);

            if (index == endIndex)
                levelList[i].bottomLineParLongOb.SetActive(false);

            if (tempList.stageDataList[i].state == StageState.CLEAR)  // 클리어했다면
            {
                levelList[i].txt.text = (i + 1).ToString();
                levelList[i].txt.color = new Color(1f, 1f, 1f, 1f);
                levelList[i].lockOb.SetActive(false);
                levelList[i].startImg.sprite = startSprite[tempList.stageDataList[i].startCnt];
            }
            else if (tempList.stageDataList[i].state == StageState.SELECT)
            {
                levelList[i].txt.text = (i + 1).ToString();
                levelList[i].txt.color = new Color(1f, 1f, 1f, 0.8f);
                levelList[i].lockOb.SetActive(false);
                levelList[i].startImg.sprite = startSprite[0];
                levelList[i].rightLineOb.SetActive(false);

                if (levelList[i].leftLineParLongOb)
                    levelList[i].leftLineLongOb.SetActive(false);
            }
            else
            {
                levelList[i].txt.gameObject.SetActive(false);
                levelList[i].startImg.gameObject.SetActive(false);
                levelList[i].rightLineOb.SetActive(false);
                levelList[i].bottomLineLongOb.SetActive(false);

                if (levelList[i].leftLineParLongOb)
                    levelList[i].leftLineLongOb.SetActive(false);
            }
        }
    }

    void BottomToTopStage(StageGroup tempList, int startIndex, int top)
    {
        int endIndex = startIndex + _inGroupCnt;

        for (int i = startIndex; i < endIndex; i++)
        {
            levelList[i].bottomLineParLongOb.SetActive(false);

            int index = i;
            if (index % _lineCnt == 0 && index / _lineCnt == top)
            {
                levelList[i].rightLineParOb.SetActive(false);
                levelList[i - 1].rightLineParOb.SetActive(false);
            }

            if (index % _lineCnt == 0)
                levelList[i].bottomLineParLongOb.SetActive(true);

            if (index == endIndex || index == startIndex)
                levelList[i].bottomLineParLongOb.SetActive(false);

            if (tempList.stageDataList[i].state == StageState.CLEAR)  // 클리어했다면
            {
                levelList[i].txt.text = (i + 1).ToString();
                levelList[i].txt.color = new Color(1f, 1f, 1f, 1f);
                levelList[i].lockOb.SetActive(false);
                levelList[i].startImg.sprite = startSprite[tempList.stageDataList[i].startCnt];
            }
            else if (tempList.stageDataList[i].state == StageState.SELECT)
            {
                levelList[i].txt.text = (i + 1).ToString();
                levelList[i].txt.color = new Color(1f, 1f, 1f, 0.8f);
                levelList[i].lockOb.SetActive(false);
                levelList[i].startImg.sprite = startSprite[0];
                levelList[i].rightLineOb.SetActive(false);

                if (levelList[i].leftLineParLongOb)
                    levelList[i].leftLineLongOb.SetActive(false);
            }
            else
            {
                levelList[i].txt.gameObject.SetActive(false);
                levelList[i].startImg.gameObject.SetActive(false);
                levelList[i].rightLineOb.SetActive(false);
                levelList[i].bottomLineLongOb.SetActive(false);

                if (levelList[i].leftLineParLongOb)
                    levelList[i].leftLineLongOb.SetActive(false);
            }
        }
    }

    public void OnClickLevelBack()
    {
        LobbySceneManager.instance?.ResetScreen();
    }

    public void OnClickNextLevel()
    {
        radioGroup.NextMove();
        scrollGroup.MoveDir(true, Direct.LEFT).OnComplete(() =>
        {
            gameStageElements.SetLock(scrollGroup.PageIndex);
        });
    }

    public void OnClickLastLevel()
    {
        radioGroup.BackMove();
        scrollGroup.MoveDir(false, Direct.RIGHT);
        gameStageElements.SetLock(scrollGroup.PageIndex);
    }

    public void EnableCenterOb()
    {
        scrollGroup.EnableCenterOb();
    }

    public void EnableOb()
    {
        scrollGroup.EnableOb();
    }

    public void DisableOb()
    {
        scrollGroup.DisableOb();
    }
}

