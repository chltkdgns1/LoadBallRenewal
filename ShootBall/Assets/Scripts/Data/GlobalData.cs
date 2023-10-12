using System;
using System.Collections.Generic;
using UnityEngine;

public class TimeDataEncry
{
    public EncryFloat[] timeArray;
}

public class TimeData
{
    public float[] timeArray;
    const float firstTime = 30f;

    public TimeData()
    {
        timeArray = new float[GlobalData.StageSize];
        timeArray[0] = firstTime;
        for (int i = 1; i < GlobalData.StageSize; i++)
            timeArray[i] = timeArray[i - 1] * 1.025f;
    }

    public TimeDataEncry GetTimeDataEncry()
    {
        TimeDataEncry timeDataEncry = new TimeDataEncry();

        timeDataEncry.timeArray = new EncryFloat[timeArray.Length];

        for(int i = 0; i < timeArray.Length; i++)
        {
            timeDataEncry.timeArray[i] = new EncryFloat(timeArray[i]);
        }
        return timeDataEncry;
    }
}

public enum StageState
{
    CLEAR,
    SELECT,
    NOT_CLEAR
}

public class StageDataEncry
{
    public EncryNumber state;
    public EncryNumber startCnt;

    public StageDataEncry()
    {
        state = new EncryNumber(0);
        startCnt = new EncryNumber(0);
    }

    public StageDataEncry(long state, long startCnt)
    {
        this.state = new EncryNumber(state);
        this.startCnt = new EncryNumber(startCnt);
    }

    public long State
    {
        get { return state.value; }
        set { state.value = value; }
    }

    public long StartCnt
    {
        get { return startCnt.value; }
        set { startCnt.value = value; }
    }
}

[Serializable]
public class StageData
{
    public StageState state;
    public int startCnt; // 별 몇개인지

    public StageData(StageState state, int startCnt)
    {
        this.state = state;
        this.startCnt = startCnt;
    }

    public void SetFromDic(Dictionary<string, object> value)
    {
        state = (StageState)UtilManager.GetIntValueDic(value, StringList.FirebaseState);
        startCnt = UtilManager.GetIntValueDic(value, StringList.FirebaseStartCnt);
    }

    public string GetJsonData()
    {
        return JsonUtility.ToJson(this);
    }
   
    public StageDataEncry GetStageDataEncry()
    {
        return new StageDataEncry((long)state, startCnt);
    }
}

public class StageGroupEncry
{
    public List<StageDataEncry> stageDataList = new List<StageDataEncry>();
    public EncryNumber sumStart = new EncryNumber(0);

    public void Reset()
    {
        sumStart.SetNumber(0);
        stageDataList.Clear();
    }

    public long SumStart
    {
        get { return sumStart.value; }
        set { sumStart.value = value; }
    }

    public StageGroup GetStageGroup()
    {
        StageGroup stageGroup = new StageGroup();

        int cnt = stageDataList.Count;
        for (int i = 0; i < cnt; i++)
        {
            stageGroup.stageDataList[i].startCnt = (int)stageDataList[i].startCnt.value;
            stageGroup.stageDataList[i].state = (StageState)stageDataList[i].state.value;
        }

        stageGroup.sumStart = (int)sumStart.value;
        return stageGroup;
    }
}

[Serializable]
public class StageGroup
{
    public List<StageData> stageDataList = new List<StageData>();
    public int sumStart;

    public StageGroup()
    {
        SetStartData();
    }

    public void SetStartData()
    {
        sumStart = 0;
        stageDataList.Add(new StageData(StageState.SELECT, 0));
        for (int i = 0; i < GlobalData.StageSize - 1; i++)
            stageDataList.Add(new StageData(StageState.NOT_CLEAR, 0)); ;
    }

    public void Reset()
    {
        sumStart = 0;
        stageDataList.Clear();
    }

    public void SetFromDic(Dictionary<string, object> value)
    {
        var temp = value["stageDataList"];

        if (temp == null)
        {
            Debug.LogError("StageGroup null Error");
            return;
        }

        var list = (List<object>)temp;

        for (int i = 0; i < GlobalData.StageSize; i++)
        {
            stageDataList[i].SetFromDic((Dictionary<string, object>)list[i]);
        }
        int sum = 0;
        for (int i = 0; i < GlobalData.StageSize; i++)
        {
            sum += stageDataList[i].startCnt;
        }
        sumStart = sum;
    }

