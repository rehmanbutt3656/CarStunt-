using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

	public GameController GC;
	public GameObject SteerWheel;
	public GameObject SteerButtons;

	// Use this for initialization
	void Start () {
//		setSteerWheelUI ();
	}

	public void setSteerWheelUI()
	{
		if (GC.steer == GameController.Steering.Wheel) {
			SteerWheel.SetActive (true);
			SteerButtons.SetActive (false);
		}
		else if (GC.steer == GameController.Steering.Buttons) {
			SteerWheel.SetActive (false);
			SteerButtons.SetActive (true);
		}
	}

	public void ChangeControls()
	{
		if (GC.steer == GameController.Steering.Buttons) {
			GC.steer = GameController.Steering.Wheel;
		}
		else if (GC.steer == GameController.Steering.Wheel) {
			GC.steer = GameController.Steering.Buttons;
		}
		setSteerWheelUI ();
	}

	// Update is called once per frame
	void Update () {
		
	}
}
