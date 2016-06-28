using UnityEngine;
using System.Collections;

public class Movement_NormalBehaviour : Movement {

	public void Move(Rigidbody rigb, Animator anim, float horizontalForce, float moveForce, float maxSpeed) {

		if (anim.GetCurrentAnimatorStateInfo(0).IsName("init Dash")) {
			rigb.velocity = new Vector2(horizontalForce * moveForce * 1.5f, rigb.velocity.y);
			Debug.Log("faster");
		}

		if (GameObject.Find("CountdownText").GetComponent<TextMesh>().text == "") {
			//Horizontal movement on the ground
			//if (Mathf.Abs (rigb.velocity.x) < maxSpeed && rigb.GetComponent<PlayerController>().grounded) {
			if (rigb.GetComponent<PlayerController>().grounded && !anim.GetCurrentAnimatorStateInfo(0).IsName("Victory")) {
				rigb.velocity = new Vector2(horizontalForce * moveForce, rigb.velocity.y);
			}

			/*if (rigb.GetComponent<PlayerController>().GetAirTurnDelay()) {
				if (rigb.GetComponent<PlayerController>().h > 0 && rigb.GetComponent<PlayerController>().faceDirection == "left") {
					rigb.AddForce(new Vector3(rigb.GetComponent<PlayerController>().h * 140, 0, 0));
				}
				if (rigb.GetComponent<PlayerController>().h < 0 && rigb.GetComponent<PlayerController>().faceDirection == "right") {
					rigb.AddForce(new Vector3(rigb.GetComponent<PlayerController>().h * 140, 0, 0));
				}



			}*/
				
			/*if (Mathf.Abs (rigb.velocity.x) < maxSpeed && !rigb.GetComponent<PlayerController>().grounded) { //&& rigb.GetComponent<PlayerController>().GetAirTurnDelay()) {
				rigb.velocity = new Vector2(horizontalForce * moveForce / 3, rigb.velocity.y);
			}

			else if (!rigb.GetComponent<PlayerController>().grounded && rigb.GetComponent<PlayerController>().GetAirTurnDelay()) {
				rigb.velocity = new Vector2(horizontalForce * moveForce, rigb.velocity.y);
			}*/
			//else {
				//Cutting the velocity over maxSpeed
				//rigb.AddForce(Vector2.right * horizontalForce * moveForce);
			//}

			if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && rigb.velocity.x != 0) {
				anim.CrossFade("init Dash", 0.01f);
			}

		if (rigb.GetComponent<PlayerController>().grounded && rigb.velocity.x == 0 && !anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack 01") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Shield") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Block Attack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Dash Stop") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Pivot") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Jump land") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Shield Drop") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Victory")) {
				//anim.CrossFade("Idle",0.1f);
				anim.Play("Idle");
			}

			if (Input.GetButtonDown(rigb.GetComponent<PlayerController>().backDashButton)) {				
				rigb.GetComponent<PlayerController>().CantMoveFor(0.5f);
				rigb.velocity = new Vector3(0,0,0);
				anim.Play("Main WD");
				if (rigb.GetComponent<PlayerController>().faceDirection == "left") {
					rigb.velocity = new Vector2(20, 0);
					rigb.GetComponent<PlayerController>().DashBackForce();
				}
				else {
					rigb.velocity = new Vector2(-20, 0);
					rigb.GetComponent<PlayerController>().DashBackForce();
				}
				
				if (!rigb.GetComponent<PlayerController>().grounded) {
					anim.Play("Jump WD");
				}


			}
		}
	}
}
