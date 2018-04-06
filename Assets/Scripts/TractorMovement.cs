using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorMovement : MonoBehaviour {

	float speed = 1.0f;
	float actualX = 1f;
	float actualZ = 1f;
	int destinationX =1;
	int destinationZ =1;
	string actualRotation = "up";
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.W)) {
			destinationZ += 1;
		}
		actualX = transform.position.x;
		actualZ = transform.position.z;
		if (actualZ < destinationZ) {
			if (actualRotation.Equals ("up"))
				transform.Translate (0, 0, Time.deltaTime * speed);
			if (destinationZ <= actualZ)
				actualZ = (float)destinationZ;
			else {
				if (actualRotation.Equals ("left")) {
					transform.Rotate (0,Time.deltaTime *(speed+45f),0);
					if (transform.rotation.y >= 0) {
						transform.Rotate (0,0,0);
						actualRotation = "up";
					}
				}
				if (actualRotation.Equals ("right")) {
					transform.Rotate (0,Time.deltaTime *(((-1)*speed)-45f),0);
					if (transform.rotation.y <= 0) {
						transform.Rotate (0,0,0);
						actualRotation = "up";
					}
				}
				if (actualRotation.Equals ("down")) {
					transform.Rotate (0,Time.deltaTime *(((-1)*speed)-45f),0);
					if (transform.rotation.y <= 0) {
						transform.Rotate (0,0,0);
						actualRotation = "up";
					}
				}
			}
		}





	}
}
