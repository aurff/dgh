using UnityEngine;
using System.Collections;

public class Attack_Aerial : Attack {

	private float attackAerialDelay = 0.0f;
	private float attackAerialTime = 0.0f;

	public void Attack(Rigidbody rigb, Animator anim, GameObject hurtBox) {
		if (rigb.GetComponent<PlayerController>().GetCanMove() && anim.GetCurrentAnimatorStateInfo(0).IsName("Jump")) {
			rigb.GetComponentInParent<PlayerController>().HurtBoxTime(hurtBox, attackAerialTime, attackAerialDelay);
			anim.Play("Jump Attack");
			rigb.GetComponent<PlayerController>().PlaySound(2);
			rigb.GetComponent<PlayerController>().IsAttackingFor(attackAerialDelay + attackAerialTime);
		}
	}
}
