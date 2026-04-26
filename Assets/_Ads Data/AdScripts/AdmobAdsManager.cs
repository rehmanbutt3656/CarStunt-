using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds.Api;
using GoogleMobileAds.Mediation;
using GoogleMobileAds.Api.Mediation.UnityAds;
using GoogleMobileAds.Common;

//using Facebook.Unity;

[Serializable]
public class AdmobId
{
    public string ADMOB_APP_ID;
    public string ADMOB_INTERTITIAL_AD_ID, /*ADMOB_VIDEO_AD_ID,*/ ADMOB_BANNER_AD_ID, ADMOB_BIG_BANNER_AD_ID, ADMOB_REWARDED_AD_ID;
}

public class AdmobAdsManager : MediationHandler
{
    public static AdmobAdsManager Instance;

    public bool enableTestAds;

    private static RewardUserDelegate NotifyReward;

    public AdmobId AndroidAdmob_ID = new AdmobId();
    public AdmobId IosAndroid_ID = new AdmobId();
    public AdmobId TestAdmob_ID = new AdmobId();

    [HideInInspector] public AdmobId ADMOB_ID = new AdmobId();
    [HideInInspector] public InterstitialAd interstitial;
   //[HideInInspector] public InterstitialAd videoAd;
    [HideInInspector] public RewardedAd rewardBasedVideo;

    [HideInInspector]
    public BannerView SmallBanner;
    [HideInInspector]
    public BannerView MediumBanner;


    bool isAdmobInitialized = false;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        if (enableTestAds)
        {
            ADMOB_ID = TestAdmob_ID;
        }
        else
        {
#if UNITY_ANDROID
            ADMOB_ID = AndroidAdmob_ID;
#elif UNITY_IOS
        ADMOB_ID = IosAndroid_ID;
#endif
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        AdmobGA_Helper.GA_Log(AdmobGAEvents.Initializing);
        MobileAds.Initialize((initStatus) =>
        {
            Debug.Log("GG >> Admob:Initialized");
            AdmobGA_Helper.GA_Log(AdmobGAEvents.Initialized);

            Dictionary<string, AdapterStatus> map = initStatus.getAdapterStatusMap();
            foreach (KeyValuePair<string, AdapterStatus> keyValuePair in map)
            {
                string className = keyValuePair.Key;
                AdapterStatus status = keyValuePair.Value;
                switch (status.InitializationState)
                {
                    case AdapterState.NotReady:
                        // The adapter initialization did not complete.

                        Debug.Log("GG >> Adapter: " + status.Description + " not ready.Name=" + className);

                        //      Debug.Log("Adapert is :: "+(AdmobGAEvents.AdaptersNotInitialized) + className);
                        AdmobGA_Helper.GA_Log(AdmobGAEvents.AdaptersNotInitialized);
                        break;
                    case AdapterState.Ready:
                        // The adapter was successfully initialized.
                        Debug.Log("GG >> Adapter: " + className + " is initialized.");

#if UNITY_ANDROID
                        MediationAdapterConsent(className);
#endif

                        break;
                }
            }

            isAdmobInitialized = true;
            CreateAdsObjects();
            BindAdsEvent();

            //Ad Lods
            LoadInterstitial();
            LoadSmallBanner();
            LoadMediumBanner();
            LoadRewardedVideo();
        });
#if UNITY_IOS
        MobileAds.SetiOSAppPauseOnBackground(true);
#endif
    }

    /// <summary>
    /// Send User Consent in Open Bidding Adapters Consent
    /// </summary>
    void MediationAdapterConsent(string AdapterClassname)
    {
        if (AdapterClassname.Contains("Unity"))
        {
            //UnityAds Consent
            UnityAds.SetGDPRConsentMetaData(true);
            Debug.Log("GG >> UnityAds consent is send");
            Constant.LogDesignEvent("Admob:Consent:UnityAds");
        }
       
    }

