//2016 Spyblood Productions
//Use for non-commerical games only. do not sell comercially
//without permission first

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SimpleInputNamespace;


public enum DriveType
{
	RWD,
	FWD,
	AWD
};
[System.Serializable]
public class WC
{
	public WheelCollider wheelFL;
	public WheelCollider wheelFR;
	public WheelCollider wheelRL;
	public WheelCollider wheelRR;
}
[System.Serializable]
public class WT
{
	public Transform wheelFL;
	public Transform wheelFR;
	public Transform wheelRL;
	public Transform wheelRR;
}
[RequireComponent(typeof(AudioSource))]//needed audiosource
[RequireComponent(typeof(Rigidbody))]//needed Rigid body
public class CarControlCS : MonoBehaviour {


	public Text SteerValTxt;
	public SteeringWheel SW;

	public WC wheels;
	public WT tires;
	public WheelCollider[] extraWheels;
	public Transform[] extraWheelObjects;
	public DriveType DriveTrain = DriveType.RWD;
	public Vector3 centerOfGravity;//car's center of mass offset
	public GUITexture gasPedal;
	public GUITexture brakePedal;
	public GUITexture HandbrakePedal;

	public GUITexture leftPedal;
	public GUITexture rightPedal;
	public float maxTorque = 1000f;//car's acceleration value
	public float maxReverseSpeed = 50f;//top speed for the reverse gear
	public float handBrakeTorque = 500f;//hand brake value
	public float maxSteer = 25f;//max steer angle
	public bool mobileInput = false;//do you want this to be a mobile game?
	public float[] GearRatio;//determines how many gears the car has, and at what speed the car shifts to the appropriate gear
	private int throttleInput;//read only
	private int steerInput;//read only
	private bool reversing;//read only
	private float currentSpeed;//read only
	public float maxSpeed = 150f;//how fast the vehicle can go
	private int gear;//current gear
	Vector3 localCurrentSpeed;

	private GameController GC;
	// Use this for initialization

	void Start () {
		if (Application.platform == RuntimePlatform.Android) {
			mobileInput = true;
		} else if (Application.platform == RuntimePlatform.WindowsEditor) {
			mobileInput = false;
		}



		GC = GameObject.FindObjectOfType<GameController> ();

		if(GC != null)
		SW = GC.SW;
		else 
		{
			Debug.Log ("GameController not Found");	
		}
		//find all the GUITextures from the scene and assign them
		gasPedal = GameObject.Find("GameController").GetComponent<GameController>().GasPedal.GetComponent<GUITexture>();
		brakePedal = GameObject.Find("GameController").GetComponent<GameController>().BrakePedal.GetComponent<GUITexture>();
		HandbrakePedal = GameObject.Find("GameController").GetComponent<GameController>().HandBrakePedal.GetComponent<GUITexture>();
		leftPedal = GameObject.Find("GameController").GetComponent<GameController>().LeftPedal.GetComponent<GUITexture>();
		rightPedal = GameObject.Find("GameController").GetComponent<GameController>().RightPedal.GetComponent<GUITexture>();

		/*
		brakePedal = GameObject.Find("BrakePedal").GetComponent<GUITexture>();
		leftPedal = GameObject.Find("LeftPedal").GetComponent<GUITexture>();
		rightPedal = GameObject.Find("RightPedal").GetComponent<GUITexture>();
		*/
		//Alter the center of mass for stability on your car
		GetComponent<Rigidbody>().centerOfMass = centerOfGravity;
	}

