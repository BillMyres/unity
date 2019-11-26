using UnityEngine;
using System.Collections;

public class visualWheel : MonoBehaviour {

	WheelCollider WC;
	public float suspensionDistance;

	void Start () {
		WC = GetComponent<WheelCollider> ();
	}

	void Update () {
		suspensionDistance = WC.suspensionDistance;
	}
}
