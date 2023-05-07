using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Positioning))]
public class AdJPositionEditor : Editor
{
    Positioning targetOb = null;

    private void OnEnable()
    {
        targetOb = (Positioning)target;
        if (targetOb == null) return;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (targetOb == null) return;
        targetOb.gameObject.transform.position = SetAdjPosition(targetOb.gameObject.transform.position);
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

