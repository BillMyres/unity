using UnityEngine;
using System.Collections;

public class NPCcontroller : MonoBehaviour {

	Animator anim;
	Rigidbody rb;

	public float speed = 0, multiplier = 0, max_speed = 1;
	public bool stab = false, block = false;

	void Start () {
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
	}

	void Update () {
		if (Input.GetKey (KeyCode.LeftShift)) {
			multiplier = max_speed * 2;
		} else {
			multiplier = max_speed;
		}

		speed = Input.GetAxis ("Vertical") * multiplier;

		if(Input.GetMouseButton(1)) {
			stab = false;
			block = true;
			speed = 0;


		}

		if (Input.GetMouseButtonDown(0)) {
			stab = true;
			block = false;
			speed = 0;
		}

		transform.position += transform.forward * speed * Time.deltaTime;

		anim.SetFloat ("speed", speed);
		anim.SetBool ("stab", stab);
		anim.SetBool ("block", block);

		stab = false;
		block = false;
	}
}
