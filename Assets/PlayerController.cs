using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float moveForce = 700.0f;
	public float maxSpeed = 5.0f;
	public float jumpForce = 350.0f;

	public Rigidbody rigb;
	private GameObject playerWonText;

	public float timeToRunning = 0.5f;
	private float timePassed;
	private float dashTimer = 0;
	public float groundAttackTimer = 0.5f;
	private float aerialTimer;
	private float outOfShieldAttackTimer;
	private float moveTimer = 0;

	public GameObject shield;
	public GameObject dashHurtBox;
	public GameObject aerialHurtBox;
	public GameObject outOfShieldAttack;

	public string horizontalAxis;
	public string shieldButton;
	public string jumpButton;
	public string attackButton;

	public string playerName;

	public bool isDashing = false;
	private bool isAerialing = false;
	private bool outOfShieldAttackActive = false;
	public bool canMove = true;
	private bool running = false;
	public bool grounded = false;
	private bool jump = false;
	private bool isPerformingAnAttack = false;
	public string faceDirection = "right";
	private float tempH;
	private bool jumpVelocity = false;

	private Vector3 tempVelocity;

	//audio
	public AudioClip[] audioClip;

	// Use this for initialization
	void Start () {
		rigb = GetComponent<Rigidbody>();
		shield.SetActive(false);
		dashHurtBox.SetActive(false);
		aerialHurtBox.SetActive(false);
		outOfShieldAttack.SetActive(false);
		playerWonText = GameObject.Find("PlayerWonText");
	}

	// Update is called once per frame
	void Update () {


		//Change here also in case of DoubleJumps needs to be enabled
		if (Input.GetButton(jumpButton) && grounded ) {
			jump = true;
		}

		if (Input.GetButtonUp(jumpButton)) {
			jump = false;
		}

		//Die ganzen if Abfragen sind unschön, aber okay für Prototyp
		if (Input.GetAxis(horizontalAxis) > 0 && canMove && grounded && !isDashing) {
			gameObject.transform.eulerAngles = new Vector3(0,0,0);
			faceDirection = "right";
		}
		else if (Input.GetAxis(horizontalAxis) < 0 && canMove && grounded && !isDashing) {
			gameObject.transform.eulerAngles = new Vector3(0,180,0);
			faceDirection = "left";
		}

		if (Input.GetButton(shieldButton)) {
			if (grounded && !isPerformingAnAttack) {
				ShieldEnable();
				//audio
				PlaySound(3);
			}

			//Out of Shield Attack
			else if (Input.GetButtonDown(attackButton)) {
				OutOfShieldAttack();
				//audio
				PlaySound(4);
			}
		}

		if (Input.GetButtonUp(shieldButton)) {
			ShieldDisable();
		}

		if (Input.GetButton(attackButton) && !grounded && !isPerformingAnAttack) {
			isAerialing = true;
			aerialHurtBox.active = true;
			isPerformingAnAttack = true;
			aerialTimer = 0;
			//audio
			PlaySound(2);
		}

		if (Input.GetButtonDown(attackButton) && grounded && !isPerformingAnAttack) {
			dashTimer = 0;
			dashHurtBox.active = true;
			isDashing = true;
			isPerformingAnAttack = true;
			//audio
			PlaySound(5);
		}

		if (dashTimer >= 0.5) {
			canMove = false;
		}

		if (dashTimer >= 0.5 && !Input.GetButton(attackButton)) {
			dashHurtBox.active = false;
			canMove = true;
			isPerformingAnAttack = false;
		}

		//0.5 seconds disable input for dash attack
		if (isDashing) {
			dashTimer += Time.deltaTime;

			//if(dashTimer >= 0.5f && !Input.GetButton(attackButton)) {
			if(dashTimer >= groundAttackTimer) {
				isDashing = false;
				dashHurtBox.active = false;
				isPerformingAnAttack = false;
				dashTimer = 0;
			}
		}

		//disable aerialing when on the ground again
		if (isAerialing) {
			aerialTimer += Time.deltaTime;
			if (grounded) {
				isAerialing = false;
				aerialHurtBox.active = false;
				isPerformingAnAttack = false;
			}
		}

		if (outOfShieldAttackActive == true) {
			outOfShieldAttackTimer += Time.deltaTime;
		}

		if ((outOfShieldAttackActive == true && !Input.GetButton(attackButton) && outOfShieldAttackTimer >= 0.5) || (outOfShieldAttackActive == true && outOfShieldAttackTimer >= 0.5)) {
			outOfShieldAttackActive = false;
			outOfShieldAttack.active = false;
		}

		if (moveTimer != 0) {
			moveTimer += Time.deltaTime;
			if (moveTimer >= 0.6) {
				canMove = true;
				moveTimer = 0.0f;
			}
		}
	}

	// FixedUpdate is called once per frame for ribidbody stuff
	void FixedUpdate() {
		print(grounded);
		float h = 0;

		if (!isDashing) {
			h = Input.GetAxis(horizontalAxis);
		}
		else {
			h = tempH;
		}


		if (!canMove && !isDashing) {
			h = 0;
		}

		//Horizontal movement on the ground
		if (h * rigb.velocity.x < maxSpeed && grounded && canMove && !isDashing) {
			rigb.AddForce(Vector2.right * h * moveForce);
		}

		//Movement in the air, maybee this will fix strange behaviour
		if (h * rigb.velocity.x < (maxSpeed + 0.1f)  && (jump || !grounded) && canMove) {
			rigb.AddForce(Vector2.right * tempH * moveForce);
		}

		//Cutting the velocity over maxSpeed
		if (Mathf.Abs (rigb.velocity.x) > maxSpeed && canMove) {
			rigb.velocity = new Vector2(Mathf.Sign (rigb.velocity.x) * maxSpeed, rigb.velocity.y);
		}

		//Stops the force of the character when no direction is active and on the ground
		if (h == 0 && grounded) {
			rigb.velocity = Vector3.zero;
			rigb.angularVelocity = Vector3.zero;
			//yield return new WaitForSeconds(0.1f);
			running = false;
		}

		if (isDashing) {

		}

		//Jumping
		if (jump && canMove) {
			if (h == 0) {
				jumpVelocity = false;
			}
			else {
				jumpVelocity = true;
			}

			rigb.AddForce(new Vector2(0, 7.5f), ForceMode.VelocityChange);

			//rigb.AddForce(new Vector3(0, 5, 0), ForceMode.VelocityChange);
			//rigb.AddForce(10.0f * rigb.transform.forward + new Vector3(0,4,0), ForceMode.VelocityChange);

			//Right now no double jumps allowed
			jump = false;

			//audio
			PlaySound(1);
		}

		if (jumpVelocity == true) {
			//tempVelocity = rigb.velocity;
			if (faceDirection == "right") {
				//rigb.velocity = new Vector3(7,tempVelocity.y, tempVelocity.z);
				print(rigb.velocity);
			}
			else {
				//rigb.velocity = new Vector3(-70,tempVelocity.y, tempVelocity.z);
				print(rigb.velocity);
			}
		}



		//For later Dashdancing
		if (h != 0 && running == false) {
			timePassed += Time.deltaTime;

			if (timeToRunning <= timePassed) {
				running = true;
				timePassed = 0;
			}
		}

		tempH = h;
	}



	void OnCollisionEnter(Collision collision) {
		//Collision with ground
		if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
			grounded = true;
		}
	}

	void OnCollisionExit(Collision collision) {
		//Not grounded anymore
		if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
			grounded = false;
			//jumpVelocity = false;
		}
	}

	void OnTriggerEnter(Collider other) {
		//Collision detection with HurtBox from other players
		if (other.gameObject.layer == LayerMask.NameToLayer("Hitbox")) {
			if ((other.GetComponent<PlayerController>().shield.activeInHierarchy == true && isAerialing == true && faceDirection != other.GetComponent<PlayerController>().faceDirection) || isDashing == true && other.GetComponent<PlayerController>().isDashing == true) {
				print("shielded!!!!");
			}
			else {
				other.gameObject.active = false;
				playerWonText.GetComponent<TextMesh>().text = playerName + " wins";
				playerWonText.GetComponent<LevelScripts>().RestartLevel();
				//audio
				PlaySound(6);
			}
		}
	}

	public void AttackVsAttackMoveTimer() {
		canMove = false;
		moveTimer = 0.1f;
	}

	public void CantMove() {
		canMove = false;
	}

	public void CanMove() {
		canMove = true;
	}

	public void ShieldEnable() {
		shield.SetActive(true);
		canMove = false;
		isPerformingAnAttack = true;
	}

	public void ShieldDisable() {
		shield.SetActive(false);
		canMove = true;
		isPerformingAnAttack = false;
	}

	public void OutOfShieldAttack() {
		shield.SetActive(false);
		outOfShieldAttack.SetActive(true);
		outOfShieldAttackActive = true;
		outOfShieldAttackTimer = 0;
	}

	//audio
	void PlaySound (int clip) {
		GetComponent<AudioSource>().clip = audioClip [clip];
		GetComponent<AudioSource>().Play ();
	}
}