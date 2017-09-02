using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetOnRespawn : MonoBehaviour {

	private Vector3 startingPosition;
	private Quaternion startingRotation;
	private Vector3 startingScale;

	private Rigidbody2D myRigidbody;

	// Use this for initialization
	void Start () {
		startingPosition = gameObject.transform.position;
		startingRotation = gameObject.transform.rotation;
		startingScale = gameObject.transform.localScale;

		if(GetComponent<Rigidbody2D>()) {
			myRigidbody = GetComponent<Rigidbody2D>();
		}
	}

	void Update() {
		
	}

	public void Reset() {
		gameObject.SetActive(true);

		transform.position = startingPosition;
		transform.rotation = startingRotation;
		transform.localScale = startingScale;

		if(myRigidbody) {
			myRigidbody.velocity = Vector3.zero;
		}
	}
}