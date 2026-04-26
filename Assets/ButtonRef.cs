using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonRef : MonoBehaviour {

	public GameObject LevelLock;
	public GameObject[] StarsOff;
	public GameObject[] StarsOn;
	public Text Leveltxt;
	// Use this for initialization
	void Start () {		
	}

	public void setText(string t)
	{
		Leveltxt.text = t;
	}

	public void Set0Star()
	{
//		LevelLock.SetActive (true);
//		StarsOn [0].SetActive (true);
	}

	public void Set1Star()
	{
		gameObject.GetComponent<Button> ().enabled = true;
		gameObject.GetComponent<Image> ().enabled = true;
		StarsOn [0].SetActive (true);
		SetNoStar ();
	}
	public void Set2Star()
	{
		gameObject.GetComponent<Button> ().enabled = true;
		gameObject.GetComponent<Image> ().enabled = true;
		StarsOn [0].SetActive (true);
		StarsOn [1].SetActive (true);
		SetNoStar ();
	}
	public void Set3Star()
	{
		gameObject.GetComponent<Button> ().enabled = true;
		gameObject.GetComponent<Image> ().enabled = true;
		StarsOn [0].SetActive (true);
		StarsOn [1].SetActive (true);
		StarsOn [2].SetActive (true);
		SetNoStar ();
	}
	public void SetNoStar()
	{
		LevelLock.SetActive (false);
//		foreach (GameObject x in StarsOff) {
//			x.SetActive (true);
//		}
	}
	public void LockLevel()
	{
		gameObject.GetComponent<Button> ().enabled = false;
		gameObject.GetComponent<Image> ().enabled = false;
		LevelLock.SetActive (true);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
