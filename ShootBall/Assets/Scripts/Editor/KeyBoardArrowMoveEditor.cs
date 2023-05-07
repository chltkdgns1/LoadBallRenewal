using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(KeyboardMove))]
public class KeyBoardArrowMoveEditor : Editor
{
    KeyboardMove targetOb = null;

    private void OnEnable()
    {
        targetOb = (KeyboardMove)target;
        if (targetOb == null) return;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        InputKeyboard();
    }

    void InputKeyboard()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKey(KeyCode.A))
            {
                targetOb.gameObject.transform.position
                    = SetAdjPosition(targetOb.gameObject.transform.position) + new Vector3(-0.5f, 0f);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                targetOb.gameObject.transform.position
                    = SetAdjPosition(targetOb.gameObject.transform.position) + new Vector3(0f, 0.5f);
            }
            else if (Input.GetKey(KeyCode.W))
            {
                targetOb.gameObject.transform.position
                    = SetAdjPosition(targetOb.gameObject.transform.position) + new Vector3(0f, 0.5f);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                targetOb.gameObject.transform.position
                    = SetAdjPosition(targetOb.gameObject.transform.position) + new Vector3(0f, -0.5f);
            }
        }
    }

    protected Vector3 SetAdjPosition(Vector3 pos)
    {
        float xpos = pos.x;
        float ypos = pos.y;

        float decimalX = xpos - (int)xpos;
        float decimalY = ypos - (int)ypos;

        return new Vector3(SetAdjGap(decimalX) + (int)xpos, SetAdjGap(decimalY) + (int)ypos);
    }

    protected float SetAdjGap(float dec)
    {
        if (-1 <= dec && dec < -0.5)
        {
            if (dec < -0.75f) return -1f;
            return -0.5f;
        }

        if (-0.5f <= dec && dec < 0)
        {
            if (dec < -0.25f) return -0.5f;
            return 0f;
        }

        if (0 <= dec && dec < 0.5)
        {
            if (dec < 0.25f) return 0f;
            return 0.5f;
        }

        if (0.5 <= dec && dec < 1f)
        {
            if (dec < 0.75f) return 0.5f;
            return 1f;
        }

        return dec;
    }

}
