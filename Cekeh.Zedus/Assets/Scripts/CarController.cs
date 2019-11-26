using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour {

    public Transform[] wheels;
    public GameObject[] wheelObj;

    public float enginePower = 150f;

    public float power = 0f, brake = 0f, steer = 0f;
    public float maxSteer = 25f, breakingPower = 2500;

    public float RPM = 0;
    public static bool inCar = false;

    [Range(-1f, 1f)]
    public float massChange = 0;
    Vector3 COM;

    Vector3 wheelCenter_FL, wheelPos_FL;

    void Start () {
        setMass();
        wheelCenter_FL = wheels[1].transform.localPosition;
        wheelPos_FL = wheelObj[0].transform.localPosition;
    }
    
    public Vector3 adjust;
    void setMass() {
        COM = (wheels[0].GetComponent<WheelCollider>().center +
              wheels[1].GetComponent<WheelCollider>().center +
              wheels[2].GetComponent<WheelCollider>().center +
              wheels[3].GetComponent<WheelCollider>().center) / 4;
        Vector3 mass = GetComponent<Rigidbody>().centerOfMass;
        GetComponent<Rigidbody>().centerOfMass = COM + adjust;
    }
    
	
	void FixedUpdate () {
        // wheels[0].GetComponent<WheelCollider>().
        setMass();

        if (inCar) {
            power = Input.GetAxis("Vertical") * enginePower;
            steer = Input.GetAxis("Horizontal") * maxSteer;
            if (Input.GetKey(KeyCode.Space)) {
                brake = breakingPower;
            } else { brake = 0; }


            //front wheels
            wheels[0].gameObject.GetComponent<WheelCollider>().steerAngle = steer;
            wheels[1].gameObject.GetComponent<WheelCollider>().steerAngle = steer;
            RPM = wheels[1].gameObject.GetComponent<WheelCollider>().rpm;

            Quaternion rot;
            Vector3 pos;
            wheels[1].GetComponent<WheelCollider>().GetWorldPose(out pos, out rot);
            wheelObj[0].transform.position = pos;
            wheelObj[0].transform.rotation = rot;

            wheels[0].GetComponent<WheelCollider>().GetWorldPose(out pos, out rot);
            wheelObj[1].transform.position = pos;
            wheelObj[1].transform.rotation = rot;

            wheels[2].GetComponent<WheelCollider>().GetWorldPose(out pos, out rot);
            wheelObj[2].transform.position = pos;
            wheelObj[2].transform.rotation = rot;

            wheels[3].GetComponent<WheelCollider>().GetWorldPose(out pos, out rot);
            wheelObj[3].transform.position = pos;
            wheelObj[3].transform.rotation = rot;

            if (brake > 0) {
                //wheels[0].gameObject.GetComponent<WheelCollider>().brakeTorque = brake;
                //wheels[1].gameObject.GetComponent<WheelCollider>().brakeTorque = brake;
                wheels[2].gameObject.GetComponent<WheelCollider>().brakeTorque = brake;
                wheels[3].gameObject.GetComponent<WheelCollider>().brakeTorque = brake;

                //Drive wheels : RWD FTW
                wheels[0].gameObject.GetComponent<WheelCollider>().motorTorque = 0f;
                wheels[1].gameObject.GetComponent<WheelCollider>().motorTorque = 0f;
                wheels[2].gameObject.GetComponent<WheelCollider>().motorTorque = 0f;
                wheels[3].gameObject.GetComponent<WheelCollider>().motorTorque = 0f;
            } else {
                wheels[0].gameObject.GetComponent<WheelCollider>().brakeTorque = 0f;
                wheels[1].gameObject.GetComponent<WheelCollider>().brakeTorque = 0f;
                wheels[2].gameObject.GetComponent<WheelCollider>().brakeTorque = 0f;
                wheels[3].gameObject.GetComponent<WheelCollider>().brakeTorque = 0f;

                //Drive wheels : RWD FTW

                wheels[0].gameObject.GetComponent<WheelCollider>().motorTorque = power/2;
                wheels[1].gameObject.GetComponent<WheelCollider>().motorTorque = power/2;
                wheels[2].gameObject.GetComponent<WheelCollider>().motorTorque = power;
                wheels[3].gameObject.GetComponent<WheelCollider>().motorTorque = power;
            }
        } else {
            wheels[0].gameObject.GetComponent<WheelCollider>().brakeTorque = breakingPower;
            wheels[1].gameObject.GetComponent<WheelCollider>().brakeTorque = breakingPower;
            wheels[2].gameObject.GetComponent<WheelCollider>().brakeTorque = breakingPower;
            wheels[3].gameObject.GetComponent<WheelCollider>().brakeTorque = breakingPower;
        }

	}
}
