using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public GameObject me;

    float height = 0;
    [Range(0f, 1f)]
    public float speed = 1;
    float scrollDragX, scrollDragZ;
    Vector3 lastMousePos;

    bool movingRight = false,
        movingLeft = false,
        movingForward = false,
        movingBackward = false;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
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

        //Mouse Wheel
        if (Input.mouseScrollDelta.y == 1) {
            Camera.main.transform.position -= Vector3.back;
            Camera.main.transform.position -= Vector3.up;
        }
        else if (Input.mouseScrollDelta.y == -1){
            Camera.main.transform.position += Vector3.back;
            Camera.main.transform.position += Vector3.up;
        }

        
        if (Input.GetMouseButtonDown(2)) {
            lastMousePos = Input.mousePosition;
        } else if (Input.GetMouseButtonUp(2)) {

        }
        if (Input.GetMouseButton(2)) {
            Vector3 pos = Input.mousePosition - lastMousePos;
            if (Input.mousePosition == lastMousePos) {

            }else{ 
                if (pos.x > 0) {
                    print(pos.x);
                    Camera.main.transform.LookAt(me.transform) ;
                    Camera.main.transform.Translate(Vector3.left * 0.1f);
                }
                if (pos.x < 0) {
                    Camera.main.transform.LookAt(me.transform) ;
                    Camera.main.transform.Translate(Vector3.right * 0.1f);
                }
            }
            //lastMousePos = pos;
        }

        if (height != GenerateTerrian.playerHeight) {
            height = GenerateTerrian.playerHeight;
            transform.position = new Vector3(transform.position.x, height, transform.position.z);
        }
	}
}
