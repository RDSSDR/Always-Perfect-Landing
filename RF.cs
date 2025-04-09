using Landfall.Haste;
using Landfall.Modding;
using System.Reflection;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Rendering.PostProcessing;
using Zorro.Settings;
using Object = UnityEngine.Object;

namespace RF;

[LandfallPlugin]
public class Program
{
    static Program()
    {
        On.PlayerCharacter.Awake += PlayerCharacter_Awake;
    }

    private static void PlayerCharacter_Awake(On.PlayerCharacter.orig_Awake orig, PlayerCharacter self)
    {
        orig(self);
        Debug.Log("PlayerCharacter Awake called");
        RemoveFog();
    }

    public static void RemoveFog()
    {
        PostProcessVolume[] volumes = GameObject.FindObjectsOfType<PostProcessVolume>();

        foreach (PostProcessVolume volume in volumes)
        {
            Debug.Log("PostProcessVolume found: " + volume.gameObject.name);
            // Check if the post-process effect matches the type
            var settings = volume.profile;

            if (settings != null && settings.TryGetSettings<TerrainHeightFadePPSSettings>(out var setting))
            {
                setting.active = false; 
                Debug.Log("Found PostProcessVolume with effect: " + volume.gameObject.name);
            }
        }
    }
}