using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class BackEndLogger : SingleTon<BackEndLogger>
{
    public enum LogType
    {
        NOMAL,
        ERROR,
        WARNNING
    }

    protected override void Init()
    {

    }

    static public void Log(string logParseKey, LogType logType, string logMessage, int remainDay = 1)
    {
#if REAL
        Param param = new Param();
        param.Add(logType.ToString(), logMessage);
        Backend.GameLog?.InsertLog(logParseKey, param, remainDay, (callback) =>
        {

        });
#else
        switch (logType)
        {
            case LogType.NOMAL:
                Debug.Log(logParseKey + " : " + logMessage);
                break;
            case LogType.WARNNING:
                Debug.LogWarning(logParseKey + " : " + logMessage);
                break;
            case LogType.ERROR:
                Debug.LogError(logParseKey + " : " + logMessage);
                break;
            default:
                Debug.Log(logParseKey + " : " + logMessage);
                break;
        }
#endif
    }
}
