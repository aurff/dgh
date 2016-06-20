using UnityEngine;
using System.Collections;

public class Attack_OutOfShieldAttack : Attack {
	
	public void Attack(Rigidbody rigb, Animator anim, GameObject hurtBox) {
		rigb.GetComponentInParent<PlayerController>().shield.SetActive(false);
		rigb.GetComponentInParent<PlayerController>().HurtBoxTime(hurtBox, 1);
	}
}