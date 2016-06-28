using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public string playerName;

	public float moveForce = 700.0f;
	public float maxSpeed = 5.0f;
	public float jumpForce = 350.0f;
	//public float movementTimeInOneDirectionTreshhold = 0.3f;
	public float turnDelay = 0.15f;
	private float turnDelayActive = 0.0f;

	public float h;
	private float hLastFrame = 0.0f;
	private bool canMove = true;
	private bool attacking = false;
	private bool airTurnDelay = false;

	public GameObject shield;
	public GameObject dashHurtBox;
	public GameObject aerialHurtBox;
	public GameObject outOfShieldAttackHurtBox;

	private Movement movementType;
	private Attack attackType;
	private GameObject activeHurtBox;

	//Button Belegung
	public string horizontalAxis;
	public string jumpButton;
	public string attackButton;
	public string shieldButton;
	public string backDashButton;

	//Unschön, dass diese Public sind
	public string faceDirection;
	public bool grounded;

	public Animator anim;

	public Rigidbody rigb;

	//audio
	public AudioClip[] audioClip;

	private float movementTimeInOneDirection = 0.0f;

	// Use this for initialization
	void Start () {
		SetMovementType(new Movement_NormalBehaviour());
		shield.SetActive(false);
		dashHurtBox.SetActive(false);
		outOfShieldAttackHurtBox.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

		h = Input.GetAxisRaw(horizontalAxis);

		//Movement Deklarieren 
		if (Input.GetButtonDown(jumpButton) && grounded && canMove) {
			SetMovementType(new Movement_Jump());
			AirTurnDelay(0.2f);
		}
		else if (!canMove) {
			SetMovementType(new Movement_CantMove());
		}
		else {
			SetMovementType(new Movement_NormalBehaviour());
		}

		if (h == hLastFrame && h != 0) {
			movementTimeInOneDirection += Time.deltaTime;
		}
		else if (h != hLastFrame && anim.GetCurrentAnimatorStateInfo(0).IsName("Dash") && turnDelayActive <= 0 && grounded) {
			turnDelayActive = turnDelay;
			anim.Play("Pivot");
			Debug.Log("Pivot");
		}
		//Not sure if needed
		else if (h != hLastFrame && anim.GetCurrentAnimatorStateInfo(0).IsName("init Dash") && turnDelayActive <= 0 && grounded) {
			anim.Play("Dash Stop");
			Debug.Log("Dash Stop");
		}
		else {
			movementTimeInOneDirection = 0;
		}

		if (h == 0 && anim.GetCurrentAnimatorStateInfo(0).IsName("init Dash") && grounded) {
			anim.Play("Dash Stop");
			Debug.Log("Dash Stop");
		}

		if (turnDelayActive > 0) {
			turnDelayActive -= Time.deltaTime;
			h = hLastFrame;
		}

		//Attacken deklaration
		if (Input.GetButtonDown(attackButton)) {
			if (shield.activeSelf) {
				SetAttackType(new Attack_OutOfShieldAttack(), outOfShieldAttackHurtBox);
			}
			else if (grounded) {
				SetAttackType(new Attack_Dash(), dashHurtBox);
			}
			else {
				SetAttackType(new Attack_Aerial(), aerialHurtBox);
				Debug.Log("set aerial");
			}
			attackType.Attack(rigb, anim, activeHurtBox);
		}
			
		//Enable/Disable Shield on Shield-Button
		if (Input.GetButtonDown(shieldButton) && GetCanMove()) {
			if (grounded) {
				rigb.velocity = new Vector2(0, 0);
				shield.SetActive(true);
				attacking = true;
				CantMove();
				//audio
				PlaySound(3);
				anim.Play("Shield");
			}
		}
		if (Input.GetButtonUp(shieldButton) && shield.activeSelf) {
			shield.SetActive(false);
			CantMoveFor(0.15f);
			attacking = false;
			anim.Play("Shield Drop");
		}
	}

	void FixedUpdate() {

		hLastFrame = h;
		
		movementType.Move(rigb, anim, h, moveForce, maxSpeed);

		//Manage Face Direction
		if (h < 0 && rigb.transform.eulerAngles.y != 270 && attacking == false && grounded) {
			rigb.transform.eulerAngles = new Vector3(0,270,0);
			faceDirection = "left";

		}
		if (h > 0 && rigb.transform.eulerAngles.y != 90 && attacking == false && grounded) {
			rigb.transform.eulerAngles = new Vector3(0,90,0);
			faceDirection = "right";
		}

		//Soll nicht gewollte Rotation von Rigidbody unterbinden
		if (rigb.transform.eulerAngles.x != 0 || rigb.transform.eulerAngles.z != 0) {
			rigb.transform.eulerAngles = new Vector3(0, rigb.transform.eulerAngles.y, 0);
		}

		//Jump Landing Animation
		if (!grounded && rigb.velocity.y < 0 && !anim.GetCurrentAnimatorStateInfo(0).IsName("Jump land") && rigb.position.y <= 1) {
			anim.Play("Jump land");
		}
	}

	void OnCollisionEnter(Collision collision) {

		switch(collision.gameObject.layer) {
		case 8: //8 = Ground
			grounded = true;
			airTurnDelay = false;
			moveForce = 14;
			break;
		case 12: //12 = LevelboundLeft
			
			break;
		case 13: //13 = LevelboundRight
			
			break;
		}
	}

	void OnCollisionExit(Collision collision) {
		//Not grounded anymore
		switch(collision.gameObject.layer) {
		case 8: //Ground
			grounded = false;
			break;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer("Hitbox")) {
			if ((other.GetComponent<PlayerController>().shield.activeInHierarchy == true && faceDirection != other.GetComponent<PlayerController>().faceDirection)) {
			}
			else {
				GameObject playerWonText = GameObject.Find("PlayerWonText");
				other.gameObject.active = false;
				playerWonText.GetComponent<TextMesh>().text = playerName + " wins";

				playerWonText.GetComponent<LevelScripts>().SetRoundsWon(playerName);
				PlaySound(6);
				playerWonText.GetComponent<LevelScripts>().RestartLevel();
				//audio
				//PlaySound(6);
			}
		}
	}

	public void SetMovementType(Movement _movementType) {
		movementType = _movementType;
	}

	public void SetAttackType(Attack _attackType, GameObject hurtBox) {
		attackType = _attackType;
		activeHurtBox = hurtBox;
	}

	public Movement GetMovementType() {
		return movementType;
	}

	public float GetTurnDelayActive() {
		return turnDelayActive;
	}

	public void CantMove() {
		canMove = false;
		h = 0;
	}

	public void CanMove() {
		canMove = true;
	}

	public bool GetCanMove() {
		return canMove;
	}

	public void IsAttacking() {
		attacking = true;
	}

	public void NotAttacking() {
		attacking = false;
	}

	public bool GetIsAttacking() {
		return attacking;
	}

	//public Start for Disable Movement Input for x Seconds
	public void CantMoveFor(float t) {
		StartCoroutine(CoCantMoveFor(t));
	}

	//Coroutine Disable Movement Input for x Seconds
	IEnumerator CoCantMoveFor(float t) {
		canMove = false;
		yield return new WaitForSeconds(t);
		canMove = true;
	}

	public void IsAttackingFor(float t) {
		StartCoroutine(CoIsAttackingFor(t));
	}

	IEnumerator CoIsAttackingFor(float t) {
		attacking = true;
		yield return new WaitForSeconds(t);
		attacking = false;
	}

	//public Start for Enable Hurtbox for x Seconds
	public void HurtBoxTime(GameObject HurtBox, float time, float delay) {
		StartCoroutine(CoHurtBoxTime(HurtBox, time, delay));
	}

	//Coroutine Enable Hurtbox for x Seconds
	IEnumerator CoHurtBoxTime(GameObject hurtBox, float t, float d) {
		yield return new WaitForSeconds(d);
		hurtBox.SetActive(true);
		yield return new WaitForSeconds(t);
		hurtBox.SetActive(false);
	}

	public void AirTurnDelay(float t) {
		StartCoroutine(CoAirTurnDelay(t));
	}

	IEnumerator CoAirTurnDelay(float t) {
		yield return 0;
		SetMovementType(new Movement_NormalBehaviour());
		airTurnDelay = true;
		yield return new WaitForSeconds(t);
		airTurnDelay = false;
	}

	public bool GetAirTurnDelay() {
		return airTurnDelay;
	}

	public void DashBackForce() {
		StartCoroutine(CoDashBackForce());
	}

	IEnumerator CoDashBackForce() {
		int x;
		if (faceDirection == "left") {
			x = -80;
		}
		else {
			x = 80;
		}

		for (int i = 0; i <= 10; i++) {
			rigb.AddForce(new Vector3(x, 0, 0));
			yield return new WaitForSeconds(0.05f);
		}
	}

	//audio
	public void PlaySound (int clip) {
		GetComponent<AudioSource>().clip = audioClip [clip];
		GetComponent<AudioSource>().Play ();
	}
}
