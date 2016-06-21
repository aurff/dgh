using UnityEngine;
using System.Collections;

public class Attack_Aerial : Attack {

	public void Attack(Rigidbody rigb, Animator anim, GameObject hurtBox) {
		rigb.GetComponentInParent<PlayerController>().HurtBoxTime(hurtBox, 1);
		anim.Play("Jump Attack");
		rigb.GetComponent<PlayerController>().PlaySound(2);
	}
}
