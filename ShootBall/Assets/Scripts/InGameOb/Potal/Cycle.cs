using UnityEngine;

public class Cycle : MonoBehaviour
{
    public float rotationScale;
    public float relayTime;
    float _time;
    void Update()
    {
        if (_time > relayTime)
        {
            _time = 0f;
            transform.Rotate(new Vector3(0, 0, rotationScale));
        }

        _time += Time.deltaTime;
    }
}
