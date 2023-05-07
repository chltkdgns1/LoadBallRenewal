using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DefineSymbolManager
{
    static public List<string> symbolList = new List<string>();

    public static void Clear()
    {
        symbolList.Clear();
    }

    public static void SetDefaultSymbol()
    {
        // 디폴트로 넣고 싶은 심볼 넣는다.
    }

    public static void OnStatic()
    {

    }

    public static void AddSymbol(string symbol)
    {
        symbolList.Add(symbol + ";");
    }

    public static void SetSymbolSetting()
    {
#if UNITY_EDITOR
        string symbolStr = "";
        for (int i = 0; i < symbolList.Count; i++)
        {
            symbolStr += symbolList[i];
        }
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, symbolStr);
#endif
    }
}

