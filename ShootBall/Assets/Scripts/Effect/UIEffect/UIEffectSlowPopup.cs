using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CallBackEvent))]
public class UIEffectSlowPopup : EffectBase
{
    [SerializeField]
    Vector3 startPos;

    [SerializeField]
    bool IsUseBack;

    [SerializeField]
    float duration;

    CallBackEvent callBackEvent;

    private void Awake()
    {
        callBackEvent = GetComponent<CallBackEvent>();
    }

    public void OnEnable()
    {
        UIEffectManager.PrintPopup(gameObject, transform.position + startPos,
            transform.position, duration, IsUseBack, callBackEvent.action).OnComplete(() =>
            {
                callBackEvent?.action?.Invoke();
            });
    }
}

