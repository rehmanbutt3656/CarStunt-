using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
//using SimpleInputNamespace;
using System;
using Random = UnityEngine.Random;


public class GameController : MonoBehaviour
{

    public SoundManager SM;
    public Button[] _removeAds;
    public GameObject _internetOffPanel;
    public GameObject skipHide;
    public bool Test = false;
    public bool TestLevel = false;

    public float GameFinishTime = 200;
    public Text Timetxt;

    public GameObject RaceStartLights;


    public int CurrentLevel = 0;
    //public SteeringWheel SW;

    public GameObject StartingPoint;

    public enum Steering
    {
        Wheel,
        Buttons
    }

    ;

    public Steering steer = Steering.Buttons;
    public GameObject LevelFinishCamera;
    public RCC_Camera VCC;
    public GameObject[] Cars;
    public GameObject CurrentCar;
    // Use this for initialization
    public GameObject CurrentCamShow;

    public GameObject[] Cameras;

    public GameObject LevelSelectionPanel;
    public GameObject AdsBreakPanel;
    public GameObject WatchVideoPanel;

    public Text _levelFText;

    public GameObject tiltSelect, touchSelect, steerSelect;


    public Material[] skybox;
    public GameObject[] ballon; //at start point anyone active


    public CustomEvents OnStartCamShowEndEvents;
    public CustomEvents OnOneTwoThreeGoEvents;
    public CustomEvents OnLevelCompleteEvents;
    public CustomEvents OnLevelCompleteEvents1;
    public CustomEvents OnLevelFailEvents;
    public CustomEvents OnAndroidBackBtnEvents;

    bool skip;

    public static GameController GC;

    public GameObject startPoint;
    public CheckPointController CPC;

    [Header("Level Complete Data")]
    [Space(6)]
    public LevelCompleteData _LCData;


    [Serializable]
    public class LevelCompleteData
    {
        public GameObject[] Stars;
        public Text timeScoreText, levelScoreText, rewardText, totalCashEarned, earnedStarsText;


    }

    private int PreviousCoinsAmountGen = 0;


    void Start ()
    {
        GC = this;
        skip = false;
        Application.targetFrameRate = 60;
        PreviousCoinsAmountGen = PlayerPrefs.GetInt("AvailableCash", 0);
        if (MainMenuController.CurrentLevel > 10)
        {
            SceneManager.LoadScene(2);
        }
        if (skip == false)
        {
            Invoke("OnClickSkipCamShow", 24.5f);
        }
        if (!Test)
            if (Application.platform == RuntimePlatform.Android)
            {
                SetCurrentLevel(MainMenuController.CurrentLevel);
                if (TestLevel)
                    CurrentLevel = MainMenuController.CurrentLevel;
                SetCarController();
            }
            else if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                SetCurrentLevel(MainMenuController.CurrentLevel);
                if (TestLevel)
                    CurrentLevel = MainMenuController.CurrentLevel;
                SetCarController();
            }

        if (Test)
        {
            SetCurrentLevel(CurrentLevel);
            VCC.playerCar = CurrentCar.GetComponent<RCC_CarControllerV3>();

        }

        if (CurrentLevel > 2)
        {
            StartingPoint.SetActive(false);
        }

        changeskybox();

        setCameras();

