using GooglePlayGames;
using System;
using UnityEngine;

public class GoogleLogin
{
    public static GoogleLogin instance = new GoogleLogin();

    GoogleLogin()
    {
        //PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
        //.Builder()
        //.RequestServerAuthCode(false)
        //.RequestIdToken()
        //.Build();
        ////커스텀 된 정보로 GPGS 초기화
        //PlayGamesPlatform.InitializeInstance(config);


        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    static public void OnStatic()
    {

    }

    public static void OnLogin(Action success, Action fail)
    {
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool bSuccess) =>
            {
                if (bSuccess == false)
                {
#if UNITY_EDITOR
                    if (ApplicationManager.instance.UseTestGoogleLogin == true)
                    {
                        GlobalData.IsGoogleLogin = true;
                        GlobalData.Uid = "123";

                        GoogleFirebaseManager.IsCheckedSignUp(GlobalData.Uid, success, fail, async (response, success, fail) =>
                        {
                            Debug.LogWarning("IsCheckedSignUp Start");
                            if (response == QueryAns.FAILED)
                            {
                                OnOffLineLogin("1 - " + StringList.NetworkExitMessage + "_01", 3f, fail);
                                return;
                            }
                            else if (response == QueryAns.RESULT_NULL)
                            {
                                OnOffLineLogin("1 - " + StringList.NetworkExitMessage + "_02", 3f, fail);
                                return;
                            }
                            else if (response == QueryAns.NOT_EXITST_DATA)
                            {
                                Debug.LogWarning("isCheckSignUp = false SignUp Start");
                                string signUpUserData = GlobalDataConnector.GetGlobalDataConnector();
                                await GoogleFirebaseManager.WriteSignUpData(GlobalData.Uid, signUpUserData);
                                Debug.LogWarning("isCheckSignUp = false SignUp End");
                            }

                            await GoogleFirebaseManager.ReadUserData(GlobalData.Uid, success, fail, async (response, ansAction) =>
                            {
                                Debug.LogWarning("ReadUserData Start");

                                if (response == QueryAns.FAILED) // 실패 시, 앱 종료
                                {
                                    OnOffLineLogin("2 - " + StringList.NetworkExitMessage + "_01", 3f, ansAction);
                                    return;
                                }
                                else if (response == QueryAns.RESULT_NULL)
                                {
                                    OnOffLineLogin("2 - " + StringList.NetworkExitMessage + "_02", 3f, ansAction);
                                    return;
                                }
                                else if (response == QueryAns.NOT_EXITST_DATA)
                                {
                                    await GoogleFirebaseManager.DeleteSignupUserData(GlobalData.Uid);
                                    // 로그인 유저 데이터 삭제하고 종료.
                                    OnOffLineLogin("2 - " + StringList.DBDataErrorExit + "_03", 3f, ansAction);
                                    return;
                                }
                                else
                                {
                                    Debug.LogWarning("Start LobbyScene");
                                    ThreadEvent.AddThreadEvent(success);
                                }
                            });
                        });
                    }

                    if (ApplicationManager.instance.UseTestGoogleLogin == false)
                    {
                        GlobalData.IsGoogleLogin = false;
                        fail?.Invoke();
                    }
#else
                        GlobalData.IsGoogleLogin = false;
                        fail?.Invoke();
#endif
                }
                else
                {
                    Debug.LogWarning("Success Login");

                    GlobalData.Uid = Social.localUser.id;

                    if (string.IsNullOrEmpty(GlobalData.Uid))
                    {
                        GlobalData.IsGoogleLogin = false;
                        fail?.Invoke();
                        return;
                    }
                   
                    GlobalData.IsGoogleLogin = true;

                    GoogleFirebaseManager.IsCheckedSignUp(GlobalData.Uid, success, fail, async (response, success, fail) =>
                    {
                        Debug.LogWarning("IsCheckedSignUp Start");
                        if (response == QueryAns.FAILED)
                        {
                            OnOffLineLogin("1 - " + StringList.NetworkExitMessage + "_01", 3f, fail);
                            return;
                        }
                        else if (response == QueryAns.RESULT_NULL)
                        {
                            OnOffLineLogin("1 - " + StringList.NetworkExitMessage + "_02", 3f, fail);
                            return;
                        }
                        else if (response == QueryAns.NOT_EXITST_DATA)
                        {
                            Debug.LogWarning("isCheckSignUp = false SignUp Start");
                            string signUpUserData = GlobalDataConnector.GetGlobalDataConnector();
                            await GoogleFirebaseManager.WriteSignUpData(GlobalData.Uid, signUpUserData);
                            Debug.LogWarning("isCheckSignUp = false SignUp End");
                        }

                        await GoogleFirebaseManager.ReadUserData(GlobalData.Uid, success, fail, async (response, ansAction) =>
                        {
                           Debug.LogWarning("ReadUserData Start");

                           if (response == QueryAns.FAILED) // 실패 시, 앱 종료
                               {
                               OnOffLineLogin("2 - " + StringList.NetworkExitMessage + "_01", 3f, ansAction);
                               return;
                           }
                           else if (response == QueryAns.RESULT_NULL)
                           {
                               OnOffLineLogin("2 - " + StringList.NetworkExitMessage + "_02", 3f, ansAction);
                               return;
                           }
                           else if (response == QueryAns.NOT_EXITST_DATA)
                           {
                               await GoogleFirebaseManager.DeleteSignupUserData(GlobalData.Uid);
                                   // 로그인 유저 데이터 삭제하고 종료.
                                   OnOffLineLogin("2 - " + StringList.DBDataErrorExit + "_03", 3f, ansAction);
                               return;
                           }
                           else
                           {
                               Debug.LogWarning("Start LobbyScene");
                               ThreadEvent.AddThreadEvent(success);
                           }
                       });
                    });
                }
            });
        }
    }

    static public void OnOffLineLogin(string str, float duration, Action act)
    {
        Debug.LogWarning(str);
        ToastMessageManager.instance.StartToastMessage(str, duration);
        ThreadEvent.AddThreadEvent(act);
    }

    public static void OnLogout()
    {
        PlayGamesPlatform.Instance.SignOut();
    }
}