	void Update()
	{
//		DriveMobile ();
//		Drive ();
//		DriveMobileTest ();
		CarControlsUpdate();
		CarTurning ();
//		SteerValTxt.text = steerval.ToString ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (GetComponent<Rigidbody>().centerOfMass != centerOfGravity)
		GetComponent<Rigidbody>().centerOfMass = centerOfGravity;
		
		AllignWheels ();
		GUIButtonControl();
		EngineAudio ();

		currentSpeed = GetComponent<Rigidbody>().velocity.magnitude * 2.23693629f;//convert currentspeed into MPH

		localCurrentSpeed = transform.InverseTransformDirection (GetComponent<Rigidbody> ().velocity);

		//CarTurning()
		//if (currentSpeed > maxSpeed || (localCurrentSpeed.z*2.23693629f) < -maxReverseSpeed){

		wheels.wheelFL.steerAngle = maxSteer * steerval;//steerInput;
		wheels.wheelFR.steerAngle = maxSteer * steerval;//steerInput;
	}

	public float SmoothNess = 3f;
	float currentLerpTime;

	void CarTurning()
	{
		if(GC != null)
		if (GC.steer == GameController.Steering.Wheel) {
			steerval = SW.Value;
		}
		else if (GC.steer == GameController.Steering.Buttons) {

			// 1 > 0.1
			//increment timer once per frame
			/*
			currentLerpTime += Time.deltaTime;
			if (currentLerpTime > lerpTime) {
				currentLerpTime = lerpTime;
			}

			//lerp
			float perc = currentLerpTime / lerpTime;
			//ToEaseOut
			perc = 1f - Mathf.Cos(perc * Mathf.PI * 0.5f);
			*/
			steerval =  Mathf.Lerp (steerval , DriveButtonTo, Time.deltaTime * SmoothNess);
			//			steerval =
			/*
			if (steerval > DriveButtonTo) {
				steerval -=  0.05f;
			}
			// -1 < 1
			else if (steerval < DriveButtonTo) {
				steerval += 0.05f;
			}
			*/

		}
	}
	void AllignWheels()
	{
		//allign the wheel objs to their colliders

			Quaternion quat;
			Vector3 pos;
			wheels.wheelFL.GetWorldPose(out pos,out quat);
			tires.wheelFL.position = pos;
			tires.wheelFL.rotation = quat;

		wheels.wheelFR.GetWorldPose(out pos,out quat);
		tires.wheelFR.position = pos;
		tires.wheelFR.rotation = quat;

		wheels.wheelRL.GetWorldPose(out pos,out quat);
		tires.wheelRL.position = pos;
		tires.wheelRL.rotation = quat;

		wheels.wheelRR.GetWorldPose(out pos,out quat);
		tires.wheelRR.position = pos;
		tires.wheelRR.rotation = quat;

		for (int i = 0; i < extraWheels.Length; i++)
		{

			for (int k = 0; k < extraWheelObjects.Length; k++) {
			
				Quaternion quater;
				Vector3 vec3;

				extraWheels [i].GetWorldPose (out vec3, out quater);
				extraWheelObjects [k].position = vec3;
				extraWheelObjects [k].rotation = quater;
			
			}

		}
	}
		

	void GUIButtonControl()
	{
		//simple function that disables/enables GUI buttons when we need and dont need them.
		if (mobileInput)
		{
			gasPedal.gameObject.SetActive(true);
			leftPedal.gameObject.SetActive(true);
			rightPedal.gameObject.SetActive(true);
			brakePedal.gameObject.SetActive(true);
		}
		else{
			gasPedal.gameObject.SetActive(false);
			leftPedal.gameObject.SetActive(false);
			rightPedal.gameObject.SetActive(false);
			brakePedal.gameObject.SetActive(false);
		}
	}

