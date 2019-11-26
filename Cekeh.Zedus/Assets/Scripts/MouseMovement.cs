using UnityEngine;
using System.Collections;

public class MouseMovement : MonoBehaviour {

    Animator animator;
    GameObject player;

    bool mouseSet = false;
    Vector2 mouseStart, mouseCurrent;
    public Vector2 mouseDistance;

    public float maxSpeed = 5, zoomSpeed = 8;

    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = player.GetComponent<Animator>();
    }
    
	void Update () {
        Camera_v4.staticDistance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;//camera distance

        mouseCurrent = Input.mousePosition;
        if (Input.GetMouseButtonDown(2)) {
            if (!mouseSet) {
                Vector2 camv4 = new Vector2(Camera_v4.staticPositionX, Camera_v4.staticPositionY);
                mouseStart = Input.mousePosition;
                mouseStart -= camv4;
                mouseSet = true;
                print("MouseSet: " + mouseStart);
            }
        }
        if (mouseSet) {
            mouseDistance = mouseCurrent - mouseStart;
            Camera_v4.staticPositionX = mouseDistance.x;
            Camera_v4.staticPositionY = -mouseDistance.y;
        }
        if (Input.GetMouseButtonUp(2)) {
            if (mouseSet) {
                mouseStart = Vector2.zero;
                mouseSet = false;
                print("Distance: " + mouseDistance);
            }
        }

        if (!CarController.inCar) {
            if (animator.GetBool("Running") == true) {
                player.transform.rotation = Quaternion.Euler(0, mouseDistance.x, 0);
            }
            if (Input.GetKey(KeyCode.LeftShift)) {
                print("LShift");
                maxSpeed = 40;
            } else {
                maxSpeed = 20;
            }
        }

        Camera_v4.staticPositionX += Input.GetAxis("Horizontal");
    }
}
