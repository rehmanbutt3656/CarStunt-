using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*[System.Serializable]
public class CustomEvents: UnityEvent
{
}*/

public class TriggerSlideExnterExit : MonoBehaviour
{
	public static TriggerSlideExnterExit TSEE;
	
	public string Activator = "";
	public GameObject carPos;
	public GameObject carControl;
	public GameObject newcar;
	public GameObject rccCamera,newCam;
	public GameObject pausebtn, respawnbtn;


	// Use this for initialization
	void Start ()
	{
		
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.GetComponentInParent<RCC_CarControllerV3>() != null)
		{
			if (col.GetComponentInParent<RCC_CarControllerV3>().gameObject.CompareTag(Activator))
			{
				
					//Debug.Log (this.gameObject.name + "vehicle collider   " + col.GetComponentInParent<RCC_CarControllerV3>().gameObject.name);
					//OnTriggerEnterEvents.Invoke ();
					col.GetComponentInParent<RCC_CarControllerV3>().gameObject.transform.position = carPos.transform.position;
					col.GetComponentInParent<RCC_CarControllerV3>().gameObject.transform.rotation = carPos.transform.rotation;
					col.GetComponentInParent<RCC_CarControllerV3>().gameObject.SetActive(false);
					Invoke("camOff", .2f);
					newcar.SetActive(true);
					newcar.transform.GetChild(MainMenuController.CurrentSelectedCar).gameObject.SetActive(true);
					//print("car body open "+ MainMenuController.CurrentSelectedCar);
					Invoke("outSideTrigger", 17.7f);
				
			}
		}
	}
	
	void OnTriggerExit (Collider col)
	{
		//if (col.GetComponentInParent<RCC_CarControllerV3>().gameObject.CompareTag(Activator))
		if (col.tag == Activator)
		{
			//OnTriggerExitEvents.Invoke ();
			//col.GetComponentInParent<RCC_CarControllerV3>().gameObject.GetComponent<pedistriansmovementscript>().enabled = false;
			newcar.SetActive(false);
			newcar.transform.GetChild(MainMenuController.CurrentSelectedCar).gameObject.SetActive(false);
			camOn();
			//col.GetComponentInParent<RCC_CarControllerV3>().gameObject.transform.position = carPos.transform.position;
			//col.GetComponentInParent<RCC_CarControllerV3>().gameObject.transform.rotation = carPos.transform.rotation;
			//col.GetComponentInParent<RCC_CarControllerV3>().gameObject.SetActive(true);
			GameController.GC.Cars[MainMenuController.CurrentSelectedCar].SetActive(true);
			carControl.SetActive(true);
			Time.timeScale = 1f;
			
		}		
	}
	void camOff()
	{
		rccCamera.SetActive(false);
		newCam.SetActive(true);
		pausebtn.SetActive(false);
		respawnbtn.SetActive(false);
	}
	void camOn()
	{
		newCam.SetActive(false);
		rccCamera.SetActive(true);
		pausebtn.SetActive(true);
		respawnbtn.SetActive(true);
	}
	public void outSideTrigger() {
		newcar.SetActive(false);
		camOn();
		//gameObject.GetComponentInParent<RCC_CarControllerV3>().gameObject.transform.position = carPos.transform.position;
		//gameObject.GetComponentInParent<RCC_CarControllerV3>().gameObject.transform.rotation = carPos.transform.rotation;
		//gameObject.GetComponentInParent<RCC_CarControllerV3>().gameObject.SetActive(true);
		GameController.GC.Cars[MainMenuController.CurrentSelectedCar].SetActive(true);
		carControl.SetActive(true);
		Time.timeScale = 1f;
	}
}
