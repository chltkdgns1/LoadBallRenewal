using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class CallBackEvent : MonoBehaviour
{
    [Serializable]
    public class CallBackValue
    {
        public GameObject targetOject;
        public MethodInfo methodInfo;
        public Type classType;
        public string compStr;
        public string methodStr;
    }

    public CallBackValue[] callBackArr;

    public Action action;

    public virtual void Awake()
    {
        InitEditValue();
    }

    public virtual void OnEnable()
    {
        //InitEditValue();
    }

    void InitEditValue()
    {
        for (int i = 0; i < callBackArr?.Length; i++)
        {
            if (callBackArr[i].compStr == null) callBackArr[i].methodStr = null;
            callBackArr[i].classType = Type.GetType(callBackArr[i].compStr);
            callBackArr[i].methodInfo = callBackArr[i].classType?.GetMethod(callBackArr[i].methodStr);
        }

        GetAction();
    }

    public void GetAction()
    {
        action = delegate { };

        for (int i = 0; i < callBackArr?.Length; i++)
        {
            if (callBackArr[i].methodInfo != null)
            {
                action += (Action)callBackArr[i].methodInfo.CreateDelegate(typeof(Action), callBackArr[i].targetOject.GetComponent(callBackArr[i].classType));
            }
        }
    }

    public void SetDirty()
    {
        if (this == null) return;
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }
}