	float steerval = 0f;
	float DriveButtonTo = 0f;
	void CarControlsUpdate()
	{
		float gasMultiplier = 0f;

		if (!reversing) {
			if (currentSpeed < maxSpeed)
				gasMultiplier = 1f;
			else
				gasMultiplier = 0f;

		} else {
			if (currentSpeed < maxReverseSpeed)
				gasMultiplier = 1f;
			else
				gasMultiplier = 0f;
		}

		if(GC != null)
		if (GC.steer == GameController.Steering.Wheel) {
			steerval = SW.Value;
		} else if (GC.steer == GameController.Steering.Buttons) {
			//				steerval = steerInput;
		}		
		//			float steerval = SW.Value;

		//			wheels.wheelFL.steerAngle = maxSteer * steerval;//steerInput;
		//			wheels.wheelFR.steerAngle = maxSteer * steerval;//steerInput;
		//`````````````````````````````````````````````
		if (DriveTrain == DriveType.RWD)
		{
			wheels.wheelRL.motorTorque = maxTorque * throttleInput * gasMultiplier;
			wheels.wheelRR.motorTorque = maxTorque * throttleInput * gasMultiplier;

			if (localCurrentSpeed.z < -0.1f && wheels.wheelRL.rpm < 10) {//in local space, if the car is travelling in the direction of the -z axis, (or in reverse), reversing will be true
				reversing = true;
			} else {
				reversing = false;
			}
		}
		if (DriveTrain == DriveType.FWD)
		{
			wheels.wheelFL.motorTorque = maxTorque * throttleInput * gasMultiplier;
			wheels.wheelFR.motorTorque = maxTorque * throttleInput * gasMultiplier;

			if (localCurrentSpeed.z < -0.1f && wheels.wheelFL.rpm < 10) {//in local space, if the car is travelling in the direction of the -z axis, (or in reverse), reversing will be true
				reversing = true;
			} else {
				reversing = false;
			}
		}
		if (DriveTrain == DriveType.AWD)
		{
			wheels.wheelFL.motorTorque = maxTorque * throttleInput * gasMultiplier;
			wheels.wheelFR.motorTorque = maxTorque * throttleInput * gasMultiplier;
			wheels.wheelRL.motorTorque = maxTorque * throttleInput * gasMultiplier;
			wheels.wheelRR.motorTorque = maxTorque * throttleInput * gasMultiplier;

			if (localCurrentSpeed.z < -0.1f && wheels.wheelRL.rpm < 10) {//in local space, if the car is travelling in the direction of the -z axis, (or in reverse), reversing will be true
				reversing = true;
			} else {
				reversing = false;
			}
		}
	}


