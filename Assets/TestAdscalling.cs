using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAdscalling : MonoBehaviour
{
   public void showAds_fun()
    {
        AdmobAdsManager.Instance.ShowInterstitial();
    }
}
