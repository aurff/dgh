using UnityEngine;
using System.Collections;

public interface Movement {

	void Move(Rigidbody rigb, Animator anim, float horizontalForce, float moveForce, float maxSpeed);
}
