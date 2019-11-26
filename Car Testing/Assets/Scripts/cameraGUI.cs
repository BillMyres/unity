using UnityEngine;
using System.Collections;

public class cameraGUI : MonoBehaviour {

	GameObject buggy;
	float driveSpeed = 80, wheelAngle = 35;

	void Start () {
		buggy = GameObject.FindWithTag ("Buggy");
	}

	void Update () {
		buggy.GetComponent<carController> ().maxMotorTorque = driveSpeed;
		buggy.GetComponent<carController> ().maxSteeringAngle = wheelAngle;
	}

	void OnGUI(){
		GUI.color = Color.black;
		Rect speedLabel = new Rect (5, 30, 20, 100);

		GUI.Label (speedLabel, "maxMotorTorque");

		Rect driveSpeedR = new Rect (20, 30, 20, 100);
		driveSpeed = GUI.VerticalSlider (driveSpeedR, driveSpeed, 100, 1);

		Rect angleLabel = new Rect (30, 5, 100, 20);
		GUI.Label (angleLabel, "maxSteeringAngle");

		Rect wheelAngleR = new Rect (30, 20, 100, 20);
		wheelAngle = GUI.HorizontalSlider (wheelAngleR, wheelAngle, 1, 90);
	}
}
