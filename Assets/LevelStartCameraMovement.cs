using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelStartCameraMovement : MonoBehaviour {

	public Transform[] Points;	 
	public GameObject Cam;
	public GameController GC;
	public float waitime = 2f;
	// Use this for initialization
	void Start () {
		GC.CurrentCamShow = gameObject;
		StartCoroutine ("ShowTrack");
		GC.OnClickSkipCamShow();
	}

	IEnumerator ShowTrack()
	{
		Cam.SetActive (true);
		yield return new WaitForSeconds (0.2f);
		foreach(Transform x in Points)
		{
			Cam.transform.DOMove (x.position, waitime);
			Cam.transform.DORotate (x.rotation.eulerAngles, waitime/2);
			yield return new WaitForSeconds (waitime);
		}

		Cam.transform.DOMove (Points[0].position, waitime);
		Cam.transform.DOLocalRotate (Points[0].localRotation.eulerAngles, waitime/2);
		yield return new WaitForSeconds (waitime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
