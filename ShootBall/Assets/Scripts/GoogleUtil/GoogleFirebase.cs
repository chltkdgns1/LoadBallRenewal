using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public enum QueryAns
{
    NONE,
    FAILED,
    RESULT_NULL,
    NOT_EXITST_DATA,
    SUCCESS
}

public class GoogleFirebaseManager
{
    static DatabaseReference databaseRef;

    static GoogleFirebaseManager instance = new GoogleFirebaseManager();

    GoogleFirebaseManager()
    {
        FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(false);
        databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        //databaseRef.KeepSynced(true);
    }

    static public void OnStatic()
    {

    }

    static public async Task WriteSignUpData(string uid, string jsonValue)
    {
        Debug.LogWarning("WriteSignUpData Start");
        await WriteUserSignUp(uid);
        Debug.LogWarning("WriteSignUpData End");
        await WrtieUserData(uid, jsonValue);
        Debug.LogWarning("WrtieUserData End");
    }

    static public Task WrtieUserData(string uid, string jsonValue)
    {
        Debug.LogWarning("WrtieUserData Start");
        return databaseRef.Child(StringList.FirebaseUsers).Child(uid).SetRawJsonValueAsync(jsonValue);
    }

    static public Task WriteUserSignUp(string uid)
    {
        Debug.LogWarning("WriteUserSignUp Start");
        return databaseRef.Child("signUpUsers").Child(uid).SetValueAsync("1");
    }

    static public Task WriteCoinData(string uid, long coin)
    {
        return databaseRef.Child(StringList.FirebaseUsers).Child(uid).Child(StringList.FirebaseCoin).SetValueAsync(coin);
    }

    static public Task WriteDeleteAds(string uid)
    {
        return databaseRef.Child(StringList.FirebaseUsers).Child(uid).Child(StringList.FirebaseDeleteAds).SetValueAsync(true);
    }

    static public Task WriteStageData(string uid, int stage, string stageData) // 스테이지 데이터
    {
        return databaseRef.Child(StringList.FirebaseUsers).Child(uid).Child(StringList.FirebaseStageGroup).Child(StringList.FirebaseStageDataList).Child(stage.ToString()).SetRawJsonValueAsync(stageData);
    }

    static public Task WriteUnLockStageData(string uid, int stage)
    {
        return databaseRef.Child(StringList.FirebaseUsers).Child(uid).Child(StringList.FirebaseUnLockPage).SetValueAsync(stage);
    }

    static public Task WriteAppVersion(string appVersion)
    {
        return databaseRef.Child(StringList.FirebaseAppVersion).SetValueAsync(appVersion);
    }

