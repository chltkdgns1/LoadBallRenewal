using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIErase : EffectBase
{
    private void Start()
    {
        
    }

    public virtual void OnErase()
    {
        gameObject.SetActive(false);
    }
}
