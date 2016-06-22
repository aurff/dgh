using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public string playerName;

	public float moveForce = 700.0f;
	public float maxSpeed = 5.0f;
	public float jumpForce = 350.0f;
	public float movementTimeInOneDirectionTreshhold = 0.2f;
	public float turnDelay = 0.1f;
	private float turnDelayActive = 0.0f;

	private float h;
	private float hLastFrame = 0.0f;
	private bool canMove = true;
	private bool attacking = false;

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
		else if (h != hLastFrame && movementTimeInOneDirection >= movementTimeInOneDirectionTreshhold && turnDelayActive <= 0) {
			turnDelayActive = turnDelay;
		}
		else {
			movementTimeInOneDirection = 0;
		}

		if (turnDelayActive >= 0) {
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
			}
			attackType.Attack(rigb, anim, activeHurtBox);
		}
			
		//Enable/Disable Shield on Shield-Button
		if (Input.GetButtonDown(shieldButton) && GetCanMove()) {
			if (grounded) {
				shield.SetActive(true);
				CantMove();
				//audio
				PlaySound(3);
				anim.Play("Block");
			}
		}
		if (Input.GetButtonUp(shieldButton) && shield.activeSelf) {
			shield.SetActive(false);
			CanMove();

			anim.Play("Idle");
		}
	}

	void FixedUpdate() {

		hLastFrame = h;
		
		movementType.Move(rigb, anim, h, moveForce, maxSpeed);

		//print (turnDelayActive);

		//Manage Face Direction
		if (h < 0 && rigb.transform.eulerAngles.y != 270) {
			rigb.transform.eulerAngles = new Vector3(0,270,0);
			faceDirection = "left";

		}
		if (h > 0 && rigb.transform.eulerAngles.y != 90) {
			rigb.transform.eulerAngles = new Vector3(0,90,0);
			faceDirection = "right";
		}
	}

	void OnCollisionEnter(Collision collision) {

		switch(collision.gameObject.layer) {
		case 8: //8 = Ground
			grounded = true;
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

	//public Start for Enable Hurtbox for x Seconds
	public void HurtBoxTime(GameObject HurtBox, float t) {
		StartCoroutine(CoHurtBoxTime(HurtBox, t));
	}

	//Coroutine Enable Hurtbox for x Seconds
	IEnumerator CoHurtBoxTime(GameObject hurtBox, float t) {
		hurtBox.SetActive(true);
		yield return new WaitForSeconds(t);
		hurtBox.SetActive(false);
	}

	//audio
	public void PlaySound (int clip) {
		GetComponent<AudioSource>().clip = audioClip [clip];
		GetComponent<AudioSource>().Play ();
	}
}
