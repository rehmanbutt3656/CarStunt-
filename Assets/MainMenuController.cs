using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using System;



public class MainMenuController : MonoBehaviour
{
    //public Button[] _removeAds,_unlockAllCar,_unlockAllLevel;
    public GameObject dailybonuspopup, _sevenDaysRewardPopup;
    public GameObject _Adspanel;
    public GameObject _internetOffPanel;
    public GameObject[] Cars;
    [Range(0, 1)]
    public float[] TopSpeed;
    public Image speed;
    [Range(0, 1)]
    public float[] Handling;
    public Image handl;
    [Range(0, 1)]
    public float[] Acceleration;
    public Image acc;
    [Range(0,1)]
    public float[] Fuel;
    public Image ful;

    public GameObject[] unlockAdsBtn;
    public GameObject LevelsParent;
    public int LevelsCompleted = 0;

    public static int CurrentLevel = 0;
    public static int CurrentCar = 0;
    public int CurrentCarAR;
    public static int CurrentSelectedCar = 0;
    public static MainMenuController Instance;

    public GameObject LevelLoadingPanel;
    public GameObject closePanel;
    public Text AvailableCashtxtMainMenu, AvailableCashtxt, AvailableCashtxtLevelSelection;
    public Text CarPricetxt;
    public GameObject FreeCash, CarPurchaseButton, playButton;
    public GameObject NotEnoughCash, NotEnoughCash1;
    public int Cash;
    public bool[] CarPurchased;
    public int[] CarPrice;
    [HideInInspector]
    public bool awakeCallforDailyRewards = false;

    public GameObject CoinsAnimationMainMenu, CoinsAnimation;
    public GameObject CoinsAnimationParentMainMenu, CoinsAnimationParent;

    public AudioSource UnlockCarSound;

    public AudioSource[] CarSounds;
    public AudioSource[] OtherSounds;
    public AudioSource[] BGSounds;

    [Header("Settings")]
    [Space(5)]
    public SettingsHandler _settingsData;


    int counter;
    bool bannerShow;
    private GameObject dailybonusobject, sevenDayRewardObject;

    public CustomEvents AndroidBackButtonEvents;

    [Serializable]
    public class SettingsHandler
    {
        public Image[] selected;
        public Slider musicSlider, soundSlider;

        [Header("AudioSources")]
        [Space(5)]
        public AudioSource _musicSource;
        public AudioSource[] _soundSources;


        private int controlSetting;
        public int ControlSetting
        {
            get
            {
                if (!PlayerPrefs.HasKey("ControlSettings")) { PlayerPrefs.SetInt("ControlSettings", 0); }
                controlSetting = PlayerPrefs.GetInt("ControlSettings", 0);
                return controlSetting;
            }
            set
            {
                controlSetting = value;
                PlayerPrefs.SetInt("ControlSettings", value);
            }
        }
    }


    // Use this for initialization
    void Start()
    {

        PlayerPrefs.GetInt("AvailableCash", 0);
        PlayerPrefs.GetInt("firstTime", 0);
        if (PlayerPrefs.GetInt("firstTime", 0)==0)
        {
            PlayerPrefs.SetInt("firstTime", 1);
            PlayerPrefs.SetInt("AvailableCash", 3000);

        }
        //PlayerPrefs.DeleteAll();
        _settingsData.selected[_settingsData.ControlSetting].gameObject.SetActive(true);
        counter = 0;
        bannerShow = false;
        Time.timeScale = 1f;
        Instance = this;
        //PlayerPrefs.SetInt("LevelsCompleted", 20);
        //		if (!PlayerPrefs.HasKey ("LevelsCompleted")) {
        LevelsCompleted = PlayerPrefs.GetInt("LevelsCompleted", 1);
        //		}
            

        UpdateCarsPurchased();

        //SoundSettings
        PlayerPrefs.GetInt("EngineSound", 1);
        PlayerPrefs.GetInt("OtherSounds", 1);
        PlayerPrefs.GetInt("BGSounds", 1);
        //setSoundSettingsUI ();

        setCarPurchaseButton(0);
        SetLevels();
        SoundCheck();

        TopSpeedd();
        TopHandlingg();
        TopAcceleration();
        TopFuel();
        //		StartCoroutine ("Tester");
        //awakeCallforDailyRewards = true;
        //Invoke("DailyBonusSevenDays", 1f);
    }

