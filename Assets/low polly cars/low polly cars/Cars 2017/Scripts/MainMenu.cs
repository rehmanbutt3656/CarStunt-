using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{


    public Transform vehicleRoot;
    public ColorPicker colorPicker;
    public float cameraRotateSpeed = 5;
    public Text carNameUI;
    public Image bodyActiveUI, wheelActiveUI;

    public VehicleSetting[] vehicleSetting;

    private float x, y = 0;

    private VehicleSetting currentVehicle;

    private int currentVehicleNumber = 0;


    [System.Serializable]
    public class VehicleSetting
    {
        public string name = "Car 1";
        public GameObject vehicle;
        public Material ringMat, bodyMat;
        public GameObject lights;
        public Vector3 doorAxle = new Vector3(0, 45, 0);
        public Transform leftDoor, rightDoor;
    }


    //Wheel Color//////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void ActiveWheelColor(Image activeImage)
    {
        activeImage.gameObject.SetActive(true);
        wheelActiveUI = activeImage;
        bodyActiveUI.gameObject.SetActive(false);
    }

    //Body Color//////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void ActiveBodyColor(Image activeImage)
    {
        activeImage.gameObject.SetActive(true);
        bodyActiveUI = activeImage;
        wheelActiveUI.gameObject.SetActive(false);
    }



    public void NextVehicle()
    {
        if (wheelActiveUI) { wheelActiveUI.gameObject.SetActive(false); bodyActiveUI.gameObject.SetActive(false); }

        currentVehicleNumber++;
        currentVehicleNumber = (int)Mathf.Repeat(currentVehicleNumber, vehicleSetting.Length);

        foreach (VehicleSetting VSetting in vehicleSetting)
        {
            if (VSetting == vehicleSetting[currentVehicleNumber])
            {
                VSetting.vehicle.SetActive(true);
                currentVehicle = VSetting;
            }
            else
            {
                VSetting.vehicle.SetActive(false);
            }
        }
    }


    public void PreviousVehicle()
    {
        if (wheelActiveUI) { wheelActiveUI.gameObject.SetActive(false); bodyActiveUI.gameObject.SetActive(false); }

        currentVehicleNumber--;
        currentVehicleNumber = (int)Mathf.Repeat(currentVehicleNumber, vehicleSetting.Length);

        foreach (VehicleSetting VSetting in vehicleSetting)
        {
            if (VSetting == vehicleSetting[currentVehicleNumber])
            {
                VSetting.vehicle.SetActive(true);
                currentVehicle = VSetting;
            }
            else
            {
                VSetting.vehicle.SetActive(false);
            }
        }
    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // 0=close   1=open
    private int DoorsMode = 0;
    public void CarDoors()
    {
        DoorsMode++;
        if (DoorsMode > 1) DoorsMode = 0;
    }

    // 0=off   1=on
    private int LightsMode = 0;
    public void CarLights()
    {
        LightsMode++;
        if (LightsMode > 1) LightsMode = 0;
    }



    void Update()
    {

        if (DoorsMode == 0)
        {
            vehicleSetting[currentVehicleNumber].leftDoor.rotation = Quaternion.Lerp(vehicleSetting[currentVehicleNumber].leftDoor.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 5.0f);
            vehicleSetting[currentVehicleNumber].rightDoor.rotation = Quaternion.Lerp(vehicleSetting[currentVehicleNumber].rightDoor.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 5.0f);
        }
        else
        {
            vehicleSetting[currentVehicleNumber].leftDoor.rotation = Quaternion.Lerp(vehicleSetting[currentVehicleNumber].leftDoor.rotation, Quaternion.Euler(vehicleSetting[currentVehicleNumber].doorAxle), Time.deltaTime * 5.0f);
            vehicleSetting[currentVehicleNumber].rightDoor.rotation = Quaternion.Lerp(vehicleSetting[currentVehicleNumber].rightDoor.rotation, Quaternion.Euler(-vehicleSetting[currentVehicleNumber].doorAxle), Time.deltaTime * 5.0f);
        }


        if (wheelActiveUI.gameObject.activeSelf)
            vehicleSetting[currentVehicleNumber].ringMat.SetColor("_Color", colorPicker.setColor);
        else if (bodyActiveUI.gameObject.activeSelf)
            vehicleSetting[currentVehicleNumber].bodyMat.SetColor("_Color", colorPicker.setColor);


        if (LightsMode == 0)
            vehicleSetting[currentVehicleNumber].lights.SetActive(false);
        else
            vehicleSetting[currentVehicleNumber].lights.SetActive(true);


        carNameUI.text = "CAR " + (currentVehicleNumber + 1).ToString();


        if (Input.GetMouseButton(0) && !colorPicker.clickDown)
        {
            x = Mathf.Lerp(x, Mathf.Clamp(Input.GetAxis("Mouse X"), -2, 2) * cameraRotateSpeed, Time.deltaTime * 5.0f);
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 50, 60);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 50, Time.deltaTime);
        }
        else {
            x = Mathf.Lerp(x, cameraRotateSpeed * 0.02f, Time.deltaTime * 10.0f);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60, Time.deltaTime);
            colorPicker.clickDown = false;
        }

        transform.RotateAround(vehicleRoot.position, Vector3.up, x);
    }

}
