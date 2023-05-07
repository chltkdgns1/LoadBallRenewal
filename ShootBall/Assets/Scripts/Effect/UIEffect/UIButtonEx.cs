using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonEx : CallBackEvent, IPointerDownHandler, IPointerUpHandler
{
    public float size;
    public float duration = 0.5f;

    public bool IsUseClickedOption = true;
    bool enableClick = true;

    [SerializeField]
    Sprite enableSprite;

    [SerializeField]
    Sprite unenableSprite;

    public bool EnableClick
    {
        get
        {
            return enableClick;
        }
        set
        {
            Image img = GetComponent<Image>();

            if (img != null)
            {
                if (value == true)
                {
                    if (enableSprite != null)
                    {
                        GetComponent<Image>().sprite = enableSprite;
                    }
                }
                else
                {
                    if (enableSprite != null)
                    {
                        GetComponent<Image>().sprite = unenableSprite;
                    }
                }
            }
            enableClick = value;
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (enableClick == false)
        {
            return;
        }

        if (IsUseClickedOption)
            transform.DOScale(size, duration);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (enableClick == false)
        {
            return;
        }

        if (IsUseClickedOption)
            transform.DOScale(1f, duration);

        OnEvent();
    }

    protected virtual void OnEvent()
    {
        action?.Invoke();
    }
}