    public void CheckSoundSettings ()
    {
        _settingsData._musicSource.volume = _settingsData.musicSlider.value;
        PlayerPrefs.SetFloat("GameMusic", _settingsData.musicSlider.value);

        foreach (AudioSource _sources in _settingsData._soundSources) { _sources.volume = _settingsData.soundSlider.value; }

        PlayerPrefs.SetFloat("GameSounds", _settingsData.soundSlider.value);
    }

    public void DailyBonusCallBack ()
    {
        dailybonusobject = Instantiate(dailybonuspopup);
    }

    void DailyBonusSevenDays ()
    {

        sevenDayRewardObject = Instantiate(_sevenDaysRewardPopup);
    }

    void SoundCheck ()
    {
        if (!PlayerPrefs.HasKey("GameSounds"))
        {
            PlayerPrefs.SetFloat("GameSounds", 0.6f);
            PlayerPrefs.SetFloat("GameMusic", 0.75f);
        }

        float mVol = PlayerPrefs.GetFloat("GameMusic", 0.75f);
        float sVol = PlayerPrefs.GetFloat("GameSounds", 0.6f);

        _settingsData._musicSource.volume = mVol;
        foreach (AudioSource _sources in _settingsData._soundSources) { _sources.volume = sVol; }
        _settingsData.soundSlider.value = sVol;
        _settingsData.musicSlider.value = mVol;
    }

    void Update ()
    {
        //PlayerPrefs.DeleteAll();
        if (PlayerPrefs.GetInt("RemoveAds") == 1)
        {
            //_removeAds[0].interactable = false;
            //_removeAds[1].interactable = false;
        }
        if (PlayerPrefs.GetInt("UnlockAllCar") == 1)
        {
            //_unlockAllCar[0].interactable = false;
            //_unlockAllCar[1].interactable = false;
            purchasedCarByIAP();
            UpdateCarsPurchased();
        }
        if (PlayerPrefs.GetInt("UnlockAllLevel") == 1)
        {
            //_unlockAllLevel[0].interactable = false;
            LevelsCompleted = PlayerPrefs.GetInt("LevelsCompleted");
            SetLevels();
        }

        //if (ControlFreak2.CF2Input.GetKey(KeyCode.Escape))
        //{
        //    AndroidBackButtonEvents.Invoke();
        //    Time.timeScale = 0f;
        //}
        if (PlayerPrefs.GetInt("rewardvdo") == 1)
        {
            if (PlayerPrefs.GetInt("FreeCashMM", 0) == 1)
            {
                RewardClick++;
                PlayerPrefs.SetInt("rewardvdo", 0);
                Invoke("RewardVideoCompleteMainMenu", 1f);
                PlayerPrefs.SetInt("FreeCashMM", 0);
            }
            if (PlayerPrefs.GetInt("RewardVideoVS", 0) == 1)
            {
                NotEnoughCash1.SetActive(false);
                RewardClick++;
                PlayerPrefs.SetInt("rewardvdo", 0);
                Invoke("RewardVideoCompleteVS", 1f);
                PlayerPrefs.SetInt("RewardVideoVS", 0);
            }           
            if (PlayerPrefs.GetInt("UnlocklvlReward", 0) == 1)
            {
                print("counter =" + counter);
                //if (counter == 2) {
                Invoke("currentlevell", 1f);
                PlayerPrefs.SetInt("rewardvdo", 0);
                PlayerPrefs.SetInt("UnlocklvlReward", 0);
                print("counter 1=" + counter);
            }
        }
        else if (PlayerPrefs.GetInt("rewardvdo", 1) == 2)
        {
            //Video Failed
            PlayerPrefs.SetInt("rewardvdo", 0);
        }    

        Cash = PlayerPrefs.GetInt("AvailableCash", 0);
        AvailableCashtxt.text = "$" + Cash.ToString();
        AvailableCashtxtMainMenu.text = "$" + Cash.ToString();
        AvailableCashtxtLevelSelection.text = "$" + Cash.ToString();
    }
    public void Exit_Btn ()
    {
        
        closePanel.SetActive(true);
    }
    public void Exit_Yes ()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
    public void Exit_No ()
    {
        Time.timeScale = 1f;

    }

    public void DpadsSelected ()
    {
        int i = 0;
        foreach (Image image in _settingsData.selected)
        {
            if (i.Equals(0)) { image.gameObject.SetActive(true); }
            else { image.gameObject.SetActive(false); }
            i++;
        }

        _settingsData.ControlSetting = 0;
    }

