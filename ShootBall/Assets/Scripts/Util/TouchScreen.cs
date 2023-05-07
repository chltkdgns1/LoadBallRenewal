using System;
using System.Collections.Generic;
using UnityEngine;

public class TouchScreen : MonoBehaviour
{
    static public List<Action<Vector3>> touchAct = new List<Action<Vector3>>();
    static public List<Action<Vector3>> ClickAct = new List<Action<Vector3>>();

    public void Awake()
    {

    }

    static public void AddEvent(int index, Action<Vector3> act)
    {
        if (touchAct.Count <= index || ClickAct.Count <= index)
            return;

        touchAct[index] += act;
        ClickAct[index] += act;
    }

    static public int AddEvent(Action<Vector3> act)
    {
        touchAct.Add(act);
        ClickAct.Add(act);
        return touchAct.Count - 1;
    }

    static public void DeleteEvent(int index, Action<Vector3> act)
    {
        touchAct[index] -= act;
        ClickAct[index] -= act;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            OnTouchEvent();
        }

        if (Input.GetMouseButtonDown(0))
        {
            OnMouseEvent();
        }
    }

    void OnTouchEvent()
    {
        Touch touch = Input.GetTouch(0);
        Vector3 pos = touch.position;

        if (touch.phase == TouchPhase.Began)
        {
            int cnt = touchAct.Count;
            for (int i = 0; i < cnt; i++)
            {
                touchAct[i]?.Invoke(pos);
            }
        }
    }

    void OnMouseEvent()
    {
        Vector3 pos = Input.mousePosition;
        int cnt = ClickAct.Count;
        for (int i = 0; i < cnt; i++)
        {
            ClickAct[i]?.Invoke(pos);
        }
    }
}
