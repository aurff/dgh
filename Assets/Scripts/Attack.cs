using UnityEngine;
using System.Collections;

public interface Attack {

	void Attack(Rigidbody rigb, Animator anim, GameObject hurtBox);
}
