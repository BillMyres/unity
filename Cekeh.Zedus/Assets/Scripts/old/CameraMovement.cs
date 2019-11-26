using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
    
    public Vector2 Zoom;
    public int RotationAngle;
    GameObject Player;

    [Range(0f, -15f)]
    public float offsetX, offsetY;

    void Start () {
        Zoom = new Vector3(-12, 12);
        Player = GameObject.FindGameObjectWithTag("Player");
        //transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z) + Zoom;
        //transform.rotation = Quaternion.Euler(transform.rotation.x, Player.transform.rotation.y, transform.rotation.z);
        
    }

    void Update() {
        if (RotationAngle > 360) { RotationAngle = RotationAngle - 360; }
        if (RotationAngle < 0) { RotationAngle = RotationAngle + 360; }

        if (Input.mouseScrollDelta.y == 1 && transform.position.y > Player.transform.position.y + 0.49f) {
            Zoom.x += 1;
            Zoom.y -= 1;
        }
        else if (Input.mouseScrollDelta.y == -1){
            Zoom.x -= 1;
            Zoom.y += 1;
        }

        //transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z) + Zoom;
        //transform.rotation = Quaternion.Euler(transform.rotation.x, Player.transform.rotation.y, transform.rotation.z);
        transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, Zoom.x);
        transform.position += Vector3.up * Zoom.y;

        Zoom = Vector2.zero;
        transform.LookAt(Player.transform);
        transform.RotateAround(Player.transform.position, new Vector3(0, 1, 0), RotationAngle);
        RotationAngle = 0;
        //print(Quaternion.EulerRotation(0, RotationAngle, 0));
    }
	
}
