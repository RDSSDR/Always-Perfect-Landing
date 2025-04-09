using Landfall.Haste;
using Landfall.Modding;
using System.Reflection;
using UnityEngine;
using UnityEngine.Localization;
using Zorro.Settings;

namespace APL;

[LandfallPlugin]
public class Program
{
    static Program()
    { 
        On.PlayerMovement.GetLanding += PlayerMovement_GetLanding;
    }

    private static object PlayerMovement_GetLanding(On.PlayerMovement.orig_GetLanding orig, PlayerMovement self, RaycastHit hit)
    {
        var landing = orig(self, hit);
        if (!GameHandler.Instance.SettingsHandler.GetSetting<PerfectLandingSetting>().Value)
        {
            return landing;
        }

        Type landingType = landing.GetType();
        FieldInfo scoreField = landingType.GetField("landingScore", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        scoreField.SetValue(landing, 1f);
        return landing;
    }
}

[HasteSetting]
public class PerfectLandingSetting : BoolSetting, IExposedSetting
{
    public override void ApplyValue() => Debug.Log($"Mod apply value {Value}");
    protected override bool GetDefaultValue() => true;
    public LocalizedString GetDisplayName() => new UnlocalizedString("Always Perfect Landing");
    public override LocalizedString OffString => new UnlocalizedString("Off");
    public override LocalizedString OnString => new UnlocalizedString("On");
    public string GetCategory() => SettingCategory.Difficulty;
}