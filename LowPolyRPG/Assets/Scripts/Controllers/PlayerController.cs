using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	Animator anim;
	Rigidbody rb;

	public float speed;
	float player_speed = 0;

	void Start () {
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
	}

	void Update () {
		
		player_speed = Input.GetAxis ("Vertical") * speed * Time.deltaTime;

		transform.position += transform.forward * player_speed * Time.deltaTime;

		anim.SetFloat ("player_speed", player_speed);
	}
}
