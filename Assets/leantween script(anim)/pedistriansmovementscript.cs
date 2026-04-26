using UnityEngine;
using System.Collections;

public class pedistriansmovementscript : MonoBehaviour {

	public Gizmopathscript Pathtofollow;

	public int CurrentWayPointID = 0;
	public float speed;
	private float reachDistance = 3f;
	public float roatationspeed = 5.0f;
	public string pathName;

	Vector3 last_position;
	Vector3 current_position;

	// Use this for initialization
	void Start () {
		last_position = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		float distance = Vector3.Distance (Pathtofollow.path_objs[CurrentWayPointID].position,transform.position);
		transform.position = Vector3.MoveTowards (transform.position,Pathtofollow.path_objs[CurrentWayPointID].position,Time.deltaTime*speed);

		var rotation = Quaternion.LookRotation (Pathtofollow.path_objs [CurrentWayPointID].position - transform.position);
		transform.rotation = Quaternion.Slerp (transform.rotation,rotation,Time.deltaTime*roatationspeed);

		if(distance<=reachDistance){
			CurrentWayPointID++;
		}

		if (CurrentWayPointID >= Pathtofollow.path_objs.Count) {
			CurrentWayPointID = 0;
			gameObject.SetActive(false);
			//GameObject.FindObjectOfType<TriggerSlideExnterExit>().outSideTrigger();
		}
}
}