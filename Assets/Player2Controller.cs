using UnityEngine;
using System.Collections;

//Zwei PlayerControllerScripts ist auch rotz, aber Prototyp und so ;D

public class Player2Controller : MonoBehaviour {

	public float moveForce = 700.0f;
	public float maxSpeed = 5.0f;
	public float jumpForce = 350.0f;

	public Rigidbody rigb;
	private GameObject playerWonText;

	public float timeToRunning = 0.5f;
	private float timePassed;
	private float dashTimer;
	private float aerialTimer;
	private float outOfShieldAttackTimer;
	private float moveTimer = 0;
	private float tempFloat = 0;
	private float directionDelay = -1;

	private GameObject shieldPlayer2;
	private GameObject dashHurtBoxPlayer2;
	public GameObject aerialHurtBoxPlayer2;
	private GameObject outOfShieldAttackPlayer2;

	public bool isDashing = false;
	private bool isAerialing = false;
	private bool outOfShieldAttackActive = false;
	private bool canMove = true;
	private bool running = false;
	public bool grounded = false;
	private bool jump = false;
	private bool isPerformingAnAttack = false; //auch unschön, könnte über klassen und vererbung von moves besser gelöst werden, aber prototyp und so :)
	public string faceDirection = "left";
	private bool godMode = false;
	private bool canTurn = true;

	//audio
	public AudioClip[] audioClip2;

	// Use this for initialization
	void Start () {
		rigb = GetComponent<Rigidbody>();
		shieldPlayer2 = GameObject.Find("ShieldPlayer2");
		shieldPlayer2.active = false;
		dashHurtBoxPlayer2 = GameObject.Find("DashHurtBoxPlayer2");
		dashHurtBoxPlayer2.active = false;
		aerialHurtBoxPlayer2 = GameObject.Find("AerialHurtBoxPlayer2");
		aerialHurtBoxPlayer2.active = false;
		outOfShieldAttackPlayer2 = GameObject.Find("OutOfShieldAttackPlayer2");
		outOfShieldAttackPlayer2.active = false;
		playerWonText = GameObject.Find("PlayerWonText");
	}

	// Update is called once per frame
	void Update () {


		//Change here also in case of DoubleJumps needs to be enabled
		if (Input.GetButton("JumpPlayer2") && grounded ) {
			jump = true;
		}

		if (Input.GetButtonUp("JumpPlayer2")) {
			jump = false;
		}

		//Die ganzen if Abfragen sind unschön, aber okay für Prototyp
		if (Input.GetAxis("HorizontalPlayer2") > 0 && canMove && grounded) {
			if (running == false) {
				gameObject.transform.eulerAngles = new Vector3(0,0,0);
				faceDirection = "right";
			}
			else {
				//yield return new WaitForSeconds(timeToRunning);
				gameObject.transform.eulerAngles = new Vector3(0,0,0);
				faceDirection = "right";
			}
		}
		else if (Input.GetAxis("HorizontalPlayer2") < 0 && canMove && grounded) {
			gameObject.transform.eulerAngles = new Vector3(0,180,0);
			faceDirection = "left";
		}

		// Moves verletzen so Open Closed Prinzip (aber jetzt Prototyping und so :D )
		if (Input.GetButton("ShieldPlayer2") && grounded) {
			canMove = false;
			isPerformingAnAttack = true;
			shieldPlayer2.active = true;
		}

		if (Input.GetButtonUp("ShieldPlayer2") && isPerformingAnAttack) {
			shieldPlayer2.active = false;
			canMove = true;
			isPerformingAnAttack = false;
		}

		if (Input.GetButton("AttackPlayer2") && !grounded && !isPerformingAnAttack) {
			isAerialing = true;
			aerialHurtBoxPlayer2.active = true;
			isPerformingAnAttack = true;
			aerialTimer = 0;
		}

		//Out of Shield Attack
		if (shieldPlayer2.active == true && Input.GetButtonDown("AttackPlayer2")) {
			shieldPlayer2.active = false;
			outOfShieldAttackPlayer2.active = true;
			outOfShieldAttackActive = true;
			outOfShieldAttackTimer = 0;
		}

		if (Input.GetButtonDown("AttackPlayer2") && grounded && !isPerformingAnAttack) {
			dashTimer = 0;
			dashHurtBoxPlayer2.active = true;
			isDashing = true;
			isPerformingAnAttack = true;
		}

		if (dashTimer >= 0.5) {
			canMove = false;
		}


		if (dashTimer >= 0.5 && !Input.GetButton("AttackPlayer2")) {
			dashHurtBoxPlayer2.active = false;
			canMove = true;
			isPerformingAnAttack = false;
		}

		//0.5 seconds disable input for dash attack
		if (isDashing) {
			dashTimer += Time.deltaTime;

			if(dashTimer >= 0.5f && !Input.GetButton("AttackPlayer2")) {
				isDashing = false;
				dashHurtBoxPlayer2.active = false;
				isPerformingAnAttack = false;
				dashTimer = 0;
			}
		}

		//0.5 seconds disable input for aerial attack
		if (isAerialing) {
			aerialTimer += Time.deltaTime;
			if (grounded) {
				isAerialing = false;
				aerialHurtBoxPlayer2.active = false;
				isPerformingAnAttack = false;
			}
		}

		if (outOfShieldAttackActive == true) {
			outOfShieldAttackTimer += Time.deltaTime;
		}

		if ((outOfShieldAttackActive == true && !Input.GetButton("AttackPlayer2") && outOfShieldAttackTimer >= 0.25) || (outOfShieldAttackActive == true && outOfShieldAttackTimer >= 0.5)) {
			outOfShieldAttackActive = false;
			outOfShieldAttackPlayer2.active = false;
		}

		if (moveTimer != 0) {
			moveTimer += Time.deltaTime;
			if (moveTimer >= 0.6) {
				canMove = true;
				moveTimer = 0.0f;
			}
		}

		if (directionDelay >= 0) {
			directionDelay -= Time.deltaTime;
		}
	}

