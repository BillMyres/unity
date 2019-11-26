using UnityEngine;
using System.Collections;

public class suspensionController : MonoBehaviour {

	Vector3 target;
	public Transform wheel;
	//WheelCollider wc;
	LineRenderer lr;

	void Start () {
		//wc = wheel.GetComponent<WheelCollider> ();
		lr = GetComponent<LineRenderer> ();
	}

	void Update () {
		target = wheel.transform.position;
		lr.SetPosition (0, transform.position);
		lr.SetPosition (1, wheel.transform.position);
	}


}
