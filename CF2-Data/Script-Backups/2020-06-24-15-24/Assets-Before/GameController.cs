using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using SimpleInputNamespace;

public class GameController : MonoBehaviour
{

	public SoundManager SM;
	public GameObject _internetOffPanel;
	public GameObject skipHide;
	public bool Test = false;
	public bool TestLevel = false;

	public GameObject GasPedal;
	public GameObject BrakePedal;
	public GameObject HandBrakePedal;
	public GameObject RightPedal;
	public GameObject LeftPedal;
	public float GameFinishTime = 200;
	public Text Timetxt;
	public Text CashEarned;
	public GameObject RaceStartLights;

	public GameObject[] Stars;
	public CheckPointController[] CPC;
	public int CurrentLevel = 0;
	public SteeringWheel SW;

	public GameObject StartingPoint;

	public enum Steering
	{
		Wheel,
		Buttons}

	;

	public Steering steer = Steering.Buttons;
	public GameObject LevelFinishCamera;
	public VehicleCameraControl VCC;
	public GameObject[] Cars;
	public GameObject CurrentCar;
	// Use this for initialization
	public GameObject CurrentCamShow;

	public GameObject[] Cameras;

	public GameObject LevelSelectionPanel;
	public GameObject AdsBreakPanel;
	public GameObject WatchVideoPanel;


	public Material[] skybox;


	public CustomEvents OnStartCamShowEndEvents;
	public CustomEvents OnOneTwoThreeGoEvents;
	public CustomEvents OnLevelCompleteEvents;
	public CustomEvents OnLevelCompleteEvents1;
	public CustomEvents OnLevelFailEvents;
	public CustomEvents OnAndroidBackBtnEvents;

	bool skip;

	public static GameController GC;

	void Start ()
	{
//		AdsManager._instance.HideBanner ();
		GC = this;
		skip = false;

		print ("skipppp" + skip);
		Application.targetFrameRate = 60;

		if (MainMenuController.CurrentLevel > 10) {
			SceneManager.LoadScene (0);
		}
		if (skip == false) {			
			Invoke("OnClickSkipCamShow",16f);
		}
		if (!Test)
		if (Application.platform == RuntimePlatform.Android) {
			SetCurrentLevel (MainMenuController.CurrentLevel);		
			if (TestLevel)
				CurrentLevel = MainMenuController.CurrentLevel;
			SetCarController ();
		} else if (Application.platform == RuntimePlatform.WindowsEditor) {
			SetCurrentLevel (MainMenuController.CurrentLevel);		
			if (TestLevel)
				CurrentLevel = MainMenuController.CurrentLevel;
			SetCarController ();
		}

		if (Test) {
			SetCurrentLevel (CurrentLevel);				
			VCC.playerCar = CurrentCar.transform;

		}

		if (CurrentLevel > 2) {
			StartingPoint.SetActive (false);
		}

		changeskybox ();

		setCameras ();

		TimerTimeSet ();

	}
	void TimerTimeSet(){
		if (CurrentLevel == 0) {
			GameFinishTime = 170;
		}else if (CurrentLevel == 1) {
			GameFinishTime = 150;
		}else if (CurrentLevel == 2) {
			GameFinishTime = 160;
		}else if (CurrentLevel == 3) {
			GameFinishTime = 155;
		}else if (CurrentLevel == 4) {
			GameFinishTime = 300;
		}else if (CurrentLevel == 5) {
			GameFinishTime = 400;
		}else if (CurrentLevel == 6) {
			GameFinishTime = 450;
		}else if (CurrentLevel == 7) {
			GameFinishTime = 500;
		}else if (CurrentLevel == 8) {
			GameFinishTime = 500;
		}else if (CurrentLevel == 9) {
			GameFinishTime = 500;
		}else if (CurrentLevel == 10) {
			GameFinishTime = 5255;
		}else if (CurrentLevel == 11) {
			GameFinishTime = 150;
		}else if (CurrentLevel == 12) {
			GameFinishTime = 200;
		}else if (CurrentLevel == 13) {
			GameFinishTime = 195;
		}else if (CurrentLevel == 14) {
			GameFinishTime = 190;
		}else if (CurrentLevel == 15) {
			GameFinishTime = 250;
		}else if (CurrentLevel == 16) {
			GameFinishTime = 250;
		}else if (CurrentLevel == 17) {
			GameFinishTime = 270;
		}else if (CurrentLevel == 18) {
			GameFinishTime = 300;
		}else if (CurrentLevel == 19) {
			GameFinishTime = 220;
		}else if (CurrentLevel == 20) {
			GameFinishTime = 250;
		}
	}
	void changeskybox(){
		int x = Random.Range(0, skybox.Length - 1);
		RenderSettings.skybox = skybox[x];
	}
	public void Exit_Yes ()
	{
		Time.timeScale = 1f;
		Application.Quit ();
	}

