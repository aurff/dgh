using UnityEngine;
using System.Collections;

public class Attack_Aerial : Attack {

	public void Attack(Rigidbody rigb, Animator anim, GameObject hurtBox) {
		if (rigb.GetComponent<PlayerController>().GetCanMove()) {
			rigb.GetComponentInParent<PlayerController>().HurtBoxTime(hurtBox, 1);
			anim.Play("Jump Attack");
			rigb.GetComponent<PlayerController>().PlaySound(2);
			rigb.GetComponent<PlayerController>().IsAttackingFor(1);
		}
	}
}