	void DriveMobile()
	{
		if (!mobileInput)
			return;
		//dont call this function if the mobileiput box is not checked in the editor
		float gasMultiplier = 0f;

		if (!reversing) {
			if (currentSpeed < maxSpeed)
				gasMultiplier = 1f;
			else
				gasMultiplier = 0f;

		} else {
			if (currentSpeed < maxReverseSpeed)
				gasMultiplier = 1f;
			else
				gasMultiplier = 0f;
		}
		foreach (Touch touch in Input.touches) {

			//if the gas button is pressed down, speed up the car.
			if (touch.phase == TouchPhase.Stationary && gasPedal.HitTest (touch.position) ) {
				throttleInput = 1;
				gasPedal.color = Color.red;
			}
			//when the gas button is released, slow the car down
			else if (touch.phase == TouchPhase.Ended && gasPedal.HitTest (touch.position)) {
				throttleInput = 0;
				gasPedal.color = Color.white;
			}
			//now the same thing for the brakes
			if (touch.phase == TouchPhase.Stationary && brakePedal.HitTest (touch.position)) {
				throttleInput = -1;
				brakePedal.color = Color.red;
			}
			//stop braking once you put your finger off the brake pedal
			else if (touch.phase == TouchPhase.Ended && brakePedal.HitTest (touch.position)) {
				throttleInput = 0;
				brakePedal.color = Color.white;
			}
			//now the left steering column...
			if (touch.phase == TouchPhase.Stationary && leftPedal.HitTest (touch.position)) {
				//turn the front left wheels according to input direction
				currentLerpTime = 0f;
				steerInput = -1;
				DriveButtonTo = -1;
				leftPedal.color = Color.red;
			}
			//and stop the steering once you take your finger off the turn button
			else if (touch.phase == TouchPhase.Ended && leftPedal.HitTest (touch.position)) {
				currentLerpTime = 0f;
				steerInput = 0;
				DriveButtonTo = 0;
				leftPedal.color = Color.white;
			}
			//now the right steering column...
			if (touch.phase == TouchPhase.Stationary && rightPedal.HitTest (touch.position)) {
				//turn the front left wheels according to input direction
				currentLerpTime = 0f;
				steerInput = 1;
				DriveButtonTo = 1;
				rightPedal.color = Color.red;
			}
			//and stop the steering once you take your finger off the turn button
			else if (touch.phase == TouchPhase.Ended && rightPedal.HitTest (touch.position)) {
				currentLerpTime = 0f;
				steerInput = 0; 
				DriveButtonTo = 0;
				rightPedal.color = Color.white;
			}


			if (touch.phase == TouchPhase.Stationary && HandbrakePedal.HitTest (touch.position)) {
				wheels.wheelFL.brakeTorque = handBrakeTorque;
				wheels.wheelFR.brakeTorque = handBrakeTorque;
				wheels.wheelRL.brakeTorque = handBrakeTorque;
				wheels.wheelRR.brakeTorque = handBrakeTorque;		
				HandbrakePedal.color = Color.red;
			} else if (touch.phase == TouchPhase.Ended && HandbrakePedal.HitTest (touch.position)) {
				wheels.wheelFL.brakeTorque = 0f;
				wheels.wheelFR.brakeTorque = 0f;
				wheels.wheelRL.brakeTorque = 0f;
				wheels.wheelRR.brakeTorque = 0f;			
				HandbrakePedal.color = Color.white;
			}

			//now that we have our input values made, it's time to feed them to the car!
			if(GC != null)
			if (GC.steer == GameController.Steering.Wheel) {
				steerval = SW.Value;
			} else if (GC.steer == GameController.Steering.Buttons) {
//				steerval = steerInput;
			}		
			//			float steerval = SW.Value;

//			wheels.wheelFL.steerAngle = maxSteer * steerval;//steerInput;
//			wheels.wheelFR.steerAngle = maxSteer * steerval;//steerInput;
			//`````````````````````````````````````````````
			if (DriveTrain == DriveType.RWD)
			{
				wheels.wheelRL.motorTorque = maxTorque * throttleInput * gasMultiplier;
				wheels.wheelRR.motorTorque = maxTorque * throttleInput * gasMultiplier;

				if (localCurrentSpeed.z < -0.1f && wheels.wheelRL.rpm < 10) {//in local space, if the car is travelling in the direction of the -z axis, (or in reverse), reversing will be true
					reversing = true;
				} else {
					reversing = false;
				}
			}
			if (DriveTrain == DriveType.FWD)
			{
				wheels.wheelFL.motorTorque = maxTorque * throttleInput * gasMultiplier;
				wheels.wheelFR.motorTorque = maxTorque * throttleInput * gasMultiplier;

				if (localCurrentSpeed.z < -0.1f && wheels.wheelFL.rpm < 10) {//in local space, if the car is travelling in the direction of the -z axis, (or in reverse), reversing will be true
					reversing = true;
				} else {
					reversing = false;
				}
			}
			if (DriveTrain == DriveType.AWD)
			{
				wheels.wheelFL.motorTorque = maxTorque * throttleInput * gasMultiplier;
				wheels.wheelFR.motorTorque = maxTorque * throttleInput * gasMultiplier;
				wheels.wheelRL.motorTorque = maxTorque * throttleInput * gasMultiplier;
				wheels.wheelRR.motorTorque = maxTorque * throttleInput * gasMultiplier;

				if (localCurrentSpeed.z < -0.1f && wheels.wheelRL.rpm < 10) {//in local space, if the car is travelling in the direction of the -z axis, (or in reverse), reversing will be true
					reversing = true;
				} else {
					reversing = false;
				}
			}
		}
	}

