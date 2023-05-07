using BackEnd;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class BackEndUser : SingleTon<BackEndUser>
{
    bool IsProcessingLogin = false;
    Action<bool> result;

    public BackEndUser()
    {

    }

    protected override void Init()
    {

    }

    public async void LoginBackEnd(Action<bool> result)
    {
        if (IsProcessingLogin)
        {
            Debug.Log("BackEnd Login ��...");
            return;
        }

        if (BackEndServer.Instance.IsCheckConnecting() == false)
        {
            Debug.Log("�ʱ�ȭ ����");
            result?.Invoke(false);
            return;
        }

        IsProcessingLogin = true;
        this.result = result;

        await Task.Run(() =>
        {
            IsProcessingLogin = false;
            bool flag = GuestLogin();
            result?.Invoke(flag);
        });
    }

    bool GuestLogin()
    {
        Debug.Log("�Խ�Ʈ �α��� ����");

        var bro = Backend.BMember.GuestLogin();

        if (bro.IsSuccess())
        {
            Debug.Log("�Խ�Ʈ �α��ο� �����߽��ϴ�. : " + bro);
            return true;
        }
        else
        {
            Debug.LogError("�Խ�Ʈ �α��ο� �����߽��ϴ�. : " + bro);
            return false;
        }
    }

}

