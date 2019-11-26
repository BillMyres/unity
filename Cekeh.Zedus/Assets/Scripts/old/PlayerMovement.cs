using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    //public Vector2 DisplayPosition;
    public Vector2 Position;
    public float speed = 1;
    bool movingRight = false,
        movingLeft = false,
        movingForward = false,
        movingBackward = false;
    void Start () {
	    
	}
	
	void Update () {
        //MOVE RIGHT
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
            movingRight = true;
        } else if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D)) {
            movingRight = false;
        }

        //MOVE LEFT
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
            movingLeft = true;
        } else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A)) {
            movingLeft = false;
        }

        //MOVE FORWARD
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
            movingForward = true;
        } else if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W)) {
            movingForward = false;
        }

        //MOVE BACKWARD
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
            movingBackward = true;
        } else if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S)) {
            movingBackward = false;
        }

        //MOVE
        if (movingRight) {
            transform.position += new Vector3(0.1f * speed, 0, 0);
            GenerateTerrian.xPosition += 0.1f * speed;
        }
        if (movingLeft) {
            transform.position -= new Vector3(0.1f * speed, 0, 0);
            GenerateTerrian.xPosition -= 0.1f * speed;
        }
        if (movingForward) {
            transform.position += new Vector3(0, 0, 0.1f * speed);
            GenerateTerrian.zPosition += 0.1f * speed;
        }
        if (movingBackward) {
            transform.position -= new Vector3(0, 0, 0.1f * speed);
            GenerateTerrian.zPosition -= 0.1f * speed;
        }
	}
}
