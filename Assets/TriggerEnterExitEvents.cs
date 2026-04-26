using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CustomEvents: UnityEvent
{
}

public class TriggerEnterExitEvents : MonoBehaviour
{
	
	public string Activator = "";
	public CustomEvents OnTriggerEnterEvents;
	public CustomEvents OnTriggerExitEvents;


	// Use this for initialization
	void Start ()
	{
		
	}

	void OnTriggerEnter (Collider col)
	{
        if (col.GetComponentInParent<RCC_CarControllerV3>()!=null)
        {
			if (col.GetComponentInParent<RCC_CarControllerV3>().gameObject.CompareTag(Activator))
			{
				Debug.Log(this.gameObject.name + "vehicle collider   " + col.GetComponentInParent<RCC_CarControllerV3>().gameObject.name);
				OnTriggerEnterEvents.Invoke();
			}
		}
			
	}

	void OnTriggerExit (Collider col)
	{
		if (col.GetComponentInParent<RCC_CarControllerV3>() != null)
		{
			if (col.GetComponentInParent<RCC_CarControllerV3>().gameObject.CompareTag(Activator))
			{
				OnTriggerExitEvents.Invoke();
			}
		}
	}

}
