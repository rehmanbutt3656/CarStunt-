using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class Follower : MonoBehaviour {

	public PathCreator pathCreator;
	public float speed;
	float distanceTravelled;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		distanceTravelled += speed * Time.deltaTime;
		transform.position = pathCreator.path.GetPointAtDistance (distanceTravelled);
		transform.rotation = pathCreator.path.GetRotationAtDistance (distanceTravelled);
	}
}
