using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIEffectBounce : EffectBase
{
    public float maxScale;
    public float minScale;
    public float duration;
    public float waitTime;
    public EffectLoop loops = EffectLoop.LOOP;

    private void Start()
    {
        UIEffectManager.EmpBounce(gameObject, maxScale, minScale, duration, waitTime, (int)loops);
    }
}

