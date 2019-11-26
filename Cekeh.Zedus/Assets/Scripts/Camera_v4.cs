using UnityEngine;
using System.Collections;

public class Camera_v4 : MonoBehaviour {

    Transform player;

    Camera cam;

    public static float staticDistance;
    public static float staticPositionX, staticPositionY;

    public float distance = 10f;
    public float sensivityX = 4f, sensivityY = 1f;
    public float positionX = 0f, positionY = 0f;
    public int speed = 20;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cam = Camera.main;
        staticDistance = distance;
        staticPositionX = positionX;
        staticPositionY = positionY;
    }

    void OnGUI() {
        if (GUI.Button(new Rect(400, 10, 110, 20), "Home Teleport")) {
            print("HOME TELEPORT: ");
            player.transform.position = new Vector3(1800, 0, 575);
        }
    }

    void Update() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate() {
        distance = staticDistance;
        positionX = staticPositionX;
        positionY = staticPositionY;

        if (Input.GetKey(KeyCode.RightArrow)) {
            positionX -= speed * sensivityX * Time.deltaTime;
        }else if (Input.GetKey(KeyCode.LeftArrow)) {
            positionX += speed * sensivityX * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow)) {
            positionY += speed * sensivityX * Time.deltaTime;
        }else if (Input.GetKey(KeyCode.DownArrow)) {
            positionY -= speed * sensivityX * Time.deltaTime;
        }

        checkPosition();

        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(positionY, positionX, 0f);
        transform.position = player.position + rotation * direction;
        transform.LookAt(player);

        
    }

    void checkPosition() {
        if (positionY > 89) {
            positionY = 89f;
        } else if (positionY < 0) {
            positionY = 0;
        }
    }
}
