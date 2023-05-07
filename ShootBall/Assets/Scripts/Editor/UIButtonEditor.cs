using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIButton), true)]
public class UIButtonEditor : Editor
{
    BaseEditor baseEdit = new BaseEditor();
    UIButton uiBtnTarget;

    private void Awake()
    {

    }

    private void OnEnable()
    {
        uiBtnTarget = (UIButton)target;
        if (uiBtnTarget == null) return;
        Reset();

        baseEdit.Read(uiBtnTarget.compStr, uiBtnTarget.methodStr);

        baseEdit.InitEnable(uiBtnTarget.targetOject);
        GetCompMethodValue();
    }

    private void OnDisable()
    {
        if (uiBtnTarget.targetOject == null)
        {
            baseEdit.ResetStr();
        }

        baseEdit.Write(ref uiBtnTarget.compStr, ref uiBtnTarget.methodStr);
        uiBtnTarget.SetDirty();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GameObject ob = uiBtnTarget.targetOject;
        baseEdit.PrintCompList(ob);
        GetCompMethodValue();
    }

    void Reset()
    {
        baseEdit.Reset();
    }

    void GetCompMethodValue()
    {
        uiBtnTarget.classType = baseEdit.compType;
        uiBtnTarget.methodInfo = baseEdit.methodInfo;
    }
}
