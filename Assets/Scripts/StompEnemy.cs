using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompEnemy : MonoBehaviour {

	private Rigidbody2D playerRigidBody;

	public float bounceForce;

	public GameObject deathSplosion;

	// Use this for initialization
	void Start () {
		playerRigidBody = gameObject.transform.parent.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "Enemy") {
			playerRigidBody.velocity = new Vector3(playerRigidBody.velocity.x, bounceForce, 0f);
			Instantiate(deathSplosion, other.transform.position, deathSplosion.transform.rotation);
			other.gameObject.SetActive(false);
		}
	}
}
