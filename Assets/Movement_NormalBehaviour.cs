using UnityEngine;
using System.Collections;

public class Movement_NormalBehaviour : Movement {

	public void Move(Rigidbody rigb, Animator anim, float horizontalForce, float moveForce, float maxSpeed) {
		//Horizontal movement on the ground
		if (Mathf.Abs (rigb.velocity.x) < maxSpeed) {
			rigb.velocity = new Vector2(horizontalForce * moveForce, rigb.velocity.y);
		}
		else {
			//Cutting the velocity over maxSpeed
			//rigb.AddForce(Vector2.right * horizontalForce * moveForce);
		}

		if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && rigb.velocity.x != 0) {
			anim.CrossFade("init Dash", 0.01f);
		}

		if (rigb.velocity.x == 0 && !anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack 01")) {
			//anim.CrossFade("Idle",0.1f);
			anim.Play("Idle");
		}

	}
}