    static public Task ReadDeleteAds(string uid, Action<QueryAns> result)
    {
        Debug.LogWarning("Start ReadDeleteAds");

        return databaseRef.Child(StringList.FirebaseUsers).Child(uid).Child(StringList.FirebaseDeleteAds).GetValueAsync().ContinueWith(task =>
        {
            Debug.LogWarning("Start CallBack ReadDeleteAds");

            if (task.IsFaulted)
            {
                Debug.LogWarning("ReadUserData failed");
                result?.Invoke(QueryAns.FAILED);
                return;
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot == null)
                {
                    Debug.LogWarning("read null or not Exitsts in ReadUserData");
                    result?.Invoke(QueryAns.RESULT_NULL);
                    return;
                }
                else if (snapshot.Exists == false)
                {
                    result?.Invoke(QueryAns.NOT_EXITST_DATA);
                    return;
                }

                Debug.LogWarning("Success ReadDeleteAds");
            }
        });
    }

    static public Task ReadUserData(string uid, Action success, Action fail, Action<QueryAns, Action> act)
    {
        Debug.LogWarning("Start ReadUserData");

        return databaseRef.Child(StringList.FirebaseUsers).Child(uid).GetValueAsync().ContinueWith(task =>
        {
            Debug.LogWarning("Start CallBack ReadUserData");

            if (task.IsFaulted)
            {
                Debug.LogWarning("ReadUserData failed");
                act?.Invoke(QueryAns.FAILED, fail);
                return;
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot == null)
                {
                    Debug.LogWarning("read null or not Exitsts in ReadUserData");
                    act?.Invoke(QueryAns.RESULT_NULL, fail);
                    return;
                }
                else if (snapshot.Exists == false)
                {
                    act?.Invoke(QueryAns.NOT_EXITST_DATA, fail);
                    return;
                }

                GlobalDataConnector value = new GlobalDataConnector();
                value.SetFromDic((Dictionary<string, object>)snapshot.Value);
                value.SetGlobalData();

                Debug.LogWarning("Success ReadUserData");
                act?.Invoke(QueryAns.SUCCESS, success);
            }
        });
    }

    static public void IsCheckedSignUp(string uid, Action success, Action fail, Action<QueryAns, Action, Action> act)
    {
        Debug.LogWarning("Start IsCheckedSignUp");

        databaseRef.Child(StringList.FirebaseSignUpUsers).Child(uid).GetValueAsync().ContinueWith(task =>
        {
            Debug.LogWarning("Start CallBack IsCheckedSignUp");

            if (task.IsFaulted)
            {
                Debug.LogWarning("IsCheckedSignUp failed");
                act?.Invoke(QueryAns.FAILED, success, fail);
                return;
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot == null)
                {
                    Debug.LogWarning("read null in IsCheckedSignUp");
                    act?.Invoke(QueryAns.RESULT_NULL, success, fail);
                    return;
                }
                else if (snapshot.Exists == false)
                {
                    Debug.LogWarning("read not Exists in IsCheckedSignUp");
                    act?.Invoke(QueryAns.NOT_EXITST_DATA, success, fail);
                    return;
                }

                Debug.LogWarning("Success IsCheckedSignUp");
                act?.Invoke(QueryAns.SUCCESS, success, fail);
            }
        });
    }

    static public Task ReadCoinData(string uid, Action<QueryAns> act)
    {
        Debug.LogWarning("Start ReadCoinData");

        return databaseRef.Child(StringList.FirebaseUsers).Child(uid).Child(StringList.FirebaseCoin).GetValueAsync().ContinueWith(task =>
        {
            Debug.LogWarning("Start CallBack ReadCoinData");

            if (task.IsFaulted)
            {
                Debug.LogWarning("ReadCoinData failed");
                act?.Invoke(QueryAns.FAILED);
                return;
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot == null)
                {
                    Debug.LogWarning("read null in ReadCoinData");
                    act?.Invoke(QueryAns.RESULT_NULL);
                    return;
                }
                else if (snapshot.Exists == false)
                {
                    Debug.LogWarning("read not Exists in ReadCoinData");
                    act?.Invoke(QueryAns.NOT_EXITST_DATA);
                    return;
                }

                GlobalData.Coin = (long)snapshot.Value;
                act?.Invoke(QueryAns.SUCCESS);
            }
        });
    }

    static public Task ReadCashData(string uid, Action<QueryAns> act)
    {
        Debug.LogWarning("Start ReadCashData");

        return databaseRef.Child(StringList.FirebaseUsers).Child(uid).Child(StringList.FirebaseCash).GetValueAsync().ContinueWith(task =>
        {
            Debug.LogWarning("Start CallBack ReadCashData");

            if (task.IsFaulted)
            {
                Debug.LogWarning("ReadCashData failed");
                act?.Invoke(QueryAns.FAILED);
                return;
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot == null)
                {
                    Debug.LogWarning("read null in ReadCashData");
                    act?.Invoke(QueryAns.RESULT_NULL);
                    return;
                }
                else if (snapshot.Exists == false)
                {
                    Debug.LogWarning("read not Exists in ReadCashData");
                    act?.Invoke(QueryAns.NOT_EXITST_DATA);
                    return;
                }

                GlobalData.Cash = (long)snapshot.Value;
                act?.Invoke(QueryAns.SUCCESS);
            }
        });
    }

    static public Task ReadPurchaseData(string purchasePath, Action<QueryAns, long> act)
    {
        Debug.LogWarning("Start ReadPurchaseData");

        return databaseRef.Child(purchasePath).GetValueAsync().ContinueWith(task =>
        {
            Debug.LogWarning("Start CallBack ReadPurchaseData");

            if (task.IsFaulted)
            {
                Debug.LogWarning("ReadPurchaseData failed");
                act?.Invoke(QueryAns.FAILED, 0);
                return;
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot == null)
                {
                    Debug.LogWarning("read null in ReadPurchaseData");
                    act?.Invoke(QueryAns.RESULT_NULL, 0);
                    return;
                }
                else if (snapshot.Exists == false)
                {
                    Debug.LogWarning("read not Exists in ReadPurchaseData");
                    act?.Invoke(QueryAns.NOT_EXITST_DATA, 0);
                    return;
                }

                long price = (long)snapshot.Value;
                act?.Invoke(QueryAns.SUCCESS, price);
            }
        });
    }

    static public Task ReadSimpleData(string path, Action<QueryAns, object> act)
    {
        Debug.LogWarning("Start ReadSimpleData");

        return databaseRef.Child(path).GetValueAsync().ContinueWith(task =>
        {
            Debug.LogWarning("Start CallBack ReadSimpleData");

            if (task.IsFaulted)
            {
                Debug.LogWarning("ReadSimpleData failed");
                act?.Invoke(QueryAns.FAILED, null);
                return;
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot == null)
                {
                    Debug.LogWarning("read null in ReadSimpleData");
                    act?.Invoke(QueryAns.RESULT_NULL, null);
                    return;
                }
                else if (snapshot.Exists == false)
                {
                    Debug.LogWarning("read not Exists in ReadSimpleData");
                    act?.Invoke(QueryAns.NOT_EXITST_DATA, null);
                    return;
                }

                act?.Invoke(QueryAns.SUCCESS, snapshot.Value);
            }
        });
    }

    static public Task DeleteSignupUserData(string uid)
    {
        Debug.LogWarning("Start DeleteSignupUserData");
        return databaseRef.Child(StringList.FirebaseSignUpUsers).Child(uid).RemoveValueAsync();
    }
}

