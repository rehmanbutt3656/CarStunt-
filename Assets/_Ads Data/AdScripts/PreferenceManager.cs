using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreferenceManager : MonoBehaviour
{
    const string RemoveAds = "RemoveAds";
    public static bool GetAdsStatus()
    {
        //0 means ads enabled and 1 means ads are disabled. so true means ads are enabled and false means ads are disabled.
        
        return (PlayerPrefs.GetInt(RemoveAds, 0) == 0);
    }

    public static void SetAdsStatus(int value)
    {
        PlayerPrefs.SetInt(RemoveAds, value);
    }
}
