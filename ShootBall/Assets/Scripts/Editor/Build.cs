using System.Collections.Generic;
using UnityEditor;

public class Build
{
    public static bool IsCheckAppBundle;

    private const string APP_NAME = "LoadBall";
    const string KEYSTORE_PASSWORD = "gkftndlTek12!";
    const string BUILD_BASIC_PATH = "../../build/";
    const string BUILD_ANDROID_PATH = BUILD_BASIC_PATH + "Android";

    [MenuItem("Builder/Build/DEV/BuildForAndroidAndRun")]
    public static void BuildForAndroidRun()
    {
        SetDevSetting();

        BuildPlayerOptions buildOption = new BuildPlayerOptions();

        buildOption.locationPathName = BUILD_ANDROID_PATH + GetAppNameApk();
        buildOption.scenes = GetBuildSceneList();
        buildOption.target = BuildTarget.Android;
        buildOption.options = BuildOptions.AutoRunPlayer;

        RunBuild(buildOption);
    }

    [MenuItem("Builder/Build/DEV/BuildForAndroid")]
    public static void BuildForAndroid()
    {
        SetDevSetting();

        BuildPlayerOptions buildOption = new BuildPlayerOptions();

        buildOption.locationPathName = BUILD_ANDROID_PATH + GetAppNameApk();
        buildOption.scenes = GetBuildSceneList();
        buildOption.target = BuildTarget.Android;

        RunBuild(buildOption);
    }

    [MenuItem("Builder/Build/DEV/BuildForAndroidOnBlueStack")]
    public static void BuildForAndroidOnBlueStack()
    {
        SetDevSetting();

        BuildPlayerOptions buildOption = new BuildPlayerOptions();

        buildOption.locationPathName = BUILD_ANDROID_PATH + GetAppNameApk();
        buildOption.scenes = GetBuildSceneList();
        buildOption.target = BuildTarget.Android;

        RunBuild(buildOption);
    }


    [MenuItem("Builder/Build/ALPHA/BuildForAndroidGooglePlayInnerTest")]
    public static void BuildForAndroidGooglePlayInnerTest()
    {
        SetAlphaSetting();

        BuildPlayerOptions buildOption = new BuildPlayerOptions();
        buildOption.locationPathName = BUILD_ANDROID_PATH + GetAppNameAab();
        buildOption.scenes = GetBuildSceneList();
        buildOption.target = BuildTarget.Android;

        RunBuild(buildOption);
    }

    [MenuItem("Builder/Build/REAL/BuildForAndroidGooglePlay")]
    public static void BuildForAndroidGooglePlay()
    {
        SetRealSetting();

        BuildPlayerOptions buildOption = new BuildPlayerOptions();
        buildOption.locationPathName = BUILD_ANDROID_PATH + GetAppNameAab();
        buildOption.scenes = GetBuildSceneList();
        buildOption.target = BuildTarget.Android;

        RunBuild(buildOption);
    }


    static void RunBuild(BuildPlayerOptions buildOption)
    {
        DefineSymbolManager.SetSymbolSetting();
        BuildPipeline.BuildPlayer(buildOption);
    }

    static string[] GetBuildSceneList()
    {
        EditorBuildSettingsScene[] scene = UnityEditor.EditorBuildSettings.scenes;
        List<string> listPath = new List<string>();

        for (int i = 0; i < scene.Length; i++)
        {
            if (scene[i].enabled)
                listPath.Add(scene[i].path);
        }

        return listPath.ToArray();
    }

    static void SetPlayerSettings()
    {
        PlayerSettings.Android.keystorePass = KEYSTORE_PASSWORD;
        PlayerSettings.Android.keyaliasPass = KEYSTORE_PASSWORD;
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
    }

    static void VersionUp()
    {
        PlayerSettings.Android.bundleVersionCode++;
        List<int> version = splitVersion(PlayerSettings.bundleVersion, '.');
        VersionUp(version);
        PlayerSettings.bundleVersion = CombinationVersion(version);
    }

    static string GetAppNameApk()
    {
        return string.Format("{0}_{1}.apk", APP_NAME, PlayerSettings.bundleVersion);
    }

    static string GetAppNameAab()
    {
        return string.Format("{0}_{1}.aab", APP_NAME, PlayerSettings.bundleVersion);
    }

    static void SetDevSetting()
    {
        SetPlayerSettings();

        EditorUserBuildSettings.buildAppBundle = false;
        PlayerSettings.Android.optimizedFramePacing = false;

        DefineSymbolManager.Clear();
        DefineSymbolManager.AddSymbol("DEV");
    }

    static void SetAlphaSetting()
    {
        SetPlayerSettings();

        EditorUserBuildSettings.buildAppBundle = true;
        PlayerSettings.Android.optimizedFramePacing = false;

        VersionUp();

        DefineSymbolManager.Clear();
        DefineSymbolManager.AddSymbol("ALPHA");
    }

    static void SetRealSetting()
    {
        SetPlayerSettings();

        EditorUserBuildSettings.buildAppBundle = true;
        PlayerSettings.Android.optimizedFramePacing = false;

        VersionUp();

        DefineSymbolManager.Clear();
        DefineSymbolManager.AddSymbol("REAL");
    }

    static void VersionUp(List<int> versionList)
    {
        int backIndex = versionList.Count - 1;
        int upCnt = 1;
        for (int i = backIndex; i >= 0; i--)
        {
            versionList[i] += upCnt;

            upCnt = versionList[i] / 10;
            versionList[i] %= 10;

            if (upCnt == 0) break;
        }
    }

    static string CombinationVersion(List<int> list)
    {
        string version = "";
        for (int i = 0; i < list.Count; i++)
        {
            version += list[i];
            if (i == list.Count - 1) break;
            version += '.';
        }
        return version;
    }

    static List<int> splitVersion(string str, char ch)
    {
        string temp = "";
        List<int> list = new List<int>();
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == ch)
            {
                list.Add(int.Parse(temp));
                temp = "";
                continue;
            }
            temp += str[i];
        }
        if (string.IsNullOrEmpty(temp) == false)
        {
            list.Add(int.Parse(temp));
        }
        return list;
    }
}

