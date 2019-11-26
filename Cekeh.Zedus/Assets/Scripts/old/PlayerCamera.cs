using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {

    GameObject Player;
    Vector3 lastMousePos;
    public Vector3 offset;
    Quaternion rotation;
    Vector3 centerClickPosition;
    public Vector2 zoom;
    [Range(0f, 1f)]
    public float scrollSpeed = 0.5f;
    [Range(-360, 360)]
    public float xRotation;
    [Range(1, 100)]
    public int cameraDistance = 50;

    void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
        rotation = Quaternion.Euler(0,0,0);
    }
	
	
	void Update () {
        transform.position = Player.transform.position + offset;
        transform.LookAt(Player.transform);
        
        
        
        if (Input.mouseScrollDelta.y == 1 && cameraDistance > 0) {
            zoom.x += 1;
            zoom.y -= 1;
            cameraDistance--;
        }else if (Input.mouseScrollDelta.y == -1 && cameraDistance < 100){
            zoom.x -= 1;
            zoom.y += 1;
            cameraDistance++;
        }

        if (Input.GetMouseButtonDown(2)) {
            if (centerClickPosition == Vector3.zero) { 
                centerClickPosition = Input.mousePosition;
            }
            lastMousePos = Input.mousePosition;
        } else if (Input.GetMouseButtonUp(2)) {
            centerClickPosition = Vector3.zero;
        }
        
        if (Input.GetMouseButton(2)) {
            Vector3 pos = Input.mousePosition - lastMousePos;
            if (Input.mousePosition == lastMousePos) {
                
            }else{ 
                if (pos.x > 0) {
                    xRotation += 5;
                }
                if (pos.x < 0) {
                    xRotation -= 5;
                }
            }
            //lastMousePos = pos;
            lastMousePos = Input.mousePosition;
        }

        if (cameraDistance == 50) { offset = new Vector3(0, 12, 12); }
        transform.RotateAround(Player.transform.position, new Vector3(0, 1, 0), xRotation);
        offset += ((new Vector3(Player.transform.position.x - transform.position.x, 0, Player.transform.position.z - transform.position.z) / 100) * (zoom.x));
        offset += ((new Vector3(0, transform.position.y - Player.transform.position.y, 0) / 100) * (zoom.y));
        zoom = Vector2.zero;
    }
}
