//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.Events;

//using System;
//using System.Collections.Generic;

//using GoogleMobileAds.Api;
//using GoogleMobileAds.Common;
//using GoogleMobileAds.Api.Mediation.UnityAds;
//using GoogleMobileAds.Api.Mediation.Chartboost;
//// using GoogleMobileAdsMediationTestSuite.Api;

//public class AdsManagerAR : MonoBehaviour {


//    public static AdsManagerAR Instance;

   
//    //all ids here are testing ids, so when you put id from the editor (inside the components)
//    //it will be automatically overridden, so no need to change ids from here in scripts also.
//    //testing id link: https://developers.google.com/admob/unity/test-ads
//    [Header("Admobe IDs Android")]
//    public string Android_Admobe_Banner_ID = "ca-app-pub-9513847263305253/2914140351";
//    [Space(4)]
//    public string Android_Admobe_InterSital_ID = "ca-app-pub-9513847263305253/9343817339";
//    [Space(4)]
//    public string Android_Admobe_Rewarded_ID = "ca-app-pub-9513847263305253/2778408980";
//    [Space(4)]
//    public string Android_Admobe_Rewarded_interstitial_ID = "ca-app-pub-9513847263305253/1316700225";
    
//    [Header("Admobe IDs iOS")]
//    public string iOS_Admobe_Banner_ID = "ca-app-pub-9513847263305253/4264619155";
//    [Space(4)]
//    public string iOS_Admobe_InterSital_ID = "ca-app-pub-9513847263305253/9088929795";
//    [Space(4)]
//    public string iOS_Admobe_Rewarded_ID = "ca-app-pub-9513847263305253/4163906170";
//    [Space(4)]
//    public string iOS_Admobe_Rewarded_interstitial_ID = "ca-app-pub-9513847263305253/3256467851";

//    [Space(10)]
//    private BannerView bannerView;
//    private InterstitialAd interstitialAd;
//    private RewardedAd rewardedAd;
//    private RewardedInterstitialAd rewardedInterstitialAd;

//    private float deltaTime;
//    [HideInInspector]
//    public UnityEvent OnAdLoadedEvent;
//    [HideInInspector]
//    public UnityEvent OnAdFailedToLoadEvent;
//    [HideInInspector]
//    public UnityEvent OnAdOpeningEvent;
//    [HideInInspector]
//    public UnityEvent OnAdFailedToShowEvent;
//    public UnityEvent OnUserEarnedRewardEvent;
//    [HideInInspector]
//    public UnityEvent OnAdClosedEvent;
    
//    private bool showFpsMeter = true;
//    private Text fpsMeter;
//    public Text statusText;

//    //statics
//    public static string Admobe_Banner_ID;
//    public static string Admobe_InterSital_ID;
//    public static string Admobe_Rewarded_ID;
//    public static string Admobe_Rewarded_interstitial_ID;


//    #region UNITY MONOBEHAVIOR METHODS

//    void Awake(){

//        if (Instance != null)
//        {
//            Destroy(gameObject);
//        }
//        else
//        {
//            Instance = this;
//        }
//        // These ad units are configured to always serve test ads.
//#if UNITY_EDITOR
//        Admobe_Banner_ID = "unused";
//            Admobe_InterSital_ID = "unused"; 
//            Admobe_Rewarded_ID =  "unused";
//            Admobe_Rewarded_interstitial_ID = "unused";

//        #elif UNITY_ANDROID
//            Admobe_Banner_ID = Android_Admobe_Banner_ID;
//            Admobe_InterSital_ID = Android_Admobe_InterSital_ID;
//            Admobe_Rewarded_ID = Android_Admobe_Rewarded_ID;
//            Admobe_Rewarded_interstitial_ID = Android_Admobe_Rewarded_interstitial_ID;

//        #elif UNITY_IPHONE
//            Admobe_Banner_ID = iOS_Admobe_Banner_ID;
//            Admobe_InterSital_ID = iOS_Admobe_InterSital_ID;
//            Admobe_Rewarded_ID = iOS_Admobe_Rewarded_ID;
//            Admobe_Rewarded_interstitial_ID = iOS_Admobe_Rewarded_interstitial_ID;

//        #else
//            Admobe_Banner_ID = "unexpected_platform";
//            Admobe_InterSital_ID = "unexpected_platform";
//            Admobe_Rewarded_ID = "unexpected_platform";
//            Admobe_Rewarded_interstitial_ID = "unexpected_platform";

//        #endif
//    }

//    public void Start() {
//        // MediationTestSuite.OnMediationTestSuiteDismissed += this.HandleMediationTestSuiteDismissed;
//        MobileAds.SetiOSAppPauseOnBackground(true);
//        UnityAds.SetGDPRConsentMetaData(true);
//        Chartboost.AddDataUseConsent(CBGDPRDataUseConsent.NonBehavioral);

