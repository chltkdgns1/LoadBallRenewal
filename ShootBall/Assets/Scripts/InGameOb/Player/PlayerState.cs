using UnityEngine;

public class PlayerState : MonoBehaviour
{
    Move _moveComp;

    GameObject tempPotalOb; // 현재 턴에서 포탈로 이동했을 경우

    Vector3 _position;

    private void Awake()
    {
        _moveComp = GetComponent<Move>();
        _moveComp.MoveEndAct = ResetPotal;
        _position = transform.position;
    }

    public void OnEnable()
    {
        ResetState();
    }

    private void Start()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _moveComp.StopMove();
        // 움직임을 멈춤
        transform.position = _moveComp.SetPosition(transform.position);
        // 충돌을 했을 때 위치 보정을 해주어야함.
    }

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    _moveComp.StopMove();
    //    // 움직임을 멈춤
    //    transform.position = _moveComp.SetPosition(transform.position);
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckCollision(collision);
    }

    void CheckCollision(Collider2D collision)
    {
        if (collision.CompareTag("OutLine")) TriggerOutLine(collision);
        else if (collision.CompareTag("EndPoint")) TriggerEndPoint(collision);
        else if (collision.CompareTag("Potal")) TriggerPotal(collision);
        else if (collision.CompareTag("Block")) TriggerBlock();
        else if (collision.CompareTag("Direction")) TriggerDirection(collision);
    }

    void TriggerDirection(Collider2D collision)
    {
        _moveComp.StopMove();
        transform.position = _moveComp.SetPosition(transform.position);
        _moveComp.ResetState();
        var dirComp = collision.GetComponent<DirectionObject>();
        MovePlayer(dirComp._dir);
    }

    void TriggerBlock()
    {
        _moveComp.StopMove();
        transform.position = _moveComp.SetPosition(transform.position);
    }

    void TriggerOutLine(Collider2D collision)
    {
        InGameManager.instance.GameOver();
        _moveComp.StopMove();
        gameObject.SetActive(false);
    }

    void TriggerEndPoint(Collider2D collision)
    {
        _moveComp.StopMove();
        if (collision.GetComponent<DirectionObject>()._dir == _moveComp.Dir)
        {
            InGameManager.instance.GameClear();
            gameObject.SetActive(false);  // 둘이 같은 경우
        }
        else
        {
            transform.position = _moveComp.SetPosition(transform.position);
        }
    }

    void TriggerPotal(Collider2D collision)
    {
        if (tempPotalOb == collision.gameObject) return;
        tempPotalOb = collision.GetComponent<PotalState>().NextObject;
        transform.position = collision.GetComponent<PotalState>().NextPosition;
    }

    public void ResetState()
    {
        transform.position = _position;
        _moveComp.ResetState();
    }

    public void MovePlayer(Direction dir)
    {
        //tempPotalOb = null;
        _moveComp.MovePlayer(dir);
    }

    public void ResetPotal()
    {
        tempPotalOb = null;
    }
}

