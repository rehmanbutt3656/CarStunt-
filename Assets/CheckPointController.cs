using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CheckPointController : MonoBehaviour
{

	public GameController GC;
	public CheckPoint[] CP;

	public CheckPoint LastCheckPoint;

	// Use this for initialization
	void Start ()
	{
	
	}

	public void RespawnToLastCP ()
	{
		foreach (CheckPoint c in CP) {
			if (c.Cleared) {
				LastCheckPoint = c;
			}
		}

		//SpawnCar
		GC.CurrentCar.transform.position = LastCheckPoint.transform.position;
		GC.CurrentCar.transform.rotation = LastCheckPoint.transform.rotation;
		//muk
		GC.CurrentCar.transform.localScale = new Vector3(1f, 1f, 1f);
		GC.CurrentCar.transform.DOPunchScale (new Vector3 (0.3f, 0.3f, 0.3f), 0.5f, 1);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
