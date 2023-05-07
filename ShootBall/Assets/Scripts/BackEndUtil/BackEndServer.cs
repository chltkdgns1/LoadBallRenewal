using UnityEngine;
using BackEnd;


public class BackEndServer : MonoSingleTon<BackEndServer>
{
    BackendReturnObject backendReturnObject = null;

    private void Awake()
    {
        Reconnect();
    }

    public bool Reconnect()
    {
        Debug.Log("뒤끝 초기화 중...");
        backendReturnObject = Backend.Initialize(true);

        if (backendReturnObject.IsSuccess())
        {
            //성공일 경우 statusCode 204 Success
            Debug.Log("초기화 성공 : " + backendReturnObject);
            return true;
        }
        else
        {
            backendReturnObject = null;
            // 실패일 경우 statusCode 400대 에러 발생 
            Debug.Log("초기화 실패 : " + backendReturnObject);
            return false;
        }
    }

    public bool IsCheckConnecting()
    {
        if (IsConnect() == true)
        {
            return true;
        }

        return Reconnect();
    }


    public bool IsConnect()
    {
        return backendReturnObject != null;
    }

    protected override void Init()
    {

    }

    void Update()
    {
        // 뒤끝 비동기 함수 사용 시, 메인쓰레드에서 콜백을 처리해주는 Dispatch
        Backend.AsyncPoll();
    }

    public override void StartSingleTon()
    {
        if (IsConnect() == false)
        {
            Reconnect();
        }
    }
}
