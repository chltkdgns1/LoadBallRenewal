using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StarEffect : MonoBehaviour
{
    [SerializeField]
    Sprite goldStar;
    [SerializeField]
    Sprite grayStar;

    public float maxScaleX;
    public float maxScaleY;
    public float duration;

    Image _img;
    RectTransform _rect;

    bool exFlag = false;

    public void ExStarEffect()
    {
        if (exFlag == true) return;
        exFlag = true;

        if (_img == null) _img = GetComponent<Image>();
        if (_rect == null) _rect = GetComponent<RectTransform>();

        float normalX = transform.localScale.x;
        float normalY = transform.localScale.y;

        DOTween.Sequence().
            Append(_rect.DOScaleX(maxScaleX + transform.localScale.x, duration / 2)).
            Join(_rect.DOScaleY(maxScaleY + transform.localScale.y, duration / 2)).
            Join(_img.DOFade(0.5f, duration)).OnComplete(() =>
            {
                _img.sprite = grayStar;
                DOTween.Sequence().
                Append(_rect.DOScaleX(normalX, duration / 2)).
                Join(_rect.DOScaleY(normalY, duration / 2)).
                Join(_img.DOFade(1f, duration));
            });
    }

    public void ExReverseStarEffect()
    {
        if (exFlag == true) return;
        exFlag = true;

        if (_img == null) _img = GetComponent<Image>();
        if (_rect == null) _rect = GetComponent<RectTransform>();

        float normalX = transform.localScale.x;
        float normalY = transform.localScale.y;

        _img.sprite = grayStar;

        DOTween.Sequence().
            Append(_rect.DOScaleX(maxScaleX + transform.localScale.x, duration / 2)).
            Join(_rect.DOScaleY(maxScaleY + transform.localScale.y, duration / 2)).
            Join(_img.DOFade(0f, duration)).OnComplete(() =>
            {
                _img.sprite = goldStar;
                DOTween.Sequence().
                Append(_rect.DOScaleX(normalX, duration / 2)).
                Join(_rect.DOScaleY(normalY, duration / 2)).
                Join(_img.DOFade(1f, duration));
            });
    }

    public void ResetState()
    {
        if (_img == null) _img = gameObject.GetComponent<Image>();
        exFlag = false;
        _img.sprite = goldStar;
        _img.color += new Color(0, 0, 0, 1f);
        gameObject.transform.localScale = new Vector3(1f, 1f);
    }

    public void ResetReverseState()
    {
        if (_img == null) _img = gameObject.GetComponent<Image>();
        exFlag = false;
        _img.sprite = grayStar;
        _img.color += new Color(0, 0, 0, 1f);
        gameObject.transform.localScale = new Vector3(1f, 1f);
    }
}
