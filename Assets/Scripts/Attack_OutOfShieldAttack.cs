using UnityEngine;
using System.Collections;

public class Attack_OutOfShieldAttack : Attack {
	
	public void Attack(Rigidbody rigb, Animator anim, GameObject hurtBox) {
		if (rigb.GetComponent<PlayerController>().GetCanMove()) {
			rigb.GetComponentInParent<PlayerController>().shield.SetActive(false);
			rigb.GetComponentInParent<PlayerController>().HurtBoxTime(hurtBox, 1);
			anim.Play("Block Attack");
			rigb.GetComponent<PlayerController>().PlaySound(4);
		}
	}
}