//        // Initialize the Google Mobile Ads SDK.
//        MobileAds.Initialize(HandleInitCompleteAction);
//        MobileAds.Initialize((initStatus) => {
//            Dictionary<string, AdapterStatus> map = initStatus.getAdapterStatusMap();
//            foreach (KeyValuePair<string, AdapterStatus> keyValuePair in map) {
//                string className = keyValuePair.Key;
//                AdapterStatus status = keyValuePair.Value;
//                switch (status.InitializationState)
//                {
//                    case AdapterState.NotReady:
//                        // The adapter initialization did not complete.
//                        MonoBehaviour.print("Adapter: " + className + " not ready.");
//                        break;
//                    case AdapterState.Ready:
//                        // The adapter was successfully initialized.
//                        MonoBehaviour.print("Adapter: " + className + " is initialized.");
//                        break;
//                }
//            }
//        });
//    }
    
//    //public void ShowMediationTestSuite()
//    //{
//    //    MediationTestSuite.Show("ca-app-pub-3940256099942544~3347511713");
//    //}

//    public void HandleMediationTestSuiteDismissed(object sender, EventArgs args)
//    {
//        MonoBehaviour.print("HandleMediationTestSuiteDismissed event received");
//    }
//    private void HandleInitCompleteAction(InitializationStatus initstatus)
//    {
//        // Callbacks from GoogleMobileAds are not guaranteed to be called on
//        // main thread.
//        // In this example we use MobileAdsEventExecutor to schedule these calls on
//        // the next Update() loop.
//        MobileAdsEventExecutor.ExecuteInUpdate(() =>
//        {
//            if (statusText != null)
//            {


//                statusText.text = "Initialization complete";
//            }
//            RequestBannerAd();
//            RequestAndLoadInterstitialAd();
//            RequestAndLoadRewardedAd();
//        });
//    }

//    private void Update()
//    {
//        //if (showFpsMeter)
//        //{
//        //    fpsMeter.gameObject.SetActive(true);
//        //    deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
//        //    float fps = 1.0f / deltaTime;
//        //    fpsMeter.text = string.Format("{0:0.} fps", fps);
//        //}
//        //else
//        //{
//        //    fpsMeter.gameObject.SetActive(false);
//        //}
//    }

//    #endregion

//    #region HELPER METHODS

//    private AdRequest CreateAdRequest()
//    {
//        return new AdRequest.Builder()
//            .AddKeyword("unity-admob-sample")
//            .Build();
//    }

//    #endregion

//    #region BANNER ADS

//    public void RequestBannerAd()
//    {
//        if (statusText != null)
//        {
//            statusText.text = "Requesting Banner Ad.";
//        }
        
//        // Clean up banner before reusing
//        if (bannerView != null)
//        {
//            bannerView.Destroy();
//        }

//        // Create a 320x50 banner at top of the screen
//        bannerView = new BannerView(Admobe_Banner_ID, AdSize.SmartBanner, AdPosition.Top);

//        // Add Event Handlers
//        bannerView.OnAdLoaded += (sender, args) => OnAdLoadedEvent.Invoke();
//        bannerView.OnAdFailedToLoad += (sender, args) => OnAdFailedToLoadEvent.Invoke();
//        bannerView.OnAdOpening += (sender, args) => OnAdOpeningEvent.Invoke();
//        bannerView.OnAdClosed += (sender, args) => OnAdClosedEvent.Invoke();

//        // Load a banner ad
//        bannerView.LoadAd(CreateAdRequest());
//    }

//    public void DestroyBannerAd()
//    {
//        if (bannerView != null)
//        {
//            bannerView.Destroy();
//        }
//    }

//    #endregion

//    #region INTERSTITIAL ADS

//    public void RequestAndLoadInterstitialAd() {
//        if (statusText != null) {
//            statusText.text = "Requesting Interstitial Ad.";
//        }
        
//        // Clean up interstitial before using it
//        if (interstitialAd != null)
//        {
//            interstitialAd.Destroy();
//        }

//        interstitialAd = new InterstitialAd(Admobe_InterSital_ID);

//        // Add Event Handlers
//        interstitialAd.OnAdLoaded += (sender, args) => OnAdLoadedEvent.Invoke();
//        interstitialAd.OnAdFailedToLoad += (sender, args) => OnAdFailedToLoadEvent.Invoke();
//        interstitialAd.OnAdOpening += (sender, args) => OnAdOpeningEvent.Invoke();
//        interstitialAd.OnAdClosed += (sender, args) => OnAdClosedEvent.Invoke();

