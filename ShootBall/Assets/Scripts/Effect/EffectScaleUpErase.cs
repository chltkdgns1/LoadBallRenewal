using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectScaleUpErase : MonoBehaviour
{
    public float duration;
    public float maxScaleX;
    public float maxScaleY;
    public float loopDuration;
    public EffectLoop loops = EffectLoop.LOOP;

    void Start()
    {
        EffectManager.ScaleUpErase(gameObject, duration, transform.localScale.x, transform.localScale.y,
            maxScaleX + transform.localScale.x, maxScaleY + transform.localScale.y, (int)loops, loopDuration);
    }
}

