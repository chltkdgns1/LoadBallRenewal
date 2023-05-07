using DG.Tweening;
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButton : EffectBase, IPointerDownHandler, IPointerUpHandler
{
    public GameObject targetOject;
    public MethodInfo methodInfo;
    public Type classType;

    public Action act;

    public float size;
    public float duration = 0.5f;

    public string compStr;
    public string methodStr;

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

    public virtual void Awake()
    {
        InitEditValue();
    }

    public virtual void OnEnable()
    {
        //InitEditValue();
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
        act?.Invoke();
        methodInfo?.Invoke(targetOject.GetComponent(classType), null);
    }

    void InitEditValue()
    {
        if (compStr == null)
        {
            methodStr = null;
            return;
        }

        if (gameObject.transform.name == "Back")
        {
            Debug.Log("Back");
        }

        classType = Type.GetType(compStr);
        methodInfo = classType?.GetMethod(methodStr);
    }

    public void SetDirty()
    {
        if (this == null) return;
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }
}

