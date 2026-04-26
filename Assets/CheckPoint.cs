using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{

	public bool Cleared = false;
	public Transform SpawnLocation;
	SoundManager SM;
	// Use this for initialization
	void Start ()
	{
		if (GameObject.Find ("SoundManager").GetComponent<SoundManager> () != null)
			SM = GameObject.Find ("SoundManager").GetComponent<SoundManager> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	bool PlaySound = true;

	void OnTriggerEnter (Collider col)
	{
        if (col.GetComponentInParent<RCC_CarControllerV3>()!=null)
        {
			if (col.GetComponentInParent<RCC_CarControllerV3>().gameObject.CompareTag("Car"))
			{
				if (SM != null && PlaySound == true)
				{if (col.GetComponentInParent<RCC_CarControllerV3> ().gameObject.CompareTag ("Car")) {
			if (SM != null && PlaySound == true) {
				SM.PlayCheckPointSound ();
				PlaySound = false;
				Debug.Log(this.gameObject.name + "bus collider   " + col.GetComponentInParent<RCC_CarControllerV3>().gameObject.name);
			}
			Cleared = true;
		}
					SM.PlayCheckPointSound();
					PlaySound = false;
					Debug.Log(this.gameObject.name + "bus collider   " + col.GetComponentInParent<RCC_CarControllerV3>().gameObject.name);
				}
				Cleared = true;
			}
		}
		
	}
}
