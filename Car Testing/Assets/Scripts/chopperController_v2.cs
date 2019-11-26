using UnityEngine;
using System.Collections;

public class chopperController_v2 : MonoBehaviour {

	Rigidbody rb;
	public GameObject main_rotor, tail_rotor;

	public float max_main_rotor_force = 22241.1081f,
		max_main_rotor_velocity = 7200,
		main_rotor_velocity = 0.0f,
		main_rotor_rotation = 0.0f;
	public float max_tail_rotor_force = 15000,
		max_tail_rotor_velocity = 2200,
		tail_rotor_velocity = 0.0f,
		tail_rotor_rotation = 0.0f;

	float forward_rotor_torque_multiplier = 0.5f,
		sideways_rotor_torque_multiplier = 0.5f;

	public bool main_rotor_active = true,
		tail_rotor_active = true;

	float main_rotor_hover_velocity,
		tail_rotor_hover_velocity;

	Vector3 torque;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		main_rotor_hover_velocity = (rb.mass * Mathf.Abs (Physics.gravity.y) / max_main_rotor_force);
		torque = Vector3.zero;
	}

	void FixedUpdate () {
		
		Vector3 input_torque = new Vector3 (Input.GetAxis ("Vertical") * forward_rotor_torque_multiplier, 1.0f, -Input.GetAxis ("Horizontal") * sideways_rotor_torque_multiplier);

		if(main_rotor_active){
			torque += (input_torque * max_main_rotor_force * main_rotor_velocity);
			rb.AddRelativeForce (Vector3.up * max_main_rotor_force * main_rotor_velocity);

			if (Vector3.Angle (Vector3.up, transform.up) < 80) {
				transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (0, transform.rotation.eulerAngles.y, 0), Time.deltaTime * main_rotor_velocity * 2);
			}
		}

		if(tail_rotor_active){
			torque -= (Vector3.up * max_tail_rotor_force * tail_rotor_velocity);
		}

		rb.AddRelativeTorque (torque);

	}

	void Update(){
		tail_rotor_hover_velocity = (max_main_rotor_force * main_rotor_velocity) / max_tail_rotor_force;

		if (main_rotor_active) {
			//main_rotor.transform.rotation = transform.rotation * Quaternion.Euler (0, main_rotor_rotation, 0);
		}
		if (tail_rotor_active) {
			//tail_rotor.transform.rotation = transform.rotation * Quaternion.Euler (tail_rotor_rotation, 0, 0);
		}

		main_rotor_rotation += max_main_rotor_velocity * main_rotor_velocity * Time.deltaTime;
		tail_rotor_rotation += max_main_rotor_velocity * tail_rotor_velocity * Time.deltaTime;

		if (Input.GetAxis ("Jump") != 0.0f) {
			main_rotor_velocity += (float)(Input.GetAxis ("Jump") * 0.001);
		} else {
			main_rotor_velocity = Mathf.Lerp (main_rotor_velocity, main_rotor_hover_velocity, Time.deltaTime * Time.deltaTime * 5);
		}

		tail_rotor_velocity = tail_rotor_hover_velocity - Input.GetAxis ("Horizontal");

		if(main_rotor_velocity > 1.0f){
			main_rotor_velocity = 1.0f;
		}else if(main_rotor_velocity < 0.0f){
			main_rotor_velocity = 0.0f;
		}
	}
}