//        // Load an interstitial ad
//        interstitialAd.LoadAd(CreateAdRequest());
//    }

//    public void ShowInterstitialAd()
//    {
//        if (interstitialAd!=null)
//        {
//            interstitialAd.Show();
//        }
//        else
//        {
//            if (statusText != null)
//            {
//                statusText.text = "Interstitial ad is not ready yet";
//            }
//            RequestAndLoadInterstitialAd();
//        }
//    }

//    public void DestroyInterstitialAd()
//    {
//        if (interstitialAd != null)
//        {
//            interstitialAd.Destroy();
//        }
//    }
//    #endregion

//    #region REWARDED ADS

//    public void RequestAndLoadRewardedAd() {

//        if (statusText != null) {
//            statusText.text = "Requesting Rewarded Ad.";
//        }
        
//        // create new rewarded ad instance
//        rewardedAd = new RewardedAd(Admobe_Rewarded_ID);

//        // Add Event Handlers
//        rewardedAd.OnAdLoaded += (sender, args) => OnAdLoadedEvent.Invoke();
//        rewardedAd.OnAdFailedToLoad += (sender, args) => OnAdFailedToLoadEvent.Invoke();
//        rewardedAd.OnAdOpening += (sender, args) => OnAdOpeningEvent.Invoke();
//        rewardedAd.OnAdFailedToShow += (sender, args) => OnAdFailedToShowEvent.Invoke();
//        rewardedAd.OnAdClosed += (sender, args) => OnAdClosedEvent.Invoke();
//        rewardedAd.OnUserEarnedReward += (sender, args) => OnUserEarnedRewardEvent.Invoke();

//        // Create empty ad request
//        rewardedAd.LoadAd(CreateAdRequest());
//    }

//    public void ShowRewardedAd()
//    {
//        if (rewardedAd != null)
//        {
//            rewardedAd.Show();
//        }
//        else
//        {
//            if (statusText != null)
//            {
//                statusText.text = "Rewarded ad is not ready yet.";
//            }
//            RequestAndLoadRewardedAd();
//        }
//    }

//    public void RequestAndLoadRewardedInterstitialAd() {

//        if (statusText != null)
//        {
//            statusText.text = "Requesting Rewarded Interstitial Ad.";
//        }
        
//        // Create an interstitial.
//        RewardedInterstitialAd.LoadAd(Admobe_Rewarded_interstitial_ID, CreateAdRequest(), (rewardedInterstitialAd, error) => {

//            if (error != null)
//            {
//                MobileAdsEventExecutor.ExecuteInUpdate(() => {
//                    if (statusText != null)
//                    {
//                        statusText.text = "RewardedInterstitialAd load failed, error: " + error;
//                    }
//                });
//                return;
//            }

//            this.rewardedInterstitialAd = rewardedInterstitialAd;
//            MobileAdsEventExecutor.ExecuteInUpdate(() => {
//                if (statusText != null)
//                {
//                    statusText.text = "RewardedInterstitialAd loaded";
//                }
               
//            });
//            // Register for ad events.
//            this.rewardedInterstitialAd.OnAdDidPresentFullScreenContent += (sender, args) =>
//            {
//                MobileAdsEventExecutor.ExecuteInUpdate(() => {
//                    if (statusText != null)
//                    {
//                        statusText.text = "Rewarded Interstitial presented.";
//                    }
//                });
//            };
//            this.rewardedInterstitialAd.OnAdDidDismissFullScreenContent += (sender, args) =>
//            {
//                MobileAdsEventExecutor.ExecuteInUpdate(() => {
//                    if (statusText != null)
//                    {
//                        statusText.text = "Rewarded Interstitial dismissed.";
//                    }
//                });
//                this.rewardedInterstitialAd = null;
//            };
//            this.rewardedInterstitialAd.OnAdFailedToPresentFullScreenContent += (sender, args) =>
//            {
//                MobileAdsEventExecutor.ExecuteInUpdate(() => {
//                    if (statusText != null)
//                    {
//                        statusText.text = "Rewarded Interstitial failed to present.";
//                    }
//                });
//                this.rewardedInterstitialAd = null;
//            };
//        });
//    }

//    private void ShowRewardedInterstitialAd()
//    {
//        if (rewardedInterstitialAd != null)
//        {
//            rewardedInterstitialAd.Show((reward) => {
//                MobileAdsEventExecutor.ExecuteInUpdate(() => {
//                    if (statusText != null)
//                    {
//                        statusText.text = "User Rewarded: " + reward.Amount;
//                    }
//                });
//            });
//        }
//        else
//        {
//            if (statusText != null)
//            {
//                statusText.text = "Rewarded ad is not ready yet.";
//            }
//        }
//    }

//    #endregion

//}