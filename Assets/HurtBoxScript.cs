using UnityEngine;
using System.Collections;

public class HurtBoxScript : MonoBehaviour {

	private float sameAttackForce;
	private float attackVsShieldForce = 200.0f;

	// Use this for initialization
	void Start () {
		gameObject.layer = LayerMask.NameToLayer("HurtBox");
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void OnTriggerEnter(Collider other) {
		GameObject parent = this.transform.parent.gameObject;

		//Dash vs Dash && Aerial vs Aerial
		if (other.gameObject.layer == LayerMask.NameToLayer("HurtBox")) {
			if (parent.GetComponent<PlayerController>().grounded) {
				sameAttackForce = 200.0f;
			}
			else {
				sameAttackForce = 400.0f;
			}

			parent.GetComponent<PlayerController>().rigb.velocity = Vector3.zero;
			other.transform.parent.gameObject.GetComponent<PlayerController>().rigb.velocity = Vector3.zero;

			if (parent.GetComponent<PlayerController>().faceDirection == "left") {
				parent.GetComponent<PlayerController>().rigb.AddForce(sameAttackForce,0,0);
				parent.GetComponent<PlayerController>().AttackVsAttackMoveTimer();
			}
			else {
				parent.GetComponent<PlayerController>().rigb.AddForce(-sameAttackForce,0,0);
				parent.GetComponent<PlayerController>().AttackVsAttackMoveTimer();
			}
		}

		//switch kann noch rausgelöscht werden, s.o.
		if (other.gameObject.layer == LayerMask.NameToLayer("Shield")) {
			switch (parent.ToString()) {
			case "Player1 (UnityEngine.GameObject)":
				Vector3 actualForce = parent.GetComponent<PlayerController>().rigb.angularVelocity;
				if (parent.GetComponent<PlayerController>().faceDirection == "left") {
					parent.GetComponent<PlayerController>().rigb.AddForce(-actualForce);
					//parent.GetComponent<PlayerController>().rigb.AddForce(200,attackVsShieldForce,0);
					parent.GetComponent<PlayerController>().rigb.velocity = new Vector3(0, 5, 0);
					parent.GetComponent<PlayerController>().aerialHurtBox.active = false;
					parent.GetComponent<PlayerController>().AttackVsAttackMoveTimer();
				}
				else {
					parent.GetComponent<PlayerController>().rigb.AddForce(-actualForce);
					//parent.GetComponent<PlayerController>().rigb.AddForce(-200,attackVsShieldForce,0);
					parent.GetComponent<PlayerController>().rigb.velocity = new Vector3(0, 5, 0);
					parent.GetComponent<PlayerController>().aerialHurtBox.active = false;
					parent.GetComponent<PlayerController>().AttackVsAttackMoveTimer();
				}
				break;
			case "Player2 (UnityEngine.GameObject)":
				Vector3 actualForce2 = parent.GetComponent<PlayerController>().rigb.angularVelocity;
				if (parent.GetComponent<PlayerController>().faceDirection == "left") {
					parent.GetComponent<PlayerController>().rigb.AddForce(-actualForce2);
					//parent.GetComponent<PlayerController>().rigb.AddForce(200,attackVsShieldForce,0);
					parent.GetComponent<PlayerController>().rigb.velocity = new Vector3(0, 5, 0);
					parent.GetComponent<PlayerController>().aerialHurtBox.active = false;
					parent.GetComponent<PlayerController>().AttackVsAttackMoveTimer();
				}
				else {
					parent.GetComponent<PlayerController>().rigb.AddForce(-actualForce2);
					//parent.GetComponent<PlayerController>().rigb.AddForce(-200,attackVsShieldForce,0);
					parent.GetComponent<PlayerController>().rigb.velocity = new Vector3(0, 5, 0);
					parent.GetComponent<PlayerController>().aerialHurtBox.active = false;
					parent.GetComponent<PlayerController>().AttackVsAttackMoveTimer();
				}
				break;
			}
		}
	}
}
