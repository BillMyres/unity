using UnityEngine;
using System.Collections;

public class Camera_v1 : MonoBehaviour {

    public Vector3 offset;
    public int xRotation;
    public Vector3 forward, up;
    public float scrollSpeed;

    GameObject Cam, Player;
    public float cameraDistanceMax, cameraDistanceMin, cameraDistance;


    void Start () {
        Cam     = Camera.main.gameObject;
        Player  = GameObject.FindGameObjectWithTag("Player");
        setCameraDistance();
    }
	
	void Update () {
        Vector3 temp = new Vector3(Cam.transform.position.x - Player.transform.position.x, 0, Cam.transform.position.z - Player.transform.position.z);
        forward = new Vector3(Cam.transform.position.x - Player.transform.position.x, 0, Cam.transform.position.z - Player.transform.position.z);
        up      = new Vector3(0, Cam.transform.position.y - Player.transform.position.y, 0);
        //print(distance);
        //make sure angle is 0-360
        xRotation = angleAdjust(xRotation);

        cameraDistance += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        setCameraDistance();

        

        if (Input.GetKey(KeyCode.RightArrow)) {
            offset -= forward * Time.deltaTime;
        }else if (Input.GetKey(KeyCode.LeftArrow)) {
            offset += forward * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow)) {
            offset -= up * Time.deltaTime;
        }else if (Input.GetKey(KeyCode.DownArrow)) {
            offset += up * Time.deltaTime;
        }
        Cam.transform.LookAt(Player.transform);
        Cam.transform.position = Player.transform.position + offset;
	}

    void setCameraDistance() {
        cameraDistanceMin   = Player.transform.position.y + 2f;
        cameraDistanceMax   = Player.transform.position.y + 100;
        cameraDistance      = Mathf.Clamp(cameraDistance, cameraDistanceMin, cameraDistanceMax);
    }

    int angleAdjust(int angle) {
        if (angle > 360) {
            angle -= 360;
        } else if (angle < 0) {
            angle += 360;
        }
        return angle;
    }
}
