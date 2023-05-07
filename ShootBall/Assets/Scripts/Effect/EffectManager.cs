using DG.Tweening;
using UnityEngine;

public enum EffectLoop
{
    UNLOOP = 0,
    LOOP = -1
}

public static class EffectManager
{
    public static Tween Bounce(GameObject ob, float duration, float maxScale, float minScale, int loops)
    {
        return DOTween.Sequence().
            Append(ob.transform.DOScaleX(minScale, duration)).
            Join(ob.transform.DOScaleY(minScale, duration)).
            Append(ob.transform.DOScaleX(maxScale, duration)).
            Join(ob.transform.DOScaleY(maxScale, duration)).
            SetLoops(loops);
    }

    public static void ScaleUpErase(GameObject ob, float duration, float nomalScaleX, float nomalScaleY, float maxScaleX, float maxScaleY, int loops, float loopDuration)
    {
        DOTween.Sequence().
           Append(ob.transform.DOScaleX(maxScaleX, duration)).
           Join(ob.transform.DOScaleY(maxScaleY, duration)).
           Join(ob.GetComponent<SpriteRenderer>().DOFade(0f, duration)).
           Append(ob.GetComponent<SpriteRenderer>().DOFade(0f, loopDuration)).
           SetLoops(loops).OnComplete(() =>
           {
               ob.transform.localScale = new Vector3(nomalScaleX, maxScaleX);
               ob.GetComponent<SpriteRenderer>().color += new Color(1, 1, 1, 1);
           });
    }

    public static Tween MoveTurnX(GameObject ob, float moveAmount, float duration, float waitTime, int loops)
    {
        float normalPosX = ob.transform.localPosition.x;
        return DOTween.Sequence().
            Append(ob.transform.DOLocalMoveX(normalPosX + moveAmount, duration)).
            Join(ob.transform.GetComponent<SpriteRenderer>().DOFade(0f, duration)).
            Append(ob.transform.DOLocalMoveX(normalPosX, duration)).
            Join(ob.transform.GetComponent<SpriteRenderer>().DOFade(1f, duration)).
            Append(ob.transform.DOLocalMoveX(normalPosX, waitTime)).
            SetLoops(loops);
    }
}