	void DriveMobileTest()
	{
		//dont call this function if the mobileiput box is not checked in the editor
		float gasMultiplier = 0f;

		if (!reversing) {
			if (currentSpeed < maxSpeed)
				gasMultiplier = 1f;
			else
				gasMultiplier = 0f;

		} else {
			if (currentSpeed < maxReverseSpeed)
				gasMultiplier = 1f;
			else
				gasMultiplier = 0f;
		}

		//if the gas button is pressed down, speed up the car.
		if (Input.GetKeyDown(KeyCode.I)) {
			throttleInput = 1;
			gasPedal.color = Color.red;
		}
		//when the gas button is released, slow the car down
		else if (Input.GetKeyUp(KeyCode.I)) {
			throttleInput = 0;
			gasPedal.color = Color.white;
		}
		//now the same thing for the brakes
		if (Input.GetKeyDown(KeyCode.K)) {
			throttleInput = -1;
			brakePedal.color = Color.red;
		}
		//stop braking once you put your finger off the brake pedal
		else if (Input.GetKeyUp(KeyCode.K)) {
			throttleInput = 0;
			brakePedal.color = Color.white;
		}
		//now the left steering column...
		if (Input.GetKeyDown(KeyCode.J)) {
			//turn the front left wheels according to input direction
			currentLerpTime = 0f;
			steerInput = -1;
			DriveButtonTo = -1;
			leftPedal.color = Color.red;
		}
		//and stop the steering once you take your finger off the turn button
		else if (Input.GetKeyUp(KeyCode.J)) {
			currentLerpTime = 0f;
			steerInput = 0;
			DriveButtonTo = 0;
			leftPedal.color = Color.white;
		}
		//now the right steering column...
		if (Input.GetKeyDown(KeyCode.L)) {
			//turn the front left wheels according to input direction
			currentLerpTime = 0f;
			steerInput = 1;
			DriveButtonTo = 1;
			rightPedal.color = Color.red;
		}
		//and stop the steering once you take your finger off the turn button
		else if (Input.GetKeyUp(KeyCode.L)) {
			currentLerpTime = 0f;
			steerInput = 0; 
			DriveButtonTo = 0;
			rightPedal.color = Color.white;
		}
		//now that we have our input values made, it's time to feed them to the car!
		if(GC != null)
		if (GC.steer == GameController.Steering.Wheel) {
			steerval = SW.Value;
		} else if (GC.steer == GameController.Steering.Buttons) {
			//				steerval = steerInput;
		}		
		//			float steerval = SW.Value;

		//			wheels.wheelFL.steerAngle = maxSteer * steerval;//steerInput;
		//			wheels.wheelFR.steerAngle = maxSteer * steerval;//steerInput;
		//`````````````````````````````````````````````
		if (DriveTrain == DriveType.RWD)
		{
			wheels.wheelRL.motorTorque = maxTorque * throttleInput * gasMultiplier;
			wheels.wheelRR.motorTorque = maxTorque * throttleInput * gasMultiplier;

			if (localCurrentSpeed.z < -0.1f && wheels.wheelRL.rpm < 10) {//in local space, if the car is travelling in the direction of the -z axis, (or in reverse), reversing will be true
				reversing = true;
			} else {
				reversing = false;
			}
		}
		if (DriveTrain == DriveType.FWD)
		{
			wheels.wheelFL.motorTorque = maxTorque * throttleInput * gasMultiplier;
			wheels.wheelFR.motorTorque = maxTorque * throttleInput * gasMultiplier;

			if (localCurrentSpeed.z < -0.1f && wheels.wheelFL.rpm < 10) {//in local space, if the car is travelling in the direction of the -z axis, (or in reverse), reversing will be true
				reversing = true;
			} else {
				reversing = false;
			}
		}
		if (DriveTrain == DriveType.AWD)
		{
			wheels.wheelFL.motorTorque = maxTorque * throttleInput * gasMultiplier;
			wheels.wheelFR.motorTorque = maxTorque * throttleInput * gasMultiplier;
			wheels.wheelRL.motorTorque = maxTorque * throttleInput * gasMultiplier;
			wheels.wheelRR.motorTorque = maxTorque * throttleInput * gasMultiplier;

			if (localCurrentSpeed.z < -0.1f && wheels.wheelRL.rpm < 10) {//in local space, if the car is travelling in the direction of the -z axis, (or in reverse), reversing will be true
				reversing = true;
			} else {
				reversing = false;
			}
		}
	}