    public void SetStageClear(int level, int cnt, long coin)
    {
        int index = level - 1;
        stageDataList[index].state = StageState.CLEAR;
        int addCnt = cnt - stageDataList[index].startCnt;
        stageDataList[index].startCnt = Mathf.Max(stageDataList[index].startCnt, cnt);
        sumStart += addCnt < 0 ? 0 : addCnt;
        GlobalData.Coin += coin;
        SetStageReady(level + 1);

        if (GlobalData.IsGoogleLogin)
        {
            GoogleFirebaseManager.WriteCoinData(GlobalData.Uid, GlobalData.Coin);
            if (addCnt > 0)
            {
                GoogleFirebaseManager.WriteStageData(GlobalData.Uid, index, JsonUtility.ToJson(stageDataList[index]));
            }
        }
        //else
        //{
        //    PlayerPrefsManager.SaveCoin(GlobalData.Coin);
        //    if (addCnt > 0)
        //    {
        //        PlayerPrefsManager.SaveStageData(index, stageDataList[index]);
        //    }
        //}
    }

    void SetStageReady(int level)
    {
        int index = level - 1;
        if (stageDataList[index].state != StageState.NOT_CLEAR) return;
        if (stageDataList.Count <= index) return;
        stageDataList[index].startCnt = 0;
        stageDataList[index].state = StageState.SELECT;

        if (GlobalData.IsGoogleLogin)
        {
            GoogleFirebaseManager.WriteStageData(GlobalData.Uid, index, JsonUtility.ToJson(stageDataList[index]));
        }
        //else
        //{
        //    PlayerPrefsManager.SaveStageData(index, stageDataList[index]);
        //}
    }

    public StageGroupEncry GetStageGroupEncry()
    {
        StageGroupEncry stageGroupEncry = new StageGroupEncry();
        int cnt = stageDataList.Count;
        for (int i = 0; i < cnt; i++)
        {
            stageGroupEncry.stageDataList.Add(stageDataList[i].GetStageDataEncry());
        }
        stageGroupEncry.sumStart = new EncryNumber(sumStart);
        return stageGroupEncry;
    }
}


[Serializable]
public class GlobalDataConnector
{
    public long coin;
    public StageGroup stageGroup;

    public bool IsDeleteAds;

    public int unLockPage;

    public GlobalDataConnector()
    {
        unLockPage = 0;
        coin = 0;
        IsDeleteAds = false;
        stageGroup = new StageGroup();
    }

    public void SetFromDic(Dictionary<string, object> value)
    {
        coin = UtilManager.GetLongValueDic(value, StringList.FirebaseCoin);
        Debug.Log("코인 : " + coin);
        unLockPage = UtilManager.GetIntValueDic(value, StringList.FirebaseUnLockPage);
        IsDeleteAds = UtilManager.GetBoolValueDic(value, StringList.FirebaseDeleteAds);
        stageGroup.SetFromDic((Dictionary<string, object>)value[StringList.FirebaseStageGroup]);
    }

    public void SetGlobalData()
    {
        int cnt = stageGroup.stageDataList.Count;
        int sum = 0;

        StageGroup tStageGroup = new StageGroup();

        for (int i = 0; i < cnt; i++)
        {
            sum += stageGroup.stageDataList[i].startCnt;
            tStageGroup.stageDataList[i].state = stageGroup.stageDataList[i].state;
            tStageGroup.stageDataList[i].startCnt = stageGroup.stageDataList[i].startCnt;
        }
        tStageGroup.sumStart = sum;

        GlobalData.StageGroupPro = tStageGroup.GetStageGroupEncry();

        GlobalData.Coin = coin;
        GlobalData.IsDeleteAds = IsDeleteAds;
        GlobalData.UnLockPage = unLockPage;

        if (GlobalData.UnLockPage == 0)  // 언락 페이지 정보가 없다면, 1페이지로 고정
            GlobalData.UnLockPage = 2;
    }

