using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*[System.Serializable]
public class CustomEvents: UnityEvent
{
}*/

public class TriggerSlowEnterExit : MonoBehaviour
{
	
	public string Activator = "";
	//public CustomEvents OnTriggerEnterEvents;
	//public CustomEvents OnTriggerExitEvents;


	// Use this for initialization
	void Start ()
	{
		
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.GetComponentInParent <RCC_CarControllerV3>().gameObject.CompareTag (Activator)) {
			//Debug.Log (this.gameObject.name + "vehicle collider   " + col.GetComponentInParent<RCC_CarControllerV3>().gameObject.name);
			//OnTriggerEnterEvents.Invoke ();
			//col.GetComponentInParent<RCC_CarControllerV3>().gameObject.GetComponent<pedistriansmovementscript>().enabled = true;
			print("enter inside");
			Time.timeScale = .2f;

		}		
	}

	void OnTriggerExit (Collider col)
	{
		if (col.GetComponentInParent<RCC_CarControllerV3>().gameObject.CompareTag (Activator)) {
			//OnTriggerExitEvents.Invoke ();
			//col.GetComponentInParent<RCC_CarControllerV3>().gameObject.GetComponent<pedistriansmovementscript>().enabled = false;
			print("enter outside");
			Time.timeScale = 1f;
		}		
	}

}