	public void Exit_No ()
	{
		Time.timeScale = 1f;
	}

	void setCameras ()
	{
		Cameras [0] = VCC.gameObject;
		Cameras [1] = CurrentCar.GetComponent<AttachedCameras> ().FrontCamera;
		Cameras [2] = CurrentCar.GetComponent<AttachedCameras> ().SideCamera;
	}

	void SetCarController ()
	{
		foreach (GameObject x in Cars) {
			x.SetActive (false);
		}
		Cars [MainMenuController.CurrentSelectedCar].SetActive (true);
		CurrentCar = Cars [MainMenuController.CurrentSelectedCar];
		VCC.playerCar = Cars [MainMenuController.CurrentSelectedCar].transform;


	}

	bool CanRespawn = true;

	public void RespawnCar ()
	{
		WatchVideoPanel.SetActive (true);
		Time.timeScale = 0.3f;
	}

	public void ReSpawnRewardAds(){
		if (Application.internetReachability != NetworkReachability.NotReachable) {
			if (AdsManager._instance != null) {
				AdsManager._instance.ShowUnityRewardedvideo ();
				PlayerPrefs.SetInt ("RespawnVideo", 1);
		}}else {
			_internetOffPanel.SetActive (true);
		}
	}

	public void RespawnBtnClick(){
		if (WatchVideoPanel.activeInHierarchy) {
			TimerTimeSet ();
		}
		Time.timeScale = 1f;
		WatchVideoPanel.SetActive(false);
		if (!CanRespawn)
			return;
		CanRespawn = false;
		Invoke ("setCanRespawnTrue", 0.5f);
		SM.PlayRespawn ();
		NumberOfRefreshedForCar++;
		if (CurrentCar.GetComponent<Rigidbody> () != null) {
			CurrentCar.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			CurrentCar.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
			CurrentCar.GetComponent<Rigidbody> ().Sleep ();
			CurrentCar.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			CurrentCar.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
		} else {
			Debug.Log ("Rigidbody Not Attacked to the car");
		}
		Levels [CurrentLevel].GetComponent<LevelSelectorScript> ().CPC.RespawnToLastCP ();
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
		SM.PlayLevelCompleteSound ();
		if (DoFinishin) {
			DoFinishin = false;
			int cc = PlayerPrefs.GetInt ("LevelsCompleted");
			if (MainMenuController.CurrentLevel + 2 > cc) {
				int LC = MainMenuController.CurrentLevel + 2;
				PlayerPrefs.SetInt ("LevelsCompleted", LC);
			}
			string s = "Level_" + (MainMenuController.CurrentLevel + 1).ToString () + "_Stars";
            int LS = PlayerPrefs.GetInt (s);
			if (NumberOfRefreshedForCar < 3) {
				PlayerPrefs.SetInt (s, 3);			
				Stars [0].SetActive (true);
				Stars [1].SetActive (true);
				Stars [2].SetActive (true);
			} else if (NumberOfRefreshedForCar < 6) {
				if (LS < 2) {				
					PlayerPrefs.SetInt (s, 2);
					Stars [0].SetActive (true);
					Stars [1].SetActive (true);
				}
			} else {
				if (LS < 1) {
					PlayerPrefs.SetInt (s, 1);
					Stars [0].SetActive (true);
				}
			}
			//Add Cash
			int Cash = PlayerPrefs.GetInt ("AvailableCash", 0);
			PlayerPrefs.SetInt ("AvailableCash", Cash + 500 + MainMenuController.CurrentLevel * 50);

			CashEarned.text = "$" + (500 + MainMenuController.CurrentLevel * 50).ToString ();
		}


		print ("complete hogya");
		foreach (GameObject c in Cameras) {
			c.SetActive (false);
		}
		LevelFinishCamera.SetActive (true);

		OnLevelCompleteEvents1.Invoke ();

		print ("level complete finish camera call");
		LevelFinishCamera.transform.parent = x.transform;
		LevelFinishCamera.transform.localPosition = new Vector3 (1f, 1f,-0.45f);
		Debug.Log ("OnFinishLine");	
		CurrentCar.GetComponent<AudioSource> ().DOFade (0, 2f);
		Invoke ("completeAdsBreak", 9f);
	}