	void Drive()
	{
		if (mobileInput)
			return;
		//dont call this function if mobile input is checked in the editor
		float gasMultiplier = 0f;

		if (!reversing) {
			if (currentSpeed < maxSpeed)
				gasMultiplier = 1f;
			else
				gasMultiplier = 0f;

		} else {
			if (currentSpeed < maxReverseSpeed) {
				if (currentSpeed < 1)
					gasMultiplier = 1f;
			}
			else
				gasMultiplier = 0f;
		}
		//the car will be 4 wheel drive or else it will be slow or feel a little sluggish
		//no matter how much you increase the max torque.
		if (DriveTrain == DriveType.RWD)
		{
			wheels.wheelRR.motorTorque = maxTorque * Input.GetAxis("Vertical") * gasMultiplier;
			wheels.wheelRL.motorTorque = maxTorque * Input.GetAxis("Vertical") * gasMultiplier;

			if (localCurrentSpeed.z < -0.1f && wheels.wheelRL.rpm < 10) {//in local space, if the car is travelling in the direction of the -z axis, (or in reverse), reversing will be true
				reversing = true;
			} else {
				reversing = false;
			}
		}
		if (DriveTrain == DriveType.FWD)
		{
			wheels.wheelFL.motorTorque = maxTorque * Input.GetAxis("Vertical") * gasMultiplier;
			wheels.wheelFR.motorTorque = maxTorque * Input.GetAxis("Vertical") * gasMultiplier;

			if (localCurrentSpeed.z < -0.1f && wheels.wheelFL.rpm < 10) {//in local space, if the car is travelling in the direction of the -z axis, (or in reverse), reversing will be true
				reversing = true;
			} else {
				reversing = false;
			}
		}
		if (DriveTrain == DriveType.AWD)
		{
			wheels.wheelFL.motorTorque = maxTorque * Input.GetAxis("Vertical") * gasMultiplier;
			wheels.wheelFR.motorTorque = maxTorque * Input.GetAxis("Vertical") * gasMultiplier;
			wheels.wheelRL.motorTorque = maxTorque * Input.GetAxis("Vertical") * gasMultiplier;
			wheels.wheelRR.motorTorque = maxTorque * Input.GetAxis("Vertical") * gasMultiplier;

			if (localCurrentSpeed.z < -0.1f && wheels.wheelRL.rpm < 10) {//in local space, if the car is travelling in the direction of the -z axis, (or in reverse), reversing will be true
				reversing = true;
			} else {
				reversing = false;
			}
		}

//		float steerval = Input.GetAxis("Horizontal");
		if(GC != null)
		if (GC != null) {
			if (GC.steer == GameController.Steering.Wheel) {
				steerval = SW.Value;
			} else if (GC.steer == GameController.Steering.Buttons) {
				steerval = steerInput;
			}
		} else {
			Debug.Log ("GC Not Found");
		}
		//			float steerval = SW.Value;

		//CopyingThis
//		wheels.wheelFL.steerAngle = maxSteer * steerval;//steerInput;
//		wheels.wheelFR.steerAngle = maxSteer * steerval;//steerInput;
		if (Input.GetButton("Jump"))//pressing space triggers the car's handbrake
		{
			wheels.wheelFL.brakeTorque = handBrakeTorque;
			wheels.wheelFR.brakeTorque = handBrakeTorque;
			wheels.wheelRL.brakeTorque = handBrakeTorque;
			wheels.wheelRR.brakeTorque = handBrakeTorque;
		}
		else//letting go of space disables the handbrake
		{
			wheels.wheelFL.brakeTorque = 0f;
			wheels.wheelFR.brakeTorque = 0f;
			wheels.wheelRL.brakeTorque = 0f;
			wheels.wheelRR.brakeTorque = 0f;
		}
	}

