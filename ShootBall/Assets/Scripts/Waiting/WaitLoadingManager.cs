using System;
using UnityEngine;

[RequireComponent(typeof(WaitManager))]
public class WaitLoadingManager : MonoBehaviour
{
    public static WaitLoadingManager instance;

    [SerializeField]
    GameObject loadingBack;

    GameObject loadingBackReal;

    private void Awake()
    {
        if (instance == null) instance = this;
        else enabled = false;

        BackEndLogger.Log("WaitLoadingManager", BackEndLogger.LogType.NOMAL, "Awake ³¡");
    }

    private void Start()
    {

    }

    public void StartWaitLoading(float duration, Action act)
    {
        if (WaitManager.instance == null) return;

        if (loadingBackReal == null)
        {
            CreateWaitOb();
        }

        loadingBackReal.SetActive(true);
        act += SetEraseLoadingBack;
        WaitManager.instance.StartWait(duration, act);
    }

    void CreateWaitOb()
    {
        GameObject canvasObject = GameObject.Find("Canvas");
        if (canvasObject == null) return;

        loadingBackReal = Instantiate(loadingBack, canvasObject.transform);
        loadingBackReal.SetActive(true);
    }

    public void Clear()
    {
        WaitManager.instance.StopAllCoroutines();
    }

    public void SetEraseLoadingBack() { loadingBackReal.SetActive(false); }
}

