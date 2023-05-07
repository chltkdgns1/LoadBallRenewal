using System;
using UnityEngine;

public class Loading : MonoBehaviour
{
    public virtual void EndLoading(float duration, Action act = null)
    {
        if (WaitManager.instance == null)
        {
            Debug.LogError("hasn't WaitManager");
            return;
        }

        if (act == null) act = OnClose;
        else act += OnClose;
        WaitManager.instance.StartWait(duration, act);
    }

    void OnClose()
    {
        gameObject.SetActive(false);
    }

    public virtual void StartLoading()
    {
        gameObject.SetActive(true);
    }
}