	void EngineAudio()
	{
		//the function called to give the car basic audio, as well as some gear shifting effects
		//it's prefered you use the engine sound included, but you can use your own if you have one.
		//~~~~~~~~~~~~[IMPORTANT]~~~~~~~~~~~~~~~~
		//make sure your last gear value is higher than the max speed variable or else you will
		//get unwanted errors!!
		
		//anyway, let's get started
		
		for (int i = 0; i < GearRatio.Length; i++) {
			if (GearRatio [i] > currentSpeed) {
				//break this value
				break;
			}

			float minGearValue = 0f;
			float maxGearValue = 0f;
			if (i == 0) {
				minGearValue = 0f;
			} else {
				minGearValue = GearRatio [i];
			}
			if(i+1 < GearRatio.Length)
			maxGearValue = GearRatio [i+1];
		
			float pitch = ((currentSpeed - minGearValue) / (maxGearValue - minGearValue)+0.2f * (gear+1));
			GetComponent<AudioSource> ().pitch = pitch;
		
			gear = i;
		}
	}
	void OnGUI()
	{
		if (Application.platform == RuntimePlatform.WindowsEditor) {
			//show the GUI for the speed and gear we are on.
			if (!reversing)
				GUI.Box(new Rect(20,20,140,60),"Gear: " + (gear+1));
			if (reversing)//if the car is going backwards display the gear as R
				GUI.Box(new Rect(20,20,140,60),"Gear: R");			

			GUI.Box(new Rect(20,20,140,60),"MPH: " + Mathf.Round(GetComponent<Rigidbody>().velocity.magnitude * 2.23693629f));
		}
	}

	//PressDownEvents
	public void RaceBtnPressDown()
	{
		throttleInput = 1;
	}
	public void RevBtnPressDown()
	{
		throttleInput = -1;
	}
	public void HandBrakeBtnPressDown()
	{
		wheels.wheelFL.brakeTorque = handBrakeTorque;
		wheels.wheelFR.brakeTorque = handBrakeTorque;
		wheels.wheelRL.brakeTorque = handBrakeTorque;
		wheels.wheelRR.brakeTorque = handBrakeTorque;		
	}
	public void LeftBtnPressDown()
	{
		currentLerpTime = 0f;
		steerInput = -1;
		DriveButtonTo = -1;
	}
	public void RightBtnPressDown()
	{
		currentLerpTime = 0f;
		steerInput = 1;
		DriveButtonTo = 1;
	}

	//PressUpEvents
	public void RaceBtnPressUp()
	{
		throttleInput = 0;
	}
	public void RevBtnPressUp()
	{
		throttleInput = 0;
	}
	public void HandBrakeBtnPressUp()
	{
		wheels.wheelFL.brakeTorque = 0f;
		wheels.wheelFR.brakeTorque = 0f;
		wheels.wheelRL.brakeTorque = 0f;
		wheels.wheelRR.brakeTorque = 0f;			
	}
	public void LeftBtnPressUp()
	{
		currentLerpTime = 0f;
		steerInput = 0;
		DriveButtonTo = 0;
	}
	public void RightBtnPressUp()
	{
		currentLerpTime = 0f;
		steerInput = 0;
		DriveButtonTo = 0;
	}

}
