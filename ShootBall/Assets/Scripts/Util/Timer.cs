using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    public class TimeValue
    {
        public double remainTime;
        public DateTime startTime;

        public TimeValue(double remainTime,DateTime startTime)
        {
            this.remainTime = remainTime;
            this.startTime = startTime;
        }
    }

    static Dictionary<int, TimeValue> dicTimer = new Dictionary<int, TimeValue>();
    static int _cnt;

    static public int StartTimer(double remainTime)
    {
        dicTimer[_cnt] = new TimeValue(remainTime, DateTime.Now);
        return _cnt++;
    }

    static public void RestartTimer(double remainTime ,int key)
    {
        dicTimer[key].remainTime = remainTime;
        dicTimer[key].startTime = DateTime.Now;
    }

    static public double GetRemainTime(int key)
    {
        if (dicTimer.ContainsKey(key) == false) return -1f;
        TimeValue temp = dicTimer[key];
        double remainTime = temp.remainTime - DateTime.Now.Subtract(temp.startTime).TotalMilliseconds;
        return remainTime <= 0 ? 0f : remainTime;
    }

    static public void Remove(int key)
    {
        if (dicTimer.ContainsKey(key) == false) return;
        dicTimer.Remove(key);
    }
}
