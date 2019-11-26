using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AxleInfo {
	public WheelCollider leftWheel;
	public WheelCollider rightWheel;
	public bool motor;
	public bool steering;
	public bool braking;
}

public class carController : MonoBehaviour {
	public List<AxleInfo> axleInfos; 
	public float maxMotorTorque;
	public float maxSteeringAngle;
	public float maxBrakeTorque;

	public bool Active = true;

	void Start(){
		
	}

	// finds the corresponding visual wheel
	// correctly applies the transform
	public void ApplyLocalPositionToVisuals(WheelCollider collider)
	{
		if (collider.transform.childCount == 0) {
			return;
		}

		Transform visualWheel = collider.transform.GetChild(0);

		Vector3 position;
		Quaternion rotation;
		collider.GetWorldPose(out position, out rotation);

		visualWheel.transform.position = position;
		visualWheel.transform.rotation = rotation;
	}

	public void Update()
	{
		if (Active) {
			float motor = maxMotorTorque * Input.GetAxis ("Vertical");
			float steering = maxSteeringAngle * Input.GetAxis ("Horizontal");
			float braking = maxBrakeTorque * Input.GetAxis ("Jump");

			foreach (AxleInfo axleInfo in axleInfos) {
				//axleInfo.leftWheel.ConfigureVehicleSubsteps (12, 12, 15);
				//axleInfo.rightWheel.ConfigureVehicleSubsteps (12, 12, 15);

				if (axleInfo.steering) {
					axleInfo.leftWheel.steerAngle = steering;
					axleInfo.rightWheel.steerAngle = steering;
				}
				if (axleInfo.motor) {
					axleInfo.leftWheel.motorTorque = motor;
					axleInfo.rightWheel.motorTorque = motor;
				}
				if (axleInfo.braking) {
					axleInfo.leftWheel.brakeTorque = braking;
					axleInfo.rightWheel.brakeTorque = braking;
				}
				ApplyLocalPositionToVisuals (axleInfo.leftWheel);
				ApplyLocalPositionToVisuals (axleInfo.rightWheel);
			}
		}
	}
}