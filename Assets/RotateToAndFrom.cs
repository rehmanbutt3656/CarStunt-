using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotateToAndFrom : MonoBehaviour {

	public Vector3 RotateTo1 = new Vector3();
	public Vector3 RotateTo2 = new Vector3();
	public float waitTime = 2f;
	public float cycleTime = 2f;
	// Use this for initialization
	void Start () {
		StartCoroutine ("RotateIt");
	}

	IEnumerator RotateIt()
	{
		while (true) {
			gameObject.transform.DORotate (RotateTo1,cycleTime);
			yield return new WaitForSeconds (waitTime);
			gameObject.transform.DORotate (RotateTo2,cycleTime);
			yield return new WaitForSeconds (waitTime);
		}
		
	}

	
	// Update is called once per frame
	void Update () {
		
	}
}
