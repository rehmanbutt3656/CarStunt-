using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetButtonAfter : MonoBehaviour {

	public float resetTime = 0.7f;
	 
	// Use this for initialization
	void Start () {
		
	}

	public void ResetBtn()
	{
		gameObject.GetComponent<Button> ().interactable = false;
		Invoke ("ResetInteractible",resetTime);
	}

	public void ResetInteractible()
	{
		gameObject.GetComponent<Button> ().interactable = true;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
