using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public enum UseSize
{
    UN_USE,
    USE
}

public class ScrollGroup : MonoBehaviour
{
    [SerializeField]
    protected GameObject[] _scrollItem;

    protected Vector3[] _positionList;

    [SerializeField]
    protected Direct _dir;

    protected UseSize useState = UseSize.UN_USE;

    protected int[] _dx = { -1, 0, 1, 0 };
    protected int[] _dy = { 0, 1, 0, -1 };

    protected float _moveSize;

    protected virtual void Start()
    {
        if (useState == UseSize.UN_USE)
        {
            if (_dir == Direct.LEFT || _dir == Direct.RIGHT)
            {
                _moveSize = Screen.width;
                if (_moveSize < 1920) _moveSize = 1920;
            }
            else
            {
                _moveSize = Screen.height;
                if (_moveSize < 1080) _moveSize = 1080;
            }
        }

        SetPosition();
    }

    void SetPosition()
    {
        if (_scrollItem.Length == 0) return;

        _positionList = new Vector3[_scrollItem.Length];
        Vector3 pos = _scrollItem[0].transform.localPosition;

        int dir = (int)_dir;

        for (int i = 0; i < _scrollItem.Length; i++)
        {
            _positionList[i] = new Vector3(pos.x + _dx[dir] * _moveSize * i, pos.y + _dy[dir] * _moveSize * i);
            _scrollItem[i].transform.localPosition = _positionList[i];
        }
    }

    void SetEnablePos()
    {
        if (_scrollItem.Length == 0) return;

        for (int i = 0; i < _scrollItem.Length; i++)
        {
            _scrollItem[i].transform.localPosition = _positionList[i];
        }
    }

    public void EnableOb()
    {
        for (int i = 0; i < _scrollItem.Length; i++)
        {
            _scrollItem[i].SetActive(true);
        }
    }

    public void DisableOb()
    {
        for (int i = 0; i < _scrollItem.Length; i++)
        {
            _scrollItem[i].SetActive(false);
        }
    }

    public void EnableCenterOb()
    {
        SetEnablePos();
        for (int i = 0; i < _scrollItem.Length; i++)
        {
            if (IsCenterPos(_scrollItem[i].transform.localPosition))
            {
                _scrollItem[i].SetActive(true);
                break;
            }
        }
    }

    bool IsCenterPos(Vector3 pos)
    {
        return 0f <= pos.x && pos.x <= Screen.width && 0f <= pos.y && pos.y <= Screen.height;
    }

    protected virtual void ChangePosition()
    {
        if (_positionList.Length == 0) return;
        int back = _positionList.Length - 1;

        Vector3 temp = _positionList[back];
        for (int i = back; i > 0; i--)
            _positionList[i] = _positionList[i - 1];
        //_positionList[i] = _positionList[i + 1];   
        _positionList[0] = temp;
    }

    private void OnDisable()
    {
        DisableOb();
    }

    public virtual Tween MoveItem(float duration = 0f)
    {
        ChangePosition();
        Tween temp = null;
        for (int i = 0; i < _positionList.Length; i++)
        {
            temp = _scrollItem[i].transform.DOLocalMove(_positionList[i], duration);
        }
        return temp;
    }
}
