﻿using UnityEngine;
using System.Collections;

public class Attack_OutOfShieldAttack : Attack {

	private float outOfShieldAttackDelay = 0.13f;
	private float outOfShieldAttackTime = 0.3f;
	
	public void Attack(Rigidbody rigb, Animator anim, GameObject hurtBox) {
		if (rigb.GetComponent<PlayerController>().GetCanMove() || (!rigb.GetComponent<PlayerController>().GetCanMove() && rigb.GetComponent<PlayerController>().shield.activeSelf)) {
			rigb.GetComponentInParent<PlayerController>().shield.SetActive(false);
			rigb.GetComponentInParent<PlayerController>().HurtBoxTime(hurtBox, outOfShieldAttackTime, outOfShieldAttackDelay);
			anim.Play("Block Attack");
			rigb.GetComponent<PlayerController>().PlaySound(4);
			rigb.GetComponent<PlayerController>().CantMoveFor(outOfShieldAttackDelay + outOfShieldAttackTime);
			rigb.GetComponent<PlayerController>().IsAttackingFor(outOfShieldAttackDelay + outOfShieldAttackTime);
		}
	}
}