using UnityEngine;
using System.Collections;

public class Movement_NormalBehaviour : Movement {

	public void Move(Rigidbody rigb, Animator anim, float horizontalForce, float moveForce, float maxSpeed) {

		if (GameObject.Find("CountdownText").GetComponent<TextMesh>().text == "") {
			//Horizontal movement on the ground
			if (Mathf.Abs (rigb.velocity.x) < maxSpeed && rigb.GetComponent<PlayerController>().grounded) {
				rigb.velocity = new Vector2(horizontalForce * moveForce, rigb.velocity.y);
			}

			else if (Mathf.Abs (rigb.velocity.x) < maxSpeed && !rigb.GetComponent<PlayerController>().grounded && rigb.GetComponent<PlayerController>().GetAirTurnDelay()) {
				rigb.velocity = new Vector2(horizontalForce * moveForce / 3, rigb.velocity.y);
			}
			else {
				//Cutting the velocity over maxSpeed
				//rigb.AddForce(Vector2.right * horizontalForce * moveForce);
			}

			if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && rigb.velocity.x != 0) {
				anim.CrossFade("init Dash", 0.01f);
			}

			if (rigb.GetComponent<PlayerController>().grounded && rigb.velocity.x == 0 && !anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack 01") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Block") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Block Attack")) {
				//anim.CrossFade("Idle",0.1f);
				anim.Play("Idle");
			}

			if (Input.GetButtonDown(rigb.GetComponent<PlayerController>().backDashButton)) {				
				rigb.GetComponent<PlayerController>().CantMoveFor(0.5f);
				rigb.velocity = new Vector3(0,0,0);
				anim.Play("Idle");
				if (rigb.GetComponent<PlayerController>().faceDirection == "left") {
					rigb.velocity = new Vector2(13, 0);
				}
				else {
					rigb.velocity = new Vector2(-13, 0);
				}
			}
		}
	}
}
