using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITouchErase : UIButtonEx
{
    // Button Bounse Erase

    public float touchSize;
    public float touchDuration;

    public bool hasErase;
    public bool IsRun;

    public override void Awake()
    {
        base.Awake();
    }

    public override void OnEnable()
    {
        InitState();
    }

    void InitState()
    {
        transform.localScale = new Vector3(1f, 1f);

        Image img = GetComponent<Image>();

        if (img != null)
        {
            GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
    }

    public override void OnPointerUp(PointerEventData eventData) { }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (IsRun) 
            return;

        IsRun = true;

        if (hasErase)
        {
            UIEffectManager.TouchSizeUpErase(gameObject, touchSize, touchDuration).OnComplete(() =>
            {
                MoveEnd();
                OnEvent();
            });
        }
        else OnEvent();
    }

    void MoveEnd()
    {
        IsRun = false;
    }
}

