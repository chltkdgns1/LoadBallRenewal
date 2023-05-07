using UnityEngine;

public class EffectMoveTurn : MonoBehaviour
{
    public float moveAmount;
    public float duration;
    public float waitTime;
    public EffectLoop loops = EffectLoop.LOOP;
    private void Start()
    {
        EffectManager.MoveTurnX(gameObject, moveAmount, duration, waitTime, (int)loops);
    }
}

