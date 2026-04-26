using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//using TechnologyWings;

public class FakeLoadingScript : MonoBehaviour {

	public static FakeLoadingScript instance;
	public float maxLoadingTime;
	float remainingTime;

	[Header("UI")]
	public Image bgImage;
	public Sprite[] arrayOfBg = new Sprite[0];

	[Space(10)]
	public Slider loadingSlider;
	public GameObject mr_bannerObject;

	//for private 
	bool stop = false;
	bool adStop = false;

	//for the static
	public static string sceneNameToLoad = "";
	public static int adCount = 0;

	void Awake() {
		instance = this;
		FakeLoadingScript.instance.gameObject.SetActive(false);
	}

	// Start is called before the first frame update
	void OnEnable() {
		stop = false;
		adStop = false;
		remainingTime = maxLoadingTime;
		
		if(arrayOfBg.Length != 0)
			bgImage.sprite = arrayOfBg[Random.Range(0, arrayOfBg.Length)];

		if(mr_bannerObject)
			mr_bannerObject.SetActive(true);
        else { }
			//AdManager.ShowBanner(BannerAdType.MEDIUM_RECTANGLE, BannerAdPosition.BOTTOM_LEFT);
	}
	
	void OnDisable() {
		sceneNameToLoad = "";
		if(mr_bannerObject)
			mr_bannerObject.SetActive(false);
		//ar else
		//ar	AdManager.HideBanner(BannerAdType.MEDIUM_RECTANGLE);
	}

	// Update is called once per frame
	void Update() {

		if(!stop) {
			remainingTime -= Time.deltaTime;
			loadingSlider.value = 1f - (remainingTime / maxLoadingTime);

			//if the loading the greater then 75f then show the interstital ad
			if(100f - (remainingTime / maxLoadingTime) * 100f >= 75f) {
				if(!adStop) {
					adStop = true;

					adCount++;
					if(adCount % 2 == 0) {
						//if any interstital available then show the ad
						//ar if (AdManager.isInterstitialAvailable()) { }
						//AdManager.ShowInterstitial();
					}
				}
			}

			if(100f - (remainingTime / maxLoadingTime) * 100f >= 90f) {

				if(mr_bannerObject) {
					if(mr_bannerObject.activeSelf)
						mr_bannerObject.SetActive(false);
					//arelse
					//ar AdManager.HideBanner(BannerAdType.MEDIUM_RECTANGLE);
				}
			}

			if(remainingTime <= 0) {
				stop = true;

				if(sceneNameToLoad == "")
					this.gameObject.SetActive(false);
				else
					SceneManager.LoadSceneAsync(sceneNameToLoad, LoadSceneMode.Single);
			}
		}
	}

	public static void LoadScene(string sceneName) {
		sceneNameToLoad = sceneName;
		FakeLoadingScript.instance.gameObject.SetActive(true);
	}
}
