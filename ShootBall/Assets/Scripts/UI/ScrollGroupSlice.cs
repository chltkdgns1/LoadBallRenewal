using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class ScrollGroupSlice : ScrollGroup
{
    private int pageIndex = 0;

    public int PageIndex { get { return pageIndex; } }

    protected virtual bool ChangePositionDir(Direct dirs)
    {
        if (_positionList.Length == 0) return false;

        int dir = (int)dirs;
        bool flag = false;

        for (int i = 0; i < _positionList.Length; i++)
        {
            Vector3 temp = new Vector3(_positionList[i].x + _dx[dir] * _moveSize, _positionList[i].y + _dy[dir] * _moveSize);
            flag = IsInBox(temp);
            if (flag) break;
        }

        if (flag == false) return false;

        for (int i = 0; i < _positionList.Length; i++)
        {
            _positionList[i] = new Vector3(_positionList[i].x + _dx[dir] * _moveSize, _positionList[i].y + _dy[dir] * _moveSize);
        }
        return true;
    }

    protected override void Start()
    {
        base.Start();
    }

    bool IsInBox(Vector3 pos)
    {
        return 0f <= pos.x && pos.x <= Screen.width && 0f <= pos.y && pos.y <= Screen.height;
    }

    public Tween MoveDir(bool flag, Direct dir)
    {
        Tween temp = null;
        if (ChangePositionDir(dir))
        {
            temp = MoveItem(1f);
            if (flag == true) NextPage();
            else BeforePage();
        }
        return temp;
    }

    void NextPage()
    {
        pageIndex++;
    }

    void BeforePage()
    {
        pageIndex--;
    }

    public override Tween MoveItem(float duration = 0f)
    {
        Tween temp = null;
        for (int i = 0; i < _positionList.Length; i++)
        {
            temp = _scrollItem[i].transform.DOLocalMove(_positionList[i], duration);
        }
        return temp;
    }
}
