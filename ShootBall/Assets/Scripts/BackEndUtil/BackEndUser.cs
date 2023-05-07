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
            Debug.Log("BackEnd Login 중...");
            return;
        }

        if (BackEndServer.Instance.IsCheckConnecting() == false)
        {
            Debug.Log("초기화 실패");
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
        Debug.Log("게스트 로그인 시작");

        var bro = Backend.BMember.GuestLogin();

        if (bro.IsSuccess())
        {
            Debug.Log("게스트 로그인에 성공했습니다. : " + bro);
            return true;
        }
        else
        {
            Debug.LogError("게스트 로그인에 실패했습니다. : " + bro);
            return false;
        }
    }

}

