using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class BaseEditor // 메서드 가져오는데 최적화되어있음.
{
    public List<Type> compList = new List<Type>();
    public List<string> compListStr = new List<string>();

    public List<string> methodList = new List<string>();
    public List<MethodInfo> methodInfoList = new List<MethodInfo>();

    public Type compType;
    public MethodInfo methodInfo;

    int localIndex = 0;
    int localMethodIndex = 0;

    string compStr;
    string methodStr;

    public void Read(string comp, string method)
    {
        compStr = comp;
        methodStr = method;
    }

    public void Write(ref string comp, ref string method)
    {
        comp = compStr;
        method = methodStr;
    }

    public void Reset()
    {
        ResetStr();
        localIndex = 0;
        localMethodIndex = 0;
    }

    public void ResetStr()
    {
        compStr = "null";
        methodStr = "null";
    }

    public void Clear()
    {
        compList.Clear();
        compListStr.Clear();
        methodList.Clear();
        methodInfoList.Clear();
    }

    void GetCompList(GameObject ob)
    {
        Component[] temp = ob.GetComponents<Component>();
        compList.Add(null);
        compListStr.Add("null");
        for (int i = 0; i < temp.Length; i++)
        {
            compList.Add(temp[i].GetType());
            compListStr.Add(temp[i].GetType().ToString());
        }
    }

    int FindCompIndex()
    {
        for (int i = 0; i < compList.Count; i++)
        {
            if (compListStr[i] == compStr)
            {
                return i;
            }
        }
        return 0;
    }

    void GetMethod(Type type)
    {
        methodInfoList.Add(null);
        methodList.Add("null");

        if (type == null) return;

        foreach (var method in type.GetMethods())
        {
            if (method.IsPublic)
            {
                methodList.Add(method.Name);
                methodInfoList.Add(method);
            }
        }
    }

    int FindMethodIndex()
    {
        for (int i = 0; i < methodInfoList.Count; i++)
        {
            if (methodList[i] == methodStr) return i;
        }
        return 0;
    }


    bool IsNullTarget(GameObject ob)
    {
        if (ob == null)
        {
            Reset();
            return true;
        }
        return false;
    }

    public void InitEnable(GameObject ob)
    {
        if (IsNullTarget(ob)) return;

        Clear();
        GetCompList(ob);
        localIndex = FindCompIndex();

        compType = compList[localIndex];

        compStr = compType?.ToString();

        GetMethod(compType);
        localMethodIndex = FindMethodIndex();

        methodInfo = methodInfoList[localMethodIndex];

        methodStr = methodInfo?.Name;
    }

    public void PrintCompList(GameObject ob)
    {
        if (IsNullTarget(ob)) return;

        Clear();
        GetCompList(ob);
        localIndex = FindCompIndex();

        PrintField("Components", GUILayout.Width(75f));
        PrintPopup(ref localIndex, compListStr, GUILayout.Width(300f));

        compType = compList[localIndex];
        PrintField("컴포넌트 타입 : " + compType, GUILayout.Width(500f));

        compStr = compType?.ToString();
        GetMethod(compType);
        localMethodIndex = FindMethodIndex();

        PrintPopup(ref localMethodIndex, methodList, GUILayout.Width(300f));

        methodInfo = methodInfoList[localMethodIndex];

        PrintField("매소드 인포 : " + methodInfo, GUILayout.Width(500));

        methodStr = methodInfo?.Name;
    }

    void PrintField(string message, GUILayoutOption option)
    {
        EditorGUILayout.LabelField(message, option);
    }

    void PrintPopup(ref int index, List<string> arr, GUILayoutOption option)
    {
        index = EditorGUILayout.Popup(index, arr.ToArray(), option);
    }
}
