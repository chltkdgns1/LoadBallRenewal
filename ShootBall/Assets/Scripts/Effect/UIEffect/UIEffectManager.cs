using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Direct
{
    LEFT,
    TOP,
    RIGHT,
    BOTTOM
}

public enum EffectAttribute
{
    DON_USE = -404,
    INFINITE = -1
}

public static class UIEffectManager
{
    static public Tween TouchSizeUpErase(GameObject ob, float scale, float duration)
    {
        Transform[] allChildren = ob.GetComponentsInChildren<Transform>();
        Tween tween = ob.transform.DOScale(scale, duration);
        foreach (Transform child in allChildren)
        {
            Image tmpImg = child.GetComponent<Image>();
            Text txt = child.GetComponent<Text>();

            if (tmpImg != null) tmpImg.DOFade(0f, duration);
            else if (txt != null) txt.DOFade(0f, duration);
        }
        return tween;
    }

    static public Tween BounceEffect(GameObject ob, float upScale, float downScale, float duration, int loops = -404)
    {
        Transform[] allChildren = ob.GetComponentsInChildren<Transform>();

        Tween tween = null;

        foreach (Transform child in allChildren)
        {
            if (loops != -404)
            {
                DOTween.Sequence().
                    Append(child.DOScale(upScale, duration)).
                    Append(child.DOScale(downScale, duration)).
                    SetLoops(loops);
            }
            else
            {
                tween = DOTween.Sequence().
                    Append(child.DOScale(upScale, duration)).
                    Append(child.DOScale(downScale, duration));
            }
        }
        return tween;
    }

    static public Tween MoveObject(GameObject ob, Direct dir, Vector3 sizeDelta, float duration)
    {
        Tween tween = null;

        switch (dir)
        {
            case Direct.LEFT:
                tween = ob.transform.DOMoveX(ob.transform.position.x - sizeDelta.x, duration);
                break;                                                                       
            case Direct.RIGHT:                                                               
                tween = ob.transform.DOMoveX(ob.transform.position.x + sizeDelta.x, duration);
                break;                                                                       
            case Direct.TOP:                                                                 
                tween = ob.transform.DOMoveX(ob.transform.position.y + sizeDelta.y, duration);
                break;                                                                       
            case Direct.BOTTOM:                                                              
                tween = ob.transform.DOMoveX(ob.transform.position.y - sizeDelta.y, duration);
                break;
        }

        return tween;
    }

    static public void SetPositionObject(GameObject ob, Direct dir, Vector3 sizeDelta)
    {
        switch (dir)
        {
            case Direct.LEFT:
                ob.transform.position = new Vector3(ob.transform.position.x - sizeDelta.x, ob.transform.position.y);
                break;
            case Direct.RIGHT:
                ob.transform.position = new Vector3(ob.transform.position.x + sizeDelta.x, ob.transform.position.y);
                break;
            case Direct.TOP:
                ob.transform.position = new Vector3(ob.transform.position.x, ob.transform.position.y + sizeDelta.y);
                break;
            case Direct.BOTTOM:
                ob.transform.position = new Vector3(ob.transform.position.x, ob.transform.position.y - sizeDelta.x);
                break;
        }
    }

    static public void EmpBounce(GameObject ob, float maxScale, float minScale, float duration, float waitTime, int loops)
    {
        DOTween.Sequence().
            Append(ob.transform.DOScaleX(maxScale, duration)).
            Join(ob.transform.DOScaleY(maxScale, duration)).
            Append(ob.transform.DOScaleX(minScale, duration)).
            Join(ob.transform.DOScaleY(minScale, duration)).
            Append(ob.transform.DOScaleX(minScale, waitTime)). // WaitTime
            SetLoops(loops);
    }

    static public Tween PrintPopup(GameObject ob, Vector3 startPos, Vector3 endPos, float duration, bool isUseBack, Action completeAction = null)
    {
        ob.transform.position = startPos;

        ob.transform.DOMove(endPos, duration);

        Transform[] allChildren = ob.GetComponentsInChildren<Transform>();

        Sequence sequence = DOTween.Sequence();

        foreach (Transform child in allChildren)
        {
            if (isUseBack && ob.transform.name == child.name) continue;

            Image img = child.GetComponent<Image>();

            if (img != null)
            {
                img.color -= new Color(0, 0, 0, 1);
                sequence.Join(img.DOFade(1f, duration));
                //img.DOFade(1f, duration);
                continue;
            }

            Text txt = child.GetComponent<Text>();

            if (txt != null)
            {
                txt.color -= new Color(0, 0, 0, 1);
                sequence.Join(txt.DOFade(1f, duration));
                //txt.DOFade(1f, duration);
                continue;
            }
            TextMeshProUGUI txtPro = child.GetComponent<TextMeshProUGUI>();

            if (txtPro != null)
            {
                txtPro.color -= new Color(0, 0, 0, 1);
                sequence.Join(txtPro.DOFade(1f, duration));
                //txtPro.DOFade(1f, duration);
                continue;
            }
        }
        return sequence;
    }

    static public Tween Pumping(GameObject ob, float startScaleX, float startScaleY, float duration)
    {
        return DOTween.Sequence()
        .OnStart(() =>
        {
            ob.transform.localScale = new Vector3(startScaleX, startScaleY);
            ob.GetComponent<CanvasGroup>().alpha = 0;
        })
        .Append(ob.transform.DOScale(1, duration).SetEase(Ease.OutBounce))
        .Join(ob.GetComponent<CanvasGroup>().DOFade(1, duration));
    }

    static public Tween ScaleUpFadeOut(GameObject ob, float normalScaleX, float normalScaleY, float maxScaleX, float maxScaleY, float duration, Sprite sprite)
    {
        RectTransform rect = ob.GetComponent<RectTransform>();
        Image img = ob.GetComponent<Image>();
        if (rect == null || ob == null) return null;

        return DOTween.Sequence().
            Append(rect.DOScaleX(maxScaleX, duration / 2)).
            Join(rect.DOScaleY(maxScaleY, duration / 2)).
            Join(img.DOFade(0.5f, duration)).
            Append(rect.DOScaleX(normalScaleX, duration / 2)).
            Join(rect.DOScaleY(normalScaleY, duration / 2)).
            Join(img.DOFade(1f, duration));
    }

    static public Tween PrintBouncePopup(GameObject ob, Vector3 startScale, float duration, float endScale, float endAlpha, float dealy)
    {
        return DOTween.Sequence()
        .OnStart(() =>
        {
            ob.transform.localScale = startScale;
            ob.GetComponent<CanvasGroup>().alpha = 0;
        })
        .Append(ob.transform.DOScale(endScale, duration).SetEase(Ease.OutBounce))
        .Join(ob.GetComponent<CanvasGroup>().DOFade(endAlpha, duration)).SetDelay(dealy);
    }

    static public Tween EraseBoucePopup(GameObject ob, Vector3 startScale, float duration, float endScale, float endAlpha)
    {
        return DOTween.Sequence()
       .OnStart(() =>
       {
           ob.transform.localScale = startScale;
           ob.GetComponent<CanvasGroup>().alpha = 0;
       })
       .Append(ob.transform.DOScale(endScale, duration).SetEase(Ease.OutBounce))
       .Join(ob.GetComponent<CanvasGroup>().DOFade(endAlpha, duration));
    }
}

