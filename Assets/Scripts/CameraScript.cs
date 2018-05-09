using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	public GameObject agent;
	private Vector3 offset;
	// Use this for initialization
	void Start () {
		offset = transform.position - agent.transform.position;
		transform.position = agent.transform.position + offset;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.position = agent.transform.position + offset;
		transform.eulerAngles= new Vector3 (90,0,0);
	}
}
