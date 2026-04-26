using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HeliCopterMovement : MonoBehaviour
{

	public Transform[] Points;
	public GameObject Heli;
	public float waitime = 2f;
	// Use this for initialization
	void Start ()
	{
		StartCoroutine ("ShowTrack");
	}

	IEnumerator ShowTrack ()
	{
		yield return new WaitForSeconds (0.2f);
		foreach (Transform x in Points) {
			Heli.transform.DOMove (x.position, waitime);
			Heli.transform.DORotate (x.rotation.eulerAngles, waitime / 2);
			yield return new WaitForSeconds (waitime);
		}

		Heli.transform.DOMove (Points [0].position, waitime);
		Heli.transform.DOLocalRotate (Points [0].localRotation.eulerAngles, waitime / 2);
		yield return new WaitForSeconds (waitime);
		Invoke ("RestartHeliMovement", 0.0f);
	}

	void RestartHeliMovement ()
	{
		StopCoroutine ("ShowTrack");
		StartCoroutine ("ShowTrack");
	}

	// Update is called once per frame
	void Update ()
	{
		
	}
}