    public static string GetGlobalDataConnector()
    {
        return JsonUtility.ToJson(new GlobalDataConnector());
    }
}

public class GlobalData
{
    private static EncryNumber stageSize = new EncryNumber(90);

    static TimeDataEncry timeDataEncry = null;
    static StageGroupEncry _stageGroupEncry = null;

    static EncryNumber _coin = new EncryNumber(0);
    static EncryNumber _cash = new EncryNumber(0);

    static EncryBool isDeleteAds = new EncryBool(false);
    static EncryNumber unLockPage = new EncryNumber(2);

    static EncryString appVerison = new EncryString("1.0.0");

    static private EncryBool isConnectNetWork = new EncryBool(false);

    static private EncryBool isGoogleLogin = new EncryBool(false);

    static private EncryString uid = new EncryString("000000000");

    static private EncryBool isOpenRankingChallenge = new EncryBool(false);

    public class Sound
    {
        public bool inGameSound;
        public bool backgroundSound;

        public Sound()
        {
            LoadAll();
        }

        public void LoadAll()
        {
            inGameSound = PlayerPrefs.GetInt("InGameSound", 1) == 1 ? true : false;
            backgroundSound = PlayerPrefs.GetInt("BackgroundSound", 1) == 1 ? true : false;
        }

        public void SaveInGameSound()
        {
            PlayerPrefs.SetInt("InGameSound", inGameSound == true ? 1 : 0);
        }

        public void SaveInBackgroundSound()
        {
            PlayerPrefs.SetInt("BackgroundSound", backgroundSound == true ? 1 : 0);
        }

        public void SaveAll()
        {
            SaveInGameSound();
            SaveInBackgroundSound();
        }
    }

    static public Sound soundSettings = null;

    static GlobalData()
    {
        appVerison.value = Application.version;
        soundSettings = new Sound();

        TimeData timeData = new TimeData();
        timeDataEncry = timeData.GetTimeDataEncry();
    }

    static public int StageSize
    {
        get { return (int)stageSize.value; }
    }

    static public string AppVersion
    {
        get { return appVerison.value; }
    }

    static public bool IsConnectNetWork
    {
        get { return isConnectNetWork.value; }
        set { isConnectNetWork.value = value; }
    }

    static public bool IsOpenRankingChallenge
    {
        get { return isOpenRankingChallenge.value; }
        set { isOpenRankingChallenge.value = value; }
    }

    static public int PageStageSize
    {
        get { return 18; }
    }

    static public bool IsDeleteAds
    {
        get { return isDeleteAds.value; }
        set { isDeleteAds.value = value; }
    }

    static public bool IsGoogleLogin
    {
        get { return isGoogleLogin.value; }
        set { isGoogleLogin.value = value; }
    }

    static public string Uid
    {
        get { return uid.value; }
        set { uid.value = value; }
    }

    // 1부터 시작
    static public int UnLockPage
    {
        get { return (int)unLockPage.value; }
        set { unLockPage.value = value; }
    }

    public static string StringCoin
    {
        get
        {
            return UtilManager.GetStrCoin(_coin.value);
        }
    }

    public static long Coin
    {
        get { return _coin.value; }
        set { _coin.value = value; }
    }

    public static long Cash
    {
        get { return _cash.value; }
        set { _cash.value = value; }
    }

    public static TimeDataEncry TimeDataPro
    {
        get { return timeDataEncry; }
    }

    public static StageGroupEncry StageGroupPro
    {
        get { return _stageGroupEncry; }
        set { _stageGroupEncry = value; }
    }

    static public void OnStatic()
    {
        new GlobalData();
    }

    static public void Reset()
    {
        _stageGroupEncry.Reset();
        StageGroup stageGroup = new StageGroup();
        _stageGroupEncry = stageGroup.GetStageGroupEncry();

        IsDeleteAds = false;
        Coin = 0;
        Cash = 0;
        UnLockPage = 2;
    }

    static public bool IsCanBuyProduct()
    {
        return GlobalData.IsConnectNetWork && GlobalData.IsGoogleLogin;
    }
}

