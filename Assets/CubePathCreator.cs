using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePathCreator : MonoBehaviour {

	public GameObject x;
	public int Number;
	public Vector3 Direction = new Vector3 ();
	// Use this for initialization
	void Start () {
		Replicate ();
	}

	void Replicate()
	{
		for (int i = 0; i < Number; i++) {
			Instantiate (x, Direction * i, Quaternion.identity, transform);
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
