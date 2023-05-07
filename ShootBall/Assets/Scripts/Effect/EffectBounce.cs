using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBounce : EffectBase
{
    public EffectLoop loops = EffectLoop.LOOP;
    public float duration = 1f;
    public float maxScale = 0.01f;
    public float minScale = -0.01f;

    protected virtual void Start()
    {
        EffectManager.Bounce(gameObject, duration, maxScale + transform.localScale.x, transform.localScale.x - minScale, (int)loops);
    }
}

