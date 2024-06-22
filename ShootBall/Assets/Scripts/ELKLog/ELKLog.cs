using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ELKLog : MonoSingleTon<ELKLog>
{
    string url = "http://vicgamestudio.elklogserver.kro.kr:8080/logstash/logstore";

    public enum LogLevel
    {
        INFO,
        WARNING,
        ERROR,
        FATAL
    }

    protected override void Init()
    {

    }
    public override void StartSingleTon()
    {
    
    }

    public void SendLog(LogLevel logLevel, string log)
    {
        //Dictionary<string, string> dic = new Dictionary<string, string>();
        //dic.Add("logData", log);
        //StartCoroutine(CoSendLog(logLevel, JsonConvert.SerializeObject(dic)));
    }

    IEnumerator CoSendLog(LogLevel logLevel, string log)
    {
        using (var request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            var jsonBytes = Encoding.UTF8.GetBytes(log);
            request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();  // 응답이 올때까지 대기한다.

            if (request.error != null)  // 에러가 나지 않으면 동작.
            {
                Debug.LogError($"SendLog(string log) - {request.error}");
                yield break;
            }
        }
    }
}
