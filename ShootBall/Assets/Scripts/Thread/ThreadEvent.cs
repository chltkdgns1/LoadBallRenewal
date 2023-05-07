using System;
using System.Collections.Generic;
using UnityEngine;

public class ThreadEvent : MonoBehaviour
{
    public delegate void EventParam(params object[] ob);

    class EventParamData
    {
        public object[] param;
        public EventParam evt;

        public EventParamData(EventParam evt, object[] param)
        {
            this.evt = evt;
            this.param = param;
        }
    }

    static Queue<Action> evtQueue = new Queue<Action>();
    static Queue<EventParamData> evtParamQueue = new Queue<EventParamData>();

    static object evtQueueLock = new object();
    static object evtParamQueueLock = new object();

    void Update()
    {
        ExcuteEvtQueue();
        ExcuteEvtParamQueue();
    }

    void ExcuteEvtQueue()
    {
        if (evtQueue.Count == 0)
            return;

        Action actTemp = null;
        lock (evtQueueLock)
        {
            actTemp = evtQueue.Dequeue();
        }
        actTemp?.Invoke();
    }

    void ExcuteEvtParamQueue()
    {
        if (evtParamQueue.Count == 0)
            return;

        EventParamData actTemp = null;
        lock (evtParamQueueLock)
        {
            actTemp = evtParamQueue.Dequeue();
        }
        actTemp.evt?.Invoke(actTemp.param);
    }

    public static void AddThreadEvent(Action act)
    {
        lock (evtQueueLock)
        {
            evtQueue.Enqueue(act);
        }
    }

    public static void AddThreadEventParam(EventParam evt, params object[] ob)
    {
        lock (evtParamQueueLock)
        {
            evtParamQueue.Enqueue(new EventParamData(evt, ob));
        }
    }
}