    public void SteeringSelected ()
    {
        int i = 0;
        foreach (Image image in _settingsData.selected)
        {
            if (i.Equals(1)) { image.gameObject.SetActive(true); }
            else { image.gameObject.SetActive(false); }
            i++;
        }
        _settingsData.ControlSetting = 1;
    }

    public void TiltSelected ()
    {
        int i = 0;
        foreach (Image image in _settingsData.selected)
        {
            if (i.Equals(2)) { image.gameObject.SetActive(true); }
            else { image.gameObject.SetActive(false); }
            i++;
        }
        _settingsData.ControlSetting = 2;

    }
    //Commented for ads

    public void UnlockLevelRewardedVideo ()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            //   AdsManagerAR.Instance.ShowInterstitialAd();
            //AdsManager._instance.ShowUnityRewardedvideo();
            // AdManager.ShowRewardBasedVideo();
            //ar TechnologyWings.AdManager.ShowRewardBasedVideo(level_unlock);
         //   AdmobAdsManager.Instance.ShowRewardedVideo(level_unlock);
            FindObjectOfType<MediationHandler>().ShowRewardedVideo(level_unlock);
        }
        else
        {
            _internetOffPanel.SetActive(true);
        }
    }
    public void UnlockAllLevels()
    {
         TechnologyWings.InAppController.Instance.BuyInAppProduct(2);
    }
    public void level_unlock()
    {
        counter++;
        PlayerPrefs.SetInt("UnlocklvlReward", 1);
    }
    public void currentlevell ()
    {
        int cc = PlayerPrefs.GetInt("LevelsCompleted");
        print(cc + "curr :" + CurrentLevel);
        if (CurrentLevel + 2 > cc)
        {
            int LC = CurrentLevel + 1;
            PlayerPrefs.SetInt("LevelsCompleted", LC);
            print("currnt : " + LC);
        }
        string s = "Level_" + (CurrentLevel + 1).ToString() + "_Stars";
        print(s);
        PlayerPrefs.SetInt(s, 3);
        CurrentLevel = PlayerPrefs.GetInt("LevelsCompleted");
        LevelsCompleted += 1;
        SetLevels();
    }

    void SetLevels ()
    {
        for (int l = 0; l < LevelsParent.transform.childCount; l++)
        {
            if (l < LevelsCompleted)
            {//Turn On
             //				print(l);
               // LevelsParent.transform.GetChild(l).gameObject.GetComponent<ButtonRef>().setText(("LEVEL "+ l + 1 ).ToString());
                string s = "Level_" + (l + 1).ToString() + "_Stars";
                //Debug.Log (s);
                int x = PlayerPrefs.GetInt(s);
                //				print("x value" + x);
                if (x == 0)
                    LevelsParent.transform.GetChild(l).gameObject.GetComponent<ButtonRef>().Set1Star();
                if (x == 1)
                    LevelsParent.transform.GetChild(l).gameObject.GetComponent<ButtonRef>().Set1Star();
                if (x == 2)
                    LevelsParent.transform.GetChild(l).gameObject.GetComponent<ButtonRef>().Set2Star();
                if (x == 3)
                    LevelsParent.transform.GetChild(l).gameObject.GetComponent<ButtonRef>().Set3Star();
                //				LevelsParent.transform.GetChild (l).gameObject.GetComponent<Button>().interactable = true;
            }
            else
            {//Locked
                LevelsParent.transform.GetChild(l).gameObject.GetComponent<ButtonRef>().LockLevel();
            }
        }
    }

    IEnumerator Tester ()
    {
        yield return new WaitForSeconds(5f);       
    }

    void UpdateCarsPurchased ()
    {
        for (int i = 0; i < CarPurchased.Length; i++)
        {
            int c;
            if (i == 0)
            {
                c = 1;//PlayerPrefs.GetInt ("AvailableCash",0);			
            }
            else
            {
                c = CheckCarPurchased(i);
                //				Debug.Log ("Car Purchased at: " + i +  " is " + c);
            }
            if (c == 1)
            {
                CarPurchased[i] = true;
            }
            else
                CarPurchased[i] = false;
        }
    }

    public int CheckCarPurchased (int carNum)
    {
        string x = "Car" + carNum.ToString();
        return PlayerPrefs.GetInt(x, 0);
    }

    public void purchaseCar (int CarNum)
    {
        string x = "Car" + CarNum.ToString();
        PlayerPrefs.SetInt(x, 1);
        UpdateCarsPurchased();
    }
    public void purchasedCarByIAP() {
        purchaseCar(1);
        purchaseCar(2);
        purchaseCar(3);
        purchaseCar(4);
        CarPurchaseButton.SetActive(false);
        UnlockCarSound.Play();
    }
  
    public void LoadLevel (int i)
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {

            // AdsManagerAR.Instance.ShowInterstitialAd();
            //AdsManager._instance.ShowInterstitial();
            //  AdManager.ShowInterstitial();
            //TechnologyWings.AdManager.ShowInterstitial();

            
        }
        else
        {
            _Adspanel.SetActive(false);
        }
        _Adspanel.SetActive(false);
        Debug.Log("LoadSceneLevelBEfore");
        LevelLoadingPanel.SetActive(true);
        CurrentLevel = i;
      //  Debug.Log("LoadSceneLevelAfter");
        Invoke("LoadSceneLevel", 1.5f);
      //  Debug.Log("LoadSceneLevelSafaAfter");
    }


    void LoadSceneLevel ()
    {
        //AdManager.admanagerInstance.OnClickHideBanner ();

        //if (CurrentLevel % 2 == 0)
        //	AdManager.admanagerInstance.ShowInterstitial ();
        //else
        //	AdManager.admanagerInstance.ShowUnityVideoAd ();
        //AdsManagerAR.Instance.ShowInterstitialAd();
        //TechnologyWings.AdManager.ShowInterstitial();
        //Debug.Log ("LoadSceneLevel");
        SceneManager.LoadScene(3);
    }


    public int i = 0;
    public int j = 0;
    public int k = 0;
    public int l = 0;
    public void TopSpeedd ()
    {

        speed.fillAmount = TopSpeed[CurrentCarAR];

        
       // CurrentCarAR = j;

       // TopSpeed[j].transform.localPosition = new Vector3(0, 0.1f, -4.03f);
       // TopSpeed[j].transform.DOLocalMoveY(0, 0.4f);
    }
    public void TopHandlingg ()
    {
        handl.fillAmount = Handling[CurrentCarAR];
       
    }
    public void TopAcceleration ()
    {
        acc.fillAmount = Acceleration[CurrentCarAR];
        

       
    }
    public void TopFuel()
    {
        ful.fillAmount = Fuel[CurrentCarAR];
        //CurrentCarAR = l;

        // Fuel[l].transform.localPosition = new Vector3(0, 0.1f, -4.03f);
        //  Fuel[l].transform.DOLocalMoveY(0, 0.4f);
    }
    public void NextCar ()
    {
        i = 0;
        foreach (GameObject x in Cars)
        {
            i++;
           // print("next cars : " + i);
            if (x.gameObject.activeInHierarchy)
            {
                x.SetActive(false);
                break;
            }
        }

        if (i == Cars.Length)
        {
            i = 0;
        }

        Cars[i].SetActive(true);
       
        CurrentCar = i;
        CurrentCarAR = i;
        Debug.Log("Current Car Active is +" + i+"   "+ CurrentCarAR);
        //		Cars [i].transform.localScale = Vector3.zero;
        //		Cars [i].transform.DOScale (1f,0.4f);
        //Cars[i].transform.localPosition = new Vector3(0.28f, 2.5f, -3.8f);
        Cars[i].transform.localPosition = new Vector3(0.3205313f, 1.70f, -4.669949f);
        //Cars [i].transform.DOLocalMoveY (2,0.4f);
        setCarPurchaseButton(i);

        TopSpeedd();
        TopHandlingg();
        TopAcceleration();
        TopFuel();

        //if (CarPurchaseButton.activeSelf) { CarPurchaseButton.GetComponent<Animator>().Play("CPI", 0, 0); }
    }
    public void PreviousCar ()
    {
        i = 0;
        foreach (GameObject x in Cars)
        {
            i++;
            if (x.gameObject.activeInHierarchy)
            {
                x.SetActive(false);
                break;
            }
        }
        i--;
        i--;
        print("prev cars : " + i);
        if (i < 0)
        {
            i = Cars.Length - 1;
        }
        Cars[i].SetActive(true);
        CurrentCar = i;
        CurrentCarAR = i;
        //		Cars [i].transform.localScale = Vector3.zero;
        //		Cars [i].transform.DOScale (1f,0.4f);
        // Cars[i].transform.localPosition = new Vector3(0.28f, 2.5f, -3.8f);
        Cars[i].transform.localPosition = new Vector3(0.3205313f, 1.70f, -4.669949f);
        //Cars [i].transform.DOLocalMoveY (0,0.4f);

        setCarPurchaseButton(i);

        TopSpeedd();
        TopHandlingg();
        TopAcceleration();
        TopFuel();
     //   if (CarPurchaseButton.activeSelf) { CarPurchaseButton.GetComponent<Animator>().Play("CPI", 0, 0); }
    }

    void setCarPurchaseButton (int i)
    {
        if (i != 0)
        {
            int x = CheckCarPurchased(i);
            if (x == 1)
            {
                CarPurchaseButton.SetActive(false);
                FreeCash.SetActive(true);
                playButton.SetActive(true);
                CurrentSelectedCar = i;
            }
            else if (x == 0)
            {
                CarPurchaseButton.SetActive(true);
                FreeCash.SetActive(false);
                playButton.SetActive(false);
                CarPricetxt.text = "$" + CarPrice[i].ToString();
            }
        }
        else
        {
            CarPurchaseButton.SetActive(false);
            playButton.SetActive(true);
            FreeCash.SetActive(true);
            CurrentSelectedCar = 0;
        }
    }

    public void OnClickPurchaeCar ()
    {
        Debug.Log("CurrentCash is :"+Cash+"Current Car is:"+ CurrentCar);
        if (CarPrice[CurrentCarAR] > Cash)
        {
            NotEnoughCash.SetActive(true);
            Debug.Log("Not Enough Cash");
        }
        else
        {
            Debug.Log("Car Bought"+ CarPrice[CurrentCarAR]);
            Cash -= CarPrice[CurrentCarAR];
            PlayerPrefs.SetInt("AvailableCash", Cash);
            AvailableCashtxt.text = "$" + Cash.ToString();
            AvailableCashtxtMainMenu.text = "$" + Cash.ToString();
            AvailableCashtxtLevelSelection.text = "$" + Cash.ToString();
            purchaseCar(CurrentCarAR);
            CarPurchaseButton.SetActive(false);
            CurrentSelectedCar = CurrentCarAR;
            UnlockCarSound.Play();
        }
    }
    int RewardClick = 0;    

    public void OnClickWatchRewardVideo ()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            //AdsManager._instance.ShowUnityRewardedvideo();
            // AdManager.ShowRewardBasedVideo();
            //   AdsManagerAR.Instance.ShowRewardedAd()
            //   ;
            //ar TechnologyWings.AdManager.ShowRewardBasedVideo(RewardVideoCompleteMainMenu);
          //  AdmobAdsManager.Instance.ShowRewardedVideo(RewardVideoCompleteMainMenu);
            FindObjectOfType<MediationHandler>().ShowRewardedVideo(RewardVideoCompleteMainMenu);
            PlayerPrefs.SetInt("FreeCashMM", 1);
        }
        else
        {
            _internetOffPanel.SetActive(true);
        }
    }
    //Commented for ads

    public void OnClickWatchRewardVideoVS ()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            //AdsManager._instance.ShowUnityRewardedvideo();
            // AdManager.ShowRewardBasedVideo();
            // AdsManagerAR.Instance.ShowRewardedAd();
            //ar TechnologyWings.AdManager.ShowRewardBasedVideo(RewardVideoCompleteMainMenu);
           // AdmobAdsManager.Instance.ShowRewardedVideo(RewardVideoCompleteMainMenu);
            FindObjectOfType<MediationHandler>().ShowRewardedVideo(RewardVideoCompleteMainMenu);
            PlayerPrefs.SetInt("RewardVideoVS", 1);
        }
        else
        {
            _internetOffPanel.SetActive(true);
        }
    }

    public void MainMenuFreeCash ()
    {
        NotEnoughCash1.SetActive(true);
    }



    public void RewardVideoCompleteVS ()
    {
        GameObject x = Instantiate(CoinsAnimation, CoinsAnimationParent.transform) as GameObject;
        x.SetActive(true);
        StartCoroutine("IncreaseCoins");
    }
    public void RewardVideoCompleteMainMenu ()
    {
        GameObject x = Instantiate(CoinsAnimationMainMenu, CoinsAnimationParentMainMenu.transform) as GameObject;
        x.SetActive(true);
        StartCoroutine("IncreaseCoins");
    }

    IEnumerator IncreaseCoins ()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.2f);
            Cash += 500;
            AvailableCashtxt.text = "$" + Cash.ToString();
            AvailableCashtxtMainMenu.text = "$" + Cash.ToString();
            AvailableCashtxtLevelSelection.text = "$" + Cash.ToString();
        }
        PlayerPrefs.SetInt("AvailableCash", Cash);
        AvailableCashtxt.text = "$" + Cash.ToString();
        AvailableCashtxtMainMenu.text = "$" + Cash.ToString();
        AvailableCashtxtLevelSelection.text = "$" + Cash.ToString();
    }

    #region SoundSection

    //void setSoundSettingsUI()
    //{
    //	if (PlayerPrefs.GetInt ("EngineSound", 1) == 1) {
    //		CarEngineTick.SetActive (true);
    //		setEngineSounds (true);
    //	}
    //	else if (PlayerPrefs.GetInt ("EngineSound", 1) == 0) {
    //		CarEngineTick.SetActive (false);
    //		setEngineSounds (false);
    //	}

    //	if (PlayerPrefs.GetInt ("OtherSounds", 1) == 1) {
    //		OtherSoundsTick.SetActive (true);
    //		setOtherSounds (true);
    //	}
    //	else if (PlayerPrefs.GetInt ("OtherSounds", 1) == 0) {
    //		OtherSoundsTick.SetActive (false);
    //		setOtherSounds (false);
    //	}

    //	if (PlayerPrefs.GetInt ("BGSounds", 1) == 1) {
    //		BGSoundTick.SetActive (true);
    //		setBGSounds (true);
    //	}
    //	else if (PlayerPrefs.GetInt ("BGSounds", 1) == 0) {
    //		BGSoundTick.SetActive (false);
    //		setBGSounds (false);
    //	}
    //}

    //public void OnClickEngineSound()
    //{
    //	if (PlayerPrefs.GetInt ("EngineSound", 1) == 1) {
    //		PlayerPrefs.SetInt ("EngineSound", 0);
    //	}
    //	else if (PlayerPrefs.GetInt ("EngineSound", 1) == 0) {
    //		PlayerPrefs.SetInt ("EngineSound", 1);
    //	}
    //	setSoundSettingsUI ();
    //}

    //public void OnClickOtherSounds()
    //{
    //	if (PlayerPrefs.GetInt ("OtherSounds", 1) == 1) {
    //		PlayerPrefs.SetInt ("OtherSounds", 0);
    //	}
    //	else if (PlayerPrefs.GetInt ("OtherSounds", 1) == 0) {
    //		PlayerPrefs.SetInt ("OtherSounds", 1);
    //	}
    //	setSoundSettingsUI ();
    //}

    //public void OnClickBGSounds()
    //{
    //	if (PlayerPrefs.GetInt ("BGSounds", 1) == 1) {
    //		PlayerPrefs.SetInt ("BGSounds", 0);
    //	}
    //	else if (PlayerPrefs.GetInt ("BGSounds", 1) == 0) {
    //		PlayerPrefs.SetInt ("BGSounds", 1);
    //	}
    //	setSoundSettingsUI ();
    //}

    //public void setOtherSounds(bool val)
    //{
    //	foreach (AudioSource x in OtherSounds) {
    //		x.enabled = val;
    //	}
    //}
    //public void setEngineSounds(bool val)
    //{
    //	foreach (AudioSource x in CarSounds) {
    //		x.enabled = val;
    //	}
    //}
    //public void setBGSounds(bool val)
    //{
    //	foreach (AudioSource x in BGSounds) {
    //		x.enabled = val;
    //	}
    //}

    #endregion
    public void OnClickLikeUsOnFB ()
    {
    }
    public void OnClickLikePrivacyPolicy ()
    {
        Application.OpenURL("https://www.blogger.com/blog/post/edit/245232758540189140/3168143151348358653");
    }

    public void OnClickLikeRateUs ()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.game.tech.mega.ramp.stunt");
    }
    public void OnClickLikeMoreGames ()
    {
        Application.OpenURL("https://play.google.com/store/apps/developer?id=Game+Tech+Games");
    }
    public void OtherGame() {
        Application.OpenURL("https://play.google.com/store/apps/developer?id=Game+Tech+Games");
    }

    #region AdRewardSection

    //void RewardAdCompleteHandler (RewardedAdNetwork network, AdLocation location)
      //muk  PlayerPrefs.SetInt("rewardvdo", 1);
    

   

    #endregion


    public void RemoveADs()
    {
         TechnologyWings.InAppController.Instance.BuyInAppProduct(0);
    }


    public void UnlockEverythings()
    {
         TechnologyWings.InAppController.Instance.BuyInAppProduct(1);
    }


}
