using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestAdsCalling : MonoBehaviour
{
    private MediationHandler mediationHandler;

    public Text cashText;

    void Start()
    {
        mediationHandler = FindObjectOfType<MediationHandler>();
        AdmobAdsManager.Instance.ShowSmallBanner(GoogleMobileAds.Api.AdPosition.Top);
        AdmobAdsManager.Instance.ShowMediumBanner(GoogleMobileAds.Api.AdPosition.Bottom);
    }

    private void Update()
    {
        cashText.text = PlayerPrefs.GetInt("CASH").ToString("F0");
    }

    public void LoadInterstitial()
    {
        AdmobAdsManager.Instance.LoadInterstitial();
    }

    public void ShowInterstitial()
    {
        AdmobAdsManager.Instance.ShowInterstitial();
    }

    public void LoadVideo()
    {
        AdmobAdsManager.Instance.LoadVideo();
    }

    public void ShowVideo()
    {
        AdmobAdsManager.Instance.ShowVideo();
    }

    public void LoadRewarded()
    {
        AdmobAdsManager.Instance.LoadRewardedVideo();
    }
    
    public void GiveCoinsWithVideo()
    {
        mediationHandler.ShowRewardedVideo(GiveReward);
    }
    void GiveReward()
    {
        PlayerPrefs.SetInt("CASH", PlayerPrefs.GetInt("CASH") + 100);
    }
}

