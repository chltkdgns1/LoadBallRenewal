using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SettingSceneAtlas : MonoBehaviour
{
    public void Reset()
    {
#if UNITY_EDITOR
        List<string> scenePaths = new List<string>();
        foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
        {
            if (S.enabled)
                scenePaths.Add(S.path);
        }
        string changeString = "    texture: {fileID: 0}";
        foreach (string str in scenePaths)
        {
            string tempPath = Application.dataPath.Replace("Assets", "") + str;
            string[] allLines = File.ReadAllLines(tempPath);
            for (int i = 0; i < allLines.Length; ++i)
            {
                string lineStr = allLines[i];
                if (lineStr.Contains("fileID:") && lineStr.Contains("guid: 00000000000000000000000000000000"))
                {
                    allLines[i] = changeString;
                }
            }
            File.WriteAllLines(tempPath, allLines);
        }
#endif
    }
}
