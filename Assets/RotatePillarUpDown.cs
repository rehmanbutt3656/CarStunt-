using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotatePillarUpDown : MonoBehaviour {

	public float TopY = 0;
	public float BottomY = 0;

	public float moveTimeTop = 1f;
	public float moveTimeBottom = 1f;

	public float waitTimeTop = 4f;
	public float waitTimeBottom = 2f;
	// Use this for initialization
	void Start () {
		StartCoroutine ("MoveUpDown");
	}

	IEnumerator MoveUpDown()
	{
		while(true)
		{
//			Debug.Log ("MoveUpDown");
			transform.DOLocalMoveY (TopY, moveTimeTop);
			yield return new WaitForSeconds (waitTimeTop);				
//			Debug.Log ("MoveUpDown");
			transform.DOLocalMoveY (BottomY, moveTimeBottom);
			yield return new WaitForSeconds (waitTimeBottom);				
//			Debug.Log ("MoveUpDown");
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
