using UnityEngine;
using System.Collections;

public class PlaneController : MonoBehaviour {

	Animator anim;
	Rigidbody rb;

	public GameObject aileron_left, aileron_right, elevator_left, elevator_right, flap_left, flap_right, rudder;

	public float forward_speed = 25, minimum_air_speed = 15;
	public float pitch_torque = .5f, yaw_torque = .25f, roll_torque = .25f;
	public float pitch = 0, yaw = 0, roll = 0, speed = 0, lift = 0;

	public Vector3 rigidbody_velocity;

	public bool grounded = true;
	void Start () {
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();

		anim.SetBool ("active", true);
	}

	void Update(){
		Debug.DrawRay (transform.position, transform.forward * speed, Color.red);
		Debug.DrawRay (transform.position, rb.velocity, Color.green);

		//elevator_left.transform.rotation =  Quaternion.Euler (pitch * 180, 180, -0.975f);
	}

	void FixedUpdate () {
		rigidbody_velocity = rb.velocity;

		speed = forward_speed * Input.GetAxis ("Jump");
		pitch = pitch_torque * Input.GetAxis ("Vertical");
		yaw = yaw_torque * Input.GetAxis ("Yaw");
		roll = roll_torque * Input.GetAxis ("Roll");

		if(!grounded){
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (0, transform.rotation.eulerAngles.y, 0), Time.deltaTime);
		}

		//pitch, roll, yaw rotation applied to rigidbody : Rigidbody movement
		transform.Rotate (Vector3.right * pitch + Vector3.forward * -roll + Vector3.up * yaw);
		rb.velocity += transform.forward * speed * Time.deltaTime;
		rb.AddForce (transform.up * pitch);

		//LOCAL VELOCITY STUFF-------------
		//limit speed on local z (forward) : limit to forward_speed
		Vector3 local_velocity = transform.InverseTransformDirection (rb.velocity);
		local_velocity.z = Mathf.Clamp (local_velocity.z, 0, forward_speed);

		//set minimum_air_speed : make sure you glide down, not just stop mid-air : only if we are in the air
		//if(target_velocity.z < minimum_air_speed && !grounded){ target_velocity.z = minimum_air_speed; }

		//smoothly move from current_velocity to target_velocity : LERP : make rigidbody straighten out
		local_velocity = Vector3.Lerp (local_velocity, new Vector3(0, 0, local_velocity.z), Time.deltaTime * 2);

		//set minimum_air_speed : make sure you glide down, not just stop mid-air : only if we are in the air
		if (!grounded && local_velocity.z < minimum_air_speed) {//(ie. math.clamp makes -25 to 25, 0 to 25 : 25 when its -25)
			local_velocity = Vector3.Lerp (local_velocity, new Vector3 (local_velocity.x, local_velocity.y, minimum_air_speed), Time.deltaTime * Mathf.Clamp(-speed, 0, forward_speed));
		}

		//apply calculated velocity to rigidbody
		rb.velocity = transform.TransformDirection (local_velocity);

		//calculate lift from local speed;
		lift = (-Physics.gravity.y) * (local_velocity.z / forward_speed);
		if (pitch < .001f) {
			rb.AddForce (transform.up * lift);
		}
	}
	void OnCollisionStay(Collision collisionInfo){
		grounded = true;
	}
	void OnCollisionExit(Collision collisionInfo){
		grounded = false;
	}
}
