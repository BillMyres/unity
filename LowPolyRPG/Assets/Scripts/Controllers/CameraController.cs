using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	GameObject Player;
	Vector3 target;

	float angle_x = 0, angle_y = 0;
	public float horizontal_angle = 0, vertical_angle = 0;
	public float horizontal_speed = 1f, vertical_speed = 1f;

	public float zoom = 0, zoom_speed = 1f;

	void Start () {
		Player = transform.parent.gameObject;
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

		transform.LookAt(Player.transform.position + new Vector3(0, .5f, 0));

		transform.position += transform.forward * zoom;
	}
	///--------------------------------------------------
	/// 
	/// 

	bool shaking = false;
	Quaternion start_rotation;
	public void Shake(float duration){
		time = duration;
		start_rotation = transform.localRotation;

		if(!shaking){
			StartCoroutine (ShakeCamera ());
		}
	}

	float time, amount = .25f;
	IEnumerator ShakeCamera(){
		shaking = true;

		while(time > 0.01f){

			Vector3 rotation = Random.insideUnitSphere * amount;
			rotation.z = 0;

			transform.localRotation = Quaternion.Euler (rotation + start_rotation.eulerAngles);

			time -= Time.deltaTime;
			yield return null;
		}

		transform.localRotation = start_rotation;
		shaking = false;
	}

//	public void Shake(){
////		if(!shaking){
////			StartCoroutine (ShakeCamera ());
////		}
//		//transform.
//	}
//
//	bool shaking = false, smooth = true;
//	float start_duration = .05f, start_amount = .25f;
//	float duration = .05f, amount = .25f, percent = 0f, smooth_amount = 5f;
//
//	IEnumerator ShakeCamera(){
//		shaking = true;
//
//		while(duration > 0.01f){
//			Vector3 rotation_amount = Random.insideUnitSphere * amount;
//			rotation_amount.z = 0;
//
//			percent = duration / start_duration;
//
//			amount = start_amount * percent;
//			duration = Mathf.Lerp (duration, 0, Time.deltaTime);
//
//			if (smooth) 
//				transform.localRotation = Quaternion.Lerp (transform.localRotation, Quaternion.Euler (rotation_amount), Time.deltaTime * smooth_amount);
//			else
//				transform.localRotation = Quaternion.Euler (rotation_amount);
//			yield return null;
//		
//		}
//		transform.localRotation = Quaternion.identity;
//		shaking = false;
//	}
}
