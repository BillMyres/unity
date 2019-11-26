using UnityEngine;
using System.Collections;

public class botController : MonoBehaviour {

	public Animator anim; 
	public float botspeed = 5.0f, speed = 0, turnspeed = 5.0f, turn = 0;

	public  bool Active = true;

	void Start () {
		
	}

	void Update () {
		if (Active) {
			speed = botspeed * Input.GetAxis ("Vertical");
			turn = turnspeed * Input.GetAxis ("Horizontal");
		}
		anim.SetFloat ("speed", speed);

		transform.Translate (Vector3.forward * speed * Time.deltaTime);
		transform.Rotate(0, turn, 0);
	}

}
