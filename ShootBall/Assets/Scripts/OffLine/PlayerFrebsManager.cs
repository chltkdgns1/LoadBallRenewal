using System.Collections.Generic;
using UnityEngine;
using static StageData;


public class PlayerPrefsManager
{
    public static void LoadAllLocalData()
    {
        GlobalData.Reset();
        GlobalData.Coin = long.Parse(PlayerPrefs.GetString("Coin", "0"));
        Read();
        GlobalData.soundSettings.LoadAll();
    }

    public static void SaveAllLocalData()
    {
        PlayerPrefs.SetString("Coin", GlobalData.Coin.ToString());
        Write();
        GlobalData.soundSettings.SaveAll();
    }

    public static void SaveCoin(long coin)
    {
        PlayerPrefs.SetString("Coin", coin.ToString());
        PlayerPrefs.Save();
    }

    public static void SaveStageData(int level, StageData data)
    {
        string dataStr = "";
        if (0 <= data.startCnt && data.startCnt <= 3)
        {
            dataStr = ((int)data.state).ToString() + "," + data.startCnt;
        }
        PlayerPrefs.SetString("Stage_" + (level + 1), dataStr);
        PlayerPrefs.Save();
    }

    static public void Read()
    {
        List<string> list = new List<string>();
        bool nullFlag = false;

        StageGroup stageGroup = new StageGroup();
        for (int i = 0; i < GlobalData.StageSize; i++)
        {
            string stageData = PlayerPrefs.GetString("Stage_" + (i + 1));

            if (nullFlag)
            {
                SetNon(stageGroup.stageDataList[i]);
                continue;
            }

            if (string.IsNullOrEmpty(stageData))
            {
                if (nullFlag == false) nullFlag = true;
                continue;
            }

            split(list, stageData);

            if (list.Count != 2)
            {
                SetNon(stageGroup.stageDataList[i]);
                continue;
            }

            int startCnt = int.Parse(list[1]);
            if (startCnt < 0 || startCnt > 3)
            {
                SetNon(stageGroup.stageDataList[i]);
                continue;
            }

            StageState state = (StageState)(int.Parse(list[0]));
            if (state == StageState.NOT_CLEAR)
            {
                SetNon(stageGroup.stageDataList[i]);
                continue;
            }

            stageGroup.stageDataList[i].startCnt = startCnt;
            stageGroup.stageDataList[i].state = state;
            stageGroup.sumStart += startCnt;
        }
        GlobalData.StageGroupPro = stageGroup.GetStageGroupEncry();
    }

    static void SetNon(StageData data)
    {
        data.startCnt = 0;
        data.state = StageState.NOT_CLEAR;
    }


    static public void Write()
    {
        StageGroup stageGroup = GlobalData.StageGroupPro.GetStageGroup();
        var stageList = stageGroup.stageDataList;

        if (GlobalData.StageSize != stageList.Count)
        {
            Debug.LogError("GlobalData.stageSize != stageDataList.Count : " +
                GlobalData.StageSize + " , " + stageList.Count);
            return;
        }

        for (int i = 0; i < stageList.Count; i++)
        {
            string data = "2,0";
            if (stageList[i].state != StageState.NOT_CLEAR)
            {
                if (0 <= stageList[i].startCnt && stageList[i].startCnt <= 3)
                    data = ((int)stageList[i].state).ToString() + "," + stageList[i].startCnt;
            }
            PlayerPrefs.SetString("Stage_" + (i + 1), data);
        }
        PlayerPrefs.Save();
    }

    static void split(List<string> list, string str, char ch = ',')
    {
        string temp = "";
        list.Clear();
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == ch)
            {
                list.Add(temp);
                temp = "";
                continue;
            }
            temp += str[i];
        }
        if (string.IsNullOrEmpty(temp)) return;
        list.Add(temp);
    }
}

