using UnityEngine;
using System.Collections;

public class Movement_Jump : Movement {

	public void Move(Rigidbody rigb, Animator anim, float horizontalForce, float moveForce, float maxSpeed) {
		

		rigb.AddForce(new Vector2(0, 12f), ForceMode.VelocityChange);
		//rigb.GetComponent<Animation>().CrossFade("Jump");

		anim.Play("init Jump");
		rigb.GetComponent<PlayerController>().PlaySound(1);
	}


}