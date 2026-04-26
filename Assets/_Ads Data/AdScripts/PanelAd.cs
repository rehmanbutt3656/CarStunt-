using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelAd : MonoBehaviour
{
    public enum Bannertype
    {
        SmallBanner,
        BigBanner,
    }

    [SerializeField] private Bannertype bannerType;
    [SerializeField] private GoogleMobileAds.Api.AdPosition adPosition;

    private void OnEnable()
    {
        if (!AdmobAdsManager.Instance) return;

        if (bannerType == Bannertype.SmallBanner)
        {
            AdmobAdsManager.Instance.ShowSmallBanner(adPosition);
        }
        else
        {
            AdmobAdsManager.Instance.ShowMediumBanner(adPosition);
        }
    }

    private void OnDisable()
    {
        if (!AdmobAdsManager.Instance) return;

        if (bannerType == Bannertype.SmallBanner)
        {
            AdmobAdsManager.Instance.HideSmallBannerEvent();
        }
        else
        {
            AdmobAdsManager.Instance.HideMediumBannerEvent();
        }
    }

    private void OnDestroy()
    {
        if (!AdmobAdsManager.Instance) return;

        if (bannerType == Bannertype.SmallBanner)
        {
            AdmobAdsManager.Instance.HideSmallBannerEvent();
        }
        else
        {
            AdmobAdsManager.Instance.HideMediumBannerEvent();
        }
    }
}