        TimerTimeSet();
        ChangeControls();
    }
    void TimerTimeSet ()
    {
        if (CurrentLevel == 0)
        {
            GameFinishTime = 160;
        }
        else if (CurrentLevel == 1)
        {
            GameFinishTime = 130;
        }
        else if (CurrentLevel == 2)
        {
            GameFinishTime = 140;
        }
        else if (CurrentLevel == 3)
        {
            GameFinishTime = 135;
        }
        else if (CurrentLevel == 4)
        {
            GameFinishTime = 250;
        }
        else if (CurrentLevel == 5)
        {
            GameFinishTime = 330;
        }
        else if (CurrentLevel == 6)
        {
            GameFinishTime = 410;
        }
        else if (CurrentLevel == 7)
        {
            GameFinishTime = 450;
        }
        else if (CurrentLevel == 8)
        {
            GameFinishTime = 430;
        }
        else if (CurrentLevel == 9)
        {
            GameFinishTime = 470;
        }
        else if (CurrentLevel == 10)
        {
            GameFinishTime = 480;
        }
        else if (CurrentLevel == 11)
        {
            GameFinishTime = 380;
        }
        else if (CurrentLevel == 12)
        {
            GameFinishTime = 290;
        }
        else if (CurrentLevel == 13)
        {
            GameFinishTime = 235;
        }
        else if (CurrentLevel == 14)
        {
            GameFinishTime = 200;
        }
        else if (CurrentLevel == 15)
        {
            GameFinishTime = 190;
        }
        else if (CurrentLevel == 16)
        {
            GameFinishTime = 220;
        }
        else if (CurrentLevel == 17)
        {
            GameFinishTime = 200;
        }
        else if (CurrentLevel == 18)
        {
            GameFinishTime = 215;
        }
        else if (CurrentLevel == 19)
        {
            GameFinishTime = 190;
        }
        else if (CurrentLevel == 20)
        {
            GameFinishTime = 190;
        }
    }
    void changeskybox ()
    {
        int x = Random.Range(0, skybox.Length - 1);
        RenderSettings.skybox = skybox[x];
    }
    void changeBallon() {
        int x = Random.Range(0, ballon.Length - 1);
        ballon[x].SetActive(true);
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

    void setCameras ()
    {
        Cameras[0] = VCC.gameObject;
        Cameras[1] = CurrentCar.GetComponent<AttachedCameras>().FrontCamera;
        Cameras[2] = CurrentCar.GetComponent<AttachedCameras>().SideCamera;
    }

    void SetCarController ()
    {
        foreach (GameObject x in Cars)
        {
            x.SetActive(false);
        }
        Cars[MainMenuController.CurrentSelectedCar].SetActive(true);
        CurrentCar = Cars[MainMenuController.CurrentSelectedCar];
        CurrentCar.transform.position = startPoint.transform.position;
        CurrentCar.transform.rotation = startPoint.transform.rotation;
        VCC.playerCar = Cars[MainMenuController.CurrentSelectedCar].GetComponent<RCC_CarControllerV3>();
    }

    bool CanRespawn = true;

    public void RespawnCar ()
    {
        WatchVideoPanel.SetActive(true);
        Time.timeScale = 0.3f;
    }
    //Commented for ads
    public void OnClickLikeRateUs ()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.game.tech.mega.ramp.stunt");
    }    public void OtherGame()
    {
        Application.OpenURL("https://play.google.com/store/apps/developer?id=Game+Tech+Games");
    }
    public void ReSpawnRewardAds ()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            //AdsManager._instance.ShowUnityRewardedvideo();
            // AdManager.ShowRewardBasedVideo();
            // AdsManagerAR.Instance.ShowRewardedAd();
            //ar TechnologyWings.AdManager.ShowRewardBasedVideo(RespawnBtnClick);
            //AdmobAdsManager.Instance.ShowRewardedVideo(RespawnBtnClick);
            FindObjectOfType<MediationHandler>().ShowRewardedVideo(RespawnBtnClick);
            PlayerPrefs.SetInt("RespawnVideo", 1);
           // Invoke("RespawnBtnClick",1);
        }
        else
        {
            _internetOffPanel.SetActive(true);
        }
    }

    public void RespawnBtnClick ()
    {
        if (WatchVideoPanel.activeInHierarchy)
        {
            TimerTimeSet();
        }
        Time.timeScale = 1f;
        WatchVideoPanel.SetActive(false);
        if (!CanRespawn)
            return;
        CanRespawn = false;
        Invoke("setCanRespawnTrue", 0.5f);
        SM.PlayRespawn();
        NumberOfRefreshedForCar++;
        if (CurrentCar.GetComponent<Rigidbody>() != null)
        {
            CurrentCar.GetComponent<Rigidbody>().velocity = Vector3.zero;
            CurrentCar.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            CurrentCar.GetComponent<Rigidbody>().Sleep();
            CurrentCar.GetComponent<Rigidbody>().velocity = Vector3.zero;
            CurrentCar.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        else
        {
            Debug.Log("Rigidbody Not Attacked to the car");
        }
        Levels[CurrentLevel].GetComponent<LevelSelectorScript>().CPC.RespawnToLastCP();
        //		 CPC[CurrentLevel].RespawnToLastCP ();
    }

    void setCanRespawnTrue ()
    {
        CanRespawn = true;
    }

    bool DoFinishin = true;
    float NumberOfRefreshedForCar = 0;

    public void OnFinishLine (GameObject x)
    {
        SM.PlayLevelCompleteSound();
        if (DoFinishin)
        {
            DoFinishin = false;
            int cc = PlayerPrefs.GetInt("LevelsCompleted");
            if (MainMenuController.CurrentLevel + 2 > cc)
            {
                int LC = MainMenuController.CurrentLevel + 2;
                PlayerPrefs.SetInt("LevelsCompleted", LC);
            }
            string s = "Level_" + (MainMenuController.CurrentLevel + 1).ToString() + "_Stars";
            int LS = PlayerPrefs.GetInt(s);
            if (NumberOfRefreshedForCar < 3)
            {
                PlayerPrefs.SetInt(s, 3);
                _LCData.Stars[0].SetActive(true);
                _LCData.Stars[1].SetActive(true);
                _LCData.Stars[2].SetActive(true);
            }
            else if (NumberOfRefreshedForCar > 3 && NumberOfRefreshedForCar < 6)
            {
                if (LS < 2)
                {
                    PlayerPrefs.SetInt(s, 2);
                    _LCData.Stars[0].SetActive(true);
                    _LCData.Stars[1].SetActive(true);
                }
            }
            else
            {
                if (LS < 1)
                {
                    PlayerPrefs.SetInt(s, 1);
                    _LCData.Stars[0].SetActive(true);
                }
            }

            _LCData.earnedStarsText.text = "YOU HAVE EARNED " + PlayerPrefs.GetInt(s, 3) + "STARS";
            //_LCData.totalCashEarned.text = "$" + (500 + MainMenuController.CurrentLevel * 50).ToString();
        }

        foreach (GameObject c in Cameras)
        {
            c.SetActive(false);
        }
        LevelFinishCamera.SetActive(true);
        OnLevelCompleteEvents1.Invoke();
        print("level complete finish camera call");
        LevelFinishCamera.transform.parent = x.transform;
        LevelFinishCamera.transform.localPosition = new Vector3(1f, 11f, -0.45f);
        Debug.Log("OnFinishLine");
        CurrentCar.GetComponent<AudioSource>().DOFade(0, 2f);
        Invoke("completeAdsBreak", 9f);
    }

    private IEnumerator UpdateEarnedAmount (bool isprevZero, Text CurrentCoinsText, int _score, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);
        // Animation for increasing and decreasing of coins amount
        float seconds = duration;
        float elapsedTime = 0;

        if (isprevZero)
            PreviousCoinsAmountGen = 0;

        while (elapsedTime < seconds)
        {
            CurrentCoinsText.text = Mathf.Floor(Mathf.Lerp(PreviousCoinsAmountGen, _score, (elapsedTime / seconds))).ToString();
            elapsedTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
        if (!isprevZero)
            PreviousCoinsAmountGen = _score;
        CurrentCoinsText.text = _score.ToString();
    }

    void completeAdsBreak ()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            AdsBreakPanel.SetActive(true);
            Invoke("complete", 2f);
        }
        else
        {
            AdsBreakPanel.SetActive(false);
            Invoke("complete", 0f);
        }
    }
    void complete ()
    {

        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            //   AdManager.ShowInterstitial();

            // AdsManagerAR.Instance.ShowInterstitialAd();
             AdmobAdsManager.Instance.ShowInterstitial();
            AdsBreakPanel.SetActive(false);
        }
        //Add Cash
        AdsBreakPanel.SetActive(false);

        int timeScore = (int)((GameFinishTime / 60) * 1000);
        StartCoroutine(UpdateEarnedAmount(true, _LCData.timeScoreText, timeScore, 3.5f, 0.01f));
        int levelScore = (MainMenuController.CurrentLevel + 1) * 100;
        StartCoroutine(UpdateEarnedAmount(true, _LCData.levelScoreText, levelScore, 4.0f, 0.5f));
        int rewardScore = 500 + MainMenuController.CurrentLevel * 50;
        StartCoroutine(UpdateEarnedAmount(true, _LCData.rewardText, rewardScore, 5.0f, 0.7f));
        int totalCash = timeScore + levelScore + rewardScore;
        StartCoroutine(UpdateEarnedAmount(false, _LCData.totalCashEarned, totalCash, 5.0f, 1.0f));

        PlayerPrefs.SetInt("AvailableCash", totalCash + PlayerPrefs.GetInt("AvailableCash"));
        OnLevelCompleteEvents.Invoke();
    }

   

    public GameObject[] Levels;
    //Testing
    public void SetCurrentLevel (int i)
    {
        foreach (GameObject x in Levels)
        {
            x.SetActive(false);
        }
        Levels[i].SetActive(true);
        startPoint = Levels[i].transform.Find("startPos").gameObject;
        CPC = Levels[i].transform.GetComponentInChildren<CheckPointController>();
        CPC.LastCheckPoint = startPoint.GetComponent<CheckPoint>();
        CurrentLevel = i;
        Time.timeScale = 1;
    }

    public void ChangeControls ()
    {
        int controlSetting = PlayerPrefs.GetInt("ControlSettings", 0);
        switch (controlSetting)
        {
            case 0: RCC.SetMobileController(RCC_Settings.MobileController.TouchScreen); touchSelect.SetActive(true); break;
            case 1: RCC.SetMobileController(RCC_Settings.MobileController.SteeringWheel); steerSelect.SetActive(true); break;
            case 2: RCC.SetMobileController(RCC_Settings.MobileController.Gyro); tiltSelect.SetActive(true); break;
        }

    }

    public void TiltSelect ()
    {
        tiltSelect.SetActive(true);
        steerSelect.SetActive(false);
        touchSelect.SetActive(false);
        PlayerPrefs.SetInt("ControlSettings", 2);
        RCC.SetMobileController(RCC_Settings.MobileController.Gyro);
    }

    public void SteerSelect ()
    {
        steerSelect.SetActive(true);
        tiltSelect.SetActive(false);
        touchSelect.SetActive(false);
        PlayerPrefs.SetInt("ControlSettings", 1);
        RCC.SetMobileController(RCC_Settings.MobileController.SteeringWheel);
    }

    public void TouchSelect ()
    {
        touchSelect.SetActive(true);
        tiltSelect.SetActive(false);
        steerSelect.SetActive(false);
        PlayerPrefs.SetInt("ControlSettings", 0);
        RCC.SetMobileController(RCC_Settings.MobileController.TouchScreen);
    }

    public void RestartScene ()
    {
        //		if (CurrentLevel % 2 == 0)
        //			AdManager.admanagerInstance.ShowUnityVideoAd ();
        //		else
        //			AdManager.admanagerInstance.ShowInterstitial ();	
        // AdsManagerAR.Instance.ShowInterstitialAd();
       // TechnologyWings.AdManager.ShowInterstitial();
        SceneManager.LoadScene(2);
    }

    int currCam = 0;

    public void ChangeCamera ()
    {

        Cameras[currCam].SetActive(false);
        currCam++;
        if (currCam >= Cameras.Length)
            currCam = 0;
        Cameras[currCam].SetActive(true);
    }


    public void OnClickNextLevel ()
    {
        AdmobAdsManager.Instance.ShowInterstitial();
        MainMenuController.CurrentLevel++;
        SceneManager.LoadScene(2);
    }

    public void OnClickMainMenu ()
    {
        SceneManager.LoadScene(2);
    }
    public void DoubleCoin() {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            //AdsManager._instance.ShowUnityRewardedvideo();
            //   AdManager.ShowRewardBasedVideo();
            //   AdsManagerAR.Instance.ShowRewardedAd();
            AdmobAdsManager.Instance.ShowInterstitial();
            PlayerPrefs.SetInt("FreeCashLC", 1);
        }
        else
        {
            _internetOffPanel.SetActive(true);
        }
    }
    public void OnStartCamShowEnd ()
    {

    }

    public void OnClickSkipCamShow ()
    {
        if (skip == true)
            return;
        skip = true;
        Debug.Log("Skip Pressed");
        skipHide.SetActive(false);
        CurrentCamShow.SetActive(false);
        OnStartCamShowEndEvents.Invoke();
        Cameras[currCam].SetActive(true);

        SM.PlayLevelStartSound();
        //		skip = true;
        StartCoroutine("RaceStartLightsShow");
    }

    IEnumerator RaceStartLightsShow ()
    {
        foreach (Transform x in RaceStartLights.transform)
        {
            x.GetComponent<Image>().color = Color.red;
        }
        yield return new WaitForSeconds(1f);
        foreach (Transform x in RaceStartLights.transform)
        {
            x.GetComponent<Image>().color = Color.yellow;
        }
        yield return new WaitForSeconds(1f);
        foreach (Transform x in RaceStartLights.transform)
        {
            x.GetComponent<Image>().color = Color.green;
        }
        yield return new WaitForSeconds(0.5f);
        RaceStartLights.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        RaceStartLights.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        RaceStartLights.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        RaceStartLights.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        RaceStartLights.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        RaceStartLights.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        RaceStartLights.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        RaceStartLights.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        RaceStartLights.SetActive(false);

        StartTimer();
        OnOneTwoThreeGoEvents.Invoke();
        RCC_UIDashboardDisplay.rccDisplay._showDashboard = true;
        yield return new WaitForSeconds(1f);


        yield return new WaitForSeconds(0.2f);
        RaceStartLights.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        RaceStartLights.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        RaceStartLights.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        RaceStartLights.SetActive(false);
    }

    void StartTimer ()
    {
        StartCoroutine("Timer");
    }

    public void PauseGame ()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            //AdsManager._instance.ShowunityAds();
            //   AdManager.ShowInterstitial();
            //  AdsManagerAR.Instance.ShowInterstitialAd();
            AdmobAdsManager.Instance.ShowInterstitial();
        }
        StopTimer();
        Time.timeScale = 0f;


    }

    public void RestartLevel ()
    {
        SceneManager.LoadScene(2);

        //		if (CurrentLevel % 2 == 0)
        //			AdManager.admanagerInstance.ShowUnityVideoAd ();
        //		else
        //			AdManager.admanagerInstance.ShowInterstitial ();		
        //			
        //			
        //	
        //				
    }

    public void GotoMainMenu ()
    {
        SceneManager.LoadScene(2);
        //TechnologyWings.AdManager.ShowInterstitial();
        // AdsManagerAR.Instance.ShowInterstitialAd();
        //		if (CurrentLevel % 2 == 0)
        //			AdManager.admanagerInstance.ShowUnityVideoAd ();
        //		else
        //			AdManager.admanagerInstance.ShowInterstitial ();				
    }

    public void ResumeGame ()
    {
        Time.timeScale = 1f;
        StartTimer();
    }

    void StopTimer ()
    {
        StopCoroutine("Timer");
    }

    IEnumerator Timer ()
    {
        while (true)
        {
            int min;
            int sec;

            min = (int)GameFinishTime / 60;
            sec = (int)GameFinishTime % 60;
            //			Debug.Log (min + " : " + sec );
            GameFinishTime--;
            if (sec < 10)
            {
                Timetxt.text = min.ToString() + " : 0" + sec.ToString();
            }
            else
                Timetxt.text = min.ToString() + " : " + sec.ToString();
            yield return new WaitForSeconds(1f);

            if (GameFinishTime < 1)
            {
                OnLevelFailEvents.Invoke();
                SM.PlayLevelFail();

                Debug.Log("Times");
                int levelScore = (MainMenuController.CurrentLevel + 1) * 100;
                StartCoroutine(UpdateEarnedAmount(true, _levelFText, levelScore, 2.5f, 0.1f));
                PlayerPrefs.SetInt("AvailableCash", PlayerPrefs.GetInt("AvailableCash", 0) + levelScore);
                StopCoroutine("Timer");
            }
        }
    }
    public void OnGameComplete (GameObject x)
    {
        Cars[MainMenuController.CurrentSelectedCar].GetComponent<Rigidbody>().isKinematic = true;
        print("kinemitic true");
    }

    public void OnGameFailed (GameObject x)
    {
        SM.BGMusic[0].GetComponent<AudioSource>().volume = .2f;
        WatchVideoPanel.SetActive(true);
    }
    public void skipbtn ()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            WatchVideoPanel.SetActive(false);
            AdsBreakPanel.SetActive(true);
            SM.BGMusic[0].GetComponent<AudioSource>().volume = .5f;
            Invoke("levelfailed", 1.5f);
        }
        else
        {
            AdsBreakPanel.SetActive(false);
            WatchVideoPanel.SetActive(false);
            Invoke("levelfailed", 0f);
        }

    }
    public void levelfailed ()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            // AdManager.ShowInterstitial();

            // AdsManagerAR.Instance.ShowInterstitialAd();
            AdmobAdsManager.Instance.ShowInterstitial();
            AdsBreakPanel.SetActive(false);
        }
        AdsBreakPanel.SetActive(false);
        WatchVideoPanel.SetActive(false);
        SM.BGMusic[0].GetComponent<AudioSource>().volume = .2f;
        OnLevelFailEvents.Invoke();
        SM.PlayLevelFail();
        int levelScore = (MainMenuController.CurrentLevel + 1) * 100;
        StartCoroutine(UpdateEarnedAmount(true, _levelFText, levelScore, 2.5f, 0.5f));
        PlayerPrefs.SetInt("AvailableCash", PlayerPrefs.GetInt("AvailableCash", 0) + levelScore);
        StopCoroutine("Timer");
        Debug.Log("Called times ");
    }
    void Update ()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 1)
        {
            _removeAds[0].interactable = false;
            _removeAds[1].interactable = false;
            _removeAds[2].interactable = false;
        }

        //if (ControlFreak2.CF2Input.GetKey(KeyCode.Escape))
        //{
        //    Time.timeScale = 0f;
        //    OnAndroidBackBtnEvents.Invoke();
        //}
        if (PlayerPrefs.GetInt("rewardvdo", 0) == 1)
        {
            PlayerPrefs.SetInt("rewardvdo", 0);
            if (PlayerPrefs.GetInt("RespawnVideo", 0) == 1)
            {
                RespawnBtnClick();
                PlayerPrefs.SetInt("RespawnVideo", 0);
                PlayerPrefs.SetInt("rewardvdo", 0);
            }
            if (PlayerPrefs.GetInt("FreeCashLC", 0) == 1)
            {

                PlayerPrefs.SetInt("AvailableCash", PlayerPrefs.GetInt("AvailableCash") + 500);
                int totalCash = PlayerPrefs.GetInt("AvailableCash");
                StartCoroutine(UpdateEarnedAmount(false, _LCData.totalCashEarned, totalCash, 5.0f, 1.0f));

                PlayerPrefs.SetInt("rewardvdo", 0);
                PlayerPrefs.SetInt("FreeCashLC", 0);
            }
        }
        else if (PlayerPrefs.GetInt("rewardvdo", 0) == 2)
        {
            PlayerPrefs.SetInt("rewardvdo", 0);
        }
    }


    #region AdRewardSection
    //void RewardAdCompleteHandler (RewardedAdNetwork network, AdLocation location)
     //muk   PlayerPrefs.SetInt("rewardvdo", 1);
   
    #endregion

}
