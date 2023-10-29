using System;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    public static ApplicationManager instance;

    public bool UseTestGoogleLogin;

    public enum VersionCheck
    {
        NONE,
        AGO,
        SAME,
        LASTEST
    }

    private void OnApplicationQuit()
    {
        GlobalData.soundSettings.SaveAll();
    }

    public void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            enabled = false;
            return;
        }

//#if UNITY_EDITOR
//        DefineSymbolManager.Clear();
//        DefineSymbolManager.AddSymbol("DEV");
//        DefineSymbolManager.SetSymbolSetting();
//#endif

        ConvertLanguage.SetLanguage();

        GoogleFirebaseManager.OnStatic();
        GoogleAds.OnStatic();
        GoogleLogin.OnStatic();
        PlayingGameManager.OnStatic();
        GlobalData.OnStatic();
        StringList.OnStatic();

        ReadApplicationVersion();
    }

    void ReadApplicationVersion()
    {
        Debug.Log("Start ReadApplicationVersion");

        ELKLog.Instance.SendLog(ELKLog.LogLevel.INFO, "App Start");

        GoogleFirebaseManager.ReadSimpleData(StringList.FirebaseAppVersion, (result, appVersion) =>
        {
            if (result == QueryAns.SUCCESS)
            {
                string releaseVersion = appVersion.ToString();

                Debug.LogWarning("InAppVersion : " + GlobalData.AppVersion + " releaseVersion : " + releaseVersion);

                List<string> appVersionList = new List<string>();
                List<string> releaseVersionList = new List<string>();

                UtilManager.Split(appVersionList, GlobalData.AppVersion, '.');
                UtilManager.Split(releaseVersionList, releaseVersion, '.');

                VersionCheck versionCheck = CheckVersion(appVersionList, releaseVersionList);

                if (versionCheck == VersionCheck.AGO)
                {
                    GoogleFirebaseManager.ReadSimpleData(StringList.FirebaseGoogleAppURL, (result, url) =>
                    {
                        if (result == QueryAns.SUCCESS)
                        {
                            string strUrl = url.ToString();
                            if (string.IsNullOrEmpty(strUrl) || strUrl.Length <= 0)
                            {
                                Debug.LogWarning("url : " + url);
                                return;
                            }
                            else
                            {
                                Debug.LogWarning("openURL : " + strUrl);
                                ThreadEvent.AddThreadEventParam((url) =>
                                {
                                    var popup = Popup<NoticePopup>.ShowPopup(PopupPath.PopupNotice, StringList.LanguageTable, StringList.NoticeAppUpdate);
                                    popup.SetOkAct(() =>
                                    {
                                        Application.OpenURL(strUrl);
                                        UtilManager.Quit();
                                    });

                                    popup.SetCancleAct(() =>
                                    {
                                        UtilManager.Quit();
                                    });

                                    popup.PushRemovePopupAct(() =>
                                    {
                                        UtilManager.Quit();
                                    });

                                    popup.IsExit = true;
                                }, strUrl);                                 
                            }
                        }
                        else
                        {
                            Debug.LogWarning("disable get url : " + result);
                        }
                    });
                }
                else
                {
                    Debug.LogWarning("Version is Same : " + appVersion.ToString());
                }
            }
            else
            {
                Debug.LogWarning("disable get Version : " + result);
            }
        });
    }


    private void Start()
    {

    }

    VersionCheck CheckVersion(List<string> appVersion, List<string> releaseVision)
    {
        if(appVersion.Count != releaseVision.Count)
        {
            return VersionCheck.NONE;
        }
        try
        {
            for (int i = 0; i < appVersion.Count; i++)
            {
                int appNum = int.Parse(appVersion[i]);
                int relNum = int.Parse(releaseVision[i]);

                if(appNum > relNum)
                {
                    return VersionCheck.LASTEST;
                }
                else if(appNum < relNum)
                {
                    return VersionCheck.AGO;
                }
            }
        }
        catch 
        {
            return VersionCheck.NONE;    
        }

        return VersionCheck.SAME;
    }

    private void Reset()
    {
        //if (GlobalData.IsGoogleLogin == false)
        //{
        //    Debug.Log("Start LocalData Save");
        //    GlobalDataConnector value = new GlobalDataConnector();
        //    value.SetGlobalData();
        //    PlayerPrefsManager.SaveAllLocalData();
        //}
    }
}

