using UnityEngine;
using System.Collections;

public class chopperController : MonoBehaviour {

	Animator anim;
	Rigidbody rBody;

	public bool Active = true;
	public float forwardTorque = 0.5f, yawTorque = 0.5f, rollTorque = 0.5f, verticalSpeed = 15;
	public float forward = 0, vertical = 0, yaw = 0, roll = 0;
	public float hover = 9;
	public Vector3 velocity;

	void Start () {
		anim = GetComponent<Animator> ();
		rBody = GetComponent<Rigidbody> ();
	}

	void FixedUpdate () {
		velocity = rBody.velocity;
		//Vertical (SPACE + LSHIFT)
		vertical = verticalSpeed * Input.GetAxis ("Jump");
		if (!grounded) {
			//Forward (W + S)
			forward = forwardTorque * Input.GetAxis ("Vertical");
			//Yaw, Spin, Turn (Q + E)
			yaw = yawTorque * Input.GetAxis ("Yaw");
			//Roll, Lean (A + D)
			roll = rollTorque * Input.GetAxis ("Roll");
		}

		rBody.velocity += (transform.up * vertical * Time.deltaTime);
		transform.Rotate (Vector3.right * forward + Vector3.forward * -roll + new Vector3 (0, yaw, 0));

		if (forward != 0.25f || yaw != 0.25f || roll != 0.25f) {
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (0, transform.rotation.eulerAngles.y, 0), Time.deltaTime * 1.5f);
		}

		if (grounded) {
			anim.SetBool ("active", false);
		} else {
			anim.SetBool ("active", true);
			//Hover
			rBody.velocity += transform.up * hover * Time.deltaTime;
		}



	}
	bool grounded = false;
	void OnCollisionStay(Collision collisionInfo){
		grounded = true;
	}
	void OnCollisionExit(Collision collisionInfo){
		grounded = false;
	}
}
