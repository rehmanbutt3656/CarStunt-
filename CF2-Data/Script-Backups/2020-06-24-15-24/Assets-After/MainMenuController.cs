using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

	public GameObject _Adspanel;
	public GameObject _internetOffPanel;
	public GameObject[] Cars;
	public GameObject[] TopSpeed;
	public GameObject[] Handling;
	public GameObject[] Acceleration;
	public GameObject[] unlockAdsBtn;
	public GameObject LevelsParent;
	public int LevelsCompleted = 0;

	public static int CurrentLevel = 0;
	public static int CurrentCar = 0;
	public static int CurrentSelectedCar = 0;

	public GameObject LevelLoadingPanel;
	public GameObject closePanel;
	public Text AvailableCashtxtMainMenu,AvailableCashtxt,AvailableCashtxtLevelSelection;
	public Text CarPricetxt;
	public GameObject FreeCash,CarPurchaseButton;
	public GameObject NotEnoughCash,NotEnoughCash1;
	public int Cash;
	public bool[] CarPurchased;
	public int[] CarPrice;

	public GameObject CoinsAnimationMainMenu,CoinsAnimation;
	public GameObject CoinsAnimationParentMainMenu,CoinsAnimationParent;

	public GameObject CarEngineTick;
	public GameObject OtherSoundsTick;
	public GameObject BGSoundTick;

	public AudioSource UnlockCarSound;

	public AudioSource[] CarSounds;
	public AudioSource[] OtherSounds;
	public AudioSource[] BGSounds;

	int counter;

	public CustomEvents AndroidBackButtonEvents;
	// Use this for initialization
	void Start () {
//		PlayerPrefs.DeleteAll();

		counter = 0;
		Time.timeScale = 1f;

//		if (!PlayerPrefs.HasKey ("LevelsCompleted")) {
			LevelsCompleted	= PlayerPrefs.GetInt ("LevelsCompleted",1);
//		}
		Cash = PlayerPrefs.GetInt ("AvailableCash",0);
		AvailableCashtxt.text = "$"+Cash.ToString ();
		AvailableCashtxtMainMenu.text = "$"+Cash.ToString ();
		AvailableCashtxtLevelSelection.text = "$"+Cash.ToString ();
		UpdateCarsPurchased ();

		//SoundSettings
		PlayerPrefs.GetInt ("EngineSound",1);		
		PlayerPrefs.GetInt ("OtherSounds",1);		
		PlayerPrefs.GetInt ("BGSounds",1);		
		setSoundSettingsUI ();

		setCarPurchaseButton (0);
		SetLevels ();
//		StartCoroutine ("Tester");
	}

	void Update()
	{
		if (ControlFreak2.CF2Input.GetKey (KeyCode.Escape)) {
			AndroidBackButtonEvents.Invoke ();
			Time.timeScale = 0f;
		}

		if(PlayerPrefs.GetInt("rewardvdo",0)==1){
			PlayerPrefs.SetInt ("rewardvdo", 0);

			if (PlayerPrefs.GetInt("RewardVideoMainMenu",0)==1) {
				RewardClick++;
				Invoke ("RewardVideoCompleteMainMenu",1f);
				PlayerPrefs.SetInt ("RewardVideoMainMenu", 0);
			}
			if (PlayerPrefs.GetInt("RewardVideoVehicleSelection",0)==1) {
				RewardClick++;
				Invoke ("RewardVideoComplete",1f);
				PlayerPrefs.SetInt ("RewardVideoVehicleSelection", 0);
			}
			if (PlayerPrefs.GetInt("UnlocklvlReward",0)==1) {
				print ("counter =" + counter);
				//if (counter == 2) {
					Invoke ("currentlevell",1f);
					PlayerPrefs.SetInt ("UnlocklvlReward", 0);
				//	counter = 0;
				//}
				print ("counter 1=" + counter);
			}
		}else if(PlayerPrefs.GetInt("rewardvdo",0)==2){
			PlayerPrefs.SetInt ("rewardvdo", 0);
		}

	}
	public void Exit_Btn(){
		closePanel.SetActive (true);
	}
	public void Exit_Yes()
	{
		Time.timeScale = 1f;
		Application.Quit ();
	}
	public void Exit_No()
	{
		Time.timeScale = 1f;
	}

	public void UnlockLevelRewardedVideo(){
		if (Application.internetReachability != NetworkReachability.NotReachable) {
			if (AdsManager._instance != null) {
				AdsManager._instance.ShowUnityRewardedvideo ();
				counter++;
				PlayerPrefs.SetInt ("UnlocklvlReward", 1);
			}
		} else {
			_internetOffPanel.SetActive (true);
		}
	}
	public void currentlevell(){
		int cc = PlayerPrefs.GetInt ("LevelsCompleted");
		print (cc + "curr :"+CurrentLevel );
		if (CurrentLevel + 2 > cc) {
			int LC = CurrentLevel + 1;
			PlayerPrefs.SetInt ("LevelsCompleted", LC);
			print ("currnt : " + LC);
		}
		string s = "Level_" + (CurrentLevel + 1).ToString () + "_Stars";
		print (s);
		PlayerPrefs.SetInt (s,3);
		CurrentLevel = PlayerPrefs.GetInt ("LevelsCompleted");
		LevelsCompleted +=1 ;
		SetLevels ();
	}

	void SetLevels()
	{
		for(int l=0 ; l<LevelsParent.transform.childCount ; l++)
		{
			if (l < LevelsCompleted) {//Turn On
//				print(l);
				LevelsParent.transform.GetChild (l).gameObject.GetComponent<ButtonRef> ().setText ((l+1).ToString());
				string s = "Level_" + (l + 1).ToString () + "_Stars";
				//Debug.Log (s);
				int x = PlayerPrefs.GetInt (s);
//				print("x value" + x);
				if (x == 0)
					LevelsParent.transform.GetChild (l).gameObject.GetComponent<ButtonRef> ().Set1Star ();
				if (x == 1)
					LevelsParent.transform.GetChild (l).gameObject.GetComponent<ButtonRef> ().Set1Star ();
				if (x == 2)
					LevelsParent.transform.GetChild (l).gameObject.GetComponent<ButtonRef> ().Set2Star ();
				if (x == 3)
					LevelsParent.transform.GetChild (l).gameObject.GetComponent<ButtonRef> ().Set3Star ();
//				LevelsParent.transform.GetChild (l).gameObject.GetComponent<Button>().interactable = true;
			} else {//Locked
				LevelsParent.transform.GetChild (l).gameObject.GetComponent<ButtonRef>().LockLevel();
			}
		}
	}		

	IEnumerator Tester()
	{
		yield return new WaitForSeconds (5f);
		purchaseCar (2);			
	}

	void UpdateCarsPurchased()
	{
		for(int i=0 ; i<CarPurchased.Length ; i++){
			int c;
			if (i == 0) {
				c = 1;//PlayerPrefs.GetInt ("AvailableCash",0);			
			} else {
				c = CheckCarPurchased(i);
//				Debug.Log ("Car Purchased at: " + i +  " is " + c);
			}
			if (c == 1) {
				CarPurchased[i] = true;
			} else
				CarPurchased[i] = false;
		}
	}

	public int CheckCarPurchased(int carNum)
	{
		string x = "Car" + carNum.ToString ();
		return PlayerPrefs.GetInt (x, 0);
	}

	public void purchaseCar(int CarNum)
	{
		string x = "Car" + CarNum.ToString ();
		PlayerPrefs.SetInt (x, 1);
		UpdateCarsPurchased ();
	}


	public void LoadLevel(int i)
	{
		if (Application.internetReachability != NetworkReachability.NotReachable) {
			if (AdsManager._instance != null) {
				_Adspanel.SetActive (true);
				AdsManager._instance.ShowInterstitial ();	
			}			
		} else {
			_Adspanel.SetActive (false);
		}

		Debug.Log ("LoadSceneLevelBEfore");
		LevelLoadingPanel.SetActive (true);
		CurrentLevel = i;
		Debug.Log ("LoadSceneLevelAfter");
		Invoke ("LoadSceneLevel", 1.5f);
		Debug.Log ("LoadSceneLevelSafaAfter");
	}


	void LoadSceneLevel()
	{
		//AdManager.admanagerInstance.OnClickHideBanner ();

		//if (CurrentLevel % 2 == 0)
		//	AdManager.admanagerInstance.ShowInterstitial ();
		//else
		//	AdManager.admanagerInstance.ShowUnityVideoAd ();
		
		//Debug.Log ("LoadSceneLevel");
		SceneManager.LoadScene (1);
	}


	public int i = 0;
	public int j = 0;
	public int k = 0;
	public int l = 0;
	public void TopSpeedd()
	{	j = 0;
		foreach (GameObject x in TopSpeed) {
			j++;
			print("next cars : " + j);
			if (x.gameObject.activeInHierarchy) {
				x.SetActive (false);
				break;
			}}
		if (j == TopSpeed.Length) {
			j = 0;
		}
		TopSpeed [j].SetActive (true);

		CurrentCar = j;

		TopSpeed [j].transform.localPosition = new Vector3(0,0.1f,-4.03f);
		TopSpeed [j].transform.DOLocalMoveY (0,0.4f);
	}
	public void TopHandlingg()
	{	k = 0;
		foreach (GameObject x in Handling) {
			k++;
			print("next cars : " + k);
			if (x.gameObject.activeInHierarchy) {
				x.SetActive (false);
				break;
			}}
		if (k == Handling.Length) {
			k = 0;
		}
		Handling [k].SetActive (true);

		CurrentCar = k;

		Handling [k].transform.localPosition = new Vector3(0,0.1f,-4.03f);
		Handling [k].transform.DOLocalMoveY (0,0.4f);
	}
	public void TopAcceleration()
	{	l = 0;
		foreach (GameObject x in Acceleration) {
			l++;
			print("next cars : " + l);
			if (x.gameObject.activeInHierarchy) {
				x.SetActive (false);
				break;
			}}
		if (l == Acceleration.Length) {
			l = 0;
		}
		Acceleration [l].SetActive (true);

		CurrentCar = l;

		Acceleration [l].transform.localPosition = new Vector3(0,0.1f,-4.03f);
		Acceleration [l].transform.DOLocalMoveY (0,0.4f);
	}
	public void NextCar()
	{
		i = 0;
		foreach (GameObject x in Cars) {
			i++;
			print("next cars : " + i);
			if (x.gameObject.activeInHierarchy) {
				x.SetActive (false);
				break;
			}
		}

		if (i == Cars.Length) {
			i = 0;
		} 

		Cars [i].SetActive (true);
		CurrentCar = i;
//		Cars [i].transform.localScale = Vector3.zero;
//		Cars [i].transform.DOScale (1f,0.4f);
		Cars [i].transform.localPosition = new Vector3(0,0.1f,-4.03f);
		Cars [i].transform.DOLocalMoveY (0,0.4f);
		setCarPurchaseButton (i);

		TopSpeedd ();
		TopHandlingg ();
		TopAcceleration ();
	}
	public void PreviousCar()
	{
		i = 0;
		foreach (GameObject x in Cars) {
			i++;
			if (x.gameObject.activeInHierarchy) {
				x.SetActive (false);
				break;
			}
		}
		i--;
		i--;
		print("prev cars : " + i);
		if (i < 0) {
			i = Cars.Length-1;
		} 
		Cars [i].SetActive (true);
		CurrentCar = i;
//		Cars [i].transform.localScale = Vector3.zero;
//		Cars [i].transform.DOScale (1f,0.4f);
		Cars [i].transform.localPosition = new Vector3(0,0.1f,-4.03f);
		Cars [i].transform.DOLocalMoveY (0,0.4f);

		setCarPurchaseButton (i);

		TopSpeedd ();
		TopHandlingg ();
		TopAcceleration ();
	}

	void setCarPurchaseButton(int i)
	{
		if (i != 0) {
			int x = CheckCarPurchased (i);
			if (x == 1) {
				CarPurchaseButton.SetActive (false);
				FreeCash.SetActive (true);
				CurrentSelectedCar = i;
			} else if (x == 0) {				
				CarPurchaseButton.SetActive (true);
				FreeCash.SetActive (false);
				CarPricetxt.text = "$" + CarPrice [i].ToString ();
			}
		} else {
			CarPurchaseButton.SetActive (false);
			FreeCash.SetActive (true);
			CurrentSelectedCar = 0;
		}
	}

	public void OnClickPurchaeCar()
	{
		if (CarPrice [CurrentCar] > Cash) {
			NotEnoughCash.SetActive (true);
			Debug.Log ("Not Enough Cash");
		} else {
			Debug.Log ("Car Bought");
			Cash -= CarPrice [CurrentCar];
			PlayerPrefs.SetInt ("AvailableCash",Cash);
			AvailableCashtxt.text = "$" + Cash.ToString ();
			AvailableCashtxtMainMenu.text = "$"+Cash.ToString ();
			AvailableCashtxtLevelSelection.text = "$"+Cash.ToString ();
			purchaseCar (CurrentCar);
			CarPurchaseButton.SetActive (false);
			CurrentSelectedCar = CurrentCar;

			UnlockCarSound.Play ();
		}
	}
	int RewardClick = 0;

	public void OnClickWatchRewardVideo()
	{
		if (Application.internetReachability != NetworkReachability.NotReachable) {
			if (AdsManager._instance != null) {
				AdsManager._instance.ShowUnityRewardedvideo ();
				PlayerPrefs.SetInt ("RewardVideoVehicleSelection", 1);
		}}else {
			_internetOffPanel.SetActive (true);
		}
	}
		

	public void OnClickWatchRewardVideoMainMenu()
	{
		if (Application.internetReachability != NetworkReachability.NotReachable) {
			if (AdsManager._instance != null) {
				AdsManager._instance.ShowUnityRewardedvideo ();
				PlayerPrefs.SetInt ("RewardVideoMainMenu", 1);
			}
		} else {
			_internetOffPanel.SetActive (true);
		}
	}

	public void MainMenuFreeCash(){
		NotEnoughCash1.SetActive (true);
	}

	public void RewardVideoComplete()
	{
		GameObject x = Instantiate (CoinsAnimation,CoinsAnimationParent.transform) as GameObject;
		x.SetActive (true);
		StartCoroutine ("IncreaseCoins");
	}
	public void RewardVideoCompleteMainMenu()
	{
		GameObject x = Instantiate (CoinsAnimationMainMenu,CoinsAnimationParentMainMenu.transform) as GameObject;
		x.SetActive (true);
		StartCoroutine ("IncreaseCoins");
	}

	IEnumerator IncreaseCoins()
	{
		for (int i = 0; i < 5; i++) {
			yield return new WaitForSeconds (0.2f);
			Cash += 100;
			AvailableCashtxt.text = "$"+Cash.ToString ();
			AvailableCashtxtMainMenu.text = "$"+Cash.ToString ();
			AvailableCashtxtLevelSelection.text = "$"+Cash.ToString ();
		}
		PlayerPrefs.SetInt ("AvailableCash",Cash);
		AvailableCashtxt.text = "$"+Cash.ToString ();
		AvailableCashtxtMainMenu.text = "$"+Cash.ToString ();
		AvailableCashtxtLevelSelection.text = "$"+Cash.ToString ();
	}

	void setSoundSettingsUI()
	{
		if (PlayerPrefs.GetInt ("EngineSound", 1) == 1) {
			CarEngineTick.SetActive (true);
			setEngineSounds (true);
		}
		else if (PlayerPrefs.GetInt ("EngineSound", 1) == 0) {
			CarEngineTick.SetActive (false);
			setEngineSounds (false);
		}

		if (PlayerPrefs.GetInt ("OtherSounds", 1) == 1) {
			OtherSoundsTick.SetActive (true);
			setOtherSounds (true);
		}
		else if (PlayerPrefs.GetInt ("OtherSounds", 1) == 0) {
			OtherSoundsTick.SetActive (false);
			setOtherSounds (false);
		}

		if (PlayerPrefs.GetInt ("BGSounds", 1) == 1) {
			BGSoundTick.SetActive (true);
			setBGSounds (true);
		}
		else if (PlayerPrefs.GetInt ("BGSounds", 1) == 0) {
			BGSoundTick.SetActive (false);
			setBGSounds (false);
		}
	}

	public void OnClickEngineSound()
	{
		if (PlayerPrefs.GetInt ("EngineSound", 1) == 1) {
			PlayerPrefs.SetInt ("EngineSound", 0);
		}
		else if (PlayerPrefs.GetInt ("EngineSound", 1) == 0) {
			PlayerPrefs.SetInt ("EngineSound", 1);
		}
		setSoundSettingsUI ();
	}

	public void OnClickOtherSounds()
	{
		if (PlayerPrefs.GetInt ("OtherSounds", 1) == 1) {
			PlayerPrefs.SetInt ("OtherSounds", 0);
		}
		else if (PlayerPrefs.GetInt ("OtherSounds", 1) == 0) {
			PlayerPrefs.SetInt ("OtherSounds", 1);
		}
		setSoundSettingsUI ();
	}

	public void OnClickBGSounds()
	{
		if (PlayerPrefs.GetInt ("BGSounds", 1) == 1) {
			PlayerPrefs.SetInt ("BGSounds", 0);
		}
		else if (PlayerPrefs.GetInt ("BGSounds", 1) == 0) {
			PlayerPrefs.SetInt ("BGSounds", 1);
		}
		setSoundSettingsUI ();
	}

	public void setOtherSounds(bool val)
	{
		foreach (AudioSource x in OtherSounds) {
			x.enabled = val;
		}
	}
	public void setEngineSounds(bool val)
	{
		foreach (AudioSource x in CarSounds) {
			x.enabled = val;
		}
	}
	public void setBGSounds(bool val)
	{
		foreach (AudioSource x in BGSounds) {
			x.enabled = val;
		}
	}


	public void OnClickLikeUsOnFB()
	{
//		Application.OpenURL ("https://www.facebook.com/billiongamesco");	
	}
	public void OnClickLikePrivacyPolicy()
	{
		Application.OpenURL ("https://gameloopentertainment.blogspot.com/p/privacy-policy-gameloop-entertainment.html");	

	}

	public void OnClickLikeRateUs()
	{
		Application.OpenURL ("https://play.google.com/store/apps/details?id=com.gle.impossibleadvance.busdriving");
	}
	public void OnClickLikeMoreGames()
	{
		Application.OpenURL ("https://play.google.com/store/apps/developer?id=Gameloop+Entertainment");
	}
}