	void completeAdsBreak(){
//		if (Application.internetReachability != NetworkReachability.NotReachable) {
//			if (AdsManager._instance != null) {
//				AdsBreakPanel.SetActive (true);
//				AdsManager._instance.ShowInterstitial ();
//				Invoke ("complete", 3f);
//		}} else {
//			AdsBreakPanel.SetActive (false);
//			Invoke ("complete", 1f);
//		}
		AdsBreakPanel.SetActive (true);
		Invoke ("complete", 2f);
	}
	void complete(){
//		AdsManager._instance.Showbanner ();
		AdsManager._instance.ShowInterstitial ();
		AdsBreakPanel.SetActive (false);
		OnLevelCompleteEvents.Invoke ();
	}

	public GameObject[] Levels;
	//Testing
	public void SetCurrentLevel (int i)
	{
		foreach (GameObject x in Levels) {
			x.SetActive (false);
		}
		Levels [i].SetActive (true);		
		CurrentLevel = i;
		Time.timeScale = 1;
	}

	public void RestartScene ()
	{
//		if (CurrentLevel % 2 == 0)
//			AdManager.admanagerInstance.ShowUnityVideoAd ();
//		else
//			AdManager.admanagerInstance.ShowInterstitial ();	
		
		SceneManager.LoadScene (0);
	}

	int currCam = 0;

	public void ChangeCamera ()
	{

		Cameras [currCam].SetActive (false);
		currCam++;
		if (currCam >= Cameras.Length)
			currCam = 0;	
		Cameras [currCam].SetActive (true);
	}


	public void OnClickNextLevel ()
	{
		MainMenuController.CurrentLevel++;
		SceneManager.LoadScene (1);

//		if (CurrentLevel % 2 == 0)
//			AdManager.admanagerInstance.ShowUnityVideoAd ();
//		else
//			AdManager.admanagerInstance.ShowInterstitial ();
		
	}

	public void OnClickMainMenu ()
	{
//		if (CurrentLevel % 2 == 0)
//			AdManager.admanagerInstance.ShowUnityVideoAd ();
//		else
//			AdManager.admanagerInstance.ShowInterstitial ();	

		SceneManager.LoadScene (0);
	}

	public void OnStartCamShowEnd ()
	{
	}

	public void OnClickSkipCamShow ()
	{
		if (skip == true)
			return;
		skip = true;
		skipHide.SetActive (false);
		CurrentCamShow.SetActive (false);
		OnStartCamShowEndEvents.Invoke ();	
		Cameras [currCam].SetActive (true);

		SM.PlayLevelStartSound ();
//		skip = true;
		StartCoroutine ("RaceStartLightsShow");
	}

