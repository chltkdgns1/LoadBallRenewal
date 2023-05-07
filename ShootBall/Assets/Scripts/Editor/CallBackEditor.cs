using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CallBackEvent), true)]
public class CallBackEditor : Editor
{
    List<BaseEditor> baseEdit = new List<BaseEditor>();
    CallBackEvent callBackTarget;

    string btnId;

    private void Awake()
    {

    }

    private void OnEnable()
    {
        callBackTarget = (CallBackEvent)target;
        if (callBackTarget == null) return;

        Init();
        Reset();
        Read();
        InitEnable();
        GetCompMethodValue();
    }

    void InitEnable()
    {
        for (int i = 0; i < baseEdit.Count; i++)
        {
            baseEdit[i].InitEnable(callBackTarget.callBackArr[i].targetOject);
        }
    }

    void Read()
    {
        for (int i = 0; i < baseEdit.Count; i++)
        {
            baseEdit[i].Read(callBackTarget.callBackArr[i].compStr, callBackTarget.callBackArr[i].methodStr);
        }
    }

    void Init()
    {
        baseEdit.Clear();

        for (int i = 0; i < callBackTarget.callBackArr?.Length; i++)
        {
            baseEdit.Add(new BaseEditor());
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < callBackTarget.callBackArr?.Length; i++)
        {
            if (callBackTarget.callBackArr[i].targetOject == null)
                baseEdit[i].ResetStr();
            baseEdit[i].Write(ref callBackTarget.callBackArr[i].compStr, ref callBackTarget.callBackArr[i].methodStr);
        }
        callBackTarget.SetDirty();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (callBackTarget.callBackArr == null)
        {
            return;
        }

        if (callBackTarget.callBackArr?.Length != baseEdit.Count)
        {
            SetSameSize();
            return;
        }

        for (int i = 0; i < callBackTarget.callBackArr?.Length; i++)
        {
            GameObject ob = callBackTarget.callBackArr[i].targetOject;
            baseEdit[i].PrintCompList(ob);
        }
    }

    void SetSameSize()
    {
        if (callBackTarget.callBackArr == null)
        {
            return;
        }

        if (callBackTarget.callBackArr.Length < baseEdit.Count)
        {
            int diff = baseEdit.Count - callBackTarget.callBackArr.Length;
            for (int i = 0; i < diff; i++)
            {
                baseEdit.RemoveAt(baseEdit.Count - 1);
            }
        }
        else
        {
            int diff = callBackTarget.callBackArr.Length - baseEdit.Count;
            for (int i = 0; i < diff; i++)
            {
                baseEdit.Add(new BaseEditor());
            }
        }
    }

    void Reset()
    {
        for (int i = 0; i < baseEdit.Count; i++)
            baseEdit[i].Reset();
    }

    void GetCompMethodValue()
    {
        for (int i = 0; i < callBackTarget.callBackArr?.Length; i++)
        {
            callBackTarget.callBackArr[i].classType = baseEdit[i].compType;
            callBackTarget.callBackArr[i].methodInfo = baseEdit[i].methodInfo;
        }
    }
}
