using System;
using System.Collections;
using UnityEngine;

public class WaitManager : MonoBehaviour
{
    public static WaitManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else enabled = false;
    }

    private void Start()
    {

    }

    public void StartWait(float fTime, Action act = null)
    {
        StartCoroutine(Wait(fTime, act));
    }

    public void StartWait(float fTime, object value, Action<object> act = null)
    {
        StartCoroutine(Wait(fTime, value, act));
    }

    public void StartWaitCacheObject(GameObject ob, float fTime, Action act = null)
    {
        ob.SetActive(true);
        Wait(fTime, act, ob);
    }

    IEnumerator Wait(float fTime, Action act, GameObject ob = null)
    {
        yield return new WaitForSecondsRealtime(fTime);
        act?.Invoke();
        ob?.SetActive(true);
    }

    IEnumerator Wait(float fTime, object value, Action<object> act, GameObject ob = null)
    {
        yield return new WaitForSecondsRealtime(fTime);
        act?.Invoke(value);
        ob?.SetActive(true);
    }
}