    /// <summary>
    /// Create Ads objects.
    /// </summary>
    private void CreateAdsObjects()
    {
        this.interstitial = new InterstitialAd(ADMOB_ID.ADMOB_INTERTITIAL_AD_ID);
        //this.videoAd = new InterstitialAd(ADMOB_ID.ADMOB_VIDEO_AD_ID);
        this.rewardBasedVideo = new RewardedAd(ADMOB_ID.ADMOB_REWARDED_AD_ID);
        this.SmallBanner = new BannerView(ADMOB_ID.ADMOB_BANNER_AD_ID, AdSize.SmartBanner, AdPosition.Center);
        this.MediumBanner = new BannerView(ADMOB_ID.ADMOB_BIG_BANNER_AD_ID, AdSize.MediumRectangle, AdPosition.TopLeft);
    }

    #region Ads Events Bind

    private void BindSmallBannerEvents()
    {

        // INTERSTITIAL EVENTS...//

        this.SmallBanner.OnAdLoaded += SmallBanner_HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.SmallBanner.OnAdFailedToLoad += SmallBanner_HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.SmallBanner.OnAdOpening += SmallBanner_HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        this.SmallBanner.OnAdClosed += SmallBanner_HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        //this.SmallBanner.OnAdLeavingApplication += SmallBanner_HandleOnAdLeavingApplication;
    }

    private void BindMediumBannerEvents()
    {

        // INTERSTITIAL EVENTS...//

        this.MediumBanner.OnAdLoaded += MediumBanner_HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.MediumBanner.OnAdFailedToLoad += MediumBanner_HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.MediumBanner.OnAdOpening += MediumBanner_HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        this.SmallBanner.OnAdClosed += MediumBanner_HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        //this.MediumBanner.OnAdLeavingApplication += MediumBanner_HandleOnAdLeavingApplication;
    }
    private void BindIntertitialEvents()
    {
        // INTERSTITIAL EVENTS...//

        this.interstitial.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;

        // Called when an ad is shown.
        this.interstitial.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        //this.interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;
    }

    private void BindVideoEvents()
    {
        //// VIDEO AD EVENTS...//

        //this.videoAd.OnAdLoaded += Video_HandleOnAdLoaded;
        //// Called when an ad request failed to load.
        //this.videoAd.OnAdFailedToLoad += Video_HandleOnAdFailedToLoad;

        //// Called when an ad is shown.
        //this.videoAd.OnAdOpening += Video_HandleOnAdOpened;
        //// Called when the ad is closed.
        //this.videoAd.OnAdClosed += Video_HandleOnAdClosed;
        //// Called when the ad click caused the user to leave the application.
        ////this.videoAd.OnAdLeavingApplication += Video_HandleOnAdLeavingApplication;
    }

    private void BindRewardedEvents()
    {
        //.....REWARDED ADS EVENTS.......//
        //// Get singleton reward based video ad reference.
        //this.rewardBasedVideo = RewardBasedVideoAd.Instance;

        // Called when an ad request has successfully loaded.
        rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
        // Called when an ad request failed to load.
        rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
        // Called when an ad is shown.
        rewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;
        // Called when the ad starts to play.
        //rewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;

        rewardBasedVideo.OnAdFailedToShow += HandleRewardedAdFailedToShow;


        // Called when the user should be rewarded for watching a video.
        rewardBasedVideo.OnUserEarnedReward += HandleRewardBasedVideoRewarded;
        // Called when the ad is closed.
        rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
        // Called when the ad click caused the user to leave the application.
        //rewardBasedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;
    }

    /// <summary>
    /// Bind Ads events to receive Ads Data.
    /// </summary>
    private void BindAdsEvent()
    {
        BindIntertitialEvents();
        //BindVideoEvents();
        BindRewardedEvents();
        BindSmallBannerEvents();
        BindMediumBannerEvents();
    }

    #endregion

    #region Load Ads

