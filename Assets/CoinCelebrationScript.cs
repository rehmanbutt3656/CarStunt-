using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CoinCelebrationScript : MonoBehaviour {

	public Transform Target;
	// Use this for initialization
	void Start () {
		transform.DOScale (4f,0.5f);	
		StartCoroutine ("ShowCoinAnimations");
	}

	IEnumerator ShowCoinAnimations()
	{	
		yield return new WaitForSeconds (0.7f);
		foreach (Transform x in transform) {
			x.DOMove (Target.position,0.2f);
			yield return new WaitForSeconds (0.1f);
		}		
		Destroy (gameObject, 0.1f);
	}


	
	// Update is called once per frame
	void Update () {
		
	}
}
