using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    Color _normalColor;

    [SerializeField]
    Color _pressColor;

    [SerializeField]
    Direction _dir;

    [SerializeField]
    GameObject parOb;

    Action<Direction> movePlayerAct;

    public Action<Direction> MovePlayerAct
    {
        set { movePlayerAct = value; }
    }

    private void Awake()
    {
        if (parOb != null)
            parOb.GetComponent<Image>().color = _normalColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (parOb == null) GetComponent<Image>().color = _pressColor;
        else parOb.GetComponent<Image>().color = _pressColor;
        movePlayerAct(_dir);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (parOb == null) GetComponent<Image>().color = _normalColor;
        else parOb.GetComponent<Image>().color = _normalColor;
    }
}