	IEnumerator RaceStartLightsShow ()
	{
		foreach (Transform x in RaceStartLights.transform) {
			x.GetComponent <Image> ().color = Color.red;
		}
		yield return new WaitForSeconds (1f);
		foreach (Transform x in RaceStartLights.transform) {
			x.GetComponent <Image> ().color = Color.yellow;
		}
		yield return new WaitForSeconds (1f);
		foreach (Transform x in RaceStartLights.transform) {
			x.GetComponent <Image> ().color = Color.green;
		}
		yield return new WaitForSeconds (0.5f);
		RaceStartLights.SetActive (false);
		yield return new WaitForSeconds (0.2f);
		RaceStartLights.SetActive (true);
		yield return new WaitForSeconds (0.2f);
		RaceStartLights.SetActive (false);
		yield return new WaitForSeconds (0.2f);
		RaceStartLights.SetActive (true);
		yield return new WaitForSeconds (0.2f);
		RaceStartLights.SetActive (false);
		yield return new WaitForSeconds (0.2f);
		RaceStartLights.SetActive (true);
		yield return new WaitForSeconds (0.2f);
		RaceStartLights.SetActive (false);
		yield return new WaitForSeconds (0.2f);
		RaceStartLights.SetActive (true);
		yield return new WaitForSeconds (0.2f);
		RaceStartLights.SetActive (false);

		StartTimer ();
		OnOneTwoThreeGoEvents.Invoke ();
		yield return new WaitForSeconds (1f);

	
		yield return new WaitForSeconds (0.2f);
		RaceStartLights.SetActive (true);
		yield return new WaitForSeconds (0.2f);
		RaceStartLights.SetActive (false);
		yield return new WaitForSeconds (0.2f);
		RaceStartLights.SetActive (true);
		yield return new WaitForSeconds (0.2f);
		RaceStartLights.SetActive (false);
	}

	void StartTimer ()
	{
		StartCoroutine ("Timer");
	}

	public void PauseGame ()
	{
		if (Application.internetReachability != NetworkReachability.NotReachable) {
				AdsManager._instance.ShowunityAds ();
		} 
		StopTimer ();
		Time.timeScale = 0f;

				
	}

	public void RestartLevel ()
	{
		SceneManager.LoadScene (1);	
//		if (CurrentLevel % 2 == 0)
//			AdManager.admanagerInstance.ShowUnityVideoAd ();
//		else
//			AdManager.admanagerInstance.ShowInterstitial ();			
	}

	public void GotoMainMenu ()
	{
		SceneManager.LoadScene (0);	
//		if (CurrentLevel % 2 == 0)
//			AdManager.admanagerInstance.ShowUnityVideoAd ();
//		else
//			AdManager.admanagerInstance.ShowInterstitial ();				
	}

	public void ResumeGame ()
	{
		Time.timeScale = 1f;
		StartTimer ();
	}

	void StopTimer ()
	{
		StopCoroutine ("Timer");
	}

	IEnumerator Timer ()
	{
		while (true) {
			int min;
			int sec;

			min = (int)GameFinishTime / 60;
			sec = (int)GameFinishTime % 60;				
//			Debug.Log (min + " : " + sec );
			GameFinishTime--;
			if (sec < 10) {
				Timetxt.text = min.ToString () + " : 0" + sec.ToString ();
			} else
				Timetxt.text = min.ToString () + " : " + sec.ToString ();
			yield return new WaitForSeconds (1f);				

			if (GameFinishTime < 1) {		
				OnLevelFailEvents.Invoke ();
				SM.PlayLevelFail ();
				StopCoroutine ("Timer");
				Time.timeScale = 0f;
			}
		}
	}
	public void OnGameComplete(GameObject x){
		Cars [MainMenuController.CurrentSelectedCar].GetComponent<Rigidbody> ().isKinematic = true;
		print ("kinemitic true");
	}

