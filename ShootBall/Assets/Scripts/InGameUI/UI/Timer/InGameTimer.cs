using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InGameTimer : MonoBehaviour
{
    int timerKey = -1;

    public Slider slider;
    public Text percentTxt;
    public bool IsFinishTimer;

    public StarEffect[] startEffectGroupArr;

    private float _reminaTime;

    public bool IsStop;

    public float RemainTime
    {
        get
        {
            return _reminaTime * 0.001f;
        }
    }

    public void StartInGameTimer()
    {
        ResetTimer();
        int level = PlayingGameManager.gameLevel.Value - 1;
        double time = GlobalData.TimeDataPro.timeArray[level].GetFloat();
        timerKey = Timer.StartTimer(time * 1000f);
        StartCoroutine(StartTimer(time * 1000f, () =>
        {
            InGameManager.instance.GameOver();
        }));
    }

    public void ResetTimer()
    {
        IsStop = false;
        slider.value = 100f;
        Timer.Remove(timerKey);
        timerKey = -1;
        percentTxt.text = "100%";
        IsFinishTimer = false;
        ResetStarGroup();
    }

    public void ResetStarGroup()
    {
        for (int i = 0; i < startEffectGroupArr.Length; i++)
        {
            startEffectGroupArr[i].ResetState();
        }
    }

    public void FinishTimer()
    {
        IsFinishTimer = true;
    }

    IEnumerator StartTimer(double usuallyRemainTime, Action act = null)
    {
        _reminaTime = (float)usuallyRemainTime;
        while (true)
        {
            if (IsFinishTimer)
            {
                ResetTimer();
                yield break;
            }

            if (IsStop)
            {
                yield return null;
                continue;
            }


            _reminaTime = (float)Timer.GetRemainTime(timerKey);
            slider.value = (float)(_reminaTime / usuallyRemainTime * 100f);
            percentTxt.text = (int)slider.value + "%";
            SetStarState(slider.value);
            if (_reminaTime <= 0f) break;
            yield return null;
        }
        if (IsFinishTimer)
        {
            ResetTimer();
            yield break;
        }
        act();
        ResetTimer();
        yield break;
    }

    void SetStarState(float timerPercent)
    {
        if (timerPercent < 25f) startEffectGroupArr[2].ExStarEffect();
        else if (timerPercent < 50f) startEffectGroupArr[1].ExStarEffect();
        else if (timerPercent < 75f) startEffectGroupArr[0].ExStarEffect();
    }

    public void Stop()
    {
        IsStop = true;
    }

    public void Restart()
    {
        if (_reminaTime <= 0)
        {
            InGameManager.instance.GameOver();
            ResetTimer();
            return;
        }
        IsStop = false;
        Timer.RestartTimer(_reminaTime, timerKey);
    }

    public int GetStar()
    {
        float percent = slider.value;
        if (percent < 25f) return 0;
        else if (percent < 50f) return 1;
        else if (percent < 75f) return 2;
        return 3;
    }
}