    public void LoadSmallBanner()
    {
        if (sbAdStatus == AdLoadingStatus.Loading || !PreferenceManager.GetAdsStatus() || Constants.adsRemoteConfigStatus == "0" /*|| IsSmallBannerReady()*/)
        {
            return;
        }
        if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork | Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            Debug.Log("GG >> Admob:smallBanner:LoadRequest");

            AdmobGA_Helper.GA_Log(AdmobGAEvents.LoadSmallBanner);
            this.SmallBanner = new BannerView(ADMOB_ID.ADMOB_BANNER_AD_ID, AdSize.Banner, AdPosition.Top);
            BindSmallBannerEvents();
            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();

            // Load the banner with the request.
            this.SmallBanner.LoadAd(request);
            this.SmallBanner.Hide();
            sbAdStatus = AdLoadingStatus.Loading;
        }
    }

    public void LoadMediumBanner()
    {
        if (mbAdStatus == AdLoadingStatus.Loading || !PreferenceManager.GetAdsStatus() || Constants.adsRemoteConfigStatus == "0" /*|| IsMediumBannerReady()*/)
        {
            return;
        }
        if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork | Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            Debug.Log("GG >> Admob:mediumBanner:LoadRequest");
            AdmobGA_Helper.GA_Log(AdmobGAEvents.LoadMediumBanner);
            this.MediumBanner = new BannerView(ADMOB_ID.ADMOB_BIG_BANNER_AD_ID, AdSize.MediumRectangle, AdPosition.TopLeft);
            BindMediumBannerEvents();
            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();

            // Load the banner with the request.
            this.MediumBanner.LoadAd(request);
            this.MediumBanner.Hide();
            mbAdStatus = AdLoadingStatus.Loading;
        }
    }

    /// <summary>
    /// Load Interstitial Ad
    /// </summary>
    public override void LoadInterstitial()
    {
        if (!isAdmobInitialized || IsInterstitialAdReady() || iAdStatus == AdLoadingStatus.Loading ||
            !PreferenceManager.GetAdsStatus() || Constants.adsRemoteConfigStatus == "0")
        {
            return;
        }

        if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork |
            Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            Debug.Log("GG >> Admob:iad:LoadRequest");
            AdmobGA_Helper.GA_Log(AdmobGAEvents.LoadInterstitialAd);

            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the interstitial with the request.
            this.interstitial.LoadAd(request);
            iAdStatus = AdLoadingStatus.Loading;
        }
    }

    /// <summary>
    /// Load Video Ad
    /// </summary>
    public override void LoadVideo()
    {
        //if (!isAdmobInitialized || IsVideoAdReady() || vAdStatus == AdLoadingStatus.Loading ||
        //    !PreferenceManager.GetAdsStatus() || Constants.adsRemoteConfigStatus == "0")
        //{
        //    return;
        //}

        //if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork |
        //    Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        //{
        //    Debug.Log("GG >> Admob:vad:LoadRequest");
        //    AdmobGA_Helper.GA_Log(AdmobGAEvents.LoadVideoAd);

        //    // Create an empty ad request.
        //    AdRequest request = new AdRequest.Builder().Build();
        //    // Load the interstitial with the request.
        //    this.videoAd.LoadAd(request);
        //    vAdStatus = AdLoadingStatus.Loading;
        //}
    }

    /// <summary>
    /// Load Rewarded Ad
    /// </summary>
    public override void LoadRewardedVideo()
    {
        if (!isAdmobInitialized || IsRewardedAdReady() || rAdStatus == AdLoadingStatus.Loading ||
            Constants.adsRemoteConfigStatus == "0")
        {
            return;
        }

        if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork |
            Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            Debug.Log("GG >> Admob:rad:LoadRequest");
            AdmobGA_Helper.GA_Log(AdmobGAEvents.LoadRewardedAd);

            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the rewarded video ad with the request.
            this.rewardBasedVideo.LoadAd(request);
            rAdStatus = AdLoadingStatus.Loading;
        }
    }
    //public bool IsSmallBannerReady()
    //{
    //    return isSmallBannerLoaded;
    //}

    //public bool IsMediumBannerReady()
    //{
    //    return isMediumBannerLoaded;
    //}
    /// <summary>
    /// Check is iAd already loaded
    /// </summary>
    //public bool IsInterstitialAdReady()
    //{
    //    return this.interstitial.IsLoaded();
    //}

    ///// <summary>
    ///// Check is vAd already loaded
    ///// </summary>
    //public bool IsVideoAdReady()
    //{
    //    return this.videoAd.IsLoaded();
    //}

    /// <summary>
    /// Check is rAd already loaded
    /// </summary>
    public override bool IsRewardedAdReady()
    {
        if (this.rewardBasedVideo != null)
            return this.rewardBasedVideo.IsLoaded();
        return false;
    }

    #endregion

    #region Show Ads

    public void HideSmallBannerEvent()
    {
        if (this.SmallBanner != null)
        {
#if UNITY_EDITOR
            Debug.Log("GG >> Admob:smallBanner:Hide");
#endif
            this.SmallBanner.Hide();
        }
    }

    public void HideMediumBannerEvent()
    {
        if (this.MediumBanner != null)
        {
#if UNITY_EDITOR
            Debug.Log("GG >> Admob:mediumBanner:Hide");
#endif
            this.MediumBanner.Hide();
        }
    }

    public void ShowSmallBanner(AdPosition position)
    {
        if (!PreferenceManager.GetAdsStatus() || !isAdmobInitialized || Constants.adsRemoteConfigStatus != "1")
        {
            return;
        }
#if UNITY_EDITOR
        Debug.Log("GG >> Admob:smallBanner:ShowCall");
#endif
        AdmobGA_Helper.GA_Log(AdmobGAEvents.ShowSmallBanner);
        if (SmallBanner != null)
        {
#if UNITY_EDITOR
            Debug.Log("GG >> Admob:smallBanner:WillDisplay");
#endif
            this.SmallBanner.Hide();

            this.SmallBanner.Show();
            this.SmallBanner.SetPosition(position);
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("GG >> Admob:smallBanner:AdNotLoaded");
#endif
            LoadSmallBanner();
        }
    }

    public void ShowMediumBanner(AdPosition position)
    {
        try
        {

            if (!PreferenceManager.GetAdsStatus() || !isAdmobInitialized || Constants.adsRemoteConfigStatus != "1")
            {
                return;
            }
#if UNITY_EDITOR
            Debug.Log("GG >> Admob:mediumBanner:ShowCall");
#endif
            AdmobGA_Helper.GA_Log(AdmobGAEvents.ShowMediumBanner);
            if (MediumBanner != null)
            {
#if UNITY_EDITOR
                Debug.Log("GG >> Admob:mediumBanner:WillDisplay");
#endif
                this.MediumBanner.Hide();
                this.MediumBanner.Show();
                this.MediumBanner.SetPosition(position);
            }
            else
            {
                Debug.Log("GG >> Admob:mediumBanner:AdNotLoaded");
                LoadMediumBanner();
            }
            //Debug.Log("GG >> Admob:smallBanner:Displayed");
        }
        catch (Exception e)
        {
            Debug.Log("Error: " + e);
        }
    }

    /// <summary>
    /// Show Interstitial Ad
    /// </summary>
    public override void ShowInterstitial()
    {
#if UNITY_EDITOR
        Debug.Log("GG >> Admob:iad:ShowCall");
#endif
        AdmobGA_Helper.GA_Log(AdmobGAEvents.ShowInterstitialAd);
        if (this.interstitial != null)
        {
            if (this.interstitial.IsLoaded())
            {

#if UNITY_EDITOR
                Debug.Log("GG >> Admob:iad:WillDisplay");
#endif
                AdmobGA_Helper.GA_Log(AdmobGAEvents.InterstitialAdWillDisplay);

                this.interstitial.Show();
            }

            //else
            //{
            //    Debug.Log("GG >> Admob:iad:NotLoaded");
            //    LoadInterstitial();
            //}
        }
    }

    public override bool IsInterstitialAdReady()
    {
        if (this.interstitial != null)
            return this.interstitial.IsLoaded();
        else
            return false;
    }

    //private IEnumerator ShowInterstitialWithDelay(float delay)
    //{
    //    yield return new WaitForSecondsRealtime(delay);
    //    this.interstitial.Show();
    //}
    /// <summary>
    /// Show Video Ad
    /// </summary>
    public override void ShowVideo()
    {
        //Debug.Log("GG >> Admob:vad:ShowCall");
        //AdmobGA_Helper.GA_Log(AdmobGAEvents.ShowVideoAd);
        //if (this.videoAd != null)
        //{
        //    if (this.videoAd.IsLoaded())
        //    {
        //        Debug.Log("GG >> Admob:vad:WillDisplay");
        //        AdmobGA_Helper.GA_Log(AdmobGAEvents.VideoAdWillDisplay);

        //        this.videoAd.Show();
        //    }

        //    //else
        //    //{
        //    //    Debug.Log("GG >> Admob:vad:NotLoaded");
        //    //    LoadVideo();
        //    //}
        //}
    }

    //public override bool IsVideoAdReady()
    //{
    //    if (this.videoAd != null)
    //        return this.videoAd.IsLoaded();
    //    else
    //        return false;
    //}

    //private IEnumerator ShowVideoWithDelay()
    //{
    //    yield return new WaitForSecondsRealtime(1f);
    //    this.videoAd.Show();
    //}
    /// <summary>
    /// Show Rewarded Ad
    /// </summary>
    //public override void ShowRewardedVideo()
    //{

    //}
    public override void ShowRewardedVideo(RewardUserDelegate _delegate)
    {
        Debug.Log("GG >> Admob:rad:ShowCall");
        NotifyReward = _delegate;
        AdmobGA_Helper.GA_Log(AdmobGAEvents.ShowRewardedAd);

        if (this.rewardBasedVideo.IsLoaded())
        {
            Debug.Log("GG >> Admob:rad:WillDisplay");

            AdmobGA_Helper.GA_Log(AdmobGAEvents.RewardedAdWillDisplay);
            this.rewardBasedVideo.Show();
        }

        //else
        //{
        //    Debug.Log("GG >> Admob:rad:NotLoaded");
        //    LoadRewardedVideo();
        //}
    }

    #endregion

    #region Intertitial Add Handler

    //******Intertitial Ad Handlers**********//
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            iAdStatus = AdLoadingStatus.Loaded;
            //Debug.Log("GG >> Admob:iad:Loaded");
            AdmobGA_Helper.GA_Log(AdmobGAEvents.InterstitialAdLoaded);
        });
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            iAdStatus = AdLoadingStatus.NoInventory;
            //Debug.Log("GG >> Admob:iad:NoInventory :: " + args.ToString());
            AdmobGA_Helper.GA_Log(AdmobGAEvents.InterstitialAdNoInventory);
        });
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            iAdStatus = AdLoadingStatus.NotLoaded;
            //Debug.Log("GG >> Admob:iad:Displayed ");
            Constants.isAdPlaying = true;
            AdmobGA_Helper.GA_Log(AdmobGAEvents.InterstitialAdDisplayed);
        });
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        this.interstitial.Destroy();
        this.interstitial = new InterstitialAd(ADMOB_ID.ADMOB_INTERTITIAL_AD_ID);
        BindIntertitialEvents();

        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            //Debug.Log("GG >> Admob:iad:Closed");
            AdmobGA_Helper.GA_Log(AdmobGAEvents.InterstitialAdClosed);
            Constants.isAdPlaying = false;
            iAdStatus = AdLoadingStatus.NotLoaded;
            LoadInterstitial();
        });
    }

    //public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    //{
    //    MobileAdsEventExecutor.ExecuteInUpdate(() =>
    //    {
    //        Debug.Log("GG >> Admob:iad:Clicked");
    //        AdmobGA_Helper.GA_Log(AdmobGAEvents.InterstitialAdClicked);
    //    });
    //}

    #endregion

    #region Small Banner Add Handler


    //******Intertitial Ad Handlers**********//
    public void SmallBanner_HandleOnAdLoaded(object sender, EventArgs args)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            sbAdStatus = AdLoadingStatus.Loaded;
            //Debug.Log("GG >> Admob:smallBanner:Loaded");
            AdmobGA_Helper.GA_Log(AdmobGAEvents.SmallBannerLoaded);
            //isSmallBannerLoaded = true;
        });
    }

    public void SmallBanner_HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            sbAdStatus = AdLoadingStatus.NoInventory;
            //Debug.Log("GG >> Admob:smallBanner:NoInventory :: " + args.LoadAdError);
            AdmobGA_Helper.GA_Log(AdmobGAEvents.SmallBannerNoInventory);
            //isSmallBannerLoaded = false;
        });
    }

    public void SmallBanner_HandleOnAdOpened(object sender, EventArgs args)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            sbAdStatus = AdLoadingStatus.NotLoaded;
            //Debug.Log("GG >> Admob:smallBanner:Displayed");
            AdmobGA_Helper.GA_Log(AdmobGAEvents.SmallBannerDisplayed);
        });
    }
    public void SmallBanner_HandleOnAdClosed(object sender, EventArgs args)
    {
        //Debug.Log("GG >> Admob:smallBanner:Closed");
        AdmobGA_Helper.GA_Log(AdmobGAEvents.SmallBannerClosed);
    }
    public void SmallBanner_HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        //Debug.Log("GG >> Admob:smallBanner:Clicked");
        AdmobGA_Helper.GA_Log(AdmobGAEvents.SmallBannerClicked);
    }

    #endregion

    #region Bannel Call Back Events
    public void MediumBanner_HandleOnAdLoaded(object sender, EventArgs args)
    {
        //Debug.Log("GG >> Admob:mediumBanner:Loaded");
        AdmobGA_Helper.GA_Log(AdmobGAEvents.MediumBannerLoaded);
        //isMediumBannerLoaded = true;
    }

    public void MediumBanner_HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        //Debug.Log("GG >> Admob:mediumBanner:NoInventory :: " + args.LoadAdError);
        AdmobGA_Helper.GA_Log(AdmobGAEvents.MediumBannerNoInventory);
        //isMediumBannerLoaded = false;
    }

    public void MediumBanner_HandleOnAdOpened(object sender, EventArgs args)
    {
        //Debug.Log("GG >> Admob:mediumBanner:Displayed");
        AdmobGA_Helper.GA_Log(AdmobGAEvents.MediumBannerDisplayed);

        //    audios = FindObjectsOfType<AudioSource>();
        //   foreach (var audio in audios)
        {
            //     audio.mute = true;
        }
    }

    public void MediumBanner_HandleOnAdClosed(object sender, EventArgs args)
    {
        //Debug.Log("GG >> Admob:mediumBanner:Closed");
        AdmobGA_Helper.GA_Log(AdmobGAEvents.MediumBannerClosed);
    }

    public void MediumBanner_HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        //Debug.Log("GG >> Admob:mediumBanner:Clicked");
        AdmobGA_Helper.GA_Log(AdmobGAEvents.MediumBannerClicked);
    }

    #endregion

    #region Video Ad Handlers

    ////******Video Ad Handlers**********//
    //public void Video_HandleOnAdLoaded(object sender, EventArgs args)
    //{
    //    MobileAdsEventExecutor.ExecuteInUpdate(() =>
    //    {
    //        vAdStatus = AdLoadingStatus.Loaded;
    //        Debug.Log("GG >> Admob:vad:Loaded");
    //        AdmobGA_Helper.GA_Log(AdmobGAEvents.VideoAdLoaded);
    //    });
    //}

    //public void Video_HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    //{
    //    MobileAdsEventExecutor.ExecuteInUpdate(() =>
    //    {
    //        vAdStatus = AdLoadingStatus.NoInventory;
    //        Debug.Log("GG >> Admob:vad:NoInventory :: " + args.ToString());
    //        AdmobGA_Helper.GA_Log(AdmobGAEvents.VideoAdNoInventory);
    //    });
    //}


    //public void Video_HandleOnAdOpened(object sender, EventArgs args)
    //{
    //    MobileAdsEventExecutor.ExecuteInUpdate(() =>
    //    {
    //        vAdStatus = AdLoadingStatus.NotLoaded;
    //        Debug.Log("GG >> Admob:vad:Displayed");
    //        AdmobGA_Helper.GA_Log(AdmobGAEvents.VideoAdDisplayed);
    //        Constants.isAdPlaying = true;
    //    });
    //}

    //public void Video_HandleOnAdClosed(object sender, EventArgs args)
    //{
    //    this.videoAd.Destroy();
    //    this.videoAd = new InterstitialAd(ADMOB_ID.ADMOB_VIDEO_AD_ID);
    //    BindVideoEvents();
    //    MobileAdsEventExecutor.ExecuteInUpdate(() =>
    //    {
    //        vAdStatus = AdLoadingStatus.NotLoaded;
    //        Debug.Log("GG >> Admob:vad:Closed");
    //        AdmobGA_Helper.GA_Log(AdmobGAEvents.VideoAdClosed);
    //        Constants.isAdPlaying = false;
    //    });
    //    //OnVideoClosed();
    //    //Invoke("OnVideoClosed", 2f);
    //}

    //public void Video_HandleOnAdLeavingApplication(object sender, EventArgs args)
    //{
    //    MobileAdsEventExecutor.ExecuteInUpdate(() =>
    //    {
    //        Debug.Log("GG >> Admob:vad:Clicked");
    //        AdmobGA_Helper.GA_Log(AdmobGAEvents.VideoAdClicked);
    //    });
    //}

    #endregion

    #region Rewarded Ad Handlers

    //***** Rewarded Events *****//
    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            rAdStatus = AdLoadingStatus.Loaded;
            //Debug.Log("GG >> Admob:rad:Loaded");
            AdmobGA_Helper.GA_Log(AdmobGAEvents.RewardedAdLoaded);
        });
    }

    //public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    //{
    //    MonoBehaviour.print(
    //        "HandleRewardedAdFailedToLoad event received with message: "
    //                         + args.Message);
    //}
    public void HandleRewardBasedVideoFailedToLoad(object sender, EventArgs args)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            rAdStatus = AdLoadingStatus.NoInventory;
            //Debug.Log("GG >> Admob:rad:NoInventory :: " + args.ToString());
            AdmobGA_Helper.GA_Log(AdmobGAEvents.RewardedAdNoInventory);
        });
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            rAdStatus = AdLoadingStatus.NotLoaded;
           // Debug.Log("GG >> Admob:rad:FailedToShow");
        });
    }

    public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            rAdStatus = AdLoadingStatus.NotLoaded;
            //Debug.Log("GG >> Admob:rad:Displayed");
            AdmobGA_Helper.GA_Log(AdmobGAEvents.RewardedAdDisplayed);
        });
    }

    public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            rAdStatus = AdLoadingStatus.NotLoaded;
            //Debug.Log("GG >> Admob:rad:Started");
            AdmobGA_Helper.GA_Log(AdmobGAEvents.RewardedAdStarted);
        });
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            rAdStatus = AdLoadingStatus.NotLoaded;
            //Debug.Log("GG >> Admob:rad:Closed");
            AdmobGA_Helper.GA_Log(AdmobGAEvents.RewardedAdClosed);

            this.rewardBasedVideo = new RewardedAd(ADMOB_ID.ADMOB_REWARDED_AD_ID);
            BindRewardedEvents();
            LoadRewardedVideo();
        });
        //Invoke("OnAdClosed", 2);
        //Debug.Log("GG >> On Ad Clicked");
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            //Debug.Log("GG >> give reward to user after watching rAd");

            NotifyReward();
            AdmobGA_Helper.GA_Log(AdmobGAEvents.RewardedAdReward);
        });
    }

    #endregion
}