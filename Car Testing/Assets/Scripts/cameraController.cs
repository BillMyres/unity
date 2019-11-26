using UnityEngine;
using System.Collections;

public class cameraController : MonoBehaviour {

	GameObject Player;
	Vector3 target;

	float angle_x = 0, angle_y = 0;
	public float horizontal_angle = 0, vertical_angle = 0;
	public float horizontal_speed = 1f, vertical_speed = 1f;

	public float zoom = 0, zoom_speed = 1f;

	void Start () {
		Player = GameObject.FindWithTag ("Player");
	}

	void FixedUpdate () {

		zoom = zoom_speed * Input.GetAxis ("Mouse ScrollWheel");

		if(Input.GetMouseButton(2)){
			horizontal_angle += horizontal_speed * Input.GetAxis ("Mouse X");
			vertical_angle += vertical_speed * -Input.GetAxis ("Mouse Y");
		}

		if(horizontal_angle > 180){
			horizontal_angle -= 360;
		}else if(horizontal_angle < -180){
			horizontal_angle += 360;
		}

		if(vertical_angle > 85){
			vertical_angle = 85;
		}else if(vertical_angle < -85){
			vertical_angle = 85;
		}


		transform.RotateAround (Player.transform.position, Vector3.up, horizontal_angle - angle_x);
		angle_x = horizontal_angle;

		transform.RotateAround (Player.transform.position, transform.right, vertical_angle - angle_y);
		angle_y = vertical_angle;

		transform.LookAt(Player.transform);

		transform.position += transform.forward * zoom;
	}
}
