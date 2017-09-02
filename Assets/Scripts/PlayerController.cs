using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float moveSpeed;
	public float jumpPower;

	public Transform groundCheck;
	public float groundCheckRadius;
	public LayerMask whatIsGround;
	private bool isGrounded;

	private Rigidbody2D myRigidbody;
	private Animator myAnimator;

	public Vector3 respawnPos;

	public LevelManager levelManager;

	public GameObject stompBox;

	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody2D>();
		myAnimator = GetComponent<Animator>();

		respawnPos = transform.position;

		levelManager = FindObjectOfType<LevelManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetAxisRaw("Horizontal") > 0f){
			myRigidbody.velocity = new Vector3(moveSpeed, myRigidbody.velocity.y, 0f);
			transform.localScale = new Vector3(1f, 1f, 1f);
		}
		else if(Input.GetAxisRaw("Horizontal") < 0f){
			myRigidbody.velocity = new Vector3(-moveSpeed, myRigidbody.velocity.y, 0f);
			transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		else{
			myRigidbody.velocity = new Vector3(0f, myRigidbody.velocity.y, 0f);
		}

		if(Input.GetAxisRaw("Jump") > 0f && isGrounded){
			myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, jumpPower, 0f);
		}

		isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
		isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(0.5f, 0.02f), 0f, whatIsGround);

		myAnimator.SetFloat("Speed", Mathf.Abs(myRigidbody.velocity.x));
		myAnimator.SetBool("Grounded", isGrounded);

		if(myRigidbody.velocity.y < 0){
			stompBox.SetActive(true);
		}
		else{
			stompBox.SetActive(false);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "KillPlane"){
			levelManager.Respawn();
		}

		if(other.tag == "Checkpoint"){
			respawnPos = other.transform.position;
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if(other.gameObject.tag == "MovingPlatform") {
			transform.parent = other.transform;
		}
	}

	void OnCollisionExit2D(Collision2D other) {
		if(other.gameObject.tag == "MovingPlatform") {
			transform.parent = null;
		}
	}

}