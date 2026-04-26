using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LiftScript : MonoBehaviour {

	public float Height = 5f;
	public float duration = 2f;
	public float waitTime = 3f;

	WaitForSeconds liftwaittime;// = new WaitForSeconds(waitTime);
	// Use this for initialization
	void Start () {
		liftwaittime = new WaitForSeconds (waitTime);

		StartCoroutine ("UpDown");
	}

	IEnumerator UpDown()
	{
		while (true) {	
			gameObject.transform.DOMoveY(Height/2, duration);
			yield return new WaitForSeconds (duration+1);

			yield return new WaitForSeconds (waitTime);

			Debug.Log ("111");
			gameObject.transform.DOMoveY(-(Height/2), duration);
			yield return new WaitForSeconds (duration+1);
			Debug.Log ("2222");

			yield return new WaitForSeconds (waitTime);
			Debug.Log ("33333");
		}
	}

	
	// Update is called once per frame
	void Update () {
		
	}
}
