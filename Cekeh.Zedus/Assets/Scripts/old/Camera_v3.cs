using UnityEngine;
using System.Collections;

public class Camera_v3 : MonoBehaviour {
    //rotation around player in degrees
    public int xRotation = 0, yRotation = 0;
    public float rotationSpeed;

    //distance from player
    Vector3 offset;

    //objects to reference / alter location / rotation
    public GameObject player;

    Vector3 centerClickPosition;

	void Start () {
        offset = new Vector3(0f, 0f, 12f);
	}
	
	void Update () {
        //keep variables in range
        xRotation = adjustRotation(xRotation);
        yRotation = adjustRotation(yRotation);

        if (Input.GetMouseButtonDown(2)) {
            centerClickPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(2)) {
            Vector3 r = Input.mousePosition - centerClickPosition;
            //xRotation += (int)r.x;
            yRotation += (int)r.y;
        }
        centerClickPosition = Input.mousePosition;
        //set camera position
        transform.position = player.transform.position + offset;
        transform.RotateAround(player.transform.position, new Vector3(0, 1, 0), xRotation);
        transform.RotateAround(player.transform.position, new Vector3(player.transform.position.x - transform.position.x, 0, player.transform.position.z - transform.position.z), yRotation);
        transform.LookAt(player.transform);
        
    }
    //make sure rotation is 0 - 360
    int adjustRotation(int angle) {
        if (angle > 360) {
            angle -= 360;
        } else if (angle < 0) {
            angle += 360;
        }
        return angle;
    }
}
