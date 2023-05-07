using DG.Tweening;
using System;
using UnityEngine.EventSystems;

public class GameStageBtn : UIButtonEx
{
    public Action<int> actInt;
    public int index;

    public override void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(1f, duration);
        actInt?.Invoke(index);
    }
}