	public void OnGameFailed(GameObject x){
		print ("game failed ads break true--");
		SM.BGMusic [0].GetComponent<AudioSource> ().volume = .2f;
		WatchVideoPanel.SetActive (true);
	}
	public void skipbtn(){	
		if (Application.internetReachability != NetworkReachability.NotReachable) {
				WatchVideoPanel.SetActive (false);
				AdsBreakPanel.SetActive (true);
//				AdsManager._instance.Showbanner ();
				AdsManager._instance.ShowInterstitial ();
				SM.BGMusic [0].GetComponent<AudioSource> ().volume = .5f;
				Invoke ("levelfailed", 2.5f);
			}else {
				AdsBreakPanel.SetActive (false);
				WatchVideoPanel.SetActive (false);
				Invoke ("levelfailed", 0f);
			} 

	}
	public void levelfailed(){
		print ("game failed ads break false--");
		AdsBreakPanel.SetActive (false);
		SM.BGMusic [0].GetComponent<AudioSource> ().volume = .2f;
		OnLevelFailEvents.Invoke ();
		SM.PlayLevelFail ();
		StopCoroutine ("Timer");
		Time.timeScale = 0f;
	}
	public void Car_Gas_Down ()
	{
		CurrentCar.GetComponent<CarControlCS> ().RaceBtnPressDown ();
	}

	public void Car_Gas_Up ()
	{
		CurrentCar.GetComponent<CarControlCS> ().RaceBtnPressUp ();
	}

	public void Car_Reverse_Down ()
	{
		CurrentCar.GetComponent<CarControlCS> ().RevBtnPressDown ();
	}

	public void Car_Reverse_Up ()
	{
		CurrentCar.GetComponent<CarControlCS> ().RevBtnPressUp ();
	}

	public void Car_Left_Down ()
	{
		CurrentCar.GetComponent<CarControlCS> ().LeftBtnPressDown ();
	}

	public void Car_Left_Up ()
	{
		CurrentCar.GetComponent<CarControlCS> ().LeftBtnPressUp ();
	}

	public void Car_Right_Down ()
	{
		CurrentCar.GetComponent<CarControlCS> ().RightBtnPressDown ();
	}

	public void Car_Right_Up ()
	{
		CurrentCar.GetComponent<CarControlCS> ().RightBtnPressUp ();
	}

	public void Car_Brake_Down ()
	{
		CurrentCar.GetComponent<CarControlCS> ().HandBrakeBtnPressDown ();
	}

	public void Car_Brake_Up ()
	{
		CurrentCar.GetComponent<CarControlCS> ().HandBrakeBtnPressUp ();
	}
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKey (KeyCode.Escape)) {
			Time.timeScale = 0f;
			OnAndroidBackBtnEvents.Invoke ();
		}


		if (Application.platform == RuntimePlatform.WindowsEditor) {
			if (Input.GetKeyDown (KeyCode.W)) {
				Car_Gas_Down ();
			}
			if (Input.GetKeyUp (KeyCode.W)) {
				Car_Gas_Up ();
			}
			if (Input.GetKeyDown (KeyCode.S)) {
				Car_Reverse_Down ();
			}
			if (Input.GetKeyUp (KeyCode.S)) {
				Car_Reverse_Up ();
			}

		}

		if(PlayerPrefs.GetInt("rewardvdo",0)==1){
			PlayerPrefs.SetInt ("rewardvdo", 0);
			if (PlayerPrefs.GetInt("RespawnVideo",0)==1) {
				RespawnBtnClick ();
				PlayerPrefs.SetInt ("RespawnVideo", 0);
			}
		}else if(PlayerPrefs.GetInt("rewardvdo",0)==2){
			PlayerPrefs.SetInt ("rewardvdo", 0);
		}
	}
}
