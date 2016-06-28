using UnityEngine;
using System.Collections;

public class Movement_Jump : Movement {

	public void Move(Rigidbody rigb, Animator anim, float horizontalForce, float moveForce, float maxSpeed) {
		

		rigb.AddForce(new Vector2(0, 11f), ForceMode.VelocityChange);
		//rigb.GetComponent<Animation>().CrossFade("Jump");

		anim.Play("Jump", -1,0f);
		rigb.GetComponent<PlayerController>().PlaySound(1);
	}


}