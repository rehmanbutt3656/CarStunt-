using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerAdsLoads : MonoBehaviour
{
    
    void Start()
    {
        // AdManager.ShowBanner(BannerAdPosition.TOP);
        //AdManager.ShowBanner_mitRect(BannerAdPosition.BOTTOM);
        //AdsManagerAR.Instance.RequestBannerAd();
        //TechnologyWings.AdManager.ShowBanner(TechnologyWings.BannerAdType.BANNER,TechnologyWings.BannerAdPosition.TOP_LEFT);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
           // AdManager.HideBanner_mitrect();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
          //  AdManager.ShowBanner_mitRect(BannerAdPosition.BOTTOM);
        }
    }
}
