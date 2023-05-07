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
        Debug.Log("�ڳ� �ʱ�ȭ ��...");
        backendReturnObject = Backend.Initialize(true);

        if (backendReturnObject.IsSuccess())
        {
            //������ ��� statusCode 204 Success
            Debug.Log("�ʱ�ȭ ���� : " + backendReturnObject);
            return true;
        }
        else
        {
            backendReturnObject = null;
            // ������ ��� statusCode 400�� ���� �߻� 
            Debug.Log("�ʱ�ȭ ���� : " + backendReturnObject);
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
        // �ڳ� �񵿱� �Լ� ��� ��, ���ξ����忡�� �ݹ��� ó�����ִ� Dispatch
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
