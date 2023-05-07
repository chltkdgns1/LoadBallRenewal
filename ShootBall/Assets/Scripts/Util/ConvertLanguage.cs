using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public static class LanguageString
{
    public static string GetStageString()
    {
        if(ConvertLanguage.languageStatus == ConvertLanguage.Contry.KOREAN)
        {
            return "스테이지";
        }
        return "STAGE";
    }
}

public class ConvertLanguage
{
    public enum Contry
    {
        OTHER,
        KOREAN
    }

    static public Contry languageStatus = Contry.KOREAN;

    static public void SetLanguage()
    {
        if(Application.systemLanguage == SystemLanguage.Korean)
        {
            UserLocalization(Contry.KOREAN);
            languageStatus = Contry.KOREAN;
        }
        else
        {
            UserLocalization(Contry.OTHER);
            languageStatus = Contry.OTHER;
        }
    }

    static void UserLocalization(Contry country)
    {
        LocalizationSettings.SelectedLocale =
            LocalizationSettings.AvailableLocales.Locales[(int)country];
    }
}
