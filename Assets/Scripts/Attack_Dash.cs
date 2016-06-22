using UnityEngine;
using System.Collections;

public class Attack_Dash : Attack {

	public void Attack(Rigidbody rigb, Animator anim, GameObject hurtBox) {
		if (rigb.GetComponent<PlayerController>().GetCanMove()) {
			rigb.GetComponentInParent<PlayerController>().HurtBoxTime(hurtBox, 1);

			anim.Play("Attack 01", -1, 0f);

			rigb.GetComponent<PlayerController>().CantMoveFor(1.0f);
			rigb.GetComponent<PlayerController>().PlaySound(5);
		}
	}
}
