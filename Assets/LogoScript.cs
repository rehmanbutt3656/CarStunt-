using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LogoScript : MonoBehaviour {

	static bool FirstTime = true;
	public Image Logo;
	public Transform JumpToTransform;

	// Use this for initialization
	void Start () {
		if (FirstTime) {
			FirstTime = false;
			Invoke ("DisableAfter", 2.5f);
		} else
			gameObject.SetActive (false);

		Logo.transform. DOPunchPosition (JumpToTransform.localPosition,1.5f,5,10f);// DOJump (JumpToTransform.position,100f,1,1.5f);
		Logo.transform. DOPunchScale (new Vector3(1,1,1),1.5f,5,10f);// DOJump (JumpToTransform.position,100f,1,1.5f);
	}



	void DisableAfter()
	{
		gameObject.SetActive (false);
	}


	void OnDisable()
	{
//		AdManager.admanagerInstance.OnClickShowBanner ();
	}


	
	// Update is called once per frame
	void Update () {
		
	}
}
