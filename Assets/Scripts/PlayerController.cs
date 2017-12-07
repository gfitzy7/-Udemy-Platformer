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

	public bool canMove;

	public Rigidbody2D myRigidbody;
	private Animator myAnimator;

	public Vector3 respawnPos;
	private bool isRespawning = false;

	public LevelManager levelManager;

	public GameObject stompBox;

	public float knockbackForce;
	public float knockbackDuration;
	private float knockbackCounter;

	public AudioSource jumpSound;
	public AudioSource hurtSound;

	private bool onPlatform;
	public float onPlatformSpeedModifier;

	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody2D>();
		myAnimator = GetComponent<Animator>();

		respawnPos = transform.position;

		levelManager = FindObjectOfType<LevelManager>();

		canMove = true;
	}
	
	// Update is called once per frame
	void Update () {

		if(knockbackCounter <= 0 && canMove) {

			float modifiedMoveSpeed = moveSpeed;

			if(onPlatform){
				modifiedMoveSpeed *= onPlatformSpeedModifier;
			}

			if(Input.GetAxisRaw("Horizontal") > 0f){
				myRigidbody.velocity = new Vector3(modifiedMoveSpeed, myRigidbody.velocity.y, 0f);
				transform.localScale = new Vector3(1f, 1f, 1f);
			}
			else if(Input.GetAxisRaw("Horizontal") < 0f){
				myRigidbody.velocity = new Vector3(-modifiedMoveSpeed, myRigidbody.velocity.y, 0f);
				transform.localScale = new Vector3(-1f, 1f, 1f);
			}
			else{
				myRigidbody.velocity = new Vector3(0f, myRigidbody.velocity.y, 0f);
			}

			if(Input.GetAxisRaw("Jump") > 0f && isGrounded){
				myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, jumpPower, 0f);
				jumpSound.Play();
			}

			levelManager.invincible = false;
		}
		else if(knockbackCounter > 0) {
			knockbackCounter -= Time.deltaTime;

			if(transform.localScale.x > 0){
				myRigidbody.velocity = new Vector3(knockbackForce, Mathf.Abs(knockbackForce), 0f);
			}
			else{
				myRigidbody.velocity = new Vector3(-knockbackForce, Mathf.Abs(knockbackForce), 0f);
			}
		}

		isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
		//isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(0.5f, 0.02f), 0f, whatIsGround);

		myAnimator.SetFloat("Speed", Mathf.Abs(myRigidbody.velocity.x));
		myAnimator.SetBool("Grounded", isGrounded);

		if(myRigidbody.velocity.y < 0){
			stompBox.SetActive(true);
		}
		else{
			stompBox.SetActive(false);
		}
	}

	public void Knockback() {
		knockbackCounter = knockbackDuration;

		levelManager.invincible = true;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "KillPlane"){
			if(!isRespawning) {
				setRespawning(true);
				levelManager.Respawn();
			}
		}

		if(other.tag == "Checkpoint"){
			respawnPos = other.transform.position;
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if(other.gameObject.tag == "MovingPlatform") {
			transform.parent = other.transform;
			onPlatform = true;
		}
	}

	void OnCollisionExit2D(Collision2D other) {
		if(other.gameObject.tag == "MovingPlatform") {
			transform.parent = null;
			onPlatform = false;
		}
	}

	public void setRespawning(bool isRespawning){
		this.isRespawning = isRespawning;
	}

}