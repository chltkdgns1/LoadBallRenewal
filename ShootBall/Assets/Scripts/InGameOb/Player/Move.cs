using System;
using System.Collections;
using UnityEngine;

public class Move : MonoBehaviour
{
    bool IsMove = false;

    [SerializeField]
    private float _speed;

    int[] _dirX = { 1, 0, -1, 0 };
    int[] _dirY = { 0, 1, 0, -1 };

    Direction _dir;

    Coroutine moveRoution = null;

    public Action MoveEndAct { get; set; }

    public Direction Dir
    {
        get { return _dir; }
    }

    public void MovePlayer(Direction dir)
    {
        // 방향이 주어지면 해당 방향으로 날아감.
        if (IsMove || dir < Direction.RIGHT || dir > Direction.BOTTOM) return;
        IsMove = true;
        _dir = dir;

        //if (moveRoution != null)
        //{
        //    StopCoroutine(moveRoution);
        //    moveRoution = null;
        //}

        //moveRoution = StartCoroutine(MovePlayerRoutine(dir));
    }

    void StopMoveRoution()
    {
        if (moveRoution != null)
        {
            StopCoroutine(moveRoution);
            moveRoution = null;
        }
    }

    private void FixedUpdate()
    {
        if (IsMove)
        {
            Vector3 vDir = new Vector3(_dirX[(int)_dir], _dirY[(int)_dir]);
            transform.position += vDir * _speed * Time.fixedDeltaTime;
        }
        else
        {
            ResetState();
        }
    }

    private void OnEnable()
    {
        ResetState();
    }

    IEnumerator MovePlayerRoutine(Direction dir)
    {
        Vector3 vDir = new Vector3(_dirX[(int)dir], _dirY[(int)dir]);

        while (IsMove)
        {
            transform.position += vDir * _speed * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        ResetState();
    }

    public void StopMove()
    {
        IsMove = false;
        StopMoveRoution();
    }

    public Vector3 SetPosition(Vector3 pos)
    {
        float xpos = pos.x;
        float ypos = pos.y;
        xpos = _dir == Direction.LEFT ? SetPositionXYPos(pos.x, false) : xpos; 
        xpos = _dir == Direction.RIGHT ? SetPositionXYPos(pos.x, true) : xpos;
        ypos = _dir == Direction.BOTTOM ? SetPositionXYPos(pos.y, false) : ypos;
        ypos = _dir == Direction.TOP ? SetPositionXYPos(pos.y, true) : ypos;
        return new Vector3(xpos, ypos);
    }

    float SetPositionXYPos(float xypos, bool isUp)
    {
        int essenceXypos = (int)xypos;
        float decimalXypos = xypos - (float)essenceXypos;

        bool IsMinus = false;
        if (decimalXypos < 0)
        {
            decimalXypos += 1f;
            IsMinus = true;
        }

        if (isUp) // 해당 좌표가 증가하고 있다면
        {
            if (0 <= decimalXypos && decimalXypos < 0.5f)
                decimalXypos = IsMinus == true ? -1f : 0f;

            else if (0.5 <= decimalXypos && decimalXypos < 1f)
                decimalXypos = IsMinus == true ? -0.5f : 0.5f;
        }
        else
        {
            if (0 < decimalXypos && decimalXypos <= 0.5f)
                decimalXypos = IsMinus == true ? -0.5f : 0.5f;

            else if (0.5 < decimalXypos && decimalXypos <= 1f)
                decimalXypos = IsMinus == true ? 0 : 1f;
        }
        return decimalXypos + essenceXypos;
    }

    public void ResetState()
    {
        IsMove = false;
        MoveEndAct?.Invoke();
    }
}
