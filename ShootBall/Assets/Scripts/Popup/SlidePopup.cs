using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SlidePopup : PopupStack
{
    public Direct outdir;
    public float duration;

    Vector3 _sizeDelta;

    public bool IsMove;

    public class PopupEventData
    {
        public int index;
        public Action act = null;

        public PopupEventData(int index, Action act)
        {
            this.index = index;
            this.act = act;
        }
    }

    int frontIndex = -1;
    Queue<PopupEventData> _eventQue = new Queue<PopupEventData>();

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        _sizeDelta = new Vector3(Screen.width, Screen.height);
        Debug.Log(_sizeDelta);
        InitPos();
    }

    protected override void OnEnable()
    {
        frontIndex = -1;
        _eventQue.Clear();
    }

    protected override void OnDisable()
    {

    }

    private void Update()
    {
        if (IsMove == false)
        {
            if (_eventQue.Count == 0) return;

            PopupEventData data = _eventQue.Dequeue();
            if (frontIndex == data.index) return;

            frontIndex = data.index;

            if (frontIndex == 0) MoveInExcute(data);
            else MoveOutExcute(data);
        }
    }

    public void InitPos()
    {
        Debug.Log("_sizeDelta : " + _sizeDelta);
        UIEffectManager.SetPositionObject(gameObject, outdir, _sizeDelta);
    }

    public void MoveInSide(Action act = null)
    {
        _eventQue.Enqueue(new PopupEventData(0, act));
    }

    public void MoveOutSide(Action act = null)
    {
        _eventQue.Enqueue(new PopupEventData(1, act));
    }

    public void MoveOutExcute(PopupEventData data)
    {
        IsMove = true;
        MoveOut(data.act);
    }

    public void MoveInExcute(PopupEventData data)
    {
        IsMove = true;
        MoveIn(data.act);
    }

    public void MoveOut(Action act = null)
    {
        RemovePopup();
        UIEffectManager.MoveObject(gameObject, outdir, _sizeDelta, duration).OnComplete(() =>
        {
            IsMove = false;
            act?.Invoke();
        });
    }

    public void MoveIn(Action act = null)
    {
        AddPopup();
        UIEffectManager.MoveObject(gameObject, (Direct)(((int)outdir + 2) % 4), _sizeDelta, duration).OnComplete(() =>
        {
            IsMove = false;
            act?.Invoke();
        });
    }
}
