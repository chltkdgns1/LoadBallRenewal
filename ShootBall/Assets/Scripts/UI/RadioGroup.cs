using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RadioGroup : MonoBehaviour
{
    [SerializeField]
    GameObject[] radioGroup;

    int _nPos;

    private void Awake()
    {
        _nPos = 0;
    }

    private void Start()
    {
        for (int i = 0; i < radioGroup.Length; i++)
        {
            radioGroup[i].SetActive(false);
        }
        radioGroup[0].SetActive(true);
    }

    public void NextMove()
    {
        if (_nPos == radioGroup.Length - 1) return;

        radioGroup[_nPos].SetActive(false);
        _nPos++;
        radioGroup[_nPos].SetActive(true);
    }

    public void BackMove()
    {
        if (_nPos == 0) return;

        radioGroup[_nPos].SetActive(false);
        _nPos--;
        radioGroup[_nPos].SetActive(true);
    }

    public int GetGroupPage()
    {
        return _nPos;
    }

    public void NextMoveEffect(float duration)
    {
        radioGroup[_nPos].transform.GetComponent<Image>().DOFade(0f, duration);
        _nPos++;
        _nPos %= radioGroup.Length;
        radioGroup[_nPos].transform.GetComponent<Image>().DOFade(1f, duration);
    }
}
