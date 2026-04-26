//using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public delegate void AdmobRewardUserDelegate(object sender, Reward args);

public enum AdmobGAEvents
{
    Initializing,
    Initialized,

    LoadInterstitialAd,
    LoadVideoAd,
    LoadRewardedAd,
    InterstitialAdLoaded,
    VideoAdLoaded,
    RewardedAdLoaded,
    ShowInterstitialAd,
    ShowVideoAd,
    ShowRewardedAd,
    InterstitialAdWillDisplay,
    VideoAdWillDisplay,
    RewardedAdWillDisplay,
    InterstitialAdDisplayed,
    VideoAdDisplayed,
    RewardedAdDisplayed,
    InterstitialAdNoInventory,
    VideoAdNoInventory,
    RewardedAdNoInventory,

    RewardedAdStarted,

    RewardedAdReward,

    InterstitialAdClicked,
    VideoAdClicked,
    RewardedAdClicked,

    InterstitialAdClosed,
    VideoAdClosed,
    RewardedAdClosed,

    AdaptersInitialized,
    AdaptersNotInitialized,

    RewardedAdFailedToShow,
    LoadSmallBanner,
    LoadMediumBanner,
    ShowSmallBanner,
    ShowMediumBanner,
    SmallBannerLoaded,
    MediumBannerLoaded,
    SmallBannerNoInventory,
    MediumBannerNoInventory,
    SmallBannerDisplayed,
    MediumBannerDisplayed,
    SmallBannerClicked,
    MediumBannerClicked,
    SmallBannerClosed,
    MediumBannerClosed
}
