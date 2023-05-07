using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DirectionObject), true)]
public class ObjectDirection : Editor
{
    DirectionObject targetOb = null;

    private void OnEnable()
    {
        targetOb = (DirectionObject)target;
        if (targetOb == null) return;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        targetOb.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, SetDir()));
    }

    float SetDir()
    {
        return targetOb._rotationArr[(int)targetOb._dir];
    }
}
