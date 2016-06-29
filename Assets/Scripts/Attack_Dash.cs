using UnityEngine;
using System.Collections;

public class Attack_Dash : Attack {

	private float attackDashDelay = 0.08f;
	private float attackDashTime = 0.26f;

	public void Attack(Rigidbody rigb, Animator anim, GameObject hurtBox) {
		if (rigb.GetComponent<PlayerController>().GetCanMove()) {
			rigb.GetComponentInParent<PlayerController>().HurtBoxTime(hurtBox, attackDashTime, attackDashDelay);

			//anim.Play("Attack 01", -1, 0f);
			anim.Play("Attack 01");

			rigb.GetComponent<PlayerController>().CantMoveFor(attackDashDelay + attackDashTime);
			rigb.GetComponent<PlayerController>().IsAttackingFor(attackDashDelay + attackDashTime);
			rigb.GetComponent<PlayerController>().PlaySound(5);
		}
	}
}