	// FixedUpdate is called once per frame for ribidbody stuff
	void FixedUpdate() {

		float h = Input.GetAxis("HorizontalPlayer2");

		//Turn Delay not quite what we want
		/*if (((tempFloat == 1 && h <= 0) || tempFloat == -1 && h >= 0) && directionDelay <= 0 && canTurn == true) {
			directionDelay = 0.5f;
			canTurn = false;
		}

		if (directionDelay >= 0) {
			print(directionDelay);
			h = tempFloat / 2;
		}
		else {
			tempFloat = h;
			canTurn = true;
		}*/


		if (!canMove && !isDashing) {
			h = 0;
		}

		//Horizontal movement on the ground
		if (h * rigb.velocity.x < maxSpeed && grounded && canMove) {
			rigb.AddForce(Vector2.right * h * moveForce);
		}

		//Movement in the air, maybee this will fix strange behaviour
		if (h * rigb.velocity.x < maxSpeed && grounded == false) {

		}

		//Cutting the velocity over maxSpeed
		if (Mathf.Abs (rigb.velocity.x) > maxSpeed && canMove) {
			rigb.velocity = new Vector2(Mathf.Sign (rigb.velocity.x) * maxSpeed, rigb.velocity.y);
		}

		//Stops the force of the character when no direction is active and on the ground
		if ((h == 0 && grounded) || isDashing) {
			rigb.velocity = Vector3.zero;
			rigb.angularVelocity = Vector3.zero;
			//yield return new WaitForSeconds(0.1f);
			running = false;
		}

		//Jumping
		if (jump && canMove) {
			rigb.AddForce(new Vector2(0, jumpForce));

			//Right now no double jumps allowed
			jump = false;

			//audio
			PlaySound(1);
		}

		//For later Dashdancing
		if (h != 0 && running == false) {
			timePassed += Time.deltaTime;

			if (timeToRunning <= timePassed) {
				running = true;
				timePassed = 0;
			}
		}
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
		}
	}

	void OnTriggerEnter(Collider other) {
		//Collision detection with HurtBox from other players
		if (other.gameObject.layer == LayerMask.NameToLayer("Hitbox")) {
			if ((Input.GetButton("Shield")  && isAerialing == true && faceDirection != other.GetComponent<PlayerController>().faceDirection) || isDashing == true && other.GetComponent<PlayerController>().isDashing == true) {
				print("shielded!!!!");
			}
			else {
				other.gameObject.active = false;
				playerWonText.GetComponent<TextMesh>().text = "Player 2 Won";
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

	public void GodModeEnable() {
		godMode = true;
	}

	public void GodModeDisable() {
		godMode = false;
	}

	//audio
	void PlaySound (int clip) {
		GetComponent<AudioSource>().clip = audioClip2 [clip];
		GetComponent<AudioSource>().Play ();
	}
}
