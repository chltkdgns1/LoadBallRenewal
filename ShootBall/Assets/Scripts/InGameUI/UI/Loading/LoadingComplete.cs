using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingComplete : Loading
{
    public EndGameState endGameState;
    public EndResultStar endResultStarState;
    public TMP_FontAsset[] fontAsset;
    public TextMeshProUGUI txt;
    public Image img;
    public float duration;
    public Action act;

    private void OnEnable()
    {
        ResetOb();
        int index = (int)endResultStarState;
        string str = GetCompleteTxt();

        GameObject tweenOb = null;

        if (str == null || string.IsNullOrEmpty(str))
        {
            tweenOb = img.gameObject;
        }
        else
        {
            txt.font = fontAsset[index];
            txt.text = str;
            tweenOb = txt.gameObject;
        }

        tweenOb.SetActive(true);
        ExTween(tweenOb);
    }

    void ExTween(GameObject ob)
    {
        UIEffectManager.Pumping(ob, 0.5f, 0.5f, 0.7f)
            .OnComplete(() =>
            {
                EndLoading(duration, act);
            });
    }

    void ResetOb()
    {
        img.gameObject.SetActive(false);
        txt.gameObject.SetActive(false);
    }

    string GetCompleteTxt()
    {
        if (endGameState == EndGameState.FAILED) return null;
        if (endResultStarState == EndResultStar.ZEROSTAR) return StringList.GameComplete_ZeroStar;
        if (endResultStarState == EndResultStar.ONESTAR) return StringList.GameComplete_OneStar;
        if (endResultStarState == EndResultStar.TWOSTAR) return StringList.GameComplete_TwoStar;
        if (endResultStarState == EndResultStar.THREESTAR) return StringList.GameComplete_ThreeStar;
        return null;
    }

    public override void StartLoading()
    {
        if (gameObject.activeSelf) OnEnable();
        else base.StartLoading();
    }
